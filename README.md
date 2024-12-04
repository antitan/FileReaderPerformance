 public class ClassMatcherContentPartDataModelFactory : IClassMatcherContentPartDataModelFactory
 {
     private ConcurrentDictionary<Type, object> dicInstanceObject = new ConcurrentDictionary<Type, object>();
     private ConcurrentDictionary<Type, int> dicNbPropertiesByType = new ConcurrentDictionary<Type, int>();
     public ClassMatcherContentPartDataModelFactory()
     {
         InitializeCache();
     }

     private void InitializeCache()
     {
         //find all classes who implement IClassMatcher 
         Assembly assembly = Assembly.GetExecutingAssembly();
         var typesWithMatchingInterface = assembly.GetTypes()
             .Where(type => type.IsClass && !type.IsAbstract)
             .Where(type => type.GetInterfaces().Any(i =>
                 i.IsGenericType &&
                 i.GetGenericTypeDefinition() == typeof(IClassMatcherContentPartDataModel<>) &&
                 typeof(IContentPartDataModel).IsAssignableFrom(i.GenericTypeArguments[0])
             ));

         //and cache them
         if (typesWithMatchingInterface != null && typesWithMatchingInterface.Any())
         {
             foreach (var type in typesWithMatchingInterface)
             {
                 dicInstanceObject[type] = Activator.CreateInstance(type)!;
                 PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                 dicNbPropertiesByType[type] = (properties != null && properties.Any()) ? properties.Length : 0;
             }
         }
     }
 }
