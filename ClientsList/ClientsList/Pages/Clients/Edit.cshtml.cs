using System.Data.SqlClient;
using ClientsList.Helpers;
using ClientsList.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientsList.Pages.Clients
{
    public class Edit : PageModel
    {
        public ClientModel ClientModel = new ClientModel();
        public string? ErrorMessage = null;
        public string? SuccessMessage  = null;

        public void OnGet()
        {
            var id = Request.Query["id"].ToString();
            try
            {
                ClientModel.Id = Int32.Parse(id);
                using (SqlConnection sqlConnection = new SqlConnection(DbHelper.ConnectionString))
                {
                    sqlConnection.Open();
                    var sqlQuery = "SELECT * FROM Clients WHERE Id=@id";
                    using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            if (sqlDataReader.Read())
                            {
                                ClientModel.Id = sqlDataReader.GetInt32(0);
                                ClientModel.Name = sqlDataReader.GetString(1);
                                ClientModel.Email = sqlDataReader.GetString(2);
                                ClientModel.Phone = sqlDataReader.GetString(3);
                                ClientModel.Address = sqlDataReader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }

        public void OnPost()
        {
            var stringId = Request.Form[nameof(ClientModel.Id)].ToString(); 
            ClientModel.Id = Int32.Parse(stringId);
            ClientModel.Name = Request.Form[nameof(ClientModel.Name)]!;
            ClientModel.Email = Request.Form[nameof(ClientModel.Email)]!;
            ClientModel.Phone = Request.Form[nameof(ClientModel.Phone)]!;
            ClientModel.Address = Request.Form[nameof(ClientModel.Address )]!;

            if (!ClientModel.IsModelValid())
            {
                ErrorMessage = "All fields are required";
                SuccessMessage = null;
                return;
            }

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(DbHelper.ConnectionString))
                {
                    sqlConnection.Open();
                    var sqlQuery = @"UPDATE Clients
                                    SET Name=@name, Email=@email, Phone=@phone, Address=@address
                                    WHERE Id=@id";
                    using (SqlCommand command = new SqlCommand(sqlQuery, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@id", ClientModel.Id);
                        command.Parameters.AddWithValue("@name", ClientModel.Name);
                        command.Parameters.AddWithValue("@email", ClientModel.Email);
                        command.Parameters.AddWithValue("@phone", ClientModel.Phone);
                        command.Parameters.AddWithValue("@address", ClientModel.Address);

                        command.ExecuteNonQuery(); 
                    }
                }
            }
            catch (Exception e)
            {
                SuccessMessage = null;
                ErrorMessage = e.Message;
            }

            Response.Redirect("/Clients/Index"); 
        }
    }
}