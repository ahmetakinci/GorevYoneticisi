using GorevYoneticisi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;

using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();
app.Run();
[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost, Route("login")]
    public IActionResult Login(LoginDTO loginDTO)
    {
        try
        {
            if (string.IsNullOrEmpty(loginDTO.UserName) ||
            string.IsNullOrEmpty(loginDTO.Password))
                return BadRequest("Username and/or Password not specified");
            if (loginDTO.UserName.Equals("joydip") &&
            loginDTO.Password.Equals("joydip123"))
            {
                var secretKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes("thisisasecretkey@123"));
                var signinCredentials = new SigningCredentials
               (secretKey, SecurityAlgorithms.HmacSha256);
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: "ABCXYZ",
                    audience: "http://localhost:51398",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signinCredentials
                );
                Ok(new JwtSecurityTokenHandler().
                WriteToken(jwtSecurityToken));
            }
        }
        catch
        {
            return BadRequest
            ("An error occurred in generating the token");
        }
        return Unauthorized();
    }
}


namespace TaskReporter
{
    class Program
    {
        static void Main(string[] args)
        {
                
            Console.WriteLine("Task Reporter");

            Dictionary<DateTime, List<string>> tasks = new Dictionary<DateTime, List<string>>();

            while (true)
            {
                Console.WriteLine("\n1. Yeni iþ ekle");
                Console.WriteLine("2. Günlük raporu görüntüle");
                Console.WriteLine("3. Haftalýk raporu görüntüle");
                Console.WriteLine("4. Aylýk raporu görüntüle");
                Console.WriteLine("5. Çýkýþ");

                Console.Write("\nSeçiminizi yapýn (1-5): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTask(tasks);
                        break;
                    case "2":
                        ShowDailyReport(tasks);
                        break;
                    case "3":
                        ShowWeeklyReport(tasks);
                        break;
                    case "4":
                        ShowMonthlyReport(tasks);
                        break;
                    case "5":
                        Console.WriteLine("Uygulamadan çýkýlýyor...");
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçenek! Lütfen tekrar deneyin.");
                        break;
                }
            }
        }

        static void AddTask(Dictionary<DateTime, List<string>> tasks)
        {
            Console.Write("Yeni iþin açýklamasýný girin: ");
            string description = Console.ReadLine();

            DateTime currentDate = DateTime.Now.Date;

            if (tasks.ContainsKey(currentDate))
            {
                tasks[currentDate].Add(description);
            }
            else
            {
                tasks.Add(currentDate, new List<string> { description });
            }

            Console.WriteLine("Ýþ baþarýyla eklendi.");
        }

        static void ShowDailyReport(Dictionary<DateTime, List<string>> tasks)
        {
            DateTime currentDate = DateTime.Now.Date;

            if (tasks.ContainsKey(currentDate))
            {
                Console.WriteLine($"\nGünlük Rapor ({currentDate.ToShortDateString()}):\n");

                foreach (string task in tasks[currentDate])
                {
                    Console.WriteLine("- " + task);
                }
            }
            else
            {
                Console.WriteLine("\nBugüne ait rapor bulunamadý.");
            }
        }

        static void ShowWeeklyReport(Dictionary<DateTime, List<string>> tasks)
        {
            DateTime startDate = DateTime.Now.Date.AddDays(-6);
            DateTime endDate = DateTime.Now.Date;

            Console.WriteLine($"\nHaftalýk Rapor ({startDate.ToShortDateString()} - {endDate.ToShortDateString()}):\n");

            bool hasTasks = false;

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (tasks.ContainsKey(date))
                {
                    hasTasks = true;

                    Console.WriteLine($"{date.ToShortDateString()}:");

                    foreach (string task in tasks[date])
                    {
                        Console.WriteLine("- " + task);
                    }

                    Console.WriteLine();
                }
            }

            if (!hasTasks)
            {
                Console.WriteLine("Bu hafta için rapor bulunamadý.");
            }
        }

        static void ShowMonthlyReport(Dictionary<DateTime, List<string>> tasks)
        {
            DateTime currentDate = DateTime.Now.Date;
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            DateTime startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime endDate = new DateTime(currentDate.Year, currentDate.Month, daysInMonth);
            Console.WriteLine($"\nAylýk Rapor ({startDate.ToShortDateString()} - {endDate.ToShortDateString()}):\n");

            bool hasTasks = false;

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (tasks.ContainsKey(date))
                {
                    hasTasks = true;

                    Console.WriteLine($"{date.ToShortDateString()}:");

                    foreach (string task in tasks[date])
                    {
                        Console.WriteLine("- " + task);
                    }

                    Console.WriteLine();
                }
            }

            if (!hasTasks)
            {
                Console.WriteLine("Bu ay için rapor bulunamadý.");
            }
        }
    }
}


