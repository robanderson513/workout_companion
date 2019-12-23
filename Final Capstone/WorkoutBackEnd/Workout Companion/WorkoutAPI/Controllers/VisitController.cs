using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutAPI.Models.DTO;
using WorkoutCompanion.DAO;
using WorkoutCompanion.Models.Database;

namespace WorkoutAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitController : ControllerBase
    {

        private readonly IWorkoutDAO _db;
        public VisitController(IWorkoutDAO db)
        {
            _db = db;
        }

        [HttpGet("/allVisits")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult GetAll()
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();

            try
            {
                var visits = _db.GetVisits();
                result = Ok(visits);

            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get visit failed." });
            }

            return result;
        }

        [HttpGet("/allVisits/{userName}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult GetAllByUser(string userName)
        {

            IActionResult result = Unauthorized();
            try
            {
                var visits = _db.GetVisitsByUser(userName);
                var visitArr = visits.Select(x => MapToDTO(x)).ToArray();
                result = Ok(visitArr);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Failed to get current visit" });
            }
            return result;
        }



        [HttpGet("/getVisit/{id}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult Get(int id)
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();

            try
            {
                VisitItem visit = _db.GetVisit(id);
                VisitDTO dTO = MapToDTO(visit);
                result = Ok(dTO);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get visit failed." });
            }

            return result;
        }

        [HttpGet("/userVisitsRange/{userName}/{dtStart}/{dtEnd}")]
        public IActionResult Get(string userName, DateTime dtStart, DateTime dtEnd)
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();

            try
            {

                var visits = _db.GetVisitsByUserDateRange(userName, dtStart, dtEnd);
                var dtoArr = visits.Select(x => MapToDTO(x)).ToArray();
                result = Ok(dtoArr);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get visits failed." });
            }

            return result;
        }

        [HttpPut("/checkOutUser/{userName}")]
        public IActionResult CheckOut(string userName)
        {
            IActionResult result = NoContent();

            var existingVisit = _db.GetCurrentVisit(userName);
            if (existingVisit == null)
            {
                result = NotFound();
            }
            else
            {
                existingVisit.CheckOut = DateTime.UtcNow;
                _db.UpdateVisit(existingVisit);
                result = Ok();
            }
            return result;
        }

        [HttpPost("/checkInUser/{userName}")]
        //[Authorize]
        public IActionResult CheckIn(string userName)
        {
            IActionResult result = Unauthorized();
            try
            {
                UserItem myUser = _db.GetUserItemByLogin(userName);
                // UserItem myUser = _db.GetUserItemByLogin(User.Identity.Name);
                VisitItem newVisit = new VisitItem()
                {
                    UserId = myUser.Id,
                    CheckIn = DateTime.UtcNow
                };
                newVisit.Id = _db.CreateVisit(newVisit);

                // Switch to 200 OK
                if (newVisit.Id > 0)
                {
                    result = Ok(newVisit.Id);
                }

            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Failed to checkin user" });
            }
            return result;
        }

        [HttpGet("/currentVisit")]
        [Authorize]
        public IActionResult GetCurrent()
        {

            string userName = User.Identity.Name;


            IActionResult result = Unauthorized();
            try
            {
                //VisitItem visit = _db.GetCurrentVisit(userName);
                VisitItem visit = _db.GetCurrentVisit(User.Identity.Name);
                VisitDTO dTo = MapToDTO(visit);
                result = Ok(dTo);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Failed to get current visit" });
            }
            return result;
        }

        [HttpGet("/currentVisit/{userName}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult GetCurrent(string userName)
        {

            IActionResult result = Unauthorized();
            try
            {
                VisitItem visit = _db.GetCurrentVisit(userName);
                VisitDTO dTo = MapToDTO(visit);
                result = Ok(dTo);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Failed to get current visit" });
            }
            return result;
        }









        private VisitDTO MapToDTO(VisitItem visitFrom)
        {
            VisitDTO visit = new VisitDTO()
            {
                Id = visitFrom.Id,
                UserId = visitFrom.UserId,
                CheckIn = visitFrom.CheckIn,
                CheckOut = visitFrom.CheckOut
            };

            return visit;

        }
    }
}
