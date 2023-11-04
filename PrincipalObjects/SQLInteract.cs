using PrincipalObjects.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PrincipalObjects
{
    public class SQLInteract
    {
        public static SqlConnection CreateConnection()
        {
            ConnectionObject connectionData = new ConnectionObject().GetConnection();

            SqlConnection connection = new SqlConnection("server= " + connectionData.Instance + " ; database= " + connectionData.Database + " ; integrated security= " + (connectionData.Security ? "true" : "false") + " ; user=" + connectionData.User + "; password=" + connectionData.Password);
            return connection;
        }

        public static dynamic GetDataFromDataBase((bool, int) requireTop, string[] colsNames, string tableName, (bool, string[]) useFilter, (bool, string, bool) useOrderBy)
        {
            string query = "select ";

            if (requireTop.Item1)
                query = query + "top " + requireTop.Item2.ToString() + " ";

            foreach (string colName in colsNames)
            {
                query = query + colName + ",";
            }
            query = query.TrimEnd(',');
            query = query + " from " + tableName + " ";

            if (useFilter.Item1)
            {
                foreach (string filter in useFilter.Item2)
                {
                    query = query + filter + " and";
                }
                query = query.TrimEnd('d');
                query = query.TrimEnd('n');
                query = query.TrimEnd('a');
            }

            if (useOrderBy.Item1)
            {
                query = query + " order by " + useOrderBy.Item2 + (useOrderBy.Item3 == false? "" : " DESC");
            }

            SqlConnection connection = CreateConnection();
            connection.Open();

            SqlCommand comando = new SqlCommand(query, connection);
            SqlDataReader reader = comando.ExecuteReader();

            string toJson = "{\"rows\":[";

            while (reader.Read())
            {
                toJson = toJson + "{";
                for (int i = 0; i < colsNames.Length; i++)
                {
                    toJson = toJson + "\"" + colsNames[i] + "\":\"" + reader[i] + "\",";
                }
                toJson = toJson.TrimEnd(',');
                toJson = toJson + "},";
            }
            toJson = toJson.TrimEnd(',');
            toJson = toJson + "]}";
            connection.Close();

            return Utilities.ConvertToDynamic(toJson);
        }

        public static dynamic GetDataFromDataBase_Special(string initialQuery, string[] colsNames)
        {
            string query = initialQuery;

            SqlConnection connection = CreateConnection();
            connection.Open();

            SqlCommand comando = new SqlCommand(query, connection);
            SqlDataReader reader = comando.ExecuteReader();

            string toJson = "{\"rows\":[";

            while (reader.Read())
            {
                toJson = toJson + "{";
                for (int i = 0; i < colsNames.Length; i++)
                {
                    toJson = toJson + "\"" + colsNames[i] + "\":\"" + reader[i] + "\",";
                }
                toJson = toJson.TrimEnd(',');
                toJson = toJson + "},";
            }
            toJson = toJson.TrimEnd(',');
            toJson = toJson + "]}";
            connection.Close();

            return Utilities.ConvertToDynamic(toJson);
        }

        public static bool UpdateDataInDataBase(string tableName, List<(string, string, Enums.eDataType)> colsAndValuesAndTypes, (bool, string[]) useFilter)
        {
            string query = "update " + tableName + " set ";// " user_Name = 'admin',user_Password = 'admin' where user_Id = 1";

            foreach ((string, string, Enums.eDataType) data in colsAndValuesAndTypes)
            {
                query = query + data.Item1 + " = ";

                switch (data.Item3)
                {
                    case Enums.eDataType.number: query = query + data.Item2 + ","; break;
                    case Enums.eDataType.text: query = query + "'" + data.Item2 + "',"; break;
                    default: query = query + "'" + data.Item2 + "',"; break;
                }
            }
            query = query.TrimEnd(',');
            query = query + " where ";

            if (useFilter.Item1)
            {
                foreach (string filter in useFilter.Item2)
                {
                    query = query + filter + " and";
                }
                query = query.TrimEnd('d');
                query = query.TrimEnd('n');
                query = query.TrimEnd('a');
            }

            SqlConnection connection = CreateConnection();
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            int i = command.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
                return true;
            else
                return false;
        }

        public static bool InsertDataInDatabase(string tableName, string[] colNames, List<(string, Enums.eDataType)> valuesAndDataType)
        {
            string query = "insert into " + tableName + " (";//

            foreach (string col in colNames)
            {
                query = query + "[" + col + "],";
            }
            query = query.TrimEnd(',');
            query = query + ") values(";

            foreach ((string, Enums.eDataType) data in valuesAndDataType)
            {
                switch (data.Item2)
                {
                    case Enums.eDataType.number: query = query + data.Item1 + ","; break;
                    case Enums.eDataType.text: query = query + "'" + data.Item1 + "',"; break;
                    default: query = query + "'" + data.Item1 + "',"; break;
                }
            }
            query = query.TrimEnd(',');
            query = query + ")";

            SqlConnection connection = CreateConnection();
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            int i = command.ExecuteNonQuery();
            connection.Close();

            if (i > 0)
                return true;
            else
                return false;
        }

        public static bool DeleteDataInDatabase(string tableName, (bool, string[]) useFilter)
        {
            string query = "delete " + tableName;//

            if (useFilter.Item1)
            {
                query = query + " where";
                foreach (string col in useFilter.Item2)
                {
                    query = query + " " + col + " and";
                }
                query = query.TrimEnd('d');
                query = query.TrimEnd('n');
                query = query.TrimEnd('a');
            }

            SqlConnection connection = CreateConnection();
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            int i = command.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
                return true;
            else
                return false;
        }

        public static long GetLastIdFromInsertedElement(string tableName, string colName)
        {
            string query = "select top 1 " + colName + " from " + tableName + " order by " + colName + " desc";//

            SqlConnection connection = CreateConnection();
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            long i = 0;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    i = Convert.ToInt64(reader[colName].ToString());
                }
            }
            connection.Close();
            return i;
        }
    }
}
