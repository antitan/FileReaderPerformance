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
