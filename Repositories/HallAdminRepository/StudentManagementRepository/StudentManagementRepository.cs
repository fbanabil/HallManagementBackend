﻿using Microsoft.EntityFrameworkCore;
using Student_Hall_Management.Data;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Dtos.HallAdmin.Student;
using Student_Hall_Management.Models;


namespace Student_Hall_Management.Repositories
{
    public class StudentManagementRepository : IStudentManagementRepository
    {
        DataContextEF _entityFramework;
        DataContextDapper _dapper;
        public StudentManagementRepository(IConfiguration entityFramework)
        {
            _entityFramework = new DataContextEF(entityFramework);
            _dapper = new DataContextDapper(entityFramework);

        }
        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _entityFramework.SaveChangesAsync() > 0;
        }


        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
            }
        }

        public async Task AddEntityAsync<T>(T entityToAdd)
        {

            if (entityToAdd != null)
            {
                await _entityFramework.AddAsync(entityToAdd);
            }

        }

        public void RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                _entityFramework.Remove(entityToRemove);
            }
        }

        public async Task RemoveEntityAsync<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                _entityFramework.Remove(entityToRemove);
            }

        }
        public void UpdateEntity<T>(T entityToUpdate)
        {
            if (entityToUpdate != null)
            {
                _entityFramework.Update(entityToUpdate);
            }
        }
        public async Task<int?> GetHallId(string? email)
        {
            HallAdmin? hallAdmin = await _entityFramework.HallAdmins
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
            return hallAdmin?.HallId ?? null;
        }

        public async Task<Tuple<int, int>> GetTotalStudentAndIsActive(int hallId)
        {
            int totalStudent = await _entityFramework.Students
                .Where(s => s.HallId == hallId)
                .CountAsync();
            int isActive = await _entityFramework.Students
                .Where(s => s.HallId == hallId && s.IsActive == true)
                .CountAsync();
            return new Tuple<int, int>(totalStudent, isActive);
        }

        public async Task<IEnumerable<Student>> GetStudents(int hallId)
        {
            IEnumerable<Student> students = await _entityFramework.Students
                .Where(s => s.HallId == hallId)
                .ToListAsync();
            return students;
        }

        public async Task<Student> GetStudentById(int studentId)
        {
            Student student = await _entityFramework.Students
                .Where(s => s.Id == studentId)
                .FirstOrDefaultAsync();
            return student;
        }

        public async Task<IEnumerable<HallFeePayment>> HallFeePayment(int studentId)
        {
            IEnumerable<HallFeePayment> hallFeePayments = await _entityFramework.HallFeePayments
                .Where(h => h.StudentId == studentId)
                .ToListAsync();
            return hallFeePayments;
        }

        public async Task<IEnumerable<DinningFeePayment>> DinningFeePayments(int studentId)
        {
            IEnumerable<DinningFeePayment> dinningFeePayments = await _entityFramework.DinningFeePayments
                .Where(d => d.StudentId == studentId)
                .ToListAsync();
            return dinningFeePayments;
        }

        public async Task<IEnumerable<HallReview>> HallReviews(int studentId)
        {
            IEnumerable<HallReview> hallReviews = await _entityFramework.HallReviews
                .Where(h => h.Reviewer == studentId)
                .ToListAsync();
            return hallReviews;
        }

        public async Task<IEnumerable<Comment>> Comments(int studentId)
        {
            IEnumerable<Comment> comments = await _entityFramework.Comments
                .Where(c => c.StudentId == studentId)
                .ToListAsync();
            return comments;
        }

        public async Task<IEnumerable<Complaint>> Complaints(int studentId)
        {
            IEnumerable<Complaint> complaints = await _entityFramework.Complaints
                .Where(c => c.StudentId == studentId)
                .ToListAsync();
            return complaints;
        }
        public async Task<IEnumerable<PendingRoomRequest>> PendingRoomRequests(int studentId)
        {
            IEnumerable<PendingRoomRequest> pendingRoomRequests = await _entityFramework.PendingRoomRequests
                .Where(p => p.StudentId == studentId)
                .ToListAsync();
            return pendingRoomRequests;
        }

        public async Task<Room> Room(string roomId)
        {
            Room room = await _entityFramework.Rooms
                .Where(r => r.RoomNo == roomId)
                .FirstOrDefaultAsync();
            return room;
        }

        public async Task<StudentAuthentication> StudentAuthentication(string email)
        {
            StudentAuthentication studentAuthentication = await _entityFramework.StudentAuthentication
                .Where(s => s.Email == email)
                .FirstOrDefaultAsync();
            return studentAuthentication;
        }

        public async Task<Room> GetRoomByRoomNoAndHallId(string roomNo, int hallId)
        {
            Room room = await _entityFramework.Rooms
                .Where(r => r.RoomNo == roomNo && r.HallId == hallId)
                .FirstOrDefaultAsync();
            return room;
        }

        public async Task<IEnumerable<Room>> GetAvailableRooms(int hallId)
        {
            IEnumerable<Room> rooms = await _entityFramework.Rooms
                .Where(r => r.HallId == hallId)
                .ToListAsync();
            return rooms;
        }

        public async Task<List<HallDetails>> GetHalls()
        {
            List<HallDetails> hallList = _entityFramework.HallDetails.ToList();

            return hallList;
        }

        public async Task<int> PaymentDue(int hallId)
        {
            int hallFeeDue = await _entityFramework.HallFeePayments
                .Where(h => h.HallId == hallId && h.PaymentStatus=="Not Paid")
                .Select(h => h.PaymentAmount)
                .SumAsync();
            int dinningFeeDue = await _entityFramework.DinningFeePayments
                .Where(d => d.HallId == hallId && d.PaymentStatus == "Not Paid")
                .Select(d => d.PaymentAmount)
                .SumAsync();
            return hallFeeDue + dinningFeeDue;
        }

        public async Task<int> DinningAttendence(int hallId)
        {
            List<DinningFeePayment> dinningFeePayments = await _entityFramework.DinningFeePayments
                .Where(d => d.PaymentStatus == "Paid" && d.HallId==hallId)
                .ToListAsync();

            List<DinningFeePayment> dinningFeePayments1 = await _entityFramework.DinningFeePayments
                .Where(d => d.PaymentStatus == "Not Paid" && d.HallId==hallId)
                .ToListAsync();
            int percentagepaid = 0;
            try
            {
                percentagepaid = (int)(double)(dinningFeePayments.Count / (dinningFeePayments.Count + dinningFeePayments1.Count)) * 100;
            }
            catch (DivideByZeroException)
            {
                percentagepaid = 0;
            }

            return  percentagepaid;


        }

        public async Task<bool> NotPaidByStudentId(int studentId)
        {
            List<HallFeePayment> hallFeePayments = await _entityFramework.HallFeePayments
                .Where(h => h.StudentId == studentId && h.PaymentStatus == "Not Paid")
                .ToListAsync();
            List<DinningFeePayment> dinningFeePayments = await _entityFramework.DinningFeePayments
                .Where(d => d.StudentId == studentId && d.PaymentStatus == "Not Paid")
                .ToListAsync();

            if (hallFeePayments.Count > 0 || dinningFeePayments.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //GetStudentsByHallId
        public async Task<List<Student>> GetStudentsByHallId(int hallId)
        {
            List<Student> students = await _entityFramework.Students
                .Where(s => s.HallId == hallId)
                .ToListAsync();
            return students;
        }
    }
}
