 public interface IClassMatcherContentPartDataModel<T> where T : IContentPartDataModel
 {
      bool CheckMatchedClass(JObject jo);
 }
 
  public interface IClassMatcherContentPartDataModelService
 {
     IEnumerable<Type>? GetMatchedClasses(JObject jo);
 }

 public class ClassMatcherContentPartDataModelService : IClassMatcherContentPartDataModelService
 {
     private ConcurrentDictionary<Type, object> dicInstanceObject = new ConcurrentDictionary<Type, object>();
     private ConcurrentDictionary<Type, int> dicNbPropertiesByType = new ConcurrentDictionary<Type, int>();
     public ClassMatcherContentPartDataModelService()
     {
         InitializeDictionnaries();
     }

     private void InitializeDictionnaries()
     {
         Assembly assembly = Assembly.GetExecutingAssembly();
         var typesWithMatchingInterface = assembly.GetTypes()
             .Where(type => type.IsClass && !type.IsAbstract)
             .Where(type => type.GetInterfaces().Any(i =>
                 i.IsGenericType &&
                 i.GetGenericTypeDefinition() == typeof(IClassMatcherContentPartDataModel<>) &&
                 typeof(IContentPartDataModel).IsAssignableFrom(i.GenericTypeArguments[0])
             )).ToList();  
          
         Parallel.ForEach(typesWithMatchingInterface, type =>
         {
             dicInstanceObject[type] = Activator.CreateInstance(type)!;
             PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
             dicNbPropertiesByType[type] = (properties != null && properties.Any()) ? properties.Length : 0;
         });
     }


     public IEnumerable<Type>? GetMatchedClasses(JObject jo)
     {
         var results = new ConcurrentBag<(Type type,int nbProperties)>();
         Parallel.ForEach(dicInstanceObject, kvp =>
         {
             if (kvp.Key.GetInterfaces().Any(i =>
                 i.IsGenericType &&
                 i.GetGenericTypeDefinition() == typeof(IClassMatcherContentPartDataModel<>) &&
                 i.GenericTypeArguments[0] == typeof(T)))
             {
                 var instance = (IClassMatcherContentPartDataModel<T>)kvp.Value;
                 var result = instance.CheckMatchedClass(jo);
                 if (result)
                 {
                     int nbProperties = dicNbPropertiesByType[kvp.Key];
                     var newItem = (instance.GetType(), nbProperties);
                     results.Add(newItem);
                 }
             }
         });

         return results?
                .OrderByDescending(o=>o.nbProperties)?
                .Select(t=>t.type);
     }

 }
