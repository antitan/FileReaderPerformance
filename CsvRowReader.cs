using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReaderPerformance
{
    public interface IRowReader
    {
        BlumbergRowFile ReadRow(string line);
    }

    public class CsvRowReader : IRowReader
    {
        public BlumbergRowFile ReadRow(string line)
        {
            if(!line.Contains(";"))
            {
                throw new FormatException($"line {line} has bad format");
            }
            var tokens = line.Split(';');
            if(tokens.Length != 3 ) 
            {
                throw new FormatException($"line {line} has bad format");
            }
            
            string isinCode = tokens[0];
            double openPosition = -1;
            var isParsed = Double.TryParse(tokens[1], out openPosition);
            if(!isParsed ) {
                throw new FormatException($"open position has bad format in line : {line} ");
            }
            double closePosition = -1;
            isParsed = Double.TryParse(tokens[2], out closePosition);
            if (!isParsed)
            {
                throw new FormatException($"close position has bad format in line : {line} ");
            }
            return new BlumbergRowFile
            {
                IsinCode = isinCode,
                ClosePosition = closePosition,
                OpenPosition = openPosition
            };
        }
    }
}
