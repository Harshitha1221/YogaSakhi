using System;
using System.Collections.Generic;

namespace YogaSakhi.Domain.Entities
{
    public enum UserRole
    {
        User = 1,
        Doctor = 2,
        YogaInstructor = 3,
        Admin = 4
    }

    public class User
    {
        public int Id { get; set; }
        public string UserId { get; set; } // GUID for external reference
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime MembershipDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool EmailVerified { get; set; } = false;
        public DateTime? LastLoginDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public virtual UserProfile Profile { get; set; }
        public virtual ICollection<UserAssessment> Assessments { get; set; } = new List<UserAssessment>();
        public virtual ICollection<UserProgress> Progresses { get; set; } = new List<UserProgress>();
        public virtual ICollection<UserExerciseLog> ExerciseLogs { get; set; } = new List<UserExerciseLog>();
    }

    public class UserProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string MedicalHistory { get; set; }
        public string CurrentMedications { get; set; }
        public string PregnancyStatus { get; set; } // "NotPregnant", "Pregnant", "PostPartum"
        public int WeeksPregnant { get; set; }
        public bool HasDiastasisRecti { get; set; }
        public string FitnessLevel { get; set; } // "Beginner", "Intermediate", "Advanced"
        public string PreferredLanguage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual User User { get; set; }
    }

    public class UserAssessment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int HealthConditionId { get; set; }
        public int AssessmentScore { get; set; } // 1-10
        public string AssessmentDetails { get; set; }
        public DateTime AssessmentDate { get; set; }
        public string AIRecommendation { get; set; }
        public string DoctorNotes { get; set; }
        public bool NeedsDoctorReview { get; set; }

        public virtual User User { get; set; }
        public virtual HealthCondition HealthCondition { get; set; }
    }

    public class UserProgress
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int HealthConditionId { get; set; }
        public DateTime ProgressDate { get; set; }
        public int ProgressScore { get; set; } // 1-10
        public string Notes { get; set; }
        public string SymptomImprovement { get; set; }
        public int ExercisesCompleted { get; set; }
        public int DietComplianceScore { get; set; }

        public virtual User User { get; set; }
        public virtual HealthCondition HealthCondition { get; set; }
    }

    public class UserExerciseLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ExerciseId { get; set; }
        public DateTime CompletedDate { get; set; }
        public int ActualDurationMinutes { get; set; }
        public string DifficultyFelt { get; set; } // "Easy", "Moderate", "Difficult"
        public string Notes { get; set; }
        public bool Completed { get; set; } = true;

        public virtual User User { get; set; }
    }
}
