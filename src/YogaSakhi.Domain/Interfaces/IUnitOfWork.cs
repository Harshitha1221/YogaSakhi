using System;
using System.Threading.Tasks;
using YogaSakhi.Domain.Entities;

namespace YogaSakhi.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<HealthCondition> HealthConditions { get; }
        IRepository<Symptom> Symptoms { get; }
        IRepository<Treatment> Treatments { get; }
        IRepository<Exercise> Exercises { get; }
        IRepository<DietPlan> DietPlans { get; }
        IRepository<User> Users { get; }
        IRepository<UserProfile> UserProfiles { get; }
        IRepository<UserAssessment> UserAssessments { get; }
        IRepository<UserProgress> UserProgresses { get; }
        IRepository<UserExerciseLog> UserExerciseLogs { get; }

        Task<int> SaveChangesAsync();
    }
}
