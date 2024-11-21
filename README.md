bonjour,

j'ai crée une la  methode c# ProcessDocuments :
[HttpPost("AIProcessedDocuments")]

//validation
[ServiceFilter(typeof(AIProcessedDocumentsActionFilter))]

[ProducesResponseType(typeof(DocumentListProcessedResponseDto), (int)HttpStatusCode.OK)]
[ProducesResponseType((int)HttpStatusCode.NotFound)]
[ProducesResponseType(typeof(DocumentListProcessedResponseDto), (int)HttpStatusCode.UnprocessableEntity)]
[ProducesResponseType(typeof(DocumentListProcessedResponseDto), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(DocumentListProcessedResponseDto), (int)HttpStatusCode.InternalServerError)]
public async Task<IActionResult> ProcessDocuments([FromBody] IEnumerable<NewDocumentRequestDto> documentRequestListDto)
{
...
}

j'ai aussi créer la classe AIProcessedDocumentsActionFilter chargée de validée certiantes condition et qui est executé avant d'atteindre la methode  ProcessDocuments : 

  public class AIProcessedDocumentsActionFilter : IActionFilter
  {
      private readonly ApplicationConfiguration applicationConfiguration;

      public AIProcessedDocumentsActionFilter(IOptions<ApplicationConfiguration> applicationConfigurationOption)
      {
          applicationConfiguration = applicationConfigurationOption.Value;
      }

      public void OnActionExecuting(ActionExecutingContext context)
      {
          if (context.ActionArguments.TryGetValue("documentRequestListDto", out var value))
          {
              var documentRequestListDto = value as IEnumerable<NewDocumentRequestDto>;

              if (documentRequestListDto == null || !documentRequestListDto.Any())
              {
                  context.Result = new NotFoundObjectResult("PayLoad needs to contain one document item at least");
                  return;
              }

              int nbItemsToIngest = documentRequestListDto.Count();
              if (nbItemsToIngest > applicationConfiguration.MaxDocumentsToIngestOneShot)
              {
                  context.Result = new UnprocessableEntityObjectResult($"You can process {applicationConfiguration.MaxDocumentsToIngestOneShot} documents maximum");
                  return;
              }
          }
          else
          { 
              context.Result = new BadRequestObjectResult("documentRequestListDto parameter is required");
          }
      }

      public void OnActionExecuted(ActionExecutedContext context)
      {
          // No action required after the method execution
      }
  }


Cela fonctionne correctement, mais je n'arrive pas a faire un test uinitarie du controller qui declenche  le filtre AIProcessedDocumentsActionFilter :
 public class ArtificialIntelligenceControllerTests
 {
     public ArtificialIntelligenceController controller;
     private readonly Mock<ILogger<ArtificialIntelligenceController>> mockLogger = new();
     private readonly Mock<IMediator> mockMediator= new();

     public ArtificialIntelligenceControllerTests()
     {
         controller = new ArtificialIntelligenceController(mockLogger.Object, mockMediator.Object);
     }

     [Fact]
     public async Task Check_ProcessDocuments_Empty_failed()
     {
         var result = await controller.ProcessDocuments( new List<NewDocumentRequestDto>());
         Assert.IsType<NotFoundObjectResult>(result);
     }

 }
 
 Lorsque la ligne var result = await controller.ProcessDocuments( new List<NewDocumentRequestDto>()); s'exeucte , AIProcessedDocumentsActionFilter n'est pas declenché.
 Peux tu m'indiquer comment le declencher ?



