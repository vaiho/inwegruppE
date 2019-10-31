using DoorinWebApp.Viewmodel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DoorinWebApp.Models.Operations
{
    public class CustomerOperations
    {
        public List<FreelancerProfileVM> GetSavedFreelancersList(int? id) //Hämtar en lista av sparade frilansare för en customer
        {
            FreelancerProfileVM fp;
            string sql = "SELECT freelancer.freelancer_id, firstname, lastname, email, nationality from customer_freelancer INNER JOIN freelancer on customer_freelancer.freelancer_id = freelancer.freelancer_id WHERE customer_id = @id";
            List<FreelancerProfileVM> list = new List<FreelancerProfileVM>();


            using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fp = new FreelancerProfileVM()
                            {
                                Freelancer_id = reader.GetInt32(0),
                                Firstname = reader.GetString(1),
                                Lastname = reader.GetString(2),
                                //Resume_id = reader.GetInt32(3),
                                //ProfileText = reader.GetString(4),
                                Email = reader.GetString(3),
                                Nationality = reader.GetString(4),
                                //City = reader.GetString(7),
                                //Birthdate = reader.GetDateTime(8),
                                //Address = reader.GetString(9),
                                //Zipcode = reader.GetString(10),
                                //Username = reader.GetString(11),
                            };
                            list.Add(fp);
                        }
                    }
                }
            }
            return list;
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