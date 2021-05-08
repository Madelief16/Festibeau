﻿using Festibeau.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using MySql.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

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
            return View();
        }
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
                // alle namen ophalen
                var products = GetProducts();

                // stop de namen in de html
                return View(products);
            }

            public IActionResult Privacy()
            {
                return View();
            }

            public List<Product> GetProducts()
            {
                // stel in waar de database gevonden kan worden
                string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110368;Uid=110368;Pwd=inf2021SQL;";

                // maak een lege lijst waar we de namen in gaan opslaan
                List<Product> products = new List<Product>();

                // verbinding maken met de database
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    // verbinding openen
                    conn.Open();

                    // SQL query die we willen uitvoeren
                    MySqlCommand cmd = new MySqlCommand("select * from product", conn);

                    // resultaat van de query lezen
                    using (var reader = cmd.ExecuteReader())
                    {
                        // elke keer een regel (of eigenlijk: database rij) lezen
                        while (reader.Read())
                        {
                            Product p = new Product
                            {
                                // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                                Id = Convert.ToInt32(reader["Id"]),
                                Beschikbaarheid = Convert.ToInt32(reader["Beschikbaarheid"]),
                                Naam = reader["Naam"].ToString(),
                                Prijs = reader["Prijs"].ToString(),
                            };

                            // voeg de naam toe aan de lijst met namen
                            products.Add(p);
                        }
                    }
                }

                // return de lijst met namen
                return products;
            }

            public List<string> GetNames()
            {
                // stel in waar de database gevonden kan worden
                string connectionString = "Server=172.16.160.21;Port=3306;Database=fastfood;Uid=lgg;Pwd=0P%Y9fI2GdO#;";

                // maak een lege lijst waar we de namen in gaan opslaan
                List<string> names = new List<string>();

                // verbinding maken met de database
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    // verbinding openen
                    conn.Open();

                    // SQL query die we willen uitvoeren
                    MySqlCommand cmd = new MySqlCommand("select * from product", conn);

                    // resultaat van de query lezen
                    using (var reader = cmd.ExecuteReader())
                    {
                        // elke keer een regel (of eigenlijk: database rij) lezen
                        while (reader.Read())
                        {
                            // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                            string Name = reader["Naam"].ToString();

                            // voeg de naam toe aan de lijst met namen
                            names.Add(Name);
                        }
                    }
                }

                // return de lijst met namen
                return names;
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
    public IActionResult Privacy => View();

    IActionResult View()
    {
        throw new NotImplementedException();
    }

    [Route("contact")]
        public IActionResult contact()
        {
            return View();
        }

        [Route("Festivals")]
        public IActionResult Festivals()
        {
            return View();
        }

        [Route("regels")]
        public IActionResult regels()
        {
            return View();
        }

        [Route("prijzen")]
        public IActionResult prijzen()
        {
            return View();
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
    }
}
