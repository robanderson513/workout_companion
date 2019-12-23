using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutAPI.Models;
using WorkoutAPI.Models.DTO;
using WorkoutCompanion.DAO;
using WorkoutCompanion.Models.Database;

namespace WorkoutAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private IWorkoutDAO _db;
        public WorkoutController(IWorkoutDAO db)
        {
            _db = db;
        }

        [HttpGet("/displayAllEquipment")]
        [Authorize]
        public IActionResult DisplayEquipment()
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();
            try
            {
                var workout = _db.GetEquipment();
                var myDTOs = workout.Select(x => MapToDTO(x)).ToArray();
                result = Ok(myDTOs);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get workout failed." });
            }
            return result;
        }

      

       

        [HttpGet("/displayEquipment/{id}")]
        [Authorize]
        public IActionResult DisplayEquipmentById(int id)
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();
            try
            {
                var workout = _db.GetEquipmentById(id);
                EquipmentDTO equip = MapToDTO(workout);
                result = Ok(equip);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get workout failed." });
            }
            return result;
        }

        [HttpPost("/addNewMachine/{model}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult AddNewMachine(NewMachineViewModel model)
        {
            IActionResult result = null;
            try
            {
                var newMachine = new EquipmentItem()
                {
                    Name = model.Name,
                    Description = model.Description,
                    CategoryId = model.Category,
                    ImageURL = model.ImageURL,
                    VideoLink = model.VideoURL,
                    MachineNumber = model.Number
                };

                // Save the user account
                _db.AddNewMachine(newMachine);

                result = Ok();
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Failed to add machine" });
            }
            return result;
        }

        [HttpDelete("/removeMachine/{id}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult RemoveMachine(int id)
        {
            IActionResult result = Unauthorized();
            try
            {
                _db.RemoveEquipmentById(id);
                result = Ok();
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Failed to remove machine" });
            }
            return result;
        }

        [HttpGet("/userMetrics")]
        //[Authorize]
        // delete default assignment, request should have argument
        // in querystring or api should return no data
        public IActionResult DisplayUserMetrics(int days = 7)
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();
            //testing - set userName to User.Identity.Name for deployment
            string userName = String.IsNullOrEmpty(User.Identity.Name) ? "pleshekc" : User.Identity.Name;
            try
            {
                var visits = (List<VisitItem>)_db.GetVisitsByUserDateRange(userName,
                    DateTime.UtcNow.AddDays(days * -1), DateTime.UtcNow);

                MetricsViewModel vm = new MetricsViewModel();

                var uvm = _db.GetUserVisitMetrics(userName, days);

                VisitMetricsDTO uvmDTO = new VisitMetricsDTO()
                {
                    AvgDuration = uvm.AvgDuration,
                    CountVisits = uvm.CountVisits,
                    SumDuration = uvm.SumDuration
                };

                vm.AvgDuration = uvmDTO.AvgDuration;
                // Top Five Machines should be refactored for the same
                // dange range as the call to this action
                vm.TopFiveWorkouts = _db.TopFiveMachines(userName);
                visits.ForEach(visit =>
                {
                    VisitMetricsItem vmi = new VisitMetricsItem();
                    vmi.Workouts = _db.WorkoutsPerVisit(visit);
                    vmi.Date = visit.CheckIn.Date;
                    if (vmi.Workouts.Count > 0)
                    {

                        vmi.SumOfDuration = vmi.Workouts
                        .Select(x => x.Duration).Sum();

                    }
                    vm.WeeklyWorkouts.Add(vmi);
                });

                result = Ok(vm);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get workout failed." });
            }
            return result;
        }
        [HttpGet("/userMetrics/{userName}")]
        //[Authorize]
        public IActionResult DisplayUserMetrics(string userName, int days=7)
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();
            try
            {
                var visits = (List<VisitItem>)_db.GetVisitsByUserDateRange(userName,
                    DateTime.UtcNow.AddDays(days * -1), DateTime.UtcNow);

                MetricsViewModel vm = new MetricsViewModel();

                var uvm = _db.GetUserVisitMetrics(userName, days);

                VisitMetricsDTO uvmDTO = new VisitMetricsDTO()
                {
                    AvgDuration = uvm.AvgDuration,
                    CountVisits = uvm.CountVisits,
                    SumDuration = uvm.SumDuration
                };

                vm.AvgDuration = uvmDTO.AvgDuration;
                vm.TopFiveWorkouts = _db.TopFiveMachines(userName);
                visits.ForEach(visit =>
                {
                    VisitMetricsItem vmi = new VisitMetricsItem();
                    vmi.Workouts = _db.WorkoutsPerVisit(visit);
                    vmi.Date = visit.CheckIn.Date;
                    if (vmi.Workouts.Count > 0)
                    {

                        vmi.SumOfDuration = vmi.Workouts
                        .Select(x => x.Duration).Sum();

                    }
                    vm.WeeklyWorkouts.Add(vmi);
                });
                //  vm.WeeklyWorkouts
                //      .Select(w => w.SumOfDuration = w.Workouts.Sum(x => x.Duration));



                //IList<VisitItem> visits = _db.GetVisitsByUserDateRange(userName,
                //    DateTime.UtcNow.AddDays(days * -1), DateTime.UtcNow);
                //MetricsViewModel vm = new MetricsViewModel();
                ////   vm.averageTime = _db.GetAvgTimeSpent(userName, days);
                //var uvm = _db.GetUserVisitMetrics(userName, days);

                //VisitMetricsDTO uvmDTO = new VisitMetricsDTO()
                //{
                //    AvgDuration = uvm.AvgDuration,
                //    CountVisits = uvm.CountVisits,
                //    SumDuration = uvm.SumDuration
                //};
                //vm.AvgDuration = uvmDTO.AvgDuration;
                //vm.TopFiveWorkouts = _db.TopFiveMachines(userName);
                //foreach (var v in visits)
                //{
                //    IList<WorkoutModel> workoutsInVisit = new List<WorkoutModel>();
                //    VisitMetricsItem vmi = new VisitMetricsItem();
                //    workoutsInVisit = _db.WorkoutsPerVisit(v);
                //    vmi.Workouts = workoutsInVisit;
                //    vmi.Date = v.CheckIn.Date;
                //    vm.WeeklyWorkouts.Add(vmi);
                //}
                //foreach (var v in vm.WeeklyWorkouts)
                //{
                //    foreach (var v2 in v.Workouts)
                //    {
                //        v.SumOfDuration += v2.Duration;
                //    }
                //}
                result = Ok(vm);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get workout failed." });
            }
            return result;
        }

        //[HttpGet("/userMetrics")]
        ////[Authorize]
        //public IActionResult DisplayUserMetrics(int days=30)
        //{
        //    // Assume the user is not authorized
        //    IActionResult result = Unauthorized();
        //    try
        //    {
        //        IList<VisitItem> visits = _db.GetVisitsByUserDateRange(User.Identity.Name,
        //            DateTime.UtcNow.AddDays(-days), DateTime.UtcNow);
        //        MetricsViewModel vm = new MetricsViewModel();
        //        vm.AvgDuration = _db.GetAvgTimeSpent(User.Identity.Name, days);
        //        vm.TopFiveWorkouts = _db.Top5Machines(User.Identity.Name);
        //        foreach (var v in visits)
        //        {
        //            IList<WorkoutModel> workoutsInVisit = new List<WorkoutModel>();
        //            VisitMetricsItem vmi = new VisitMetricsItem();
        //            workoutsInVisit = _db.WorkoutsPerVisit(v);
        //            vmi.Workouts = workoutsInVisit;
        //            vmi.Date = v.CheckIn.Date;
        //            vm.WeeklyWorkouts.Add(vmi);
        //        }
        //        foreach (var v in vm.WeeklyWorkouts)
        //        {
        //            foreach (var v2 in v.Workouts)
        //            {
        //                v.SumOfDuration += v2.Duration;
        //            }
        //        }
        //        string myAvg = string.Format("{0:0.0}", vm.AvgDuration);
        //        vm.AvgDuration = decimal.Parse(myAvg);
        //        result = Ok(vm);
        //    }
        //    catch (Exception)
        //    {
        //        result = BadRequest(new { Message = "Get workout failed." });
        //    }
        //    return result;
        //}
        //[HttpGet("test")]
        ////[Authorize]
        //public IActionResult DisplayUserMetrics(string userName, int days)
        //{
        //    // Assume the user is not authorized
        //    IActionResult result = Unauthorized();
        //    try
        //    {
        //        IList<VisitItem> visits = _db.GetVisitsByUserDateRange(_db.GetUserItemByLogin(userName),
        //            DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
        //        MetricsViewModel vm = new MetricsViewModel();
        //        vm.averageTime = _db.GetAvgTimeSpent(userName, days);
        //        vm.top5Workouts = _db.Top5Machines(userName);
        //        foreach (var v in visits)
        //        {
        //            IList<WorkoutModel> workoutsInVisit = new List<WorkoutModel>();
        //            VisitMetricsItem vmi = new VisitMetricsItem();
        //            workoutsInVisit = _db.WorkoutsPerVisit(v);
        //            vmi.Workouts = workoutsInVisit;
        //            vmi.Date = v.CheckIn.Date;
        //            vm.weeklyWorkouts.Add(vmi);
        //        }
        //        foreach (var v in vm.weeklyWorkouts)
        //        {
        //            foreach (var v2 in v.Workouts)
        //            {
        //                v.SumOfDuration += v2.Duration;
        //            }
        //        }
        //        result = Ok(vm);
        //    }
        //    catch (Exception)
        //    {
        //        result = BadRequest(new { Message = "Get workout failed." });
        //    }
        //    return result;
        //}

        private EquipmentDTO MapToDTO(EquipmentItem eqItem)
        {
            EquipmentDTO eqDTO = new EquipmentDTO()
            {
                Id = eqItem.Id,
                Name = eqItem.Name,
                CategoryId = eqItem.CategoryId,
                Description = eqItem.Description,
                ImageURL = eqItem.ImageURL,
                VideoLink = eqItem.VideoLink,
                MachineNumber = eqItem.MachineNumber
            };
            return eqDTO;
        }


    }
}
