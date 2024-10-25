
 public async Task<IDictionary<string, Node>> GetDocumentInformations(IEnumerable<Document> documents)
   {
       var result = new Dictionary<string, Node>();
       if (documents == null || !documents.Any()) 
           return result;
       //by paquet of 20 ids
       var chunkIds = documents.Select(i => i.ChatGptDocumentId).Chunk(20);
       ParallelOptions parallelOptions = new(){ MaxDegreeOfParallelism = Environment.ProcessorCount - 1};
       var resultBag = new ConcurrentBag<IEnumerable<Node>>();
       await Parallel.ForEachAsync(chunkIds, parallelOptions, async (idLists, _) =>
       {
           var nodes = await GetDocumentInformationsByIds(idLists);
           if(nodes.Any())
               resultBag.Add(nodes);
       });
       result = resultBag.SelectMany(n => n)
                       .ToDictionary(k => k.id, v => v);
       return result;  
   }
