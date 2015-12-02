using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ExchangeRateWinService
{
    /// <summary>
    /// used to consume the external web service for the exchange rate information
    /// </summary>
    internal class ExternalServiceConsumer
    {
        /// <summary>
        /// Returns Euro value for a 1 GBP/£
        /// </summary>
        internal static decimal? GetEuroExchangeRateValue()
        {
            //string xmlResult = null;
            //decimal? euroExchange = null;
            //string url = CSVReader.ConfigCsv.EuroUrl;
            //try
            //{
            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //    StreamReader resStream = new StreamReader(response.GetResponseStream());
            //    XmlDocument doc = new XmlDocument();
            //    xmlResult = resStream.ReadToEnd();
            //    doc.LoadXml(xmlResult);
            //    euroExchange = Convert.ToDecimal(doc.GetElementsByTagName("double").Item(0).InnerText);
            //    Logger.LogInfo("Euro exchange rate received");
            //}
            //catch (Exception exce)
            //{
            //    Logger.LogExceptions("Exception - Euro exchange rate consumption issue", exce);
            //    throw exce;
            //}
            //return euroExchange;

            string result = null;
            decimal? euroExchange = null;
            // old - do not work - http://www.webservicex.net/CurrencyConvertor.asmx/ConversionRate?FromCurrency=GBP&ToCurrency=USD
            // new - works - "http://currencies.apps.grandtrunk.net/getlatest/gbp/usd";  // http://currencies.apps.grandtrunk.net/getlatest/gbp/eur 
            string url = CSVReader.ConfigCsv.EuroUrl;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader resStream = new StreamReader(response.GetResponseStream());
                result = resStream.ReadToEnd();              
                euroExchange = Convert.ToDecimal(result);
                if (euroExchange == null || euroExchange.ToString() == string.Empty || euroExchange.ToString() == "0.0" || euroExchange.ToString() == "0")
                {
                    Logger.LogWarn(string.Format("Unusual euro exchange rate value {0} at {1}", euroExchange, DateTime.Now));
                    new Emailer().InformViaEmail("BCMY Exchange Rate Windows Service - Unusual euro value", string.Format("Unusual euro exchange rate value {0} at {1}", euroExchange, DateTime.Now), null, null);
                }
                else {
                    Logger.LogInfo("Euro exchange rate received");
                }                
            }
            catch (Exception exce)
            {
                Logger.LogExceptions("Exception - Euro exchange rate consumption issue", exce);
                new Emailer().InformViaEmail("BCMY Exchange Rate Windows Service - Error", string.Format("Error occured when cosuming euro exchange rate at {0}", DateTime.Now), exce.Source, exce.InnerException.Message);
                throw exce;
            }
            return euroExchange;
        }

        /// <summary>
        /// Returns USD/$ value for a 1 GBP/£
        /// </summary>
        internal static decimal? GetUSDExchangeRateValue()
        {
            //string xmlResult = null;
            //decimal? usdExchange = null;
            //string url = CSVReader.ConfigCsv.UsdUrl;
            //try
            //{
            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //    StreamReader resStream = new StreamReader(response.GetResponseStream());
            //    XmlDocument doc = new XmlDocument();
            //    xmlResult = resStream.ReadToEnd();
            //    doc.LoadXml(xmlResult);
            //    usdExchange = Convert.ToDecimal(doc.GetElementsByTagName("double").Item(0).InnerText);
            //    Logger.LogInfo("USD exchange rate received");
            //}
            //catch (Exception exce)
            //{
            //    Logger.LogExceptions("Exception - USD exchange rate consumption issue", exce);
            //    throw exce;
            //}
            //return usdExchange;

            string result = null;
            decimal? usdExchange = null;
            // old - do not work - http://www.webservicex.net/CurrencyConvertor.asmx/ConversionRate?FromCurrency=GBP&ToCurrency=USD
            // new - works - "http://currencies.apps.grandtrunk.net/getlatest/gbp/usd";  // http://currencies.apps.grandtrunk.net/getlatest/gbp/eur 
            string url = CSVReader.ConfigCsv.UsdUrl; 
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader resStream = new StreamReader(response.GetResponseStream());                
                result = resStream.ReadToEnd();
                usdExchange = Convert.ToDecimal(result);
                if (usdExchange == null || usdExchange.ToString() == string.Empty || usdExchange.ToString() == "0.0" || usdExchange.ToString() == "0")
                {
                    Logger.LogWarn(string.Format("Unusual usd exchange rate value {0} at {1}", usdExchange, DateTime.Now));
                    new Emailer().InformViaEmail("BCMY Exchange Rate Windows Service - Unusual usd value", string.Format("Unusual usd exchange rate value {0} at {1}", usdExchange, DateTime.Now), null, null);
                }
                else
                {
                    Logger.LogInfo("USD exchange rate received");
                }               
            }
            catch (Exception exce)
            {
                Logger.LogExceptions("Exception - USD exchange rate consumption issue", exce);
                new Emailer().InformViaEmail("BCMY Exchange Rate Windows Service - Error", string.Format("Error occured when cosuming euro exchange rate at {0}", DateTime.Now), exce.Source, exce.InnerException.Message);
                throw exce;
            }
            return usdExchange;
        }

    }
}
