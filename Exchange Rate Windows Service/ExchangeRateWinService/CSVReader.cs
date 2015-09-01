using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateWinService
{
    // used to read and provide CSV file contents such as web service URLs, refersh rate...etc
    // Ref - http://www.codeproject.com/Articles/25133/LINQ-to-CSV-library
    internal static class CSVReader
    {
        internal static CsvModel ConfigCsv { get; set; }

        // used to read the CSV file and prepare it for the usage with in the windows service
        internal static void PrepareCsvProperties()
        {
            if (ConfigCsv == null || string.IsNullOrWhiteSpace(ConfigCsv.UsdUrl))
            {
                try
                {
                    // reading the CSV file
                    ReadCSV();
                    Logger.LogInfo("config.csv file reading success");
                }
                catch (Exception exc)
                {
                    Logger.LogExceptions("Exception - Reading config.CSV file", exc);
                    throw; // bubble to the caller
                }
            }
        }


        // a helper to read the CSV
        private static void ReadCSV()
        {
            try
            {
                CsvFileDescription inputFileDescription = new CsvFileDescription
                {
                    SeparatorChar = ',',
                    FirstLineHasColumnNames = true
                };
                CsvContext cc = new CsvContext();
                ConfigCsv = cc.Read<CsvModel>(AppDomain.CurrentDomain.BaseDirectory + "\\config.csv", inputFileDescription).FirstOrDefault<CsvModel>();
            }
            catch (Exception exc)
            {
                Logger.LogExceptions("Exception - Reading config.CSV file", exc);
                throw;
            }
        }

    }
}
