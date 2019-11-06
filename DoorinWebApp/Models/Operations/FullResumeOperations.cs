using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DoorinWebApp.Models;
using DoorinWebApp.Viewmodel;

namespace DoorinWebApp.Models.Operations
{
    public class FullResumeOperations
    {
        doorinDBEntities db = new doorinDBEntities();
        //FreelancerProfileOperations fp = new FreelancerProfileOperations();

        public FullResume GetFullResumeById(int? id) //Metod för att hämta information om en freelancer
        {
           FullResume fullResume = new FullResume();

            string sql = "SELECT freelancer.freelancer_id, firstname, lastname, resume_id, profile, email, nationality, city, birthdate, driving_license, address, zipcode from resume INNER JOIN freelancer on resume.freelancer_id = freelancer.freelancer_id WHERE resume.resume_id = @resume_id";

            using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("resume_id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fullResume.Freelancer_id = reader.GetInt32(0);
                            fullResume.Firstname = reader.GetString(1);
                            fullResume.Lastname = reader.GetString(2);
                            fullResume.Resume_id = reader.GetInt32(3);
                            fullResume.Profile = reader.GetString(4);
                            fullResume.Email = reader.GetString(5);
                            fullResume.Nationality = reader.GetString(6);
                            fullResume.City = reader.GetString(7);
                            fullResume.Birthdate = reader.GetDateTime(8);
                            fullResume.Driving_license = reader.GetString(9).Trim();
                            fullResume.Address = reader.GetString(10);
                            fullResume.Zipcode = reader.GetString(11);
                            
                        }
                    }
                }
            }
            GetMyCompetences(fullResume);
            GetMyTechnologies(fullResume);
            GetCompetenceList(fullResume);
            GetTechnologyList(fullResume);
            GetEducationsListByResume(fullResume);
            

            return fullResume;
        }

        private void GetEducationsListByResume (FullResume fullResume) {

            try {
                var list = (from ed in db.education
                            join re in db.resume on ed.resume_id equals re.resume_id
                            where ed.resume_id == fullResume.Resume_id
                            select new { ed.resume_id, ed.title, ed.description, ed.date, ed.education_id, ed.resume }).ToList();

                foreach (var item in list)
                {
                    education edu = new education();
                    edu.date = item.date;
                    edu.description = item.description;
                    edu.education_id = item.education_id;
                    edu.resume = item.resume;
                    edu.resume_id = item.resume_id;
                    edu.title = item.title;

                    fullResume.MyEducations = new List<education>();
                    fullResume.MyEducations.Add(edu);
                }
            }
            catch (SqlException ex)
            {
                //TODO: gör något med felmeddelandet
                throw;
            }

        }

        private void GetMyCompetences(FullResume fullResume)//Metod för att hämta kompetenser på inskickad freelancerVM och lagra dessa i en lista
        {
            fullResume.MyCompetences.Clear();
            competence c;
            string sql = "SELECT competence.competence_id, resume_id, name from competence_resume " +
                "INNER JOIN competence on competence_resume.competence_id = competence.competence_id " +
                "WHERE resume_id = @resume_id";

            using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("resume_id", fullResume.Resume_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            c = new competence()
                            {
                                competence_id = (reader.GetInt32(0)),
                                resume_id = (reader.GetInt32(1)),
                                name = (reader.GetString(2))
                            };
                            fullResume.MyCompetences.Add(c);
                        }
                    }
                }
            }
        }
        public void GetMyTechnologies(FullResume fullResume) //Metod för att hämta teknologier på inskickad freelancerVM och lagra dessa i en lista
        {
            fullResume.MyTechnologies.Clear();
            string sql = "SELECT technology.technology_id, technology.name, technology_resume.rank, " +
                "technology_resume.core_technology, technology.competence_id from technology_resume " +
                "INNER JOIN technology on technology_resume.technology_id = technology.technology_id " +
                "WHERE resume_id = @resume_id";
            FullTechnology t;

            using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("resume_id", fullResume.Resume_id);
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
                            fullResume.MyTechnologies.Add(t);
                        }


                    }
                }
            }

        }
        
    
            

        private void GetCompetenceList(FullResume fullResume) //Metod för att hämta teknologier på inskickad freelancerVM och lagra dessa i en lista
        {
            string sql = "SELECT competence_id, name FROM competence";
            competence c;

            using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("resume_id", fullResume.Resume_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            c = new competence()
                            {
                                competence_id = (reader.GetInt32(0)),
                                name = (reader.GetString(1))
                            };
                            fullResume.Competences.Add(c);
                        }
                    }
                }
            }

        }

        public void GetTechnologyList(FullResume fullResume) //Metod för att hämta teknologier med inskickad ResumeVM och lagra dessa i en lista
        {
            fullResume.Technologies.Clear();
            foreach (competence competence in fullResume.MyCompetences)
            {
                string sql = "SELECT name, technology_id FROM technology WHERE competence_id = @competenceID";
                technology t;

                using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("competenceID", competence.competence_id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                t = new technology()
                                {
                                    name = (reader.GetString(0)),
                                    technology_id = (reader.GetInt32(1))
                                };
                                fullResume.Technologies.Add(t);
                            }
                        }
                    }
                }
            }   
        }

        public void AddMyCompetences(int competence_id, int resume_id) //Sparar en kombination mellan freelancer och customer
        {
            string sql = "INSERT INTO competence_resume(competence_id, resume_id) VALUES (@competenceID, @reumeID)";
            try
            {
                using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        command.Connection = conn;
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("competenceID", competence_id);
                        command.Parameters.AddWithValue("reumeID", resume_id);
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

        public void AddMyTechnologies(int technology_id, int resume_id, bool? core_technology, int? rank) //Sparar en kombination mellan freelancer och customer
        {
            string sql = "INSERT INTO technology_resume(technology_id, resume_id, core_technology, rank) " +
                "VALUES (@technologyID, @reumeID, @core, @rank)";
            try
            {
                using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        command.Connection = conn;
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("technologyID", technology_id);
                        command.Parameters.AddWithValue("reumeID", resume_id);
                        command.Parameters.AddWithValue("core", core_technology);
                        command.Parameters.AddWithValue("rank", rank);
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

        public void RemoveMyTechnologies(int technology_id, int resume_id) {

         string sql = "DELETE FROM technology_resume WHERE technology_id = @technologyID AND resume_id = @resumeID";


            using (SqlConnection conn = new SqlConnection(GetBuilder().ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Connection = conn;
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("technologyID", technology_id);
                    command.Parameters.AddWithValue("resumeID", resume_id);
                    command.ExecuteNonQuery();
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
    }
}