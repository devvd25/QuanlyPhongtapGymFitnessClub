using QuanlyPhongtapGymFitnessClub.Data;
using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace QuanlyPhongtapGymFitnessClub.Services
{
    public class TrainerService : ITrainerService
    {
        private readonly AppDbContext _context;

        public TrainerService(AppDbContext context)
        {
            _context = context;
        }

        public List<TrainerResponseDto> GetAll(string? search, string? specialty, int? minExperienceYears)
        {
            var query = _context.Trainers.Include(t => t.AssignedMembers).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(t => t.FullName.ToLower().Contains(searchLower) || t.Username.ToLower().Contains(searchLower));
            }

            if (!string.IsNullOrWhiteSpace(specialty))
            {
                query = query.Where(t => t.Specialty == specialty);
            }

            if (minExperienceYears.HasValue)
            {
                query = query.Where(t => t.ExperienceYears >= minExperienceYears.Value);
            }

            return query.ToList().Select(t => new TrainerResponseDto
            {
                Id = t.Id,
                Username = t.Username,
                FullName = t.FullName,
                Email = t.Email,
                Phone = t.Phone,
                Gender = t.Gender,
                DateOfBirth = t.DateOfBirth,
                Specialty = t.Specialty,
                ExperienceYears = t.ExperienceYears,
                Certifications = t.Certifications,
                Rating = t.Rating,
                HourlyRate = t.HourlyRate,
                MaxMembers = t.MaxMembers,
                AssignedMemberIds = t.AssignedMembers != null ? t.AssignedMembers.Select(m => m.Id).ToList() : new List<int>(),
                CurrentMembersCount = t.AssignedMembers != null ? t.AssignedMembers.Count : 0,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt
            }).ToList();
        }

        public TrainerResponseDto? GetById(int id)
        {
            var t = _context.Trainers.Include(tr => tr.AssignedMembers).FirstOrDefault(x => x.Id == id);
            if (t == null) return null;

            return new TrainerResponseDto
            {
                Id = t.Id,
                Username = t.Username,
                FullName = t.FullName,
                Email = t.Email,
                Phone = t.Phone,
                Gender = t.Gender,
                DateOfBirth = t.DateOfBirth,
                Specialty = t.Specialty,
                ExperienceYears = t.ExperienceYears,
                Certifications = t.Certifications,
                Rating = t.Rating,
                HourlyRate = t.HourlyRate,
                MaxMembers = t.MaxMembers,
                AssignedMemberIds = t.AssignedMembers != null ? t.AssignedMembers.Select(m => m.Id).ToList() : new List<int>(),
                CurrentMembersCount = t.AssignedMembers != null ? t.AssignedMembers.Count : 0,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt
            };
        }

        public TrainerResponseDto Create(TrainerCreateDto dto)
        {
            var newTrainer = new Trainer
            {
                Username = dto.Username,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                Specialty = dto.Specialty,
                ExperienceYears = dto.ExperienceYears,
                Certifications = dto.Certifications,
                HourlyRate = dto.HourlyRate,
                MaxMembers = dto.MaxMembers,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Trainers.Add(newTrainer);
            _context.SaveChanges();

            return GetById(newTrainer.Id)!;
        }

        public TrainerResponseDto? Update(int id, TrainerUpdateDto dto)
        {
            var trainer = _context.Trainers.FirstOrDefault(t => t.Id == id);
            if (trainer == null) return null;

            if (!string.IsNullOrWhiteSpace(dto.FullName)) trainer.FullName = dto.FullName;
            if (!string.IsNullOrWhiteSpace(dto.Email)) trainer.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.Phone)) trainer.Phone = dto.Phone;
            if (!string.IsNullOrWhiteSpace(dto.Specialty)) trainer.Specialty = dto.Specialty;
            if (dto.ExperienceYears.HasValue) trainer.ExperienceYears = dto.ExperienceYears.Value;

            trainer.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return GetById(id);
        }

        public bool Delete(int id)
        {
            var trainer = _context.Trainers.FirstOrDefault(t => t.Id == id);
            if (trainer == null) return false;

            _context.Trainers.Remove(trainer);
            _context.SaveChanges();
            return true;
        }

        public List<UserResponseDto> GetAssignedMembers(int trainerId)
        {
            var trainer = _context.Trainers.Include(t => t.AssignedMembers).FirstOrDefault(t => t.Id == trainerId);
            if (trainer == null || trainer.AssignedMembers == null) return new List<UserResponseDto>();

            var members = trainer.AssignedMembers.ToList();

            return members.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Role,
                MembershipPackage = u.MembershipPackage,
                IsActive = u.IsActive
            }).ToList();
        }

        public bool AssignMember(int trainerId, int memberId)
        {
            var trainer = _context.Trainers.Include(t => t.AssignedMembers).FirstOrDefault(t => t.Id == trainerId);
            if (trainer == null) return false;

            if (trainer.AssignedMembers.Count >= trainer.MaxMembers) return false;
            if (trainer.AssignedMembers.Any(m => m.Id == memberId)) return true;

            var user = _context.Users.FirstOrDefault(u => u.Id == memberId);
            if (user != null)
            {
                trainer.AssignedMembers.Add(user);
                trainer.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }
            
            return true;
        }
    }
}
