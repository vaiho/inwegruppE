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
            GetEducation(fp); //Hämtar och sparar utbildningar
            GetWorkHistory(fp); //Hämtar och sparar workhistory
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
        private void GetEducation(FreelancerProfileVM fp)
        {
            var list = (from e in db.education
                                  join r in db.resume on e.resume_id equals r.resume_id where e.resume_id == fp.Resume_id
                                  select new { e.education_id, e.resume_id, e.title, e.description, e.date }).ToList();

            foreach (var item in list)
            {
                education ed = new education();
                ed.education_id = item.education_id;
                ed.resume_id = item.resume_id;
                ed.title = item.title;
                ed.description = item.description;
                ed.date = item.date;
                fp.EducationsList.Add(ed);
            }          
        }
        private void GetWorkHistory(FreelancerProfileVM fp)
        {
            var list = (from w in db.workhistory
                        join r in db.resume on w.resume_id equals r.resume_id
                        where w.resume_id == fp.Resume_id
                        select new { w.workhistory_id, w.resume_id, w.employer, w.position, w.description, w.date }).ToList();

            foreach (var item in list)
            {
                workhistory wo = new workhistory();
                wo.workhistory_id = item.workhistory_id;
                wo.resume_id = item.resume_id;
                wo.employer = item.employer;
                wo.position = item.position;
                wo.description = item.description;
                wo.date = item.date;
                fp.WorkHistoryList.Add(wo);
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

        public List<FreelancerProfileVM> FilterByCompetence(int? id)
        {
            List<FreelancerProfileVM> allFreelancers = new List<FreelancerProfileVM>();
            List<FreelancerProfileVM> filteredFreelancers = new List<FreelancerProfileVM>();
            allFreelancers = GetFreelancersList();

            //filteredFreelancers = allFreelancers.Where(f => f.CompetencesList[].competence_id == 1);

            foreach (var freelancer in allFreelancers)
            {
                foreach (var competence in freelancer.CompetencesList)
                {
                    if (competence.competence_id == id)
                        filteredFreelancers.Add(freelancer);

                }
            }
            
            return filteredFreelancers;
        }

        public void SaveFreelancerToCustomerList(int? f, int c)
        {
            string sql = "INSERT INTO customer_freelancer(freelancer_id, customer_id) VALUES (@free_id, @cus_id)";
            try
            {
                using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        command.Connection = conn;
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("free_id", f);
                        command.Parameters.AddWithValue("cus_id", c);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                //TODO: gör något med meddelandet
            }
            

        }


    }
}