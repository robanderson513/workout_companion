using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WorkoutCompanion.BusinessLogic;
using WorkoutCompanion.Models;
using WorkoutCompanion.Models.Database;


namespace WorkoutCompanion.DAO
{
    public class WorkoutDAO : IWorkoutDAO
    {
        private const string _getLastIdSQL = "SELECT CAST(SCOPE_IDENTITY() as int);";
        private readonly string _connectionString;
        public WorkoutDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region UserTable
        private UserItem GetUserFromReader(SqlDataReader reader)
        {
            UserItem result = new UserItem
            {
                Id = Convert.ToInt32(reader["Id"]),
                Username = Convert.ToString(reader["DisplayName"]),
                Email = Convert.ToString(reader["Email"]),
                Salt = Convert.ToString(reader["Salt"]),
                Hash = Convert.ToString(reader["Hash"]),
                RoleId = Convert.ToInt32(reader["RoleId"]),
                FirstName = Convert.ToString(reader["FirstName"]),
                LastName = Convert.ToString(reader["LastName"]),
                PhotoURL = Convert.ToString(reader["ImageURL"]),
                WorkoutGoals = Convert.ToString(reader["Goals"]),
                CreationDate = Convert.ToDateTime(reader["CreationDate"]),
                ExperienceLevel = Convert.ToString(reader["ExperienceLevel"]),
                WeeklyExercise = Convert.ToString(reader["WeeklyExercise"])
            };
            return result;
        }
        public UserItem GetUserItemByLogin(string login)
        {
            UserItem result = new UserItem();
            const string sql = "SELECT * from [User] WHERE Email = @login OR DisplayName = @login;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@login", login);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result = GetUserFromReader(reader);
                }
            }
            return result;
        }
        public IList<UserItem> GetUserItemsByRole(RoleItem role)
        {
            IList<UserItem> result = new List<UserItem>();
            const string sql = "SELECT * from [User] WHERE RoleId = @roleId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@roleId", role.Id);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UserItem user = new UserItem();
                    user = GetUserFromReader(reader);
                    result.Add(user);
                }
            }
            return result;
        }
        public int AddUserAccount(UserAccountModel userAccount)
        {
            const string sql = "INSERT [User] (" +
                "FirstName, " +
                "LastName, " +
                "Email, " +
                "Hash, " +
                "Salt, " +
                "RoleId, " +
                "ImageURL, " +
                "Goals, " +
                "DisplayName, " +
                "ExperienceLevel, " +
                "WeeklyExercise, " +
                "CreationDate) " +
                "VALUES (" +
                "@FirstName, " +
                "@LastName, " +
                "@Email, " +
                "@Hash, " +
                "@Salt, " +
                "@RoleId, " +
                "@ImageURL, " +
                "@Goals, " +
                "@Username, " +
                "@Exp, " +
                "@WE, " +
                "@CreationDate);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@FirstName", userAccount.User.FirstName);
                cmd.Parameters.AddWithValue("@LastName", userAccount.User.LastName);
                cmd.Parameters.AddWithValue("@Email", userAccount.User.Email);
                cmd.Parameters.AddWithValue("@Hash", userAccount.User.Hash);
                cmd.Parameters.AddWithValue("@Salt", userAccount.User.Salt);
                cmd.Parameters.AddWithValue("@RoleId", userAccount.User.RoleId);
                cmd.Parameters.AddWithValue("@ImageURL", userAccount.User.PhotoURL);
                cmd.Parameters.AddWithValue("@Goals", userAccount.User.WorkoutGoals);
                cmd.Parameters.AddWithValue("@Username", userAccount.User.Username);
                cmd.Parameters.AddWithValue("@CreationDate", userAccount.User.CreationDate);
                cmd.Parameters.AddWithValue("@Exp", userAccount.User.ExperienceLevel);
                cmd.Parameters.AddWithValue("@WE", userAccount.User.WeeklyExercise);
                userAccount.User.Id = (int)cmd.ExecuteScalar();
            }

            return userAccount.User.Id;
        }
        public void ChangeUserPassword(UserItem User, string newPassword)
        {
            const string sql = "UPDATE [User] SET Salt = @salt, " +
                               "Hash = @hash, " +
                               "WHERE id = @id;";
            var pwdMgr = new PasswordManager(newPassword);
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@salt", pwdMgr.Salt);
                cmd.Parameters.AddWithValue("@hash", pwdMgr.Hash);
                cmd.Parameters.AddWithValue("@id", User.Id);
                if (cmd.ExecuteNonQuery() != 1)
                {
                    throw new Exception("Failed to update the user password.");
                }
            }
        }
        public void UpdateUserProfile(UserItem User)
        {
            const string sql = "UPDATE [User] SET FirstName = @FN, " +
                               "LastName = @LN, " +
                               "Email = @Email, " +
                               "ImageURL = @Image, " +
                               "Goals = @Goal " +
                               "WHERE id = @id;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", User.Id);
                cmd.Parameters.AddWithValue("@FN", User.FirstName);
                cmd.Parameters.AddWithValue("@LN", User.LastName);
                cmd.Parameters.AddWithValue("@Email", User.Email);
                cmd.Parameters.AddWithValue("@Image", User.PhotoURL);
                cmd.Parameters.AddWithValue("@Goal", User.WorkoutGoals);
                if (cmd.ExecuteNonQuery() != 1)
                {
                    throw new Exception("Failed to update the user password.");
                }
            }
        }
        public IList<UserItem> GetUsersByName(string firstName, string lastName)
        {
            IList<UserItem> result = new List<UserItem>();
            const string sql = "SELECT * from [User] WHERE FirstName LIKE @fN OR LastName LIKE @lN; ";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@fN", $"%{firstName}%");
                cmd.Parameters.AddWithValue("@lN", $"%{lastName}%");
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UserItem user = new UserItem();
                    user = GetUserFromReader(reader);
                    result.Add(user);
                }
            }
            return result;
        }
        #endregion

        #region RoleTable
        private RoleItem GetRoleItemFromReader(SqlDataReader reader)
        {
            RoleItem item = new RoleItem
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = Convert.ToString(reader["Name"])
            };
            return item;
        }
        public IList<RoleItem> GetRoleItems()
        {
            IList<RoleItem> roles = new List<RoleItem>();
            const string sql = "SELECT * FROM Role;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    roles.Add(GetRoleItemFromReader(reader));
                }
            }

            return roles;
        }
        public void UpdateUserRole(UserItem User, int RoleId)
        {
            const string sql = "UPDATE [User] SET RoleId = @RoleId WHERE Id = @UserId;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@RoleId", RoleId);
                cmd.Parameters.AddWithValue("@UserId", User.Id);
                if (cmd.ExecuteNonQuery() != 1)
                {
                    throw new Exception("Failed to update the user role.");
                }
            }
        }
        #endregion

        #region VisitTable
        private VisitItem GetVisitFromReader(SqlDataReader reader)
        {
            VisitItem visit = new VisitItem()
            {
                Id = Convert.ToInt32(reader["Id"]),
                UserId = Convert.ToInt32(reader["UserId"]),
                CheckIn = Convert.ToDateTime(reader["CheckIn"]),
                CheckOut = !reader.IsDBNull(reader.GetOrdinal("CheckOut")) ?
                            Convert.ToDateTime(reader["CheckOut"]) : (DateTime?)null

            };
            return visit;
        }
        public IList<VisitItem> GetVisits()
        {
            IList<VisitItem> result = new List<VisitItem>();

            // Create a SQL string
            const string sql = "SELECT * FROM Visit ORDER BY Id;";

            // Create a connection object
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    VisitItem visit = GetVisitFromReader(reader);
                    result.Add(visit);
                }
            }

            return result;
        }
        public IList<VisitItem> GetVisitsByDate(DateTime dt)
        {
            IList<VisitItem> result = new List<VisitItem>();

            // Create a SQL string
            const string sql = "SELECT * FROM Visit WHERE CAST(CheckIn as DATE)=@dt ORDER BY Id;";

            // Create a connection object
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@dt", dt.Date);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    VisitItem visit = GetVisitFromReader(reader);
                    result.Add(visit);
                }
            }
            return result;
        }
        public IList<VisitItem> GetVisitsByUser(int userId)
        {
            IList<VisitItem> result = new List<VisitItem>();
            // Create a SQL string
            const string sql = "SELECT * FROM Visit WHERE UserId=@UserId ORDER BY Id;";

            // Create a connection object
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    VisitItem visit = GetVisitFromReader(reader);
                    result.Add(visit);
                }
            }
            return result;
        }

        public IList<VisitItem> GetVisitsByUser(string userName)
        {

            IList<VisitItem> result = new List<VisitItem>();
            // Create a SQL string
            const string sql = "SELECT Visit.* FROM Visit WHERE " +
            "UserId=(SELECT [User].Id from[User] WHERE [User].Email = @userName OR [User].DisplayName =@userName) " +
            "ORDER BY Visit.Id;";

            // Create a connection object
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userName", userName);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    VisitItem visit = GetVisitFromReader(reader);
                    result.Add(visit);
                }
            }
            return result;
        }
        public IList<VisitItem> GetVisitsByUser(UserItem user)
        {
            return GetVisitsByUser(user.Id);
        }
        public IList<VisitItem> GetVisitsByDateRange(DateTime dtStart, DateTime dtEnd)
        {
            IList<VisitItem> result = new List<VisitItem>();

            // Create a SQL string
            const string sql = "SELECT * FROM Visit WHERE CheckIn BETWEEN @start AND @end ORDER BY Id;";

            // Create a connection object
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@start", dtStart.Date);
                cmd.Parameters.AddWithValue("@end", dtEnd.Date);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    VisitItem visit = GetVisitFromReader(reader);
                    result.Add(visit);
                }
            }
            return result;
        }
        public IList<VisitItem> GetVisitsByUserDateRange(string userName, DateTime dtStart, DateTime dtEnd)
        {

            //var result = GetVisitsByDateRange(dtStart, dtEnd).Where(v => v.UserId == user.Id);

            IList<VisitItem> result = new List<VisitItem>();
            // proc getVisitsByUserDateRange definition -
            // SELECT * FROM Visit WHERE Visit.UserId=@userid AND
            // Visit.Checkin BETWEEN @dtstart AND @dtend
            const string proc = "getVisitsByUserDateRange";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(proc, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@user_name", userName);
                cmd.Parameters.AddWithValue("@dtstart", dtStart.Date);
                cmd.Parameters.AddWithValue("@dtend", dtEnd.Date);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    VisitItem visit = GetVisitFromReader(reader);
                    result.Add(visit);
                }
            }
            return result;
        }

        public int CreateVisit(VisitItem newVisit)
        {

            const string sql = "INSERT INTO Visit (UserId,CheckIn) VALUES (@userId,@checkIn);";
            int result;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@userId", newVisit.UserId);
                cmd.Parameters.AddWithValue("@checkIn", newVisit.CheckIn);
                //   cmd.Parameters.AddWithValue("@checkOut", newVisit.CheckOut);
                result = (int)cmd.ExecuteScalar();
            }

            return result;
        }

        public void DeleteVisit(VisitItem visit)
        {
            const string sql = "DELETE FROM Visit WHERE Id=@Id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", visit.Id);
                int row = cmd.ExecuteNonQuery();
            }

        }

        public void UpdateVisit(VisitItem updatedVisit)
        {
            const string sql = "UPDATE Visit SET CheckIn=@checkin, CheckOut=@checkout WHERE Id = @id";
            // Create a connection object
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@checkin", updatedVisit.CheckIn);
                cmd.Parameters.AddWithValue("@checkout", updatedVisit.CheckOut);
                cmd.Parameters.AddWithValue("@id", updatedVisit.Id);

                if (cmd.ExecuteNonQuery() != 1)
                {
                    throw new Exception("Failed to update the visit");
                }
            }
        }
        public VisitItem GetVisit(int visitId)
        {
            VisitItem result = new VisitItem();
            const string sql = "SELECT * FROM Visit WHERE Id=@visitId";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@visitId", visitId);
                SqlDataReader reader = cmd.ExecuteReader();


                if (reader.Read())
                {
                    result = GetVisitFromReader(reader);
                }

            }

            return result;
        }
        public VisitItem GetCurrentVisit(string userName)
        {
            VisitItem result = new VisitItem();
            string sqlUserId = "(SELECT Id FROM [User] WHERE DisplayName=@userName OR Email=@userName)";
            string sqlMaxCheckin = "(SELECT MAX(CheckIn) FROM Visit)";
            string sql = "SELECT * FROM Visit WHERE CheckIn=" +
                         $"{sqlMaxCheckin} AND " +
                         $"UserId={sqlUserId} AND " +
                         "CheckOut IS NULL;";
            //int userId = GetUserItemByLogin(userName).Id;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userName", userName);
                SqlDataReader reader = cmd.ExecuteReader();


                if (reader.Read())
                {
                    result = GetVisitFromReader(reader);
                }

            }

            return result;
        }
        #endregion

        #region UsageTable
        private UsageItem GetUsageItemFromReader(SqlDataReader reader)
        {
            UsageItem result = new UsageItem
            {
                VisitId = Convert.ToInt32(reader["VisitId"]),
                EquipmentId = Convert.ToInt32(reader["EquipmentId"]),
                Reps = Convert.ToInt32(reader["Reps"]),
                Weight = Convert.ToInt32(reader["Weight"]),
                Duration = Convert.ToInt32(reader["Duration"])
            };
            return result;
        }
        public IList<UsageItem> GetEquipmentHistory(EquipmentItem Equip)
        {
            IList<UsageItem> result = new List<UsageItem>();
            const string sql = "SELECT * FROM UsageDetails WHERE EquipmentId = @Equip ORDER BY VisitId;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Equip", Equip.Id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UsageItem use = new UsageItem();
                    use = GetUsageItemFromReader(reader);
                    result.Add(use);
                }
            }
            return result;
        }
        public IList<UsageItem> GetUserHistory(UserItem User)
        {
            IList<UsageItem> result = new List<UsageItem>();
            const string sql = "SELECT * FROM UsageDetails AS ud " +
                "JOIN Visit AS v ON v.Id = ud.VisitId " +
                "WHERE v.UserId = @User;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@User", User.Id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UsageItem use = new UsageItem();
                    use = GetUsageItemFromReader(reader);
                    result.Add(use);
                }
            }
            return result;
        }
        public IList<LoggedMachineItem> GetLast30Days()
        {
            IList<LoggedMachineItem> result = new List<LoggedMachineItem>();
            const string sql = "SELECT e.Name, SUM(ud.Duration) as minutes from UsageDetails as ud " +
                               "JOIN Visit as v on v.Id = ud.VisitId " +
                               "JOIN Equipment as e on e.Id = ud.EquipmentId " +
                               "WHERE v.CheckIn < getutcdate() AND v.CheckIn > getutcdate() - 30 " +
                               "GROUP BY e.Name;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    LoggedMachineItem use = new LoggedMachineItem();
                    use.Name = Convert.ToString(reader["Name"]);
                    use.NumMinutes = Convert.ToInt32(reader["minutes"]);
                    result.Add(use);
                }
            }
            return result;
        }
        public int LogEquipmentUsage(UsageItem Equipment)
        {
            int result;
            const string sql = "INSERT UsageDetails (VisitId, EquipmentId, Reps, Weight, Duration)" +
                "VALUES (@VId, @EId, @Reps, @Weight, @Duration);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@VId", Equipment.VisitId);
                cmd.Parameters.AddWithValue("@EId", Equipment.EquipmentId);
                cmd.Parameters.AddWithValue("@Reps", Equipment.Reps);
                cmd.Parameters.AddWithValue("@Weight", Equipment.Weight);
                cmd.Parameters.AddWithValue("@Duration", Equipment.Duration);
                result = cmd.ExecuteNonQuery();
            }

            return result;
        }
        #endregion

        #region EquipmentTable
        private EquipmentItem GetEquipmentFromReader(SqlDataReader reader)
        {
            EquipmentItem result = new EquipmentItem
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = Convert.ToString(reader["Name"]),
                Description = Convert.ToString(reader["Description"]),
                CategoryId = Convert.ToString(reader["Category"]),
                ImageURL = Convert.ToString(reader["ImageURL"]),
                VideoLink = Convert.ToString(reader["VideoURL"]),
                MachineNumber = Convert.ToInt32(reader["Number"])
            };
            return result;
        }

        public IList<EquipUse> GetEquipUses()
        {
            IList<EquipUse> result = new List<EquipUse>();
            const string sql = "SELECT eq.Id,eq.Name,COUNT(ud.VisitId) as count_use,SUM(ud.Duration) as sum_use " +
                               "FROM Equipment eq " +
                               "JOIN UsageDetails ud ON ud.EquipmentId = eq.Id " +
                               "GROUP BY eq.Id,eq.Name;";

            //            public int EquipId { get; set; }
            //public string EquipName { get; set; }
            //public int CountUses { get; set; }
            //public int SumUses { get; set; }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    EquipUse equ = new EquipUse()
                    {
                        EquipId = Convert.ToInt32(reader["Id"]),
                        EquipName = Convert.ToString(reader["Name"]),
                        CountUses = Convert.ToInt32(reader["count_use"]),
                        SumUses = Convert.ToInt32(reader["sum_use"])
                    };

                    result.Add(equ);
                }
            }
            return result;


        }
        public IList<EquipmentItem> GetEquipment()
        {
            IList<EquipmentItem> result = new List<EquipmentItem>();

            // Create a SQL string
            const string sql = "SELECT * FROM Equipment ORDER BY Name;";

            // Create a connection object
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    EquipmentItem equipment = new EquipmentItem();
                    equipment = GetEquipmentFromReader(reader);
                    result.Add(equipment);
                }
            }
            return result;
        }
        public EquipmentItem GetEquipmentById(int Id)
        {
            EquipmentItem result = new EquipmentItem();
            const string sql = "SELECT * FROM Equipment WHERE Id = @Id;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = GetEquipmentFromReader(reader);
                }
            }
            return result;
        }
        public IList<EquipmentItem> GetEquipmentByName(string Name)
        {
            IList<EquipmentItem> result = new List<EquipmentItem>();
            const string sql = "SELECT * FROM Equipment WHERE Name = @Name ORDER BY Name;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", Name);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    EquipmentItem equipment = new EquipmentItem();
                    equipment = GetEquipmentFromReader(reader);
                    result.Add(equipment);
                }
            }
            return result;
        }
        public int AddNewMachine(EquipmentItem Equipment)
        {
            int result;
            const string sql = "insert into Equipment (Name, Description, Category, ImageURL, VideoURL, Number)" +
                "VALUES (@name, @desc, @cat, @img, @vid, @num);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@name", Equipment.Name);
                cmd.Parameters.AddWithValue("@desc", Equipment.Description);
                cmd.Parameters.AddWithValue("@cat", Equipment.CategoryId);
                cmd.Parameters.AddWithValue("@img", Equipment.ImageURL);
                cmd.Parameters.AddWithValue("@vid", Equipment.VideoLink);
                cmd.Parameters.AddWithValue("@num", Equipment.MachineNumber);
                result = (int)cmd.ExecuteScalar();
            }
            return result;
        }
        public void RemoveEquipmentById(int id)
        {
            const string sql = "delete from Equipment WHERE Id = @id;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                int row = cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region ClassTable

        public IList<ClassItem> GetClasses()
        {
            IList<ClassItem> result = new List<ClassItem>();

            // Create a SQL string
            const string sql = "SELECT * FROM [Classes] ORDER BY Id;";

            // Create a connection object
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ClassItem classItem = GetClassFromReader(reader);
                    result.Add(classItem);
                }
            }

            return result;
        }

        public ClassItem GetClass(int id)
        {
            ClassItem result = new ClassItem();
            const string sql = "SELECT * FROM [Classes] WHERE Id=@classId";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@classId", id);
                SqlDataReader reader = cmd.ExecuteReader();


                if (reader.Read())
                {
                    result = GetClassFromReader(reader);
                }

            }

            return result;
        }

        public IList<ClassItem> GetClassesByDate(DateTime date)
        {
            IList<ClassItem> result = new List<ClassItem>();
            // Create a SQL string
            const string sql = "SELECT * FROM Classes WHERE CAST([Date] as DATE)=@date ORDER BY Id;";

            // Create a connection object
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@date", date.Date);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ClassItem classItem = GetClassFromReader(reader);
                    result.Add(classItem);
                }
            }
            return result;
        }

        public IList<ClassItem> GetClassesByDateRange(DateTime dtStart, DateTime dtEnd)
        {
            IList<ClassItem> result = new List<ClassItem>();

            //Enumerable.Range(0, 1 + dtEnd.Date.Subtract(dtStart).Days)
            //          .Select(offset => dtStart.Date.AddDays(offset))
            //          .ToList()
            //          .ForEach(dt => result.AddRange(GetClassesByDate(dt)));
            // Create a SQL string
            const string sql = "SELECT * FROM Classes WHERE CAST([Date] as DATE) " +
                               "BETWEEN @dtStart AND @dtEnd ORDER BY Id;";

            // Create a connection object
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@dStart", dtStart.Date);
                cmd.Parameters.AddWithValue("@dtEnd", dtEnd.Date);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ClassItem classItem = GetClassFromReader(reader);
                    result.Add(classItem);
                }
            }
            return result;
        }

        public int CreateClass(ClassItem newClass)
        {
            int result;
            const string sql = "INSERT INTO [Classes] VALUES (@Name,@Color,@Date)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@Name", newClass.Name);
                cmd.Parameters.AddWithValue("@Color", newClass.Color);
                cmd.Parameters.AddWithValue("@Date", newClass.Date);

                result = (int)cmd.ExecuteScalar();
            }


            return result;


        }

        public void DeleteClass(int classId)
        {
            const string sqlClass = "DELETE FROM Classes WHERE Id=@Id;";
            const string sqlUsers = "DELETE FROM UserClasses WHERE ClassId=@Id;";
            int result;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlUsers + sqlClass, conn);
                cmd.Parameters.AddWithValue("@Id", classId);
                result = cmd.ExecuteNonQuery();
                // cmd.CommandText = sqlClass;
                // cmd.Parameters.AddWithValue("@Id", classId);
                //result= cmd.ExecuteNonQuery();

            }


        }



        private ClassItem GetClassFromReader(SqlDataReader reader)
        {
            ClassItem classItem = new ClassItem()
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = Convert.ToString(reader["Name"]),
                Color = Convert.ToString(reader["Color"]),
                Date = Convert.ToDateTime(reader["Date"]),

            };
            return classItem;
        }
        #endregion

        #region UserClassesTable



        public IList<UserClassModel> GetUsersByClassId(int classId)
        {
            IList<UserClassModel> result = new List<UserClassModel>();

            const string sql = "SELECT uc.UserId,u.DisplayName,uc.ClassId,c.Name,c.[Date] " +
                               "FROM UserClasses uc " +
                               "JOIN[User] u ON uc.UserId = u.Id " +
                               "JOIN Classes c ON c.Id = uc.ClassId " +
                               "WHERE uc.ClassId=@classId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@classId", classId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserClassModel ucm = GetUCModelFromReader(reader);
                    result.Add(ucm);
                }
            }
            return result;
        }

        public IList<UserClassModel> GetClassesByUserName(string userName)
        {
            IList<UserClassModel> result = new List<UserClassModel>();

            const string sql = "SELECT uc.UserId,u.DisplayName,uc.ClassId,c.Name,c.[Date] " +
                              "FROM UserClasses uc JOIN [User] u ON uc.UserId = u.Id " +
                              "JOIN [Classes] c ON c.Id = uc.ClassId WHERE uc.UserId = " +
                              " (SELECT Id FROM [User] WHERE DisplayName=@userName);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userName", userName);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserClassModel ucm = GetUCModelFromReader(reader);
                    result.Add(ucm);
                }
            }
            return result;


        }

        public int AddUserToClass(string userName, int classId)
        {
            int result;
            const string sql = "INSERT INTO [UserClasses] VALUES (" +
                "(SELECT Id FROM [User] WHERE DisplayName=@userName)," +
                "@classId);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@userName", userName);
                cmd.Parameters.AddWithValue("@classId", classId);
                result = (int)cmd.ExecuteScalar();
            }

            return result;
        }

        public void DeleteUserFromClass(string userName, int classId)
        {
            const string sql = "DELETE FROM UserClasses WHERE UserId=" +
                               "(SELECT Id FROM [User] WHERE DisplayName=@userName) " +
                               "AND ClassId=@classId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userName", userName);
                cmd.Parameters.AddWithValue("@classId", classId);
                int row = cmd.ExecuteNonQuery();
            }
        }

        private UserClassModel GetUCModelFromReader(SqlDataReader reader)
        {
            UserClassModel ucm = new UserClassModel()
            {
                UserId = Convert.ToInt32(reader["UserId"]),
                ClassId = Convert.ToInt32(reader["ClassId"]),
                UserName = Convert.ToString(reader["DisplayName"]),
                ClassName = Convert.ToString(reader["Name"]),
                ClassDate = Convert.ToDateTime(reader["Date"])
            };
            return ucm;
        }



        #endregion

        #region Misc
        public VisitMetricsModel GetUserVisitMetrics(string userName, int days)
        {
            VisitMetricsModel result = new VisitMetricsModel();
            //const string sql = "SELECT u.DisplayName, " +
            //                   "CAST(SUM(ud.Duration) AS decimal(5,2))/cast(COUNT(v.Id) AS decimal(5,2)) " +
            //                   "AS avgminutes FROM UsageDetails AS ud " +
            //                   "JOIN Visit AS v ON v.Id = ud.VisitId " +
            //                   "JOIN [User] AS u ON u.Id = v.UserId " +
            //                   "WHERE v.CheckIn < getutcdate() AND v.CheckIn > getutcdate() - @days AND DisplayName = @username " +
            //                   "GROUP BY u.DisplayName;";
            string sqlUser = "(SELECT Id FROM[User] WHERE DisplayName = @username)";
            string sql = "SELECT Times.UserId, AVG(Duration) AS avg_duration, " +
              "COUNT(Duration) as count_duration, " +
              "SUM(Duration) as sum_duration " +
             "FROM " +
             $"(SELECT{sqlUser} AS UserId, " +
             "DATEDIFF(minute, CheckIn, Checkout) as Duration " +
             "FROM Visit " +
             $"WHERE UserId={sqlUser} AND CheckOut IS NOT NULL AND Checkin > DATEADD(day,@days,CAST(getutcdate() as DATE)) " +
             "GROUP BY Visit.Id, CheckIn, Checkout) Times " +
             "GROUP BY Times.UserId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@days", days * -1);
                cmd.Parameters.AddWithValue("@userName", userName);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.AvgDuration = Convert.ToInt32(reader["avg_duration"]);
                    result.CountVisits = Convert.ToInt32(reader["count_duration"]);
                    result.SumDuration = Convert.ToInt32(reader["sum_duration"]);
                }
            }
            return result;
        }
        public IList<TopUsageItemModel> TopFiveMachines(string userName)
        {
            IList<TopUsageItemModel> result = new List<TopUsageItemModel>();
            const string sql = "SELECT TOP 5 SUM(ud.duration) AS sum_duration, eq.Name " +
                               "FROM UsageDetails ud " +
                               "JOIN Equipment eq ON ud.EquipmentId = eq.Id " +
                               "JOIN Visit v ON v.Id = ud.VisitId " +
                               "JOIN [User] u ON u.Id = v.UserId " +
                               "WHERE u.DisplayName = @userName " +
                               "GROUP BY eq.Name " +
                               "ORDER BY sum_duration desc;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userName", userName);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TopUsageItemModel newUsageItem = new TopUsageItemModel();
                    newUsageItem.Name = Convert.ToString(reader["Name"]);
                    newUsageItem.Duration = Convert.ToInt32(reader["sum_duration"]);
                    result.Add(newUsageItem);
                }
            }
            return result;
        }
        public IList<WorkoutModel> WorkoutsPerVisit(VisitItem v)
        {
            IList<WorkoutModel> result = new List<WorkoutModel>();
            const string sql = "SELECT eq.Name, ud.Reps, ud.Weight, ud.Duration from UsageDetails ud " +
                               "JOIN Visit v on v.Id = ud.VisitId " +
                               "JOIN Equipment eq on eq.Id = ud.EquipmentId " +
                               "WHERE v.Id = @id";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", v.Id);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    WorkoutModel newWorkout = new WorkoutModel();
                    newWorkout.Name = Convert.ToString(reader["Name"]);
                    newWorkout.Reps = Convert.ToInt32(reader["Reps"]);
                    newWorkout.Weight = Convert.ToInt32(reader["Weight"]);
                    newWorkout.Duration = Convert.ToInt32(reader["Duration"]);
                    result.Add(newWorkout);
                }
            }
            return result;
        }
        //public MetricsModel GetMetrics(string userName, int days)
        //{
        //    const string sqlTopFive = "SELECT TOP 5 SUM(ud.duration) AS sum_duration, eq.Name " +
        //                              "FROM UsageDetails ud " +
        //                              "JOIN Equipment eq ON ud.EquipmentId = eq.Id " +
        //                              "JOIN Visit v ON v.Id = ud.VisitId " +
        //                              "JOIN [User] u ON u.Id = v.UserId " +
        //                              "WHERE u.DisplayName = @userName " +
        //                              "GROUP BY eq.Name " +
        //                              "ORDER BY sum_duration desc;";

        //    const string sqlVisitIds = "SELECT Id FROM Visit WHERE UserId = (SELECT Id FROM[User] WHERE DisplayName=@userName)" +
        //                                "AND CAST(CheckIn as DATE) BETWEEN CAST(DATEADD(Day,@days,GETUTCDATE()) as DATE) AND CAST(GETUTCDATE() as DATE)";

        //    const string sqlAllMetrics = "SELECT v.Id as VisitId,v.CheckIn as Date,DATEDIFF(minute,v.CheckIn,v.Checkout) visit_duration " +
        //                                 "eq.Name, ud.Reps, ud.Weight, ud.Duration " +
        //                                 "FROM UsageDetails as ud " +
        //                                 "JOIN Visit as v on v.Id = ud.VisitId " +
        //                                 "JOIN Equipment as eq on eq.Id = ud.EquipmentId " +
        //                                 "WHERE v.Id IN (SELECT Id " +
        //                                 "FROM Visit " +
        //                                 "WHERE UserId = (SELECT Id FROM [User] WHERE DisplayName = @userName) " +
        //                                 "AND CAST(CheckIn as DATE) BETWEEN CAST(DATEADD(Day,-10,GETUTCDATE()) as DATE) AND CAST(GETUTCDATE() as DATE)) " +
        //                                 "ORDER BY v.Id;";



        //    //    SqlCommand cmdTopFive = new SqlCommand(sql, conn);
        //    //   cmdTopFive.Parameters.AddWithValue("@userName", userName);

        //    return new MetricsModel();
        //}

        //public decimal GetAvgTimeSpent(string username, int days)
        //{
        //    decimal result = 0;
        //    const string sql = "select u.DisplayName, " +
        //                       "cast(SUM(ud.Duration) as decimal(5,2))/cast(COUNT(v.Id) as decimal(5,2)) " +
        //                       "as avgminutes from UsageDetails as ud " +
        //                       "join Visit as v on v.Id = ud.VisitId " +
        //                       "join [User] as u on u.Id = v.UserId " +
        //                       "where v.CheckIn < getdate() AND v.CheckIn > getdate() - @days AND DisplayName = @username " +
        //                       "group by u.DisplayName;";

        //    using (SqlConnection conn = new SqlConnection(_connectionString))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.Parameters.AddWithValue("@days", days);
        //        cmd.Parameters.AddWithValue("@username", username);
        //        var reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            result = Convert.ToDecimal(reader["avgminutes"]);
        //        }
        //    }
        //    return result;
        //}
        //public IList<TopUsageItemModel> Top5Machines(string username)
        //{
        //    IList<TopUsageItemModel> result = new List<TopUsageItemModel>();
        //    const string sql = "select top 5 sum(ud.duration) as numMins, e.Name " +
        //                       "from UsageDetails as ud " +
        //                       "join Equipment as e on ud.EquipmentId = e.Id " +
        //                       "join Visit as v on v.Id = ud.VisitId " +
        //                       "join [User] as u on u.Id = v.UserId " +
        //                       "where u.DisplayName = @userName " +
        //                       "group by e.Name " +
        //                       "order by numMins desc;";

        //    using (SqlConnection conn = new SqlConnection(_connectionString))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.Parameters.AddWithValue("@userName", username);
        //        var reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            TopUsageItemModel newUsageItem = new TopUsageItemModel();
        //            newUsageItem.Name = Convert.ToString(reader["Name"]);
        //            newUsageItem.Duration = Convert.ToInt32(reader["numMins"]);
        //            result.Add(newUsageItem);
        //        }
        //    }
        //    return result;
        //}
        //public IList<WorkoutModel> WorkoutsPerVisit(VisitItem v)
        //{
        //    IList<WorkoutModel> result = new List<WorkoutModel>();
        //    const string sql = "select e.Name, ud.Reps, ud.Weight, ud.Duration from UsageDetails as ud " +
        //                       "join Visit as v on v.Id = ud.VisitId " +
        //                       "join Equipment as e on e.Id = ud.EquipmentId " +
        //                       "where v.Id = @id";
        //    using (SqlConnection conn = new SqlConnection(_connectionString))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.Parameters.AddWithValue("@id", v.Id);
        //        var reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            WorkoutModel newWorkout = new WorkoutModel();
        //            newWorkout.Name = Convert.ToString(reader["Name"]);
        //            newWorkout.Reps = Convert.ToInt32(reader["Reps"]);
        //            newWorkout.Weight = Convert.ToInt32(reader["Weight"]);
        //            newWorkout.Duration = Convert.ToInt32(reader["Duration"]);
        //            result.Add(newWorkout);
        //        }
        //    }
        //    return result;
        //}


        #endregion

    }
}