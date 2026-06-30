using System;
using System.Threading.Tasks;
using YogaSakhi.Domain.Entities;
using YogaSakhi.Domain.Interfaces;

namespace YogaSakhi.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YogaSakhiDbContext _context;
        private IRepository<HealthCondition> _healthConditions;
        private IRepository<Symptom> _symptoms;
        private IRepository<Treatment> _treatments;
        private IRepository<Exercise> _exercises;
        private IRepository<DietPlan> _dietPlans;
        private IRepository<User> _users;
        private IRepository<UserProfile> _userProfiles;
        private IRepository<UserAssessment> _userAssessments;
        private IRepository<UserProgress> _userProgresses;
        private IRepository<UserExerciseLog> _userExerciseLogs;

        public UnitOfWork(YogaSakhiDbContext context)
        {
            _context = context;
        }

        public IRepository<HealthCondition> HealthConditions => 
            _healthConditions ??= new Repository<HealthCondition>(_context);

        public IRepository<Symptom> Symptoms => 
            _symptoms ??= new Repository<Symptom>(_context);

        public IRepository<Treatment> Treatments => 
            _treatments ??= new Repository<Treatment>(_context);

        public IRepository<Exercise> Exercises => 
            _exercises ??= new Repository<Exercise>(_context);

        public IRepository<DietPlan> DietPlans => 
            _dietPlans ??= new Repository<DietPlan>(_context);

        public IRepository<User> Users => 
            _users ??= new Repository<User>(_context);

        public IRepository<UserProfile> UserProfiles => 
            _userProfiles ??= new Repository<UserProfile>(_context);

        public IRepository<UserAssessment> UserAssessments => 
            _userAssessments ??= new Repository<UserAssessment>(_context);

        public IRepository<UserProgress> UserProgresses => 
            _userProgresses ??= new Repository<UserProgress>(_context);

        public IRepository<UserExerciseLog> UserExerciseLogs => 
            _userExerciseLogs ??= new Repository<UserExerciseLog>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
