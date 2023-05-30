using System.Data.SqlClient;
using ClientsList.Helpers;
using ClientsList.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientsList.Pages.Clients
{
    public class Create : PageModel
    {
        public ClientModel ClientModel = new ClientModel();
        public string? ErrorMessage = null;
        public string? SuccessMessage  = null;
        
        public void OnGet()
        { }

        public void OnPost()
        {
            ClientModel.Name = Request.Form[nameof(ClientModel.Name)]!;
            ClientModel.Email = Request.Form[nameof(ClientModel.Email)]!;
            ClientModel.Phone = Request.Form[nameof(ClientModel.Phone)]!;
            ClientModel.Address = Request.Form[nameof(ClientModel.Address )]!;

            if (!ClientModel.IsModelValid())
            {
                ErrorMessage = "All the fields are required";
                SuccessMessage = null;
                return;
            }
            
            // Save into the database
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(DbHelper.ConnectionString))
                {
                    sqlConnection.Open();
                    string sqlQuery = @"INSERT INTO CLIENTS
                                        (name, email, phone, address)
                                        VALUES (@name, @email, @phone, @address)";

                    using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@name", ClientModel.Name);
                        sqlCommand.Parameters.AddWithValue("@email", ClientModel.Email);
                        sqlCommand.Parameters.AddWithValue("@phone", ClientModel.Phone);
                        sqlCommand.Parameters.AddWithValue("@address", ClientModel.Address);

                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return; 
            }
            ClientModel = new ClientModel();
            ErrorMessage = null;
            SuccessMessage = "New client added correctly";
             
        }
    }
}