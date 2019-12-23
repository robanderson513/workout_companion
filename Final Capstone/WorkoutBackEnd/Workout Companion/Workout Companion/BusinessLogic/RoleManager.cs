using WorkoutCompanion.DAO;
using WorkoutCompanion.Models.Database;
using System.Collections.Generic;

namespace WorkoutCompanion.BusinessLogic
{
    /// <summary>
    /// Holds a user and manages their permissions
    /// </summary>
    public class RoleManager
    {
        public const string MEMBER = "Member";
        public const string EMPLOYEE = "Employee";
        public const string ADMINISTRATOR = "Admin";
        public const string UNKNOWN = "Unknown";

        private Dictionary<int, RoleItem> _roles = new Dictionary<int, RoleItem>();

        /// <summary>
        /// The user to manage permissions for
        /// </summary>
        public UserItem User { get; set; }

        /// <summary>
        /// The name of the user's role
        /// </summary>
        public string RoleName
        {
            get
            {
                return User != null ? _roles[User.RoleId].Name : UNKNOWN;
            }
        }

        public int AdministratorRoleId
        {
            get
            {
                return GetIdForRole(ADMINISTRATOR);
            }
        }

        public int UserRoleId
        {
            get
            {
                return GetIdForRole(MEMBER);
            }
        }

        public int EmployeeRoleId
        {
            get
            {
                return GetIdForRole(EMPLOYEE);
            }
        }

        /// <summary>
        /// Constructor for the role manager. Create this everytime a user changes.
        /// </summary>
        /// <param name="user">The user to get the permissions for</param>
        public RoleManager(IWorkoutDAO db)
        {
            if(_roles.Count == 0)
            {
                var roles = db.GetRoleItems();
                foreach(var role in roles)
                {
                    _roles.Add(role.Id, role);
                }
            }
        }

        private int GetIdForRole(string roleName)
        {
            int result = BaseItem.InvalidId;

            foreach(var role in _roles)
            {
                if(role.Value.Name == roleName)
                {
                    result = role.Key;
                }
            }

            return result;
        }

        /// <summary>
        /// Specifies if the user has administrator permissions
        /// </summary>
        public bool IsAdministrator
        {
            get
            {
                return RoleName == ADMINISTRATOR;
            }
        }

        /// <summary>
        /// Specifies if the user has standard permissions
        /// </summary>
        public bool IsMember
        {
            get
            {
                return RoleName == MEMBER;
            }
        }
        /// <summary>
        /// Specifies if the user has standard permissions
        /// </summary>
        public bool IsEmployee
        {
            get
            {
                return RoleName == EMPLOYEE;
            }
        }

        /// <summary>
        /// Specifies if the user has unknown permissions
        /// </summary>
        public bool IsUnknown
        {
            get
            {
                return RoleName == UNKNOWN;
            }
        }
    }
}
