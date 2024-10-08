﻿using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace CarShowRoom
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CarShowroomConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CustomerId, Name FROM Customer WHERE Email=@Email AND Password=@Password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        Session["CustomerId"] = reader["CustomerId"];
                        Session["CustomerName"] = reader["Name"];
                        Response.Redirect("Home.aspx");
                    }
                    else
                    {
                        // Show alert box if login fails
                        string message = "Invalid email or password.";
                        string script = $"showAlert('{message}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "LoginAlert", script, true);
                    }
                }
            }
        }
    }
}