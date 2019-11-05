SELECT freelancer.freelancer_id, firstname, lastname, resume_id, profile, email, nationality, city, 
birthdate, driving_license, address, zipcode 
from resume
INNER JOIN freelancer on resume.freelancer_id = freelancer.freelancer_id 
WHERE resume.resume_id = 15