using Festibeau.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using MySql.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using SendAndStore.Models;
using Festibeau.database;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace Festibeau.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["User"] = HttpContext.Session.GetString("User");

            //alle namen ophalen
            var names = GetNames();

            //stop de namen in de html
            return View(names);
        }
        public List<string> GetNames()
        {
            // stel in waar de database gevonden kan worden
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110368;Uid=110368;Pwd=inf2021sql;";

            // maak een lege lijst waar we de namen in gaan opslaan
            List<string> names = new List<string>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from artiesten", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                        string voornaam = reader["voornaam"].ToString();
                        string achternaam = reader["achternaam"].ToString();
                        // voeg de naam toe aan de lijst met namen
                        names.Add(achternaam);
                        names.Add(voornaam);
                    }
                }
            }

            // return de lijst met namen
            return names;
        }

        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Login")]
        public IActionResult Login(string username, string password)
        {
            // hash voor "wachtwoord"
            string hash = "dc00c903852bb19eb250aeba05e534a6d211629d77d055033806b783bae09937";

            // is er een wachtwoord ingevoerd?
            if (!string.IsNullOrWhiteSpace(password))
            {

                //Er is iets ingevoerd, nu kunnen we het wachtwoord hashen en vergelijken met de hash "uit de database"
                string hashVanIngevoerdWachtwoord = ComputeSha256Hash(password);
                if (hashVanIngevoerdWachtwoord == hash)
                {
                    HttpContext.Session.SetString("User", username);
                    return Redirect("/");
                }

                if (password == "geheim")
                {
                    HttpContext.Session.SetString("User", username);
                    return Redirect("/");
                }
                return View();
            }

            return View();
        }

        [Route("Contact")]
        public IActionResult Contact()
        {
            var contact = GetContact();



            return View();
        }

        private object GetContact()
        {
            // stel in waar de database gevonden kan worden
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110368;Uid=110368;Pwd=inf2021sql;";

            // maak een lege lijst waar we de namen in gaan opslaan
            List<string> names = new List<string>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from contact", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                        string naam = reader["naam"].ToString();

                        // voeg de naam toe aan de lijst met namen
                        names.Add(naam);
                    }
                }
            }

            // return de lijst met namen
            return names;
        }

        [HttpPost]
        [Route("Contact")]
        public IActionResult Contact(Person person)
        {
            if (ModelState.IsValid)
                return Redirect("/succes");

            return View(person);
        }

        [Route("Festivals")]
        public IActionResult Festivals()
        {
            var model = GetFestivals();

            return View(model);

        }

        [Route("festival/{id}/{naam}")]
        public IActionResult FestivalDetail(string id, string naam)
        {
            var model = GetFestival(Convert.ToInt32(id));

            if (model.naam != naam)
                return Redirect($"/festival/{id}/{model.naam}");

            return View(model);
        }


        private List<Festival> GetFestivals()
        {
            // stel in waar de database gevonden kan worden
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110368;Uid=110368;Pwd=inf2021sql;";

            // maak een lege lijst waar we de namen in gaan opslaan
            List<Festival> festivals = new List<Festival>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from festivals", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        Festival f = new Festival
                        {
                            // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                            Id = Convert.ToInt32(reader["Id"]),
                            naam = reader["naam"].ToString(),
                            beschrijving = reader["beschrijving"].ToString(),
                            foto = reader["foto"].ToString(),
                            // data = reader["data"].ToString(),
                            locatie = reader["locatie"].ToString(),

                        };

                        //voeg de naam toe aan de lijst met namen
                        festivals.Add(f);
                    }
                }
            }

            // return de lijst met namen
            return festivals;
        }

        private Festival GetFestival(int id)
        {
            // stel in waar de database gevonden kan worden
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110368;Uid=110368;Pwd=inf2021sql;";

            // maak een lege lijst waar we de namen in gaan opslaan
            List<Festival> festivals = new List<Festival>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand($"select * from festivals where id = {id}", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        Festival f = new Festival
                        {
                            // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                            Id = Convert.ToInt32(reader["Id"]),
                            naam = reader["naam"].ToString(),
                            beschrijving = reader["beschrijving"].ToString(),
                            foto = reader["foto"].ToString(),
                            // data = reader["data"].ToString(),
                            locatie = reader["locatie"].ToString(),

                        };

                        //voeg de naam toe aan de lijst met namen
                        festivals.Add(f);
                    }
                }
            }

            // return de lijst met namen
            return festivals[0];
        }

        [Route("festivalregels")]
        public IActionResult festivalregels()
        {
            var regels = GetRegels();

            return View(regels);
        }

        private List<Regel> GetRegels()
        {
            // stel in waar de database gevonden kan worden
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110368;Uid=110368;Pwd=inf2021sql;";

            // maak een lege lijst waar we de namen in gaan opslaan
            List<Regel> regels = new List<Regel>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from regels", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        Regel r = new Regel
                        {
                            // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                            Id = Convert.ToInt32(reader["Id"]),
                            richtlijnen = reader["richtlijnen"].ToString(),
                        };

                        //voeg de naam toe aan de lijst met namen
                        regels.Add(r);
                    }
                }
            }

            // return de lijst met namen
            return regels;
        }

        [Route("prijzen")]
        public IActionResult prijzen()
        {
            var tickets = GetTickets();



            return View(tickets);
        }

        private object GetTickets()
        {
            // stel in waar de database gevonden kan worden
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110368;Uid=110368;Pwd=inf2021sql;";

            // maak een lege lijst waar we de namen in gaan opslaan
            List<Ticket> tickets = new List<Ticket>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from tickets", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        Ticket t = new Ticket
                        {
                            // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                            Id = Convert.ToInt32(reader["Id"]),
                            soort = reader["soort"].ToString(),
                            prijs = reader["prijs"].ToString(),
                        };

                        //voeg de naam toe aan de lijst met namen
                        tickets.Add(t);
                    }
                }
            }

            // return de lijst met namen
            return tickets;
        }
        [Route("Locaties")]
        public IActionResult Locaties()
        {
            return View();
        }

        [Route("Lowlands")]
        public IActionResult Lowlands()
        {
            return View();
        }

        [Route("Hockeyloverz")]
        public IActionResult Hockeyloverz()
        {
            return View();
        }

        [Route("MysteryLand")]
        public IActionResult MysteryLand()
        {
            return View();
        }

        [Route("Pinkpop")]
        public IActionResult Pinkpop()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }


}
