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
            try
            {
                var free = (from f in db.freelancer
                            join r in db.resume on f.freelancer_id equals r.freelancer_id
                            where f.freelancer_id == id
                            select new { f.freelancer_id, f.firstname, f.lastname, r.resume_id, r.profile, f.email, f.nationality, f.city, f.birthdate, f.address }).ToList();

                foreach (var item in free)
                {
                    fp.Freelancer_id = item.freelancer_id;
                    fp.Firstname = item.firstname;
                    fp.Lastname = item.lastname;
                    fp.Resume_id = item.resume_id;
                    fp.ProfileText = item.profile;
                    fp.Email = item.email;
                    fp.Nationality = item.nationality;
                    fp.City = item.city;
                    fp.Birthdate = item.birthdate;
                    fp.Address = item.address;
                }

                GetCompetences(fp); //Hämtar och sparar kompetenser
                GetTechnology(fp); //Hämtar och sparar teknologier
                GetEducation(fp); //Hämtar och sparar utbildningar
                GetWorkHistory(fp); //Hämtar och sparar workhistory
            }
            catch (SqlException ex)
            {
                //TODO: Gär något med felmedelandet
                throw;
            }
            

            return fp;


            //OLD:
            //string sql = "SELECT freelancer.freelancer_id, firstname, lastname, resume_id, profile, email, nationality, city, birthdate, address, zipcode, username from freelancer INNER JOIN resume on freelancer.freelancer_id = resume.freelancer_id WHERE freelancer.freelancer_id = @freelancer_id";

            //using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
            //{
            //    conn.Open();
            //    using (SqlCommand command = new SqlCommand(sql, conn))
            //    {
            //        command.Parameters.AddWithValue("freelancer_id", id);

            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                fp.Freelancer_id = reader.GetInt32(0);
            //                fp.Firstname = reader.GetString(1);
            //                fp.Lastname = reader.GetString(2);
            //                fp.Resume_id = reader.GetInt32(3);
            //                fp.ProfileText = reader.GetString(4);
            //                fp.Email = reader.GetString(5);
            //                fp.Nationality = reader.GetString(6);
            //                fp.City = reader.GetString(7);
            //                fp.Birthdate = reader.GetDateTime(8);
            //                fp.Address = reader.GetString(9);
            //                //fp.Zipcode = reader.GetString(10);
            //                fp.Username = reader.GetString(11);
            //            }
            //        }
            //    }
            //}

        }

        private void GetCompetences(FreelancerProfileVM fp)//Metod för att hämta kompetenser på inskickad freelancerVM och lagra dessa i dennes kompetens-lista
        {
            //Kan inte göra Linq här då tabellen competence_resume inte finns i doorinDBEntities..
            competence c;
            string sql = "SELECT competence.competence_id, name from competence_resume INNER JOIN competence on competence_resume.competence_id = competence.competence_id WHERE resume_id = @resume_id";

            try
            {
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
            catch (SqlException ex)
            {
                //TODO: Gör något med felmeddelandet?
                throw;
            }         
        }

        private void GetTechnology(FreelancerProfileVM fp) //Metod för att hämta teknologier på inskickad freelancerVM och lagra dessa i en dennes teknologi-lista
        {
            try
            {
                var list = (from tr in db.technology_resume
                            join te in db.technology on tr.technology_id equals te.technology_id
                            where tr.resume_id == fp.Resume_id
                            select new { te.technology_id, te.name, tr.rank, tr.core_technology, te.competence_id }).ToList();

                foreach (var item in list)
                {
                    FullTechnology ft = new FullTechnology();
                    ft.technology_id = item.technology_id;
                    ft.name = item.name;
                    ft.rank = item.rank;
                    ft.core_technology = item.core_technology;
                    ft.competence_id = item.competence_id;

                    fp.TechnologysList.Add(ft);
                }
            }
            catch (SqlException ex)
            {
                //TODO: gör något med felmeddelandet
                throw;
            }


            //OLD:
            //string sql = "SELECT technology.technology_id, technology.name, technology_resume.rank, technology_resume.core_technology, technology.competence_id from technology_resume INNER JOIN technology on technology_resume.technology_id = technology.technology_id WHERE resume_id = @resume_id";
            //FullTechnology t;

            //using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
            //{
            //    conn.Open();
            //    using (SqlCommand command = new SqlCommand(sql, conn))
            //    {
            //        command.Parameters.AddWithValue("resume_id", fp.Resume_id);
            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                t = new FullTechnology()
            //                {
            //                    technology_id = (reader.GetInt32(0)),
            //                    name = (reader.GetString(1)),
            //                    rank = (reader.GetInt32(2)),
            //                    core_technology = (reader.GetBoolean(3)),
            //                    competence_id = (reader.GetInt32(4)),
            //                };
            //                fp.TechnologysList.Add(t);
            //            }
            //        }
            //    }
            //}
        }

        private void GetEducation(FreelancerProfileVM fp) //Metod för att hämta utbildningar på inskickad freelancerVM och lagra dessa i dennes utbildnings-lista
        {
            try
            {
                var list = (from e in db.education
                            join r in db.resume on e.resume_id equals r.resume_id
                            where e.resume_id == fp.Resume_id
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
            catch (SqlException ex)
            {
                //TODO: gör något med felmeddelandet
                throw;
            }
        }

        private void GetWorkHistory(FreelancerProfileVM fp) //Metod för att hämta erfarenheter på inskickad freelancerVM och lagra dessa i dennes workhistory-lista
        {
            try
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
            catch (SqlException ex)
            {
                //TODO: gör något med felmeddelandet
                throw;
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

        public List<FreelancerProfileVM> GetFreelancersList() //Hämtar och returnerar en lista av alla freelancers ink kompetenser och teknologier
        {
            List<FreelancerProfileVM> freelancersList = new List<FreelancerProfileVM>();
            try
            {
                var l = (from f in db.freelancer
                         join r in db.resume on f.freelancer_id equals r.freelancer_id
                         select new { f.freelancer_id, f.firstname, f.lastname, r.resume_id, f.nationality }).ToList();

                foreach (var item in l)
                {
                    FreelancerProfileVM fp = new FreelancerProfileVM();
                    fp.Freelancer_id = item.freelancer_id;
                    fp.Firstname = item.firstname;
                    fp.Lastname = item.lastname;
                    fp.Resume_id = item.resume_id;
                    fp.Nationality = item.nationality;
                    GetCompetences(fp); //Hämtar och sparar kompetenser
                    GetTechnology(fp); //Hämtar och sparar teknologier

                    freelancersList.Add(fp);
                }
                
            }
            catch (SqlException ex)
            {
                //TODO: gör något med felmeddelandet
                throw;
            }
            return freelancersList;



            //FreelancerProfileVM fp;
            //string sql = "SELECT freelancer.freelancer_id, firstname, lastname, resume_id, profile, email, nationality, city, birthdate, address, zipcode, username from freelancer INNER JOIN resume on freelancer.freelancer_id = resume.freelancer_id";

            //using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
            //{
            //    conn.Open();
            //    using (SqlCommand command = new SqlCommand(sql, conn))
            //    {
            //        //command.Parameters.AddWithValue("resume_id", fp.Resume_id);
            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                fp = new FreelancerProfileVM()
            //                {
            //                    Freelancer_id = reader.GetInt32(0),
            //                    Firstname = reader.GetString(1),
            //                    Lastname = reader.GetString(2),
            //                    Resume_id = reader.GetInt32(3),
            //                    //ProfileText = reader.GetString(4),
            //                    //Email = reader.GetString(5),
            //                    Nationality = reader.GetString(6),
            //                    //City = reader.GetString(7),
            //                    //Birthdate = reader.GetDateTime(8),
            //                    //Address = reader.GetString(9),
            //                    //Zipcode = reader.GetString(10),
            //                    //Username = reader.GetString(11),
            //                };
            //                GetCompetences(fp); //Hämtar och sparar kompetenser
            //                GetTechnology(fp); //Hämtar och sparar teknologier
            //                list.Add(fp);
            //            }
            //        }
            //    }
            //}

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

        public void SaveFreelancerToCustomerList(int? f, int c) //Sparar en kombination mellan freelancer och customer
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
                //TODO: gör något med felmeddelandet
                throw;
            }


        }

        public void RemoveFreelancerFromCustomerList(int? f, int? c) //Tar bort kombination mellan freelancer och customer
        {
            string sql = "DELETE FROM customer_freelancer WHERE freelancer_id = @free_id AND customer_id = @cus_id";
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
                //TODO: gör något med felmeddelandet
                throw;
            }
        }

    }
}