Dans un exemple precedent tu m'avais ecris le code suivant pour obtenir une list de batchId de facon asynchrone:

var resultDictionary = new ConcurrentDictionary<string, Node>();
if (documents == null || !documents.Any())
    return resultDictionary;

var chunkIds = documents.Select(i => i.ChatGptDocumentId).Chunk(20);
var semaphore = new SemaphoreSlim(Environment.ProcessorCount - 1);
var tasks = new List<Task>();

foreach (var idLists in chunkIds)
{
    await semaphore.WaitAsync();
    tasks.Add(Task.Run(async () =>
    {
        try
        {
            var nodes = await GetDocumentInformationsByIds(idLists);
            foreach (var node in nodes)
            {
                resultDictionary[node.id] = node;
            }
        }
        finally
        {
            semaphore.Release();
        }
    }));
}

await Task.WhenAll(tasks);
return resultDictionary;


peux tu m'adapter ce code  la meme  logique pour le la mtehode ci dessous 
 
 public async Task<IDictionary<string, bool>> DeleteDocuments(IEnumerable<Document> documents)
 {
     var result = new Dictionary<string, bool>();
     if (documents == null || !documents.Any())
         return result;
     ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 };
     var resultBag = new ConcurrentDictionary<string, bool>();
     await Parallel.ForEachAsync(documents, parallelOptions, async (document, _) =>
     {
         var isSuccess = await DeleteDocumentAsync(document);
         resultBag.TryAdd(document.ChatGptDocumentId, isSuccess);
     });
     result = resultBag.ToDictionary();
     return result;
 }
