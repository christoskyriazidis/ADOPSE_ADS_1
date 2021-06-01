using DypaApi.Interfaces;
using DypaApi.Models.Weather;
using DypaApi.Models.Weather.SingleCall;
using DypaApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DypaApi.Controllers
{
    public class WorkerController : Controller
    {
        private readonly IWorker _workerRepo = new WorkerRepository();

        [HttpGet]
        [Route("/worker")]
        public IActionResult GetWorks()
        {
            return Json(_workerRepo.GetWorkers());
        }

        

    }
}
