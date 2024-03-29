﻿using DoorinWebApp.Viewmodel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DoorinWebApp.Models.Operations
{
    public class CustomerOperations
    {
        doorinDBEntities db = new doorinDBEntities();

        public List<FreelancerProfileVM> GetSavedFreelancersList(int? id) //Hämtar en lista av sparade frilansare för en customer
        {
            //customer_freelancer finns ej i doorinDBEntities så kan ej göra Linq
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
                                Email = reader.GetString(3),
                                Nationality = reader.GetString(4),
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

        public customer GetCustomerById(int? id) //Metod för att hämta information om customer
        {
            customer co = new customer();

            //FreelancerProfileVM fp = new FreelancerProfileVM();
            try
            {
                var cust = (from c in db.customer
                            where c.customer_id == id
                            select new { c.customer_id, c.firstname, c.lastname, c.phonenumber, c.email, c.username, c.password, c.company, c.position }).ToList();

                foreach (var item in cust)
                {
                    co.customer_id = item.customer_id;
                    co.firstname = item.firstname;
                    co.lastname = item.lastname;
                    co.email = item.email;
                    co.phonenumber = item.phonenumber;
                }
                
            }
            catch (SqlException ex)
            {
                //TODO: Gär något med felmedelandet
                throw;
            }


            return co;
        }

    }
}