using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateWinService
{
    /// <summary>
    /// Used to manage the database interactions
    /// </summary>
    internal static class DataAccess
    {
        public static string connectionString { get; set; }

        /// <summary>
        /// Used to Inser/Update exchange rate values with in the database
        /// </summary>
        internal static bool InsertUpdateExchangeRates(decimal? usd, decimal? euro)
        {
            bool wasSuccess = false;
            try
            {
                // call SP to insert or update
                // Ref - http://www.codeproject.com/Articles/748619/ADO-NET-How-to-call-a-stored-procedure-with-output
                connectionString = GetConConnectionString();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // create the SqlCommand object
                    SqlCommand cmd = new SqlCommand("SP_InserUpdateExchangeRates", con);

                    // specify that the SqlCommand is a stored procedure
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // add the input parameters to the command object
                    cmd.Parameters.AddWithValue("@usd", 0.567891);
                    cmd.Parameters.AddWithValue("@euro", 1.66999);

                    // add the output parameter to the command object
                    SqlParameter outPutParameter = new SqlParameter();
                    outPutParameter.ParameterName = "@insertEditStatus";
                    outPutParameter.SqlDbType = System.Data.SqlDbType.VarChar;
                    outPutParameter.Size = 1000;
                    outPutParameter.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(outPutParameter);

                    // open the connection and execute the query
                    con.Open();
                    cmd.ExecuteNonQuery();

                    // retrieve the value of the output parameter
                    string insertEditStatus = outPutParameter.Value.ToString();
                    Logger.LogDebug(string.Format("Output from the Stored Procedure : {0}", insertEditStatus));
                }
                Logger.LogInfo("Exchanges rates updated");
                wasSuccess = true;
            }
            catch (Exception exc)
            {
                wasSuccess = false;
                Logger.LogExceptions("Exception - Insert/Update exchange rates", exc);
                throw;
            }
            return wasSuccess;
        }


        /// <summary>
        /// a helper to get the connection string
        /// </summary>
        private static string GetConConnectionString()
        {
            string connectionString = null;
            try
            {
                // Data Source=77.73.5.221,3306;Initial Catalog=BCMY_Stock;Persist Security Info=True;User ID=sa;Password=***********
                connectionString = string.Format("Data Source={0},{1};Initial Catalog={2};Persist Security Info={3};User ID={4};Password={5}",
                    CSVReader.ConfigCsv.DbServerIp, CSVReader.ConfigCsv.DbPort, CSVReader.ConfigCsv.DbName, "True", CSVReader.ConfigCsv.DbUser, CSVReader.ConfigCsv.DbSecret);
                Logger.LogDebug("Connection string created");
            }
            catch (Exception exc)
            {
                Logger.LogExceptions("Exception - Connection string creation", exc);
                throw;
            }
            return connectionString;
        }

    }
}
