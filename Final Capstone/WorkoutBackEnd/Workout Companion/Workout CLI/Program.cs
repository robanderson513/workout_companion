using WorkoutCompanion.BusinessLogic;
using WorkoutCompanion.DAO;
using WorkoutCompanion.Models;
using System;
using System.IO;

namespace WorkoutCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            TestFileManager();
        }

        private static void TestFileManager()
        {
            IWorkoutDAO db = new WorkoutDAO(@"Data Source=.\SQLEXPRESS;Initial Catalog=BuddyBux;Integrated Security=true");
            File.WriteAllBytes($@"C:\BuddyBux\Destination\{fileModel.Name}", fileModel.Data);

            var fileData = File.ReadAllBytes(@"C:\BuddyBux\Source\baseball.jpg");
            File.WriteAllBytes($@"C:\BuddyBux\Destination\baseball2.jpg", fileModel.Data);

            //FileModel testFile = mgr.GetFile(1);
            //Console.WriteLine("Test file: " + testFile.Name + " file content " + System.Text.Encoding.Default.GetString(testFile.Data));
            //testFile = mgr.GetFile(6);
            //Console.WriteLine("Test file: " + testFile.Name + " file content " + System.Text.Encoding.Default.GetString(testFile.Data));
            //testFile = mgr.GetFile(3);
            //Console.WriteLine("Test file: " + testFile.Name + " file content " + System.Text.Encoding.Default.GetString(testFile.Data));
            //testFile = mgr.GetFile(6);
            //Console.WriteLine("Test file: " + testFile.Name + " file content " + System.Text.Encoding.Default.GetString(testFile.Data));
            //Console.ReadKey();
        }
    }
}
