using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoorinWebApp.Models;
using DoorinWebApp.Viewmodel;

namespace DoorinWebApp.Models.Operations
{
    public class FreelancerProfileOperations
    {
        public List<FreelancerProfileVM> GetProfileDetails()
        {
            doorinDBEntities db = new doorinDBEntities();
            List<FreelancerProfileVM> list = new List<FreelancerProfileVM>();
            var studentlist = (from fr in db.freelancer
                               join res in db.resume on fr.freelancer_id equals 
                               res.freelancer_id select new
                               {
                                fr.freelancer_id,
                                 fr.firstname,
                                 fr.lastname,
                                 fr.email,
                                 fr.nationality,
                                 res.resume_id,
                                 res.profile }).ToList();
            foreach (var item in studentlist)
            {
                FreelancerProfileVM fp = new FreelancerProfileVM();
                fp.Freelancer_id = item.freelancer_id;
                fp.Firstname = item.firstname;
                fp.Lastname = item.lastname;
                fp.Email = item.email;
                fp.Nationality = item.nationality;
                fp.Resume_id = item.resume_id;
                fp.ProfileText = item.profile;
                list.Add(fp);
            }

            return list;
        }
        public List<FreelancerProfileVM> GetProfile(int? id) //Returnerar en lista med en freelancer och dennes CV. Ska göras om så den returnerar ett objekt av personen istället
        {
            doorinDBEntities db = new doorinDBEntities();
            List<FreelancerProfileVM> list = new List<FreelancerProfileVM>();
            var studentlist = (from fr in db.freelancer
                               join res in db.resume on fr.freelancer_id equals
                               res.freelancer_id
                               select new
                               {
                                   fr.freelancer_id,
                                   fr.firstname,
                                   fr.lastname,
                                   fr.email,
                                   fr.nationality,
                                   res.resume_id,
                                   res.profile
                               }).ToList();
            foreach (var item in studentlist)
            {
                FreelancerProfileVM fp = new FreelancerProfileVM();
                if (item.freelancer_id == id)
                {
                    fp.Freelancer_id = item.freelancer_id;
                    fp.Firstname = item.firstname;
                    fp.Lastname = item.lastname;
                    fp.Email = item.email;
                    fp.Nationality = item.nationality;
                    fp.Resume_id = item.resume_id;
                    fp.ProfileText = item.profile;
                    list.Add(fp);
                }             
            }
            return list;
        }
    }
}