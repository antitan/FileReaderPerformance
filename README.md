de la meme facon, peux tu m'ecire la classe AIProcessedDocumentsListByIdsActionFilter qui implemente IActionFilter pour la méthode suivante :

[HttpGet("AIProcessedDocuments/ListByIds")]
[ProducesResponseType(typeof(IEnumerable<GetDocumentByIdsQueryResponseDto>), (int)HttpStatusCode.OK)]
[ProducesResponseType((int)HttpStatusCode.NotFound)]
[ProducesResponseType((int)HttpStatusCode.BadRequest)]
[ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public async Task<IActionResult> AIProcessedDocumentsListByIds( [FromBody] RequestIdsDto dto)
{
    List<GetDocumentByIdsQueryResponseDto> results = new List<GetDocumentByIdsQueryResponseDto>(); 
    if(dto.Ids == null || !dto.Ids.Any())
    {
        return StatusCode((int)HttpStatusCode.NotFound, "PayLoad needs to contain one Id at least");
    }

    int nbItemsToGetOneShot = dto.Ids.Count();
    if (nbItemsToGetOneShot > applicationConfiguration.MaxDocumentsToGetOnShot)
    {
        return StatusCode((int)HttpStatusCode.UnprocessableEntity, $"You can retrieve {applicationConfiguration.MaxDocumentsToGetOnShot} document rows maximum.  ");
    }
    //traitement...
}
 avec 
 
  public class RequestIdsDto
 {
     public List<Guid> Ids { get; set; } 
 }
  
