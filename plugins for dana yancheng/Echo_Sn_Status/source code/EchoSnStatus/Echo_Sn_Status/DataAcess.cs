using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;


namespace EchoSnStatus
{
    public class DataAcess
    {

        public void UpdateSNandProStauts(string connectString, int proId, string sn, int status)
        {
            string updateSN = string.Empty;
            string updatePRO = string.Empty;
            if (status == 0)
            {
                updateSN = "update SN set Status = 'In progress' where PRO_Id ="+ proId.ToString() + " and SN like \'%" + sn.Trim()+"%\'";

            }
            else
            {
                updateSN = "update SN set Status = 'Completed' where PRO_Id =" + proId.ToString() + " and SN like \'%" + sn.Trim() + "%\'";
                string quantity = GetQuantityByProId(connectString, proId);
                if(int.Parse(quantity)==int.Parse(sn.Substring(6,4)))
                {
                    updatePRO = "update PRO set Status = 'Completed' where Id =" + proId.ToString() ;
                }

            }
            

            try
            {
                SqlConnection myConnection = new SqlConnection(connectString);
                myConnection.Open();
                SqlCommand MyCommand = new SqlCommand(updateSN, myConnection);
                MyCommand.ExecuteNonQuery();
                if(updatePRO!=string.Empty)
                {
                    MyCommand = new SqlCommand(updatePRO, myConnection);
                    MyCommand.ExecuteNonQuery();

                }
                myConnection.Close();
            }
            catch (Exception ex)
            {

            }

        }


        public string GetQuantityByProId(string connectString,int proId)
        {

            using (IDbConnection db = new SqlConnection(connectString))
            {
                string query = " SELECT TotalQuantity FROM PRO where Id =" + proId.ToString();
                List<string> totalQuantitys = (List<string>)db.Query<string>(query);
                string totalQuantity = totalQuantitys.FirstOrDefault().ToString();
                return totalQuantity;

            }

        }


    }
}
