using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace PROTraveller
{
    public class DataAccess
    {

        public DataSet GetProDb(string line, string proNumber, string status)
        {
            using (SqlConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["PROTravellerConnectionString"].ConnectionString))
            {
                string select = " SELECT Id,Line,Year,OrderNumber,TotalQuantity,Model,Status,StartDate,EndDate,Description,CreateTime FROM PRO";
                string where = " where ";
                string con1 = " line like " + " \'%"+line+"%\' and ";
                string con2 = " status like " + " \'%" + status + "%\'";
                string con3 = " and OrderNumber like " + " \'%" + proNumber + "%\'";
                string query = select + where + con1 + con2 + con3;
                string sql = String.Format(query);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sql,db);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return dataSet;

            }

        }

        public void InsertResult(PROTravellerModel result)
        {

            string insert1 = "insert into PRO(Line,Year,OrderNumber,TotalQuantity,Model,StartDate,EndDate,Description,CreateTime,Status) values (";
            string insert2 = " \'" + result.Line + "\' ,";
            string insert3 = " \'" + result.Year + "\',\'" + result.OrderNumber + "\',\'" + result.TotalQuantity + "\',\'" + result.Model + "\',\'" + result.StartDate + " \',";
            string insert4 = " \'" + result.EndDate + "\',\'" + result.Description + "\',\'" + result.CreateTime + "\',\'" + result.Status + "\' )";
            string insert = insert1 + insert2 + insert3 + insert4;
            try
            {
                SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PROTravellerConnectionString"].ConnectionString);
                myConnection.Open();
                SqlCommand MyCommand = new SqlCommand(insert, myConnection);
                MyCommand.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Update(string id)
        {
            string update = "update PRO set status = 'In progress' where id = " + id;
           
            try
            {
                SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PROTravellerConnectionString"].ConnectionString);
                myConnection.Open();
                SqlCommand MyCommand = new SqlCommand(update, myConnection);
                MyCommand.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception ex)
            {

            }

        }

        public DataSet GetSNbyProId(string proId)
        {
            using (SqlConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["PROTravellerConnectionString"].ConnectionString))
            {
                string query = " SELECT SN,Status,OrderNumber,Model,LatestModifyTime FROM SN where pro_id = "+ proId + " order by SN ";
                string sql = String.Format(query);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, db);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return dataSet;

            }

        }

        public void InsertSN(SNModel sn)
        {

            string insert1 = "insert into SN(PRO_Id,SN,Status,LatestModifyTime,OrderNumber,Model,Line,Description,JobNumber,Reserved4,Reserved3) values (";
            string insert2 = " \'" + sn.Pro_Id+ "\' ,";
            string insert3 = " \'" + sn.SN + "\',\'" + sn.Status + "\',\'" + sn.LatestModifyTime+ "\',";
            string insert4 = " \'" + sn.OrderNumber + "\',\'" + sn.Model + "\',\'" + sn.Line + "\',\'" + sn.Description + "\',\'" + sn.JobNumber + "\',\'" + sn.Reserved4 + "\',\'" + sn.Reserved3 + "\' )";
            string insert = insert1 + insert2 + insert3 + insert4;
            try
            {
                SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PROTravellerConnectionString"].ConnectionString);
                myConnection.Open();
                SqlCommand MyCommand = new SqlCommand(insert, myConnection);
                MyCommand.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception ex)
            {

            }

        }

        public string GetYearMonthCode(string year ,string month)
        {
  

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["PROTravellerConnectionString"].ConnectionString))
            {
                string query = " SELECT [" + month +"] FROM YearMonthCodeList where Year like \'%" + year + "%\'";
                List<string> codes = (List<string>)db.Query<string>(query);
                string code = codes.FirstOrDefault().ToString();
                return code;

            }

        }

        public List<SNModel> GetSnItemsByProId(int proId)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["PROTravellerConnectionString"].ConnectionString))
            {
                string query = "SELECT * FROM SN where PRO_Id = " + proId;
                List<SNModel> SNItems = (List<SNModel>)db.Query<string>(query);
                return SNItems;

            }

        }
    }

}

