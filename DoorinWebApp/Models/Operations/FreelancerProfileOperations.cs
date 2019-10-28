using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DoorinWebApp.Models;
using DoorinWebApp.Viewmodel;

namespace DoorinWebApp.Models.Operations
{
    public class FreelancerProfileOperations
    {
       
        public FreelancerProfileVM GetFreelancerProfileById(int? id)
        {
            FreelancerProfileVM fp = new FreelancerProfileVM();

            string sql = "SELECT freelancer.freelancer_id, firstname, lastname, resume_id, profile from freelancer INNER JOIN resume on freelancer.freelancer_id = resume.freelancer_id WHERE freelancer.freelancer_id = @freelancer_id";
            //string sql = "SELECT * FROM freelancer WHERE freelancer_id = @freelancer_id";

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "doorin.database.windows.net";
            builder.UserID = "doorinadmin";
            builder.Password = "Secretpassword123!";
            builder.InitialCatalog = "doorinDB";


            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("freelancer_id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fp.Freelancer_id = reader.GetInt32(0);
                            fp.Firstname = reader.GetString(1);
                            fp.Lastname = reader.GetString(2);
                            fp.Resume_id = reader.GetInt32(3);
                            fp.ProfileText = reader.GetString(4);
                        }
                    }
                }
                return fp;
            }
        }
    }

    
}