using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateWinService
{
    public partial class ExchangeRateWinService : ServiceBase
    {
        public ExchangeRateWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Logger.LogInfo("BCMY Exchange Rate Windows Service Started");
                CSVReader.PrepareCsvProperties();
                CustomTimer.ConsumeWebServiceRecurringly();
            }
            catch (Exception exc)
            {
                Logger.LogExceptions("Exception thrown to the top most caller - check the app code", exc);
                //throw; // no where to throw from here
            }          
        }

        protected override void OnStop()
        {
            try
            {
                Logger.LogInfo("BCMY Exchange Rate Windows Service Stopped");
                CustomTimer.StopRecurringConsumption();
            }
            catch (Exception exc)
            {
                Logger.LogExceptions("Exception thrown to the top most caller - check the app code", exc);
                //throw; // no where to throw from here
            }     
        }

    }
}
