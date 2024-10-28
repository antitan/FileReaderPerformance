private async Task DeleteDocumentAsync(Document document, ConcurrentDictionary<string, bool> resultDictionary, SemaphoreSlim semaphore)
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
}

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
        tasks.Add(DeleteDocumentAsync(document, resultDictionary, semaphore));
    }

    await Task.WhenAll(tasks);
    return resultDictionary;
}