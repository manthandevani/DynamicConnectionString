using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DynamicConnectionString
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = @".\SQLExpress";
            var database = "demo";
            var username = "sa";
            var password = "sql@2014";
            string connectionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", server, database, username, password);
            Console.WriteLine($"Connection String : {connectionString}");
            Console.WriteLine("Trying to connect..");
            try
            {
                SqlHelper helper = new SqlHelper(connectionString);
                if (helper.IsConnection)
                {
                    Console.WriteLine("Test Connection Succeeded.");
                    Console.WriteLine("Press any key to save this new connection string");
                    Console.ReadLine();
                    Console.WriteLine("Saving your connection string");
                    AppSetting setting = new AppSetting();
                    setting.SaveConnectionString("cn", connectionString);
                    Console.WriteLine("Your connection string has been successfully saved... Press any key to read user data");
                    Console.ReadLine();
                    using (SqlConnection connection = new SqlConnection(setting.GetConnectionString("cn")))
                    {
                        connection.Open();
                        using (SqlCommand cmd = new SqlCommand("Select * from Users", connection))
                        {
                            using(SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while(reader.Read())
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        Console.WriteLine(reader.GetValue(i));
                                    }
                                    Console.WriteLine();
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
