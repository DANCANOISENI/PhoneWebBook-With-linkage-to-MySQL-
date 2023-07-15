using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using PhoneWebBook.Models;
using System.Collections.Generic;

namespace PhoneWebBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Contacts";
                var command = new MySqlCommand(query, connection);

                var contacts = new List<Contact>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var contact = new Contact
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            PhoneNumber = (string)reader["PhoneNumber"]
                        };

                        contacts.Add(contact);
                    }
                }

                return View(contacts);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = "INSERT INTO Contacts (Name, PhoneNumber) VALUES (@Name, @PhoneNumber)";
                    var command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", contact.Name);
                    command.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);

                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }

            return View(contact);
        }

        public IActionResult Edit(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Contacts WHERE Id = @Id";
                var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var contact = new Contact
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            PhoneNumber = (string)reader["PhoneNumber"]
                        };

                        return View(contact);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = "UPDATE Contacts SET Name = @Name, PhoneNumber = @PhoneNumber WHERE Id = @Id";
                    var command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", contact.Id);
                    command.Parameters.AddWithValue("@Name", contact.Name);
                    command.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);

                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }

            return View(contact);
        }

        public IActionResult Delete(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Contacts WHERE Id = @Id";
                var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

    }
}
