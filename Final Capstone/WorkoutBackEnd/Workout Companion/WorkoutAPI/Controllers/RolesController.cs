using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutCompanion.DAO;
using WorkoutCompanion.Models.Database;

namespace WorkoutAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        IWorkoutDAO _db;
        public RolesController(IWorkoutDAO db)
        {
            _db = db;
        }
        [HttpPut("/editUserRole/{userName}/{roleId}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult PromoteUser(string userName, int roleId)
        {
            IActionResult result = Unauthorized();
            try
            {
                if (roleId < 4 && roleId > 0)
                {
                    _db.UpdateUserRole(_db.GetUserItemByLogin(userName), roleId);
                }
                result = Ok();
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Role update failed." });
            }
            return result;
        }
        [HttpGet("/getUsernameAndRole")]
        //[Authorize]
        public IActionResult GetUserNameAndRole()
        {
            IActionResult result = Unauthorized();
            try
            {
                UserItem myUser = _db.GetUserItemByLogin(User.Identity.Name);
                var userInfo = new { UserName = myUser.Username, RoleId = myUser.RoleId, FirstName = myUser.FirstName};
                result = Ok(userInfo);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Fuck." });
            }
            return result;
        }
    }
}