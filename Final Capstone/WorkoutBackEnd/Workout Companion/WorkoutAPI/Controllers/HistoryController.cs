using System;
using System.Collections.Generic;
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
    public class HistoryController : ControllerBase
    {
        private IWorkoutDAO _db;
        public HistoryController(IWorkoutDAO db)
        {
            _db = db;
        }


        [HttpGet("/userHistory/{userName}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult DisplayUserHistory(string userName)
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();
            try
            {
                var user = _db.GetUserItemByLogin(userName);
                var history = _db.GetUserHistory(user);

                result = Ok(history);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get history for user failed." });
            }
            return result;
        }

        [HttpGet("/equipmentHistory/{id}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult DisplayEquipmentHistory(int id)
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();
            try
            {
                EquipmentItem equip = _db.GetEquipmentById(id);
                IList<UsageItem> history = _db.GetEquipmentHistory(equip);
                result = Ok(history);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get equipment history failed." });
            }
            return result;
        }

        [HttpGet("/equipmentHistory/summary")]
        public IActionResult DisplayEquipmentUseSummary()
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();
            try
            {
                IList<EquipUse> equs = _db.GetEquipUses();
                var equArr = equs.Select(x => MapToDTO(x)).ToArray();
                result = Ok(equArr);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get equipment history failed." });
            }
            return result;

        }

        // POST: api/History
        [HttpPost("/logMachineUse")]
        [Authorize]
        public IActionResult UseMachine(UsageItem useItem)
        {
            IActionResult result = Unauthorized();
            try
            {
                result = Ok(_db.LogEquipmentUsage(useItem));
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Failed to log machine use" });
            }
            return result;
        }

        private EquipUseDTO MapToDTO(EquipUse equ)
        {
            EquipUseDTO dto = new EquipUseDTO()
            {
                EquipId = equ.EquipId,
                EquipName = equ.EquipName,
                CountUses = equ.CountUses,
                SumUses = equ.SumUses
            };

            return dto;
        }

    }
}
