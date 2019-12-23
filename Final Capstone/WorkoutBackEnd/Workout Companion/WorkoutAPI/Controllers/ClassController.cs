using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutAPI.Models.DTO;
using WorkoutCompanion.DAO;
using WorkoutCompanion.Models;
using WorkoutCompanion.Models.Database;

namespace WorkoutAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IWorkoutDAO _db;
        public ClassController(IWorkoutDAO db)
        {
            _db = db;
        }


        [HttpGet("/allClasses")]
        //  [Authorize(Roles = "Admin, Employee")]
        public IActionResult GetAll()
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();

            try
            {
                var classes = _db.GetClasses();
                var dtoArr = classes.Select(x => MapToDTO(x)).ToArray();
                result = Ok(dtoArr);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get all classes failed." });
            }

            return result;
        }

        [HttpGet("/getClass/{id}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult Get(int id)
        {
            IActionResult result = Unauthorized();

            try
            {
                var dto = MapToDTO(_db.GetClass(id));
                result = Ok(dto);

            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get all classes failed." });
            }

            return result;
        }

        [HttpDelete("/removeClass/{id}")]
        //[Authorize(Roles = "Admin, Employee")]
        public IActionResult Delete(int id)
        {
            IActionResult result = Unauthorized();
            try
            {

                _db.DeleteClass(id);
                result = Ok();
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Failed to remove class" });
            }
            return result;
        }

        [HttpGet("/classesOnDate/{date}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult GetAll(DateTime date)
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();

            try
            {
                var classes = _db.GetClassesByDate(date);
                var dtoArr = classes.Select(x => MapToDTO(x)).ToArray();
                result = Ok(dtoArr);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get all classes failed." });
            }

            return result;
        }

        [HttpGet("/classesOnDateRange/{dtStart}/{dtEnd}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult GetAll(DateTime dtStart, DateTime dtEnd)
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();

            try
            {
                var classes = _db.GetClassesByDateRange(dtStart, dtEnd);
                var dtoArr = classes.Select(x => MapToDTO(x)).ToArray();
                result = Ok(dtoArr);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get all classes failed." });
            }

            return result;
        }

        [HttpPost("/createClass/{newClass}")]
        //  [Authorize(Roles = "Admin, Employee")]
        public IActionResult Create(ClassDTO newClass)
        {
            IActionResult result = null;


            try
            {

                ClassItem classItem = new ClassItem()
                {
                    Name = newClass.Name,
                    Color = newClass.Color,
                    Date = newClass.Date,
                };

                newClass.Id = _db.CreateClass(classItem);

                result = Ok(newClass);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Class not created" });
            }
            // Return the token
            return result;
        }

        [HttpGet("/classAttendants/{classId}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult GetAllUsers(int classId)
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();

            try
            {
                var users = _db.GetUsersByClassId(classId);
                var dtoArr = users.Select(x => MapToDTO(x)).ToArray();
                result = Ok(dtoArr);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get all users failed." });
            }

            return result;
        }
        [HttpGet("/classes/{userName}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult GetAllClassesByUser(string userName)
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();

            try
            {
                var classes = _db.GetClassesByUserName(userName);
                var dtoArr = classes.Select(x => MapToDTO(x)).ToArray();
                result = Ok(dtoArr);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get all classes failed." });
            }

            return result;
        }

        [HttpPost("/addUserToClass/{userName}/{classId}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult AddUserToClass(string userName, int classId)
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();

            try
            {
                var row = _db.AddUserToClass(userName, classId);
                result = Ok(row);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Failed to add user to class" });
            }

            return result;
        }

        [HttpDelete("/removeUserFromClass/{userName}/{classId}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult DeleteUserFromClass(string userName, int classId)
        {
            IActionResult result = Unauthorized();
            try
            {

                _db.DeleteUserFromClass(userName, classId);
                result = Ok();
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Failed to remove user from class" });
            }
            return result;
        }

        private ClassDTO MapToDTO(ClassItem classFrom)
        {
            ClassDTO visit = new ClassDTO()
            {
                Id = classFrom.Id,
                Name = classFrom.Name,
                Color = classFrom.Color,
                Date = classFrom.Date,
            };

            return visit;

        }

        private UserClassDTO MapToDTO(UserClassModel ucm)
        {
            UserClassDTO ucDTO = new UserClassDTO()
            {
                UserId = ucm.UserId,
                ClassId = ucm.ClassId,
                UserName = ucm.UserName,
                ClassName = ucm.ClassName,
                ClassDate = ucm.ClassDate
            };

            return ucDTO;

        }

    }
}