 public static void ZipFile(string inputFilePath, string outputTextFilePath)
 {
     // Lire le fichier d'entrée et obtenir ses octets
     byte[] fileBytes = File.ReadAllBytes(inputFilePath);

     // Compresser les octets en utilisant GZipStream
     using (var compressedStream = new MemoryStream())
     {
         using (var gzipStream = new GZipStream(compressedStream, CompressionLevel.Optimal, leaveOpen: true))
         {
             gzipStream.Write(fileBytes, 0, fileBytes.Length);
         }
         // Convertir les octets compressés en chaîne base64
         string compressedBase64 = Convert.ToBase64String(compressedStream.ToArray());

         // Écrire la chaîne compressée dans le fichier texte spécifié
         File.WriteAllText(outputTextFilePath, compressedBase64);
     }
 }

 public static void UnzipFile(string compressedTextFilePath, string outputFilePath)
 {
     // Lire la chaîne compressée depuis le fichier texte
     string compressedBase64 = File.ReadAllText(compressedTextFilePath);

     // Convertir la chaîne base64 en octets compressés
     byte[] compressedBytes = Convert.FromBase64String(compressedBase64);

     // Décompresser les octets en utilisant GZipStream
     using (var compressedStream = new MemoryStream(compressedBytes))
     using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
     using (var outputFileStream = new FileStream(outputFilePath, FileMode.Create))
     {
         gzipStream.CopyTo(outputFileStream);
     }
 }
