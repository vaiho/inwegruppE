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
        public FreelancerProfileVM GetFreelancerProfileById(int? id) //Metod för att hämta information om en freelancer
        {
            FreelancerProfileVM fp = new FreelancerProfileVM();
            string sql = "SELECT freelancer.freelancer_id, firstname, lastname, resume_id, profile, email, nationality, city, birthdate, address, zipcode, username from freelancer INNER JOIN resume on freelancer.freelancer_id = resume.freelancer_id WHERE freelancer.freelancer_id = @freelancer_id";

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
                            fp.Freelancer_id = reader.GetInt32(0);
                            fp.Firstname = reader.GetString(1);
                            fp.Lastname = reader.GetString(2);
                            fp.Resume_id = reader.GetInt32(3);
                            fp.ProfileText = reader.GetString(4);
                            fp.Email = reader.GetString(5);
                            fp.Nationality = reader.GetString(6);
                            fp.City = reader.GetString(7);
                            fp.Birthdate = reader.GetDateTime(8);
                            fp.Address = reader.GetString(9);
                            //fp.Zipcode = reader.GetString(10);
                            fp.Username = reader.GetString(11);
                        }
                    }
                }
            }
            GetCompetences(fp); //Hämtar och sparar kompetenser
            GetTechnology(fp); //Hämtar och sparar teknologier
            //GetFreelancersList();

            return fp;
        }

        private void GetCompetences(FreelancerProfileVM fp)//Metod för att hämta kompetenser på inskickad freelancerVM och lagra dessa i en lista
        {
            competence c;
            string sql = "SELECT competence.competence_id, name from competence_resume INNER JOIN competence on competence_resume.competence_id = competence.competence_id WHERE resume_id = @resume_id";

            using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
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

        private void GetTechnology(FreelancerProfileVM fp) //Metod för att hämta teknologier på inskickad freelancerVM och lagra dessa i en lista
        {
            string sql = "SELECT technology.technology_id, technology.name, technology_resume.rank, technology_resume.core_technology, technology.competence_id from technology_resume INNER JOIN technology on technology_resume.technology_id = technology.technology_id WHERE resume_id = @resume_id";
            FullTechnology t;

            using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
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

        private SqlConnectionStringBuilder GetBuilder() //Anropa vid användning för connection mot databasen
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "doorin.database.windows.net";
            builder.UserID = "doorinadmin";
            builder.Password = "Secretpassword123!";
            builder.InitialCatalog = "doorinDB";

            return builder;
        }

        public List<FreelancerProfileVM> GetFreelancersList()
        {
            FreelancerProfileVM fp;
            string sql = "SELECT freelancer.freelancer_id, firstname, lastname, resume_id, profile, email, nationality, city, birthdate, address, zipcode, username from freelancer INNER JOIN resume on freelancer.freelancer_id = resume.freelancer_id";
            List<FreelancerProfileVM> list = new List<FreelancerProfileVM>();

            using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    //command.Parameters.AddWithValue("resume_id", fp.Resume_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fp = new FreelancerProfileVM()
                            {
                                Freelancer_id = reader.GetInt32(0),
                                Firstname = reader.GetString(1),
                                Lastname = reader.GetString(2),
                                Resume_id = reader.GetInt32(3),
                                //ProfileText = reader.GetString(4),
                                //Email = reader.GetString(5),
                                Nationality = reader.GetString(6),
                                //City = reader.GetString(7),
                                //Birthdate = reader.GetDateTime(8),
                                //Address = reader.GetString(9),
                                //Zipcode = reader.GetString(10),
                                //Username = reader.GetString(11),
                            };
                            GetCompetences(fp); //Hämtar och sparar kompetenser
                            GetTechnology(fp); //Hämtar och sparar teknologier
                            list.Add(fp);
                        }
                    }
                }
            }

            return list;
        }

    }    
}