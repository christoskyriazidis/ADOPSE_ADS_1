using DypaApi.Helpers;
using DypaApi.Interfaces;
using DypaApi.Models.PostRequest;
using DypaApi.Models.Xorafi;
using DypaApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Controllers
{
    public class XorafiController : Controller
    {
       
        private readonly IXorafi _xorafiRepo = new XorafiRepository();
        private readonly IWorker _workerRepo = new WorkerRepository();

        [Authorize]
        [HttpPost]
        [Route("/xorafi")]
        public IActionResult AddXorafi([FromBody] Xorafi xorafi)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            if (string.IsNullOrEmpty(xorafi.LocationTitle))
            {
                xorafi.LocationTitle = "UnnamedLocation";
            }
            if (string.IsNullOrEmpty(xorafi.Title))
            {
                xorafi.Title = "UnnamedTitle";
            }
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _workerRepo.GetCustomerIdFromSub(subId);
            xorafi.Owner = intId;
            if (_xorafiRepo.AddXorafi(xorafi))
            {
                return Json(new { response ="Xorafi added!"});
            }
            return BadRequest(new {response="Something went wrong while trying add new xorafi " });
        }

        [HttpGet]
        [Route("/xorafi/{xid}")]
        public IActionResult GetXorafi(int xid)
        {
            var xorafi = _xorafiRepo.GetXorafi(xid);
            if (xorafi!=null)
            {
                return Json(new { xorafi = xorafi });
            }
            return BadRequest(new {response="Something went wrong maybe wrong xorafi id " });
        }
        
        [Authorize]
        [HttpDelete]
        [Route("/xorafi/{xid}")]
        public IActionResult DeleteXorafi(int xid)
        {
            if (xid < 0)
            {
                return BadRequest(new {response=$"Lathos id:{xid}"});
            }
            var xorafi = _xorafiRepo.RemoveXorafi(xid);
            if (xorafi)
            {
                return Json(new { response=$"Xorafi removed:{xid}" });
            }
            return BadRequest(new {response="Something went wrong maybe wrong xorafi id " });
        }

        [HttpPost]
        [Route("/xorafi/sensor")]
        public IActionResult AddSensorToXorafiAsync([FromBody] SensorToXorafi SensorID)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            if (_xorafiRepo.AddSensorToXorafi(SensorID))
            {
                return Json(new { response="Sensor Added to xorafi"});
            }
            return BadRequest(new { response="Kati pige la8os"+SensorID});
        }


    }
}
