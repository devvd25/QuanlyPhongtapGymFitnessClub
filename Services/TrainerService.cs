using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Models;

namespace QuanlyPhongtapGymFitnessClub.Services
{
    /// <summary>
    /// Service xá»­ lĂ½ nghiá»‡p vá»¥ cho Huáº¥n luyá»‡n viĂªn (ÄÄƒng kĂ½ Scoped trong Program.cs)
    /// </summary>
    public class TrainerService : ITrainerService
    {
        private static readonly List<Trainer> _trainers = new()
        {
            new Trainer
            {
                Id = 1,
                Username = "trainer_tuan",
                FullName = "Nguyá»…n Quá»‘c Tuáº¥n",
                Email = "tuan.nguyen@fitnessclub.vn",
                Phone = "0981112233",
                Gender = "Male",
                DateOfBirth = new DateTime(1995, 4, 12),
                Specialty = "Bodybuilding",
                ExperienceYears = 6,
                Certifications = "NASM-CPT, ISSA Master Trainer",
                Rating = 4.9,
                HourlyRate = 450000,
                MaxMembers = 12,
                AssignedMemberIds = new List<int> { 1, 2, 5 },
                IsActive = true,
                CreatedAt = new DateTime(2024, 3, 1)
            },
            new Trainer
            {
                Id = 2,
                Username = "trainer_cam",
                FullName = "LĂª Thá»‹ Cáº©m",
                Email = "cam.le@fitnessclub.vn",
                Phone = "0923456789",
                Gender = "Female",
                DateOfBirth = new DateTime(1998, 8, 20),
                Specialty = "Yoga",
                ExperienceYears = 4,
                Certifications = "Yoga Alliance RYT-500",
                Rating = 5.0,
                HourlyRate = 400000,
                MaxMembers = 10,
                AssignedMemberIds = new List<int> { 8, 9 },
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 15)
            },
            new Trainer
            {
                Id = 3,
                Username = "trainer_giang",
                FullName = "Äáº·ng Thá»‹ Giang",
                Email = "giang.dang@fitnessclub.vn",
                Phone = "0967890123",
                Gender = "Female",
                DateOfBirth = new DateTime(1996, 11, 25),
                Specialty = "Pilates",
                ExperienceYears = 5,
                Certifications = "Polestar Pilates Comprehensive",
                Rating = 4.8,
                HourlyRate = 500000,
                MaxMembers = 8,
                AssignedMemberIds = new List<int> { 10 },
                IsActive = true,
                CreatedAt = new DateTime(2024, 6, 10)
            },
            new Trainer
            {
                Id = 4,
                Username = "trainer_hoang",
                FullName = "VÅ© Viá»‡t HoĂ ng",
                Email = "hoang.vu@fitnessclub.vn",
                Phone = "0933445566",
                Gender = "Male",
                DateOfBirth = new DateTime(1994, 2, 18),
                Specialty = "Boxing",
                ExperienceYears = 7,
                Certifications = "WBC Muay Thai Coach, NASM-PES",
                Rating = 4.9,
                HourlyRate = 550000,
                MaxMembers = 14,
                AssignedMemberIds = new List<int>(),
                IsActive = true,
                CreatedAt = new DateTime(2023, 9, 20)
            }
        };

        // Danh sĂ¡ch há»™i viĂªn máº«u Ä‘á»ƒ hiá»ƒn thá»‹ khi gá»i nested route
        private static readonly List<UserResponseDto> _mockAssignedMembers = new()
        {
            new UserResponseDto { Id = 1, Username = "nguyenvanan", FullName = "Nguyá»…n VÄƒn An", Email = "an.nguyen@gmail.com", Phone = "0901234567", Role = "Member", MembershipPackage = "VIP", MembershipStatus = "Active", IsActive = true },
            new UserResponseDto { Id = 2, Username = "tranvanbinh", FullName = "Tráº§n VÄƒn BĂ¬nh", Email = "binh.tran@gmail.com", Phone = "0912345678", Role = "Member", MembershipPackage = "Basic", MembershipStatus = "Active", IsActive = true },
            new UserResponseDto { Id = 5, Username = "hoangthimai", FullName = "HoĂ ng Thá»‹ Mai", Email = "mai.hoang@gmail.com", Phone = "0945678901", Role = "Member", MembershipPackage = "VIP", MembershipStatus = "Active", IsActive = true },
            new UserResponseDto { Id = 8, Username = "buivanhuy", FullName = "BĂ¹i VÄƒn Huy", Email = "huy.bui@gmail.com", Phone = "0978901234", Role = "Member", MembershipPackage = "VIP", MembershipStatus = "Active", IsActive = true },
            new UserResponseDto { Id = 9, Username = "ngothiyen", FullName = "NgĂ´ Thá»‹ Yáº¿n", Email = "yen.ngo@gmail.com", Phone = "0989012345", Role = "Member", MembershipPackage = "Basic", MembershipStatus = "Active", IsActive = true },
            new UserResponseDto { Id = 10, Username = "dinhvankhoa", FullName = "Äinh VÄƒn VÅ©", Email = "khoa.dinh@gmail.com", Phone = "0990123456", Role = "Member", MembershipPackage = "Diamond", MembershipStatus = "Active", IsActive = true },
        };

        public List<TrainerResponseDto> GetAll(string? search, string? specialty, int? minExperienceYears)
        {
            var query = _trainers.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t => t.FullName.Contains(search, StringComparison.OrdinalIgnoreCase)
                                      || t.Username.Contains(search, StringComparison.OrdinalIgnoreCase)
                                      || t.Specialty.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(specialty))
            {
                query = query.Where(t => t.Specialty.Equals(specialty, StringComparison.OrdinalIgnoreCase));
            }

            if (minExperienceYears.HasValue)
            {
                query = query.Where(t => t.ExperienceYears >= minExperienceYears.Value);
            }

            return query.Select(MapToResponseDto).ToList();
        }

        public TrainerResponseDto? GetById(int id)
        {
            var trainer = _trainers.FirstOrDefault(t => t.Id == id);
            return trainer == null ? null : MapToResponseDto(trainer);
        }

        public TrainerResponseDto Create(TrainerCreateDto dto)
        {
            var newTrainer = new Trainer
            {
                Id = _trainers.Any() ? _trainers.Max(t => t.Id) + 1 : 1,
                Username = dto.Username,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                Specialty = dto.Specialty,
                ExperienceYears = dto.ExperienceYears,
                Certifications = dto.Certifications,
                Rating = 5.0,
                HourlyRate = dto.HourlyRate,
                MaxMembers = dto.MaxMembers,
                AssignedMemberIds = new List<int>(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _trainers.Add(newTrainer);
            return MapToResponseDto(newTrainer);
        }

        public TrainerResponseDto? Update(int id, TrainerUpdateDto dto)
        {
            var trainer = _trainers.FirstOrDefault(t => t.Id == id);
            if (trainer == null) return null;

            if (!string.IsNullOrWhiteSpace(dto.FullName)) trainer.FullName = dto.FullName;
            if (!string.IsNullOrWhiteSpace(dto.Email)) trainer.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.Phone)) trainer.Phone = dto.Phone;
            if (!string.IsNullOrWhiteSpace(dto.Specialty)) trainer.Specialty = dto.Specialty;
            if (dto.ExperienceYears.HasValue) trainer.ExperienceYears = dto.ExperienceYears.Value;
            if (dto.Certifications != null) trainer.Certifications = dto.Certifications;
            if (dto.HourlyRate.HasValue) trainer.HourlyRate = dto.HourlyRate.Value;
            if (dto.MaxMembers.HasValue) trainer.MaxMembers = dto.MaxMembers.Value;
            if (dto.IsActive.HasValue) trainer.IsActive = dto.IsActive.Value;
            trainer.UpdatedAt = DateTime.UtcNow;

            return MapToResponseDto(trainer);
        }

        public bool Delete(int id)
        {
            var trainer = _trainers.FirstOrDefault(t => t.Id == id);
            if (trainer == null) return false;

            _trainers.Remove(trainer);
            return true;
        }

        public List<UserResponseDto> GetAssignedMembers(int trainerId)
        {
            var trainer = _trainers.FirstOrDefault(t => t.Id == trainerId);
            if (trainer == null) return new List<UserResponseDto>();

            return _mockAssignedMembers
                .Where(m => trainer.AssignedMemberIds.Contains(m.Id))
                .ToList();
        }

        public bool AssignMember(int trainerId, int memberId)
        {
            var trainer = _trainers.FirstOrDefault(t => t.Id == trainerId);
            if (trainer == null) return false;

            if (!trainer.AssignedMemberIds.Contains(memberId))
            {
                if (trainer.AssignedMemberIds.Count >= trainer.MaxMembers)
                {
                    return false; // ÄĂ£ Ä‘áº¡t sá»‘ lÆ°á»£ng há»c viĂªn tá»‘i Ä‘a
                }
                trainer.AssignedMemberIds.Add(memberId);
            }

            return true;
        }

        private static TrainerResponseDto MapToResponseDto(Trainer t) => new()
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
            CurrentMembersCount = t.AssignedMemberIds.Count,
            AssignedMemberIds = t.AssignedMemberIds,
            IsActive = t.IsActive,
            CreatedAt = t.CreatedAt
        };
    }
}
