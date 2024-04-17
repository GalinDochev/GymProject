using Gym_Project.Models;
using Gym_Project.Models.HomeModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using GymProject.Common.Constants;

namespace Gym_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        [HttpGet]
        public  IActionResult ContactUs()
        {
            var model = new EmailViewModel();
            return  View(model);
        }

        [HttpPost]
        public IActionResult SendEmail(EmailViewModel model)
        {
            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    var emailFrom = model.From;
                    var username = _configuration["GmailCredentials:Username"];
                    var password = _configuration["GmailCredentials:Password"];
                    MailMessage mm = new MailMessage(username, model.EmailAddress);
                    mm.Subject = model.Subject;
                    mm.Body = $"{model.Message}{Environment.NewLine}{Environment.NewLine}Message is from: {emailFrom}";
                    mm.IsBodyHtml = false;
                    smtpClient.Credentials = new NetworkCredential(username, password);

                    smtpClient.Send(mm);
                    ViewBag.Message = "Message sent";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in ContactUs/HomeController");
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] = $"{CustomExceptionMessages.FailedEmailSend}"; ;
                return RedirectToAction(nameof(ContactUs));
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}