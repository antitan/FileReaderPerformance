using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReaderPerformance
{

    public interface IBlumbergFileReaderService
    {
        Task LoadFile(string csvFile);
        Task<BlumbergRowFile> Read(string isinCode);
    }
    public class BlumbergFileReaderService: IBlumbergFileReaderService
    {
        private IRowReader rowReader;
        private ConcurrentDictionary<string, BlumbergRowFile> dicIsinCodeRowFile;
        public BlumbergFileReaderService(IRowReader rowReader)
        {
            this.rowReader = rowReader;
        }

        public  Task LoadFile(string csvFile)
        {
            if(!File.Exists(csvFile)) 
            {
                throw new FileNotFoundException($"file {csvFile} doesn't exist");
            }
            dicIsinCodeRowFile = new ConcurrentDictionary<string, BlumbergRowFile>();

            int numberOfTasks = Environment.ProcessorCount; 

            var fileInfo = new FileInfo(csvFile);

            long sizePerTask = fileInfo.Length / numberOfTasks;

            Parallel.For(0, numberOfTasks, taskId =>
            {
                long start = taskId * sizePerTask;

                long end = (taskId == numberOfTasks - 1) ? fileInfo.Length : start + sizePerTask;

                using (var stream = new FileStream(csvFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        stream.Seek(start, SeekOrigin.Begin);

                        if (taskId != 0)
                        {
                            reader.ReadLine();
                        }

                        while (stream.Position < end)
                        {
                            string line = reader.ReadLine();
                            if (line != null)
                            {
                                var row = rowReader.ReadRow(line);
                                dicIsinCodeRowFile.TryAdd(row.IsinCode, row);
                            }
                        }
                    }
                }
            });
            return Task.CompletedTask;
        }


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

        public async Task<BlumbergRowFile> Read(string isinCode)
        {
            BlumbergRowFile result;
            if (!dicIsinCodeRowFile.TryGetValue(isinCode, out result))
            {
                throw new KeyNotFoundException($" {isinCode} was not found");
            }
            return result;
        }
         
    }
}
