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
                            select new { f.freelancer_id, f.firstname, f.lastname, f.phonenumber, r.resume_id, r.profile, f.email, f.nationality, f.city, f.birthdate, f.address, r.driving_license }).ToList();

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
                    fp.DrivingLicence = item.driving_license;
                    fp.phonenumber = item.phonenumber;
                }

                GetCompetences(fp); //Hämtar och sparar kompetenser
                GetTechnology(fp); //Hämtar och sparar teknologier
                GetEducation(fp); //Hämtar och sparar utbildningar
                GetWorkHistory(fp); //Hämtar och sparar workhistory
                GetLinks(fp); // Hämtar och sparar länkar
            }
            catch (SqlException ex)
            {
                //TODO: Gör något med felmedelandet
                throw;
            }
            
            return fp;
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
                //TODO: Gör något med felmeddelandet
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
                            select new { te.technology_id, te.name, tr.rank, tr.core_technology, te.competence_id }).OrderByDescending(o => o.rank).OrderByDescending(o => o.core_technology).ToList();
                

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
        }

        public void GetEducation(FreelancerProfileVM fp) //Metod för att hämta utbildningar på inskickad freelancerVM och lagra dessa i dennes utbildnings-lista
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
            int id = 5;
            CustomerOperations co = new CustomerOperations();
            var list = co.GetSavedFreelancersList(id);

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
                    GetCompetences(fp); //Hämtar och sparar kompetenser för freelancer
                    GetTechnology(fp); //Hämtar och sparar teknologier för freelancer

                    //test
                    foreach (var x in list)
                    {
                        if (list.Any(z => z.Freelancer_id == item.freelancer_id))
                        {
                            fp.IsSaved = true;
                        }
                        else
                        {
                            fp.IsSaved = false;
                        }
                    }
                    freelancersList.Add(fp);
                }              
            }
            catch (SqlException ex)
            {
                //TODO: gör något med felmeddelandet
                throw;
            }
            return freelancersList;
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

        public void GetLinks(FreelancerProfileVM fp)
        {
            try
            {
                var list = (from l in db.links
                            join r in db.resume on l.resume_id equals r.resume_id
                            where l.resume_id == fp.Resume_id
                            select new { l.link_id, l.name, l.link }).ToList();

                foreach (var item in list)
                {
                    links li = new links();
                    li.link_id = item.link_id;
                    li.name = item.name;
                    li.link = item.link;
                    fp.LinkList.Add(li);
                }
            }
            catch (SqlException ex)
            {
                //TODO: Felmeddelande
                throw;
            }
        }

    }
}