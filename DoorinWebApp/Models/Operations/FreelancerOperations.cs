using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace DoorinWebApp.Models.Operations
{
    public class FreelancerOperations
    {
        doorinDBEntities db = new doorinDBEntities();
        public freelancer GetFreelancerById(int? id)              // Denna används inte längre. Ta bort?
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
        public List<competence> GetAllCompetences()
        {
            List<competence> CList = new List<competence>();
            var competencelist = (from c in db.competence
                                  select new { c.name, c.competence_id }).ToList();

            foreach (var c in competencelist)
            {
                competence item = new competence();
                item.name = c.name;
                item.competence_id = c.competence_id;
                CList.Add(item);
            }
            return (CList);
        }

        public List<technology> GetAllTechnologies()
        {
            List<technology> TList = new List<technology>();
            var technologylist = (from t in db.technology
                                  select new { t.name }).ToList();

            foreach (var t in technologylist)
            {
                technology item = new technology();
                item.name = t.name;
                TList.Add(item);
            }
            return (TList);
        }

        public List<technology> GetTechnologiesByCompetenceId(int? id)
        {

            List<technology> techList = new List<technology>();
            var tList = (from t in db.technology
                         where t.competence_id == id
                         select new { t.name, t.competence_id }).ToList();

            foreach (var technology in tList)
            {
                technology item = new technology();
                item.name = technology.name;
                techList.Add(item);
            }

            return techList;
        }

    }
}