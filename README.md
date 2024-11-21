j'ai une methode web api qui fait 2 controles avant de faire le traitement:

[HttpPost("AIProcessedDocuments")]
[ProducesResponseType(typeof(DocumentListProcessedResponseDto), (int)HttpStatusCode.OK)]
[ProducesResponseType((int)HttpStatusCode.NotFound)]
[ProducesResponseType(typeof(DocumentListProcessedResponseDto), (int)HttpStatusCode.UnprocessableEntity)]
[ProducesResponseType(typeof(DocumentListProcessedResponseDto), (int)HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(DocumentListProcessedResponseDto), (int)HttpStatusCode.InternalServerError)]
public async Task<IActionResult> ProcessDocuments([FromBody] IEnumerable<NewDocumentRequestDto> documentRequestListDto)
{
        if (documentRequestListDto == null || !documentRequestListDto.Any())
        {
            return StatusCode((int)HttpStatusCode.NotFound, "PayLoad needs to contain one document item at least");
        }

        int nbItemsToIngest = documentRequestListDto.Count();
        if (nbItemsToIngest > applicationConfiguration.MaxDocumentsToIngestOneShot)
        {
            return StatusCode((int)HttpStatusCode.UnprocessableEntity, $"You can process {applicationConfiguration.MaxDocumentsToIngestOneShot} documents maximum  ");
        }
		//traitement...
}

j'aimerai "deplacer" ces 2 controles dans une classe AIProcessedDocumentsActionFilter qui hérite de IActionFilter.
Est-ce possible, peux tu écrire AIProcessedDocumentsActionFilter ?
