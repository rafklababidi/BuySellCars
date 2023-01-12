using Microsoft.AspNetCore.Mvc;
using Rafik_Lababidi.Models;
using System.Data.SqlClient;

namespace Rafik_Lababidi.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbConnection dbConnection = new DbConnection();
        SqlConnection con = new SqlConnection("Server = .; DataBase = rafikLababidi; Integrated Security = True");

        public IActionResult Search()
        {
            return View();
        }

        public IActionResult SearchAsGuest()
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Append("admin", "error", cookieOptions);
            return View("Search");
        }

        [HttpPost]
        public IActionResult Search(string car, string monthly, string down)
        {
            List<Car> cars = new List<Car>();
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM cars WHERE type = '" + car + "' AND price BETWEEN '" + monthly + "' AND '" + down + "' AND isValid = 1", con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Car c = new Car();
                c.id = dr[0].ToString();
                c.fullName = dr[1].ToString();
                c.email = dr[2].ToString();
                c.contactNumber = dr[3].ToString();
                c.vehicleDetails = dr[4].ToString();
                c.price = dr[5].ToString();
                c.type = dr[6].ToString();
                c.vehicleEngine = dr[7].ToString();
                c.vehicleKm = dr[8].ToString();
                c.imageFront = dr[9].ToString();
                c.imageBack = dr[10].ToString();

                cars.Add(c);
            }

            con.Close();

            ViewBag.cars = cars;

            return View("Buy");
        }

        public IActionResult Buy()
        {
            List<Car> cars = new List<Car>();
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM cars WHERE isValid = 1", con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Car c = new Car();
                c.id = dr[0].ToString();
                c.fullName = dr[1].ToString();
                c.email = dr[2].ToString();
                c.contactNumber = dr[3].ToString();
                c.vehicleDetails = dr[4].ToString();
                c.price = dr[5].ToString();
                c.type = dr[6].ToString();
                c.vehicleEngine = dr[7].ToString();
                c.vehicleKm = dr[8].ToString();
                c.imageFront = dr[9].ToString();
                c.imageBack = dr[10].ToString();

                cars.Add(c);
            }

            con.Close();

            ViewBag.cars = cars;
            ViewBag.isAdmin = Request.Cookies["admin"];
            return View();
        }

        public IActionResult Sell()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Sell(string fullName, string email, string contactNumber, string vehicleDetails, string price, string type, string vehicleEngine, string vehicleKm, IFormFile imageFront, IFormFile imageBack)
        {
            string front = "";
            string back = "";
            if (imageFront != null)
            {
                front = DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "") + "_" + imageFront.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/carImages", front);

                FileStream fs = System.IO.File.Create(path);
                imageFront.CopyTo(fs);
            }

            if (imageBack != null)
            {
                back = DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "") + "_" + imageBack.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/carImages", back);

                FileStream fs = System.IO.File.Create(path);
                imageBack.CopyTo(fs);
            }

            dbConnection.executeNonQuery("INSERT INTO cars VALUES ('" + fullName + "', '" + email + "', '" + contactNumber + "', '" + vehicleDetails + "', '" + price + "', '" + type + "', '" + vehicleEngine + "', '" + vehicleKm + "', '" + front + "', '" + back + "', '0')");
            ViewBag.Ok = "Car Add With Success";
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    
        public IActionResult SellAsAdmin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SellAsAdmin(string fullName, string email, string contactNumber, string vehicleDetails, string price, string type, string vehicleEngine, string vehicleKm, IFormFile imageFront, IFormFile imageBack)
        {
            string front = "";
            string back = "";
            if (imageFront != null)
            {
                front = DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "") + "_" + imageFront.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/carImages", front);

                FileStream fs = System.IO.File.Create(path);
                imageFront.CopyTo(fs);
            }

            if (imageBack != null)
            {
                back = DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "") + "_" + imageBack.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/carImages", back);

                FileStream fs = System.IO.File.Create(path);
                imageBack.CopyTo(fs);
            }

            dbConnection.executeNonQuery("INSERT INTO cars VALUES ('" + fullName + "', '" + email + "', '" + contactNumber + "', '" + vehicleDetails + "', '" + price + "', '" + type + "', '" + vehicleEngine + "', '" + vehicleKm + "', '" + front + "', '" + back + "', '1')");
            ViewBag.Ok = "Car Add With Success";
            return View();
        }

        [HttpPost]
        public string SendMessage(string fullName, string message)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO contact VALUES (N'" + fullName + "', N'" + message + "')", con);
                cmd.ExecuteNonQuery();
                con.Close();

                return "OK";
            }
            catch
            {
                return "Error";
            }
        }
    }
}
