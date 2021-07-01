using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WarehouseGrpc.TestWebApp.Models;

namespace WarehouseGrpc.TestWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        private static readonly GrpcChannel GrpcChannel = GrpcChannel.ForAddress("https://localhost:5001");

        public IActionResult Index()
        {
            var client = new Warehouse.WarehouseClient(GrpcChannel);
            var cranes = new List<Crane>();
            var craneStream = client.GetCranes(new ID { Id = 0 }).ResponseStream;
            while (craneStream.MoveNext(System.Threading.CancellationToken.None).Result)
            {
                cranes.Add(craneStream.Current);
            }
            ViewBag.Cranes = cranes;
            return View();
        }

        public IActionResult Privacy()
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
