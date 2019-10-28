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
        doorinDBEntities db = new doorinDBEntities();
        public FreelancerProfileVM GetFreelancerProfileById(int? id)
        {
            FreelancerProfileVM fp = new FreelancerProfileVM();

            List<competence> list = new List<competence>();

            string sql = "SELECT freelancer.freelancer_id, firstname, lastname, resume_id, profile, email, nationality, city, birthdate from freelancer INNER JOIN resume on freelancer.freelancer_id = resume.freelancer_id WHERE freelancer.freelancer_id = @freelancer_id";

            //string sql = "SELECT * FROM freelancer WHERE freelancer_id = @freelancer_id";

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "doorin.database.windows.net";
            builder.UserID = "doorinadmin";
            builder.Password = "Secretpassword123!";
            builder.InitialCatalog = "doorinDB";


            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
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
                            fp.Email = reader.GetString(5);
                            fp.Nationality = reader.GetString(6);
                            fp.City = reader.GetString(7);
                            fp.Birthdate = reader.GetDateTime(8);
                        }
                    }
                }
            }

            GetCompetences(fp, builder);
            GetTechnology(fp, builder);

            return fp;
        }

        private void GetCompetences(FreelancerProfileVM fp, SqlConnectionStringBuilder builder)
        {
            competence c;
            string sql2 = "SELECT competence.competence_id, name from competence_resume INNER JOIN competence on competence_resume.competence_id = competence.competence_id WHERE resume_id = @resume_id";

            using (SqlConnection conn2 = new SqlConnection(builder.ConnectionString))
            {
                conn2.Open();
                using (SqlCommand command = new SqlCommand(sql2, conn2))
                {
                    command.Parameters.AddWithValue("resume_id", fp.Resume_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            c = new competence()
                            {
                                competence_id = (reader.GetInt32(0)),
                                name = (reader.GetString(1))
                            };
                            fp.CompetencesList.Add(c);
                        }
                    }
                }
            }
        }

        private void GetTechnology(FreelancerProfileVM fp, SqlConnectionStringBuilder builder)
        {
            string sql3 = "SELECT technology.technology_id, technology.name, technology_resume.rank, technology_resume.core_technology, technology.competence_id from technology_resume INNER JOIN technology on technology_resume.technology_id = technology.technology_id WHERE resume_id = @resume_id";
            FullTechnology t;

            using (SqlConnection conn3 = new SqlConnection(builder.ConnectionString))
            {
                conn3.Open();
                using (SqlCommand command = new SqlCommand(sql3, conn3))
                {
                    command.Parameters.AddWithValue("resume_id", fp.Resume_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            t = new FullTechnology()
                            {
                                technology_id = (reader.GetInt32(0)),
                                name = (reader.GetString(1)),
                                rank = (reader.GetInt32(2)),
                                core_technology = (reader.GetBoolean(3)),
                                competence_id = (reader.GetInt32(4)),
                            };
                            fp.TechnologysList.Add(t);
                        }
                    }
                }
            }
        }

    }    
}