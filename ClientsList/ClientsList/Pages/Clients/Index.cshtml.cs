using System.Data.SqlClient;
using ClientsList.Helpers;
using ClientsList.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientsList.Pages.Clients
{
    public class Index : PageModel
    {
        public readonly List<ClientModel> ClientInfo = new List<ClientModel>();
         
        public void OnGet()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(DbHelper.ConnectionString))
                {
                    sqlConnection.Open();
                    string sqlQuery = "SELECT * FROM Clients";
                    using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection) )
                    {
                        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                ClientModel clientModel = new ClientModel()
                                {
                                    Id = sqlDataReader.GetInt32(0),
                                    Name = sqlDataReader.GetString(1),
                                    Email = sqlDataReader.GetString(2),
                                    Phone = sqlDataReader.GetString(3),
                                    Address = sqlDataReader.GetString(4),
                                    CreatedAt = sqlDataReader.GetDateTime(5)
                                };
                                ClientInfo.Add(clientModel);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
        }
    }
}