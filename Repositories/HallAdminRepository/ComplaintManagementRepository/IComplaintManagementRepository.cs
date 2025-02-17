﻿using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public interface IComplaintManagementRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public void UpdateEntity<T>(T entityToUpdate);
        public Task<int?> GetHallId(string? email);
        public Task AddEntityAsync<T>(T entityToAdd);
        public Task<bool> SaveChangesAsync();
        public Task RemoveEntityAsync<T>(T entityToRemove);
        public Task<Tuple<int, int, int, int>> GetComplaintDetails(int hallId);
        public Task<IEnumerable<Complaint>> GetComplaintsOfHall(int hallId);
        public Task<IEnumerable<Comment>> GetCommentsByComplaitId(int complaintId);
        public Task<Complaint> GetComplaintById(int complaintId);



    }
}
