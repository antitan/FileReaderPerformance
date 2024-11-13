  public static IEnumerable<(string imagePath, string label)> LoadImagesFromDirectory(
     string folder,
     bool useFolderNameasLabel)
  {
      var imagesPath = Directory
          .GetFiles(folder, "*", searchOption: SearchOption.AllDirectories)
             .Where(x => FuncExtensions.FuncImageExtensionsSupported(x));

      return useFolderNameasLabel
          ? imagesPath.Select(imagePath => (imagePath, Directory.GetParent(imagePath).Name))
          : imagesPath.Select(imagePath =>
          {
              var label = Path.GetFileName(imagePath);
              for (var index = 0; index < label.Length; index++)
              {
                  if (!char.IsLetter(label[index]))
                  {
                      label = label.Substring(0, index);
                      break;
                  }
              }
              return (imagePath, label);
          });
  }
