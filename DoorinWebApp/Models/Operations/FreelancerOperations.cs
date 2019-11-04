using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace DoorinWebApp.Models.Operations
{
    public class FreelancerOperations
    {
        public freelancer GetFreelancerById(int? id)
        {
            freelancer freelanc = new freelancer();
            FreelancerProfileOperations fpop = new FreelancerProfileOperations();

            string sql = "SELECT freelancer_id, firstname, lastname, address, city, zipcode, phonenumber, email, birthdate, birthcity, nationality, username, password from freelancer WHERE freelancer_id = @freelancer_id";

            using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("freelancer_id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            freelanc.freelancer_id = (int)reader["freelancer_id"];
                            freelanc.firstname = (string)reader["firstname"];
                            freelanc.lastname = (string)reader["lastname"];
                            freelanc.address = (string)reader["address"];
                            freelanc.city = (string)reader["city"];
                            freelanc.zipcode = (string)reader["zipcode"];
                            freelanc.phonenumber = (string)reader["phonenumber"];
                            freelanc.email = (string)reader["email"];
                            freelanc.birthdate = (DateTime)reader["birthdate"];
                            freelanc.birthcity = (string)reader["birthcity"];
                            freelanc.nationality = (string)reader["nationality"];
                            freelanc.username = (string)reader["username"];
                            freelanc.password = (string)reader["password"];
                        }
                    }
                }
                freelanc.FreelancerProfileResume = fpop.GetFreelancerProfileById(id);
            }
            return freelanc;
        }


        private SqlConnectionStringBuilder GetBuilder() //Anropa vid användning för connection mot databasen
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "doorin.database.windows.net";
            builder.UserID = "doorinadmin";
            builder.Password = "Secretpassword123!";
            builder.InitialCatalog = "doorinDB";

            return builder;
        }
    }
}