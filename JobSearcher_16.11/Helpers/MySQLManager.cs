using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace JobSearcher_16._11.Helpers
{
    public class MySQLManager
    {
        public string connect { get; set; }
        private MySqlConnection connection;
        private MySql.Data.MySqlClient.MySqlCommand cmd;
        MySql.Data.MySqlClient.MySqlConnection conn;
        string myConnectionString;


        public MySQLManager()
        {
            connection = new MySqlConnection();


            myConnectionString = "server=localhost;uid=root;" +
                                "pwd=\"\";database=jobs;AllowZeroDateTime=true ";
            // createJobnetTable();

        }

        private void openConnection()
        {
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                throw (ex);
            }
        }

        private void closeConnection()
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private void runQuery(string i_SQLstr)
        {
            openConnection();

            try
            {
                cmd = new MySql.Data.MySqlClient.MySqlCommand(i_SQLstr, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }



        }

    

        public void insertJobToTable(string i_table_name, string i_JobID)
        {
            string JobID = i_JobID;
            DateTime currentDate = DateTime.Now;
            string formatForMySql = currentDate.ToString("yyyy-MM-dd HH:mm");

          //  using(
         //   var testlinq = from v
            string strSQL = "INSERT INTO " + i_table_name + " ( JobID,isrelevant)" +
                        "VALUES" +
                        "('" + JobID + "',1 ) ON DUPLICATE key UPDATE LastSent = NOW();";
            /*
             * INSERT INTO jobnet ( JobID) VALUES ("12345") ON DUPLICATE key UPDATE LastSent = NOW()
             */
            try
            {
                runQuery(strSQL);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public void insertJobViewedToTable(string i_table_name, string i_JobID)
        {
            string JobID = i_JobID;
            DateTime currentDate = DateTime.Now;
            string formatForMySql = currentDate.ToString("yyyy-MM-dd HH:mm");
            string strSQL = "iNSERT INTO " + i_table_name + " ( JobID, lastsent,isrelevant) VALUES ('" + JobID + "',0,1) ON DUPLICATE key UPDATE LastSent = 0";

            try
            {
                runQuery(strSQL);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }


        public void changeDate(string i_table_name, string i_JobID, string i_day, string i_month)
        {
            string JobID = i_JobID;
            // DateTime currentDate = DateTime.Now;
            //string formatForMySql = currentDate.ToString("yyyy-MM-dd HH:mm");
            // string strSQL = "update " + i_table_name + "   set lastsent='2014-" + i_month + "-" + i_day + " 00:00:01' WHERE JobID ='" + JobID +"'";
            string strSQL = "iNSERT INTO " + i_table_name + " ( JobID, lastsent) VALUES ('" + JobID + "','2014-" + i_month + "-" + i_day + " 00:00:01',1) ON DUPLICATE key UPDATE LastSent='2014-" + i_month + "-" + i_day + " 00:00:01'";
            try
            {
                if ((i_day != null) && (i_month != null))
                {
                    runQuery(strSQL);
                }

            }

            catch (Exception e)
            {
                throw (e);
            }

        }

        public void changeJobtoIrrelevat(string i_table_name, string i_JobID)
        {
            string JobID = i_JobID;
            string strSQL = "iNSERT INTO " + i_table_name + " ( JobID, lastsent,isrelevant) VALUES ('" + JobID + "',0,2) ON DUPLICATE key UPDATE isrelevant = 2";
            try
            {
                runQuery(strSQL);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public List<string> CheckJob(string i_table_name, string i_JobID)
        {
            string JobID = i_JobID;
            DateTime currentDate = DateTime.Now;
            DateTime dateOnly = currentDate.Date;
            string strSQL = "SELECT LastSent,isrelevant FROM " + i_table_name + " where JobId= '" + i_JobID + "' ";
            try
            {
                List<string> JobsRead = readFromQuery(strSQL);
                if (JobsRead != null)
                {
                    if (JobsRead.Count == 2)
                    {
                        return JobsRead;
                    }
                    else if (JobsRead.Count == 0)
                    {
                        return null;
                    }

                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);

            }
            return null;
        }


        private List<string> readFromQuery(string i_query)
        {
            openConnection();

            try
            {
                cmd = new MySql.Data.MySqlClient.MySqlCommand(i_query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<string> JobsRead = new List<string>();
                int ColumnsNumber = reader.FieldCount;
                List<String> res = new List<string>();
                while (reader.Read())
                {




                    for (int ColumnIndex = 0; ColumnIndex < ColumnsNumber; ColumnIndex++)
                    {
                        res.Add(reader[ColumnIndex].ToString());
                    }
                }
                return res;
            }

            catch (Exception e)
            {
                throw (e);
            }

            closeConnection();


        }
    }
}
