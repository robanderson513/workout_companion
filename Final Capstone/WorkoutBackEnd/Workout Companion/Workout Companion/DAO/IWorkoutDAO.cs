using System;
using System.Collections.Generic;
using WorkoutCompanion.Models;
using WorkoutCompanion.Models.Database;

namespace WorkoutCompanion.DAO
{
    public interface IWorkoutDAO
    {
        #region UserTable
        UserItem GetUserItemByLogin(string login);
        IList<UserItem> GetUserItemsByRole(RoleItem role);
        int AddUserAccount(UserAccountModel userAccount);
        void ChangeUserPassword(UserItem User, string newPassword);
        void UpdateUserProfile(UserItem User);
        IList<UserItem> GetUsersByName(string firstName, string lastName);
        #endregion

        #region RoleTable
        IList<RoleItem> GetRoleItems();
        void UpdateUserRole(UserItem User, int RoleId);

        #endregion

        #region VisitTable
        IList<VisitItem> GetVisits();
        IList<VisitItem> GetVisitsByDate(DateTime dt);
        IList<VisitItem> GetVisitsByUser(UserItem user);
        IList<VisitItem> GetVisitsByDateRange(DateTime dtStart, DateTime dtEnd);
        IList<VisitItem> GetVisitsByUserDateRange(string userName, DateTime dtStart, DateTime dtEnd);
        IList<VisitItem> GetVisitsByUser(int userId);
        IList<VisitItem> GetVisitsByUser(string userName);
        int CreateVisit(VisitItem newVisit);
        void UpdateVisit(VisitItem updatedVisit);
        VisitItem GetVisit(int visitId);
        VisitItem GetCurrentVisit(string userName);


        #endregion

        #region UsageTable

        IList<UsageItem> GetEquipmentHistory(EquipmentItem Equip);
        IList<UsageItem> GetUserHistory(UserItem User);
        IList<LoggedMachineItem> GetLast30Days();
        int LogEquipmentUsage(UsageItem Equipment);
        #endregion

        #region EquipmentTable
        IList<EquipmentItem> GetEquipment();
        EquipmentItem GetEquipmentById(int Id);
        IList<EquipmentItem> GetEquipmentByName(string Name);
        int AddNewMachine(EquipmentItem Equipment);
        void RemoveEquipmentById(int id);
        IList<EquipUse> GetEquipUses();
        #endregion

        #region ClassesTable
        IList<ClassItem> GetClasses();
        ClassItem GetClass(int id);
        IList<ClassItem> GetClassesByDate(DateTime date);
        IList<ClassItem> GetClassesByDateRange(DateTime dtStart, DateTime dtEnd);
        int CreateClass(ClassItem newClass);
        void DeleteClass(int classId);
        #endregion

        #region UserClassesTable

        IList<UserClassModel> GetUsersByClassId(int classId);
        IList<UserClassModel> GetClassesByUserName(string userName);
        int AddUserToClass(string userName, int classId);
        void DeleteUserFromClass(string userName, int classId);
        #endregion

        #region Misc
        VisitMetricsModel GetUserVisitMetrics(string userName, int days);
        IList<TopUsageItemModel> TopFiveMachines(string username);
        IList<WorkoutModel> WorkoutsPerVisit(VisitItem v);
        //  IList<TopUsageItemModel> Top5Machines(string username);
        //  decimal GetAvgTimeSpent(string username, int days);
        #endregion
    }
}
