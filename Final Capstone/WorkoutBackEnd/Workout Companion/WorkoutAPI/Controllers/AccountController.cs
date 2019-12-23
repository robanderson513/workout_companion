using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutAPI.Models;
using WorkoutAPI.Models.DTO;
using WorkoutAPI.Security;
using WorkoutCompanion.BusinessLogic;
using WorkoutCompanion.DAO;
using WorkoutCompanion.Models;
using WorkoutCompanion.Models.Database;

namespace WorkoutAPI.Controllers
{
    /// <summary>
    /// Creates a new account controller used to authenticate the user.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ITokenGenerator _tokenGenerator;
        private IWorkoutDAO _db;

        /// <summary>
        /// Creates a new account controller.
        /// </summary>
        /// <param name="tokenGenerator">A token generator used when creating auth tokens.</param>
        /// <param name="db">Access to the BuddyBux database.</param>
        public AccountController(ITokenGenerator tokenGenerator, IWorkoutDAO db)
        {
            _tokenGenerator = tokenGenerator;
            _db = db;
        }

        /// <summary>
        /// Registers a user and provides a bearer token.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("/registerUser")]
        public IActionResult Register(RegisterAccountViewModel model)
        {
            IActionResult result = null;

            // Does user already exist
            try
            {
                // Generate a password hash
                var pwdMgr = new PasswordManager(model.Password);
                var roleMgr = new RoleManager(_db);

                // Create a user object
                var user = new UserItem
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Hash = pwdMgr.Hash,
                    Salt = pwdMgr.Salt,
                    Email = model.Email,
                    RoleId = roleMgr.UserRoleId,
                    WorkoutGoals = model.Goals,
                    Username = model.UserName,
                    WeeklyExercise = model.WeeklyExercise,
                    ExperienceLevel = model.ExperienceLevel
                };

                var userAccount = new UserAccountModel
                {
                    User = user
                };

                // Save the user account
                _db.AddUserAccount(userAccount);

                // Generate a token
                var token = _tokenGenerator.GenerateToken(user.Username, roleMgr.RoleName);

                result = Ok(token);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Username has already been taken." });
                //result = Ok(new { error = true });
            }

            // Return the token
            return result;
        }

        /// <summary>
        /// Authenticates the user and provides a bearer token.
        /// </summary>
        /// <param name="model">An object including the user's credentials.</param>
        /// <returns></returns>
        [HttpPost("/loginUser")]
        public IActionResult Login(AuthenticationViewModel model)
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();

            try
            {
                var roleMgr = new RoleManager(_db)
                {
                    //User = _db.GetUserItemByEmail(model.Email)
                    User = _db.GetUserItemByLogin(model.EmailOrUsername)
                };

                // Generate a password hash
                var pwdMgr = new PasswordManager(model.Password, roleMgr.User.Salt);

                if (pwdMgr.Verify(roleMgr.User.Hash))
                {
                    // Create an authentication token
                    var token = _tokenGenerator.GenerateToken(roleMgr.User.Username,
                        roleMgr.RoleName);

                    // Switch to 200 OK
                    result = Ok(token);
                }
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Username or password is invalid." });
            }

            return result;
        }

        [HttpPut("/updateProfile")]
        //[Authorize(Roles = "Admin")]
        public IActionResult UpdateProfile(UpdateProfileModel model)
        {
            IActionResult result = Unauthorized();
            try
            {
                UserItem u = _db.GetUserItemByLogin(model.Username);
                u.Email = model.Email;
                u.FirstName = model.FirstName;
                u.LastName = model.LastName;
                u.PhotoURL = model.PhotoURL;
                u.WorkoutGoals = model.Goals;
                _db.UpdateUserProfile(u);
                result = Ok();
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Role update failed." });
            }
            return result;
        }

        [HttpPost("/logoutUser")]
        public IActionResult Logout()
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();
            try
            {
                var rmgr = new RoleManager(_db);
                rmgr.User = new UserItem();

                // Switch to 200 OK
                result = Ok();
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "User failed to sign out" });
            }

            return result;
        }

        [HttpGet("/userProfile")]
        [Authorize]
        public IActionResult GetUser()
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();

            try
            {
                var user = _db.GetUserItemByLogin(User.Identity.Name);
                UserDTO _dto = MapToDTO(user);

                result = Ok(_dto);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get user failed." });
            }

            return result;
        }

        [HttpGet("/userProfile/{userName}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult GetUser(string userName)
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();

            try
            {
                var user = _db.GetUserItemByLogin(userName);
                UserDTO _dto = MapToDTO(user);
                result = Ok(_dto);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get user failed." });
            }

            return result;
        }

        [HttpGet("/searchUsers/{firstName}/{lastName}")]
        //[Authorize]
        public IActionResult GetUsers(string firstName, string lastName)
        {
            IActionResult result = Unauthorized();
            try
            {
                IList<UserItem> users = _db.GetUsersByName(firstName, lastName);
                var dtoArr = users.Select(x => MapToDTO(x)).ToArray();
                result = Ok(dtoArr);

                // We don't return the domain object, use DTO
                // result = Ok(users);
            }
            catch (Exception)
            {
                result = BadRequest(new { Message = "Get users failed." });
            }

            return result;
        }

        private UserDTO MapToDTO(UserItem user)
        {
            UserDTO userDTO = new UserDTO()
            {
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleId = user.RoleId,
                PhotoURL = user.PhotoURL,
                CreationDate = user.CreationDate,
                WorkoutGoals = user.WorkoutGoals,
                ExperienceLevel = user.ExperienceLevel,
                WeeklyExercise = user.WeeklyExercise
            };
            return userDTO;
        }
    }
}