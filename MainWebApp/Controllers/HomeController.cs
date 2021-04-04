using MainWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MainWebApp.Controllers
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
            ViewBag.MasterContent = "Master content from: " + LocalAddress();
            ViewBag.ForumContent = "Forum application url not set";
            ViewBag.FeedbackContent = "Feedback application url not set";
            return View();
        }

        public IActionResult UpdateEndpoints(Endpoints model )
        {
            if (model.FeedbackEndpoint == null || model.ForumEndpoint == null)
            {
                ViewBag.MasterContent = "Master content from: " + LocalAddress();
                ViewBag.ForumContent = "Forum application url not set";
                ViewBag.FeedbackContent = "Feedback application url not set";
                return View("Index");
            }

            ViewBag.MasterContent = "Master content from: " + LocalAddress();

            string ForumUrl = model.ForumEndpoint;
            string FeedbackUrl = model.FeedbackEndpoint;

            string ForumContent = "";
            using (WebClient client = new WebClient())
            {
                ForumContent = client.DownloadString(ForumUrl + "/Home/Forum");
            }

            string FeedbackContent = "";
            using (WebClient client = new WebClient())
            {
                FeedbackContent = client.DownloadString(FeedbackUrl + "/Home/Feedback");
            }

            ViewBag.ForumContent = ForumContent;
            ViewBag.FeedbackContent = FeedbackContent;

            return View("Index");
        }

        public IActionResult Forum()
        {
            ViewBag.IP = LocalAddress();
            return View();
        }

        public IActionResult Feedback()
        {
            ViewBag.IP = LocalAddress();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string LocalAddress()
        {
            string localIP = "";
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        }
    }
}
