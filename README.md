
de la meme facon, peux tu m'ecire la classe DeleteContentIdsActionFilter qui implemente IActionFilter pour la méthode suivante :

  [HttpGet("AIProcessedDocuments/DeleteContentIds")]
  [ProducesResponseType(typeof(IEnumerable<GetDocumentByIdsQueryResponseDto>), (int)HttpStatusCode.OK)]
  [ProducesResponseType((int)HttpStatusCode.NotFound)]
  [ProducesResponseType((int)HttpStatusCode.BadRequest)]
  [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
  [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
  public async Task<IActionResult> DeleteContentIds([FromBody] RequestIdsDto dto)
  {
      List<DeleteContentIdsResponseDto> results = new List<DeleteContentIdsResponseDto>();
      if (dto.Ids == null || !dto.Ids.Any())
      {
          return StatusCode((int)HttpStatusCode.NotFound, "PayLoad needs to contain one Id at least");
      }

      int nbItemsToDeleteOneShot = dto.Ids.Count();
      if (nbItemsToDeleteOneShot > applicationConfiguration.MaxDocumentsToDeleteOnShot)
      {
          return StatusCode((int)HttpStatusCode.UnprocessableEntity, $"You can delete {applicationConfiguration.MaxDocumentsToGetOnShot} document items maximum one shot.  ");
      }
    //traitement...
}
 avec 
 
  public class RequestIdsDto
 {
     public List<Guid> Ids { get; set; } 
 }
  
