  public IEnumerable<Type>? GetMatchedClasses(JObject jo)
  {
      var results = new ConcurrentBag<(Type type, int nbMatchedProperties)>();

      int nbMatchedProps = 0;

      Parallel.ForEach(dicInstanceObject, kvp =>
      {
          //ici j'aimerai ecrire nbMatchedProps = 0 mais cela ne fonctionne pas
          nbMatchedProps = 0;
          foreach (var prop in  dicTypeJsonProperties[kvp.Key])
          {
              if (jo[prop] != null)
              Interlocked.Increment(ref nbMatchedProps); 
          }
          results.Add((kvp.Key, nbMatchedProps));      
      });

      return results?
          .OrderByDescending(o => o.nbMatchedProperties)
          .Select(t => t.type);
  }
