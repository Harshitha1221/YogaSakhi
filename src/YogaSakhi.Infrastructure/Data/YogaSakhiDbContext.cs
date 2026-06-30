using Microsoft.EntityFrameworkCore;
using YogaSakhi.Domain.Entities;
using System;

namespace YogaSakhi.Infrastructure.Data
{
    public class YogaSakhiDbContext : DbContext
    {
        public YogaSakhiDbContext(DbContextOptions<YogaSakhiDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<HealthCondition> HealthConditions { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<DietPlan> DietPlans { get; set; }
        public DbSet<UserAssessment> UserAssessments { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }
        public DbSet<UserExerciseLog> UserExerciseLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // One-to-One: User -> UserProfile
            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: User -> UserAssessment
            modelBuilder.Entity<UserAssessment>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.Assessments)
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: User -> UserProgress
            modelBuilder.Entity<UserProgress>()
                .HasOne(up => up.User)
                .WithMany(u => u.Progresses)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: User -> UserExerciseLog
            modelBuilder.Entity<UserExerciseLog>()
                .HasOne(uel => uel.User)
                .WithMany(u => u.ExerciseLogs)
                .HasForeignKey(uel => uel.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // HealthCondition relationships
            modelBuilder.Entity<Symptom>()
                .HasOne(s => s.HealthCondition)
                .WithMany(h => h.Symptoms)
                .HasForeignKey(s => s.HealthConditionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Treatment>()
                .HasOne(t => t.HealthCondition)
                .WithMany(h => h.Treatments)
                .HasForeignKey(t => t.HealthConditionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exercise>()
                .HasOne(e => e.HealthCondition)
                .WithMany(h => h.Exercises)
                .HasForeignKey(e => e.HealthConditionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DietPlan>()
                .HasOne(d => d.HealthCondition)
                .WithMany(h => h.DietPlans)
                .HasForeignKey(d => d.HealthConditionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAssessment>()
                .HasOne(ua => ua.HealthCondition)
                .WithMany(h => h.UserAssessments)
                .HasForeignKey(ua => ua.HealthConditionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Health Conditions
            modelBuilder.Entity<HealthCondition>().HasData(
                new HealthCondition
                {
                    Id = 1,
                    ConditionType = HealthConditionType.PCOS,
                    Name = "PCOS Management",
                    Description = "Polycystic Ovary Syndrome - Hormonal & Metabolic Health",
                    Overview = "PCOS affects insulin production and reproductive hormones. Our program focuses on weight management, exercise, and diet to regulate hormones naturally.",
                    IconUrl = "/images/pcos.png",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IsActive = true
                },
                new HealthCondition
                {
                    Id = 2,
                    ConditionType = HealthConditionType.Thyroid,
                    Name = "Thyroid Management",
                    Description = "Thyroid Health & Metabolism Support",
                    Overview = "Optimize thyroid function through targeted exercises, nutritional support, and lifestyle modifications. Our program supports both hyperthyroidism and hypothyroidism.",
                    IconUrl = "/images/thyroid.png",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IsActive = true
                },
                new HealthCondition
                {
                    Id = 3,
                    ConditionType = HealthConditionType.CoreStrengthening,
                    Name = "Core Strengthening",
                    Description = "Build Core Stability & Strength",
                    Overview = "Strengthen your core muscles for better posture, reduced back pain, and improved functional fitness. Progressive exercises from beginner to advanced.",
                    IconUrl = "/images/core.png",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IsActive = true
                },
                new HealthCondition
                {
                    Id = 4,
                    ConditionType = HealthConditionType.PregnancyYoga,
                    Name = "Pregnancy Yoga",
                    Description = "Safe Yoga During All Pregnancy Stages",
                    Overview = "Gentle yoga practices designed for pregnant women. Reduce pain, prepare for labor, improve breathing, and maintain fitness safely throughout pregnancy.",
                    IconUrl = "/images/pregnancy.png",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IsActive = true
                },
                new HealthCondition
                {
                    Id = 5,
                    ConditionType = HealthConditionType.DiastasisRecti,
                    Name = "Diastasis Recti Recovery",
                    Description = "Abdominal Separation Recovery Program",
                    Overview = "Targeted recovery program for abdominal muscle separation. Progressive exercises to heal and strengthen your core after pregnancy.",
                    IconUrl = "/images/diastasis.png",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IsActive = true
                }
            );

            // Seed Symptoms for PCOS
            modelBuilder.Entity<Symptom>().HasData(
                new Symptom { Id = 1, HealthConditionId = 1, SymptomName = "Irregular Periods", Description = "Unpredictable menstrual cycles", SeverityLevel = 8, Category = "Menstrual" },
                new Symptom { Id = 2, HealthConditionId = 1, SymptomName = "Weight Gain", Description = "Difficulty losing weight", SeverityLevel = 7, Category = "Metabolic" },
                new Symptom { Id = 3, HealthConditionId = 1, SymptomName = "Acne", Description = "Increased acne due to hormones", SeverityLevel = 6, Category = "Skin" },
                new Symptom { Id = 4, HealthConditionId = 1, SymptomName = "Hair Loss", Description = "Excessive hair loss from scalp", SeverityLevel = 7, Category = "Hair" },
                new Symptom { Id = 5, HealthConditionId = 1, SymptomName = "Fatigue", Description = "Constant tiredness and low energy", SeverityLevel = 7, Category = "Energy" },
                // Thyroid Symptoms
                new Symptom { Id = 6, HealthConditionId = 2, SymptomName = "Fatigue", Description = "Persistent tiredness", SeverityLevel = 8, Category = "Energy" },
                new Symptom { Id = 7, HealthConditionId = 2, SymptomName = "Weight Gain", Description = "Unexplained weight gain", SeverityLevel = 7, Category = "Weight" },
                new Symptom { Id = 8, HealthConditionId = 2, SymptomName = "Cold Sensitivity", Description = "Feeling cold easily", SeverityLevel = 6, Category = "Temperature" },
                new Symptom { Id = 9, HealthConditionId = 2, SymptomName = "Brain Fog", Description = "Difficulty concentrating", SeverityLevel = 7, Category = "Cognitive" },
                new Symptom { Id = 10, HealthConditionId = 2, SymptomName = "Dry Skin", Description = "Dry and itchy skin", SeverityLevel = 5, Category = "Skin" }
            );

            // Seed Exercises for PCOS
            modelBuilder.Entity<Exercise>().HasData(
                new Exercise
                {
                    Id = 1,
                    HealthConditionId = 1,
                    ExerciseName = "Brisk Walking",
                    Description = "Low-impact cardio exercise",
                    DurationMinutes = 30,
                    IntensityLevel = 2,
                    Instructions = "Walk at a pace where you can talk but not sing. Maintain good posture.",
                    Modifications = "Can be done on treadmill or outdoors",
                    IsSafeInPregnancy = true,
                    VideoUrl = "https://www.youtube.com/watch?v=walk"
                },
                new Exercise
                {
                    Id = 2,
                    HealthConditionId = 1,
                    ExerciseName = "Yoga Flow",
                    Description = "Gentle to moderate yoga sequence",
                    DurationMinutes = 45,
                    IntensityLevel = 2,
                    Instructions = "Follow along with instructor. Focus on breathing and alignment.",
                    Modifications = "Use props for support as needed",
                    IsSafeInPregnancy = true,
                    VideoUrl = "https://www.youtube.com/watch?v=yoga"
                },
                new Exercise
                {
                    Id = 3,
                    HealthConditionId = 1,
                    ExerciseName = "Resistance Training",
                    Description = "Strength building with weights",
                    DurationMinutes = 30,
                    IntensityLevel = 4,
                    Instructions = "3 sets of 8-12 reps. Rest 60 seconds between sets.",
                    Modifications = "Reduce weight or reps as needed",
                    IsSafeInPregnancy = false,
                    VideoUrl = "https://www.youtube.com/watch?v=weights"
                }
            );

            // Seed Diet Plans for PCOS
            modelBuilder.Entity<DietPlan>().HasData(
                new DietPlan
                {
                    Id = 1,
                    HealthConditionId = 1,
                    PlanName = "Low GI PCOS Diet",
                    Description = "Focus on low glycemic index foods to manage insulin resistance",
                    FoodsToInclude = "Quinoa, brown rice, oats, lentils, chickpeas, sweet potatoes, leafy greens, berries, nuts, seeds, lean proteins",
                    FoodsToAvoid = "White bread, white rice, sugary drinks, processed foods, refined carbs, high sugar fruits",
                    CaloriesPerDay = 1800,
                    MealPlan = "Breakfast: Oatmeal with berries\nLunch: Grilled chicken with brown rice\nDinner: Lentil curry with quinoa\nSnacks: Nuts and seeds"
                }
            );

            // Seed Treatments for PCOS
            modelBuilder.Entity<Treatment>().HasData(
                new Treatment
                {
                    Id = 1,
                    HealthConditionId = 1,
                    TreatmentName = "Lifestyle Modification",
                    Description = "Primary approach for PCOS management",
                    MedicalAdvice = "5-10% weight loss can restore ovulation and improve symptoms",
                    LifestyleChanges = "Regular exercise (150 min/week), balanced diet, stress management, adequate sleep",
                    IsRecommended = true
                }
            );
        }
    }
}
