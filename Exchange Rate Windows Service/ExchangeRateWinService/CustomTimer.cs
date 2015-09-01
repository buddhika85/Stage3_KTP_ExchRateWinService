using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateWinService
{
    // The timer - used to raise the event of the consumption of the web service with in intervals
    internal static class CustomTimer
    {

        private static System.Timers.Timer aTimer;                      // timer to manage executions in specified intervals
        internal delegate decimal? ConsumeExchangeRateService();        // delegate

        // Ref - 25/08/2015 - https://msdn.microsoft.com/en-us/library/system.timers.timer(v=vs.110).aspx
        internal static void ConsumeWebServiceRecurringly()
        {
            try
            {
                // Normally, the timer is declared at the class level, so that it stays in scope as long as it 
                // is needed. If the timer is declared in a long-running method, KeepAlive must be used to prevent 
                // the JIT compiler from allowing aggressive garbage collection to occur before the method ends. 
                // You can experiment with this by commenting out the class-level declaration and uncommenting  
                // the declaration below; then uncomment the GC.KeepAlive(aTimer) at the end of the method.         
                //System.Timers.Timer aTimer; 

                // Create a timer and set a two second interval.
                aTimer = new System.Timers.Timer();
                //aTimer.Interval = 2000;

                // Alternate method: create a Timer with an interval argument to the constructor. 
                //aTimer = new System.Timers.Timer(2000); 

                // Create a timer with a two second interval.
                aTimer = new System.Timers.Timer(10000);

                // Hook up the Elapsed event for the timer. 
                aTimer.Elapsed += OnTimedEvent;

                // Have the timer fire repeated events (true is the default)
                aTimer.AutoReset = true;

                // Start the timer
                aTimer.Enabled = true;


                // If the timer is declared in a long-running method, use KeepAlive to prevent garbage collection 
                // from occurring before the method ends.  
                //GC.KeepAlive(aTimer) 

                Logger.LogDebug(string.Format("Exchange rates successfully inserted/updated at {0} ", DateTime.Now));
            }
            catch (Exception exc)
            {
                Logger.LogExceptions("Exception - Recurring consumption of the extrenal service failed", exc);
                throw;
            }
        }


        // <summary>
        /// Event raised from the Timer obj
        /// </summary>
        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                // 2 instances of delegates for euro and usd
                // Ref - http://stackoverflow.com/questions/6045343/how-to-make-an-asynchronous-method-return-a-value
                ConsumeExchangeRateService usdDelegator = ExternalServiceConsumer.GetUSDExchangeRateValue;
                ConsumeExchangeRateService euroDelegator = ExternalServiceConsumer.GetEuroExchangeRateValue;

                // beging invoke and end invoke - accessing the serive async manner                
                IAsyncResult asyncResultUsd = usdDelegator.BeginInvoke(null, null);     // first thread
                IAsyncResult asyncResultEuro = euroDelegator.BeginInvoke(null, null);   // second thread

                // do other work
                // ..
                // .

                // blocking storing results in the database untill we actually gets the results
                asyncResultUsd.AsyncWaitHandle.WaitOne();
                asyncResultEuro.AsyncWaitHandle.WaitOne();

                Logger.LogDebug("Async access to USD and Euro exchange rate values complete");

                decimal? usdValue = usdDelegator.EndInvoke(asyncResultUsd);
                decimal? euroValue = euroDelegator.EndInvoke(asyncResultEuro);

                DataAccess.InsertUpdateExchangeRates(usdValue, euroValue);
                Logger.LogDebug(string.Format("Exchange rates successfully inserted/updated at {0} ", DateTime.Now));
            }
            catch (Exception exce)
            {
                Logger.LogExceptions("Exception - Recurring consumption of the extrenal service failed", exce);
                Console.WriteLine("Time {0} - Exception thrown : {1} \n{2} \n{3}", e.SignalTime, exce.Message, exce.Source, exce.InnerException.Data);
            }
        }
    }
}
