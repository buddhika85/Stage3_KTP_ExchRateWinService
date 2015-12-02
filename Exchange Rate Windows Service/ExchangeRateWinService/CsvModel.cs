using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateWinService
{
    // used to model the CSV file structure/columns
    internal class CsvModel
    {
        [CsvColumn(Name = "usd")]
        public string UsdUrl { get; set; }

        [CsvColumn(Name = "euro")]
        public string EuroUrl { get; set; }

        [CsvColumn(Name = "refreshRate")]
        public int TimerRefreshRate { get; set; }

        [CsvColumn(Name = "senderEmail")]
        public string SenderEmail { get; set; }

        [CsvColumn(Name = "receiverEmail")]
        public string ReceviverEmail { get; set; }

        [CsvColumn(Name = "databaseserverIP")]
        public string DbServerIp { get; set; }

        [CsvColumn(Name = "port")]
        public string DbPort { get; set; }

        [CsvColumn(Name = "dbServerUsername")]
        public string DbUser { get; set; }

        [CsvColumn(Name = "dbServerPassword")]
        public string DbSecret { get; set; }

        [CsvColumn(Name = "dbName")]
        public string DbName { get; set; }

        [CsvColumn(Name = "SenderEmailPassword")]
        public string SenderEmailPassword { get; set; }
    }
}
