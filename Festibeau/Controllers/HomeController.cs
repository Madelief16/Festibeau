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
        // stel in waar de database gevonden kan worden
        string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110368;Uid=110368;Pwd=inf2021sql;";

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


        //private List<festivaldag> Getfestivaldagen(string id)
        //{
        //    List<festivaldag> festivaldagen = new List<Festivaldag>();
        //    using (MySqlConnection conn = new MySqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        MySqlCommand cmd = new MySqlCommand($"select * from voorstelling where film_id = {id}", conn);
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                Festivaldag p = new Festivaldag
        //                {
        //                    Id = Convert.ToInt32(reader["Id"]),
        //                    Festivals_id = Convert.ToInt32(reader["festivals_id"]),
        //                    Voorraad = Convert.ToInt32(reader["Voorraad"]),
        //                    Datum = DateTime.Parse(reader["Datum"].ToString()),

        //                };
        //                festivaldagen.Add(p);
        //            }
        //        }
        //    }
        //    return festivaldagen;
        //}




        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }


        [Route("Login")]
        public IActionResult Login(string email, string password)
        {
            // is er een wachtwoord ingevoerd?
            if (!string.IsNullOrWhiteSpace(password))
            {
                Person p = GetPersonByEmail(email);
                if (p == null)
                    return View();

                //Er is iets ingevoerd, nu kunnen we het wachtwoord hashen en vergelijken met de hash "uit de database"
                string hashVanIngevoerdWachtwoord = ComputeSha256Hash(password);
                if (hashVanIngevoerdWachtwoord == p.Wachtwoord)
                {
                    HttpContext.Session.SetString("User", p.Email);
                    return Redirect("/");
                }
                
               
                return View();
            }

            return View();
        }
        private Person GetPersonByEmail(string email)
        {
            List<Person> persons = new List<Person>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand($"select * from klant where email = '{email}'", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        Person p = new Person();
                        p.Email = reader["email"].ToString();
                        p.Wachtwoord = reader["wachtwoord"].ToString();
                        


                        // voeg de naam toe aan de lijst met namen
                        persons.Add(p);
                    }
                }
            }

            // return de lijst met namen
            return persons[0];
        }


        [Route("Contact")]
        public IActionResult Contact()
        {
            var contact = GetContact();



            return View();
        }

        private object GetContact()
        {
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
            {
                SavePerson(person);
                
                return Redirect("/succes");
            }
                

            return View(person);
        }

        [Route("succes")]
        public IActionResult succes()
        {
            return View();
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

        [Route("Tickets")]
        public IActionResult Tickets()
        {
            var tickets = GetTickets();



            return View(tickets);
        }

        private object GetTickets()
        {
            

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

        private void SavePerson(Person person)
            
        {
            person.Wachtwoord = ComputeSha256Hash(person.Wachtwoord);
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO klant(voornaam, achternaam, email, wachtwoord, telefoon, adres, bericht) VALUES(?voornaam, ?achternaam, ?email, ?wachtwoord, ?telefoon, ?adres, ?bericht)", conn);

                cmd.Parameters.Add("?voornaam", MySqlDbType.Text).Value = person.FirstName;
                cmd.Parameters.Add("?achternaam", MySqlDbType.Text).Value = person.LastName;
                cmd.Parameters.Add("?email", MySqlDbType.Text).Value = person.Email;
                cmd.Parameters.Add("?wachtwoord", MySqlDbType.Text).Value = person.Wachtwoord;
                cmd.Parameters.Add("?telefoon", MySqlDbType.Text).Value = person.Phone;
                cmd.Parameters.Add("?adres", MySqlDbType.Text).Value = person.Address;
                cmd.Parameters.Add("?bericht", MySqlDbType.Text).Value = person.Description;
                cmd.ExecuteNonQuery();
            }
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

        [Route("viernulvier")]
        public IActionResult viernulvier()
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
