using Microsoft.AspNetCore.Mvc;
using Rafik_Lababidi.Models;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;

namespace Rafik_Lababidi.Controllers
{
    public class UserController : Controller
    {
        private readonly DbConnection dbConnection = new DbConnection();
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            bool isAdmin = dbConnection.executeBool("SELECT isAdmin FROM users WHERE username = '" + username + "' AND password = '" + password + "'");
            if (!isAdmin)
            {
                string id = dbConnection.executeScalar("SELECT id FROM users WHERE username = '" + username + "' AND password = '" + password + "'");
                
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.Error = "Invalid Username Or Password";
                    return View();
                }
                else
                {
                    return Redirect("/Home/Search");
                }
            }

            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(1);

            if (isAdmin)
                Response.Cookies.Append("admin", "ok", cookieOptions);
            else
                Response.Cookies.Append("admin", "error", cookieOptions);

            return Redirect("/Home/Search");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string name, string email, string username, string password)
        {
            dbConnection.executeNonQuery("INSERT INTO users VALUES ('" + name + "', '" + email + "', '" + username + "', '" + password + "', '0')");
            return View("Login");
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgetPassword(string emailTo)
        {
            string password = dbConnection.executeScalar("SELECT password FROM users WHERE email = '" + emailTo + "'");

            if (string.IsNullOrEmpty(password))
            {
                // create email message
                var email = new MailMessage();
                email.From = new MailAddress("derkaouim43@gmail.com");
                email.To.Add(new MailAddress(emailTo));
                email.Subject = "Recovert Password";
                email.Body = password;

                // send email
                var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("derkaouim43@gmail.com", "jtkvwqnbvxzecsef"),
                    EnableSsl = true,
                };

                smtp.Send(email);

                ViewBag.Ok = "Password Sent To Your Mail";
            }
            else
            {
                ViewBag.Error = "Incorrect Email";
            }
            
            return View();
        }
    }
}
