  bonjour, la requete suivante avec dapper a un probleme, la ligne documentEntry.DocumentType = documentType; foncnne mal, elle ne rempli pas correctement la propriété documentEntry.DocumentType.Id avec la bonne valeur. (elle met 0 systematiquement) :
  
  
  using (var connection = new SqlConnection(connectionString))
  {
      await connection.OpenAsync(cancellationToken);
      //Un dictionnaire est utilisé pour suivre et regrouper les documents par leur identifiant unique.
      //Cela garantit que chaque document n'est créé qu'une seule fois et que toutes les métadonnées associées sont ajoutées à la liste de métadonnées de ce document.
      var documentDictionary = new Dictionary<Guid, Document>();
       
      var entities = await connection.QueryAsync<Document, DocumentType, DocumentMetadata, Document>(
          @$"SELECT [Document].[Id],
           [Document].[Name],
           [Document].[ChatGptScopeId],
           [Document].[ChatGptDocumentId],
           [Document].[DocumentTypeId],
           [Document].[IngestionState],
           [Document].[IngestionStateUpdatedAt],
           [Document].[ProcessingErrorDetail], 
           [DocumentType].[Id] AS DocumentTypeId,
           [DocumentType].[Extension],
           [DocumentType].[MimeType], 
           [DocumentMetadata].[Id] AS MetadataId,
           [DocumentMetadata].[MetadataKey],
           [DocumentMetadata].[MetadataValue]
            FROM [Document]
            INNER JOIN [DocumentType] ON [DocumentType].[Id] = [Document].[DocumentTypeId] 
            LEFT JOIN [DocumentMetadata] ON [DocumentMetadata].[DocumentId] = [Document].[Id]
            WHERE {whereClause}",
          (document, documentType, metadata) =>
          {
              Document? documentEntry;

              if (!documentDictionary.TryGetValue(document.Id, out documentEntry))
              {
                  documentEntry = document;
                  documentEntry.DocumentType = documentType;
                  documentEntry.DocumentMetadatas = new List<DocumentMetadata>();
                  documentDictionary.Add(documentEntry.Id, documentEntry);
              }

              if (metadata != null)
              {
                  ((List<DocumentMetadata>)documentEntry.DocumentMetadatas).Add(metadata);
              }

              return documentEntry;
          },

      //La propriété `splitOn` de la méthode `QueryAsync` vous permet de spécifier les colonnes qui délimitent les différents objets dans votre requête.
      //Cela permet à Dapper de savoir comment diviser la ligne de résultats en plusieurs objets.
      splitOn: "DocumentTypeId,MetadataId"
      );
}

et voici les classes referencées :

 public class Document  
 {
         public Guid Id { get; set; }
         public byte DocumentTypeId { get; set; }
         public string IngestionState { get; set; }
         public DateTime?  IngestionStateUpdatedAt { get; set; }
         public string Name { get; set; }
         public string ChatGptScopeId { get; set; }
         public string ChatGptDocumentId { get; set; } 
        

         public DocumentType DocumentType { get; set; }
         public  IEnumerable<DocumentMetadata> DocumentMetadatas { get; set; } 
         public string? ProcessingErrorDetail { get; set; }   
 }
 
  public class DocumentType : IEntityWithId<byte>
 {
     public byte Id {  get; set; }   
     public string Extension { get; set; }
     public string MimeType { get; set; }
 }
 
   public class DocumentMetadata : IEntityWithId<long>
  {
      public long Id { get; set; }
      public Guid DocumentId { get; set; }
      public string  MetadataKey { get; set; }
      public string MetadataValue { get; set; }
  }
  
  Peux-tu m'indiquer qu'es ce que je dois changer pour que cela fonctionne et que la documentEntry.DocumentType.Id soit correctement valorisée
