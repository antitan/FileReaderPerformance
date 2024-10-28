 Merci pour ton aide, j'ai un petit soucis maintenant avec l'implementation , voici le code complet :
 
 public async Task<IDictionary<string, bool>> DeleteDocuments(IEnumerable<Document> documents)
 {
     var resultDictionary = new ConcurrentDictionary<string, bool>();
     if (documents == null || !documents.Any())
         return resultDictionary;

     var semaphore = new SemaphoreSlim(Environment.ProcessorCount - 1);
     var tasks = new List<Task>();

     foreach (var document in documents)
     {
         await semaphore.WaitAsync();
         tasks.Add(Task.Run(async () =>
         {
             try
             {
                 var isSuccess = await DeleteDocumentAsync(document);
                 resultDictionary.TryAdd(document.ChatGptDocumentId, isSuccess);
             }
             finally
             {
                 semaphore.Release();
             }
         }));
     }

     await Task.WhenAll(tasks);
     return resultDictionary;

 }

 private async Task<bool> DeleteDocumentAsync(Document document)
 {
     bool isDeleted = false;
     var client = new RestClient(new RestClientOptions
     {
         BaseUrl = new Uri(applicationConfiguration.IngestionUrlBase, UriKind.Absolute),
         MaxTimeout = apiCallTimeoutInSeconds,
         Proxy = new WebProxy(applicationConfiguration.UbpProxyUrl, true)
         {
             Credentials = new NetworkCredential(applicationConfiguration.ProxyUser, applicationConfiguration.ProxyPassword)
         }
     });

     var request = new RestRequest(applicationConfiguration.IngestionUrlResource, Method.Post);
     request.AddHeader("Content-Type", "application/json");
     request.AddHeader("Accept", "application/json");

     var token = await tokenProvider.GetAccessTokenAsync();
     request.AddHeader("Authorization", $"Bearer {token}");

     string embeddedTemplateFileInProject = IngestionEmbeddedResourceFiles.DeleteIngestedDocument;

     var ingestTemplateContentFile = await embeddedResourcesManager.GetResourceFileContent(Assembly.GetExecutingAssembly(), embeddedTemplateFileInProject);

     var formatedJson = ingestTemplateContentFile.Replace("[ChatGptDocumentId]", document.ChatGptDocumentId);

     request.AddParameter("application/json", formatedJson, ParameterType.RequestBody);


     //get retry policy
     var unauthorizedRetryPolicy = retryPolicyProvider.GetPolicy();


     var response = await unauthorizedRetryPolicy.ExecuteAsync(() => client.ExecuteAsync(request));

     if (response.IsSuccessful)
     {

         var operationSuccessFull = JsonDocument.Parse(response.Content!)?
                                        .RootElement
                                     .GetProperty("data")
                                     .GetRawText();

         if (operationSuccessFull == "null")
         {
             isDeleted = false;
             logger.LogError($"Impossible to Delete Document, content response : [{response.Content}] ");
         }
         else
         {
             var result = JsonDocument.Parse(response.Content!)?
                                            .RootElement
                                         .GetProperty("data")
                                         .GetProperty("contentDelete")
                                         .GetBoolean();
             isDeleted = result!.Value;
         }   
     }
     else
     {
         logger.LogError($"Impossible to Delete Document, content response : [{response.Content}] , Status Code: {response.StatusCode}, Error Message: {response.ErrorMessage}");
         throw new Exception($"Couldn't generate token from . Status Code: " + response.StatusCode + ", Error Message: " + response.ErrorMessage);
     }

     return isDeleted;
 }
 
 le probleme est que si j'envoi une liste de documents :
 
 [ 
 "ChatGptDocumentId1_ok",//ce document existe dans le cloud et sera supprimé
 "ChatGptDocumentId_ko",//ce document n'existe pas 
 ] 
 
 la reponse que j'obtiens est un dictionnaire avec seulement une clé valeur avec l'id qui a reussi {key : "ChatGptDocumentId1_ok", value:true}. 
 Alors que le dictionnaire doit contenir 2 clé valeurs : {key : "ChatGptDocumentId1_ok", value:true} et {key : "ChatGptDocumentId2_ko", value:false} 
 
Si j'envoi une liste ChatGptDocumentId qui n'existe pas :
 
 [ 
 "ChatGptDocumentId2_ko",//ce document n'existe pas 
 "ChatGptDocumentId3"_ko,//ce document n'existe pas 
 ] 
 
 la reponse est correcte, le dictionnaire de retour doit contenir 2 clé valeurs : {key : "ChatGptDocumentId2_ko", value:false} et {key : "ChatGptDocumentI3_ko", value:false} 
 
 Peux tu me corriger cela ?
 
 
