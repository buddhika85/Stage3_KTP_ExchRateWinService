using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

//Here is the once-per-application setup information
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace ExchangeRateWinService
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Ref - 25/08/2015 - http://www.c-sharpcorner.com/UploadFile/naresh.avari/develop-and-install-a-windows-service-in-C-Sharp/
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new ExchangeRateWinService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
