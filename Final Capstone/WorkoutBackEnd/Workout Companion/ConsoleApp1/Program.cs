using System;
using WorkoutCompanion.DAO;
using WorkoutCompanion.Models.Database;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=WorkoutDB;Integrated Security=True";
            string connectionString = "Server=tcp:tec6workout.database.windows.net,1433;Initial Catalog=WorkoutDB;Persist Security Info=False;User ID=testudent;Password=Techelevator1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            WorkoutDAO myDAO = new WorkoutDAO(connectionString);
            //Console.WriteLine("Enter a Username");
            //string myUsername = Console.ReadLine();
            //Console.WriteLine("Enter a First Name");
            //string myFirstName = Console.ReadLine();
            //Console.WriteLine("Enter a Last Name");
            //string myLastName = Console.ReadLine();
            //Console.WriteLine("Enter a Password");
            //string myPassword = Console.ReadLine();
            //Console.WriteLine("Enter a RoleId");
            //int myRoleId = int.Parse(Console.ReadLine());
            //Console.WriteLine("Enter an Email");
            //string myEmail = Console.ReadLine();
            //Console.WriteLine("Enter a Photo URL");
            //string myURL = Console.ReadLine();
            //Console.WriteLine("What are your workout goals?");
            //string myGoals = Console.ReadLine();

            //PasswordManager myPwManager = new PasswordManager(myPassword);
            //UserItem myUser = new UserItem()
            //{
            //    Username = myUsername,
            //    Email = myEmail,
            //    Salt = myPwManager.Salt,
            //    Hash = myPwManager.Hash,
            //    RoleId = myRoleId,
            //    FirstName = myFirstName,
            //    LastName = myLastName,
            //    WorkoutGoals = myGoals,
            //    CreationDate = DateTime.UtcNow
            //};
            //if (myURL != null && myURL != "")
            //{
            //    myUser.PhotoURL = myURL;
            //}
            //UserAccountModel myAccount = new UserAccountModel()
            //{
            //    User = myUser
            //};
            //myDAO.AddUserAccount(myAccount);


            // var visit = myDAO.GetCurrentVisit("pleshekc");
            // Type type = visit.GetType();
            //  var visits = myDAO.GetVisitsByUserDateRange("pleshekc", DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddDays(-5));
            /*var getMetrics = myDAO.GetUserVisitMetrics("pleshekc", 30);
            Console.WriteLine("Avg:{0}\nCount:{1}\nSum:{2}", getMetrics.AvgDuration, getMetrics.CountVisits, getMetrics.SumDuration);

            string userName = "tn";
            UserItem myUser = myDAO.GetUserItemByLogin(userName);
            VisitItem newVisit = new VisitItem()
            {
                UserId = myUser.Id,
                CheckIn = DateTime.UtcNow
            };
            var checkin = myDAO.CreateVisit(newVisit);

            Console.WriteLine("checkin:{0}",checkin);*/
            //PropertyInfo[] properties = type.GetProperties();
            //foreach (PropertyInfo property in properties)
            //{
            //    Console.WriteLine("Name:{0}\tValue:{1}", property.Name, property.GetValue(visit, null));

            //}
            Random r1 = new Random();

            for (int i = 1; i < 11; i++)
            {

                int v1 = r1.Next(6, 19);
                int v2 = r1.Next(10, 59);
                int v3 = r1.Next(100, 999);
                int v4 = r1.Next(10, 59);
                int v5 = r1.Next(100, 999);
                int v6 = r1.Next(1, 3);
                Console.WriteLine($"UPDATE Visit set CheckIn = \'2019-12-{v1} 10:30:{v2}.{v3}\' WHERE Id = XX");
                Console.WriteLine($"UPDATE Visit set CheckOut = \'2019-12-{v1} 1{v6}:30:{v4}.{v5}\' WHERE Id = XX");
            }
            Console.ReadKey();
        }
    }
}
