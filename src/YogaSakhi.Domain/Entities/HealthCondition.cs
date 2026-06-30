using System;
using System.Collections.Generic;

namespace YogaSakhi.Domain.Entities
{
    public enum HealthConditionType
    {
        PCOS = 1,
        Thyroid = 2,
        CoreStrengthening = 3,
        PregnancyYoga = 4,
        DiastasisRecti = 5
    }

    public class HealthCondition
    {
        public int Id { get; set; }
        public HealthConditionType ConditionType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Overview { get; set; }
        public string IconUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Symptom> Symptoms { get; set; } = new List<Symptom>();
        public virtual ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
        public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
        public virtual ICollection<DietPlan> DietPlans { get; set; } = new List<DietPlan>();
        public virtual ICollection<UserAssessment> UserAssessments { get; set; } = new List<UserAssessment>();
    }

    public class Symptom
    {
        public int Id { get; set; }
        public int HealthConditionId { get; set; }
        public string SymptomName { get; set; }
        public string Description { get; set; }
        public int SeverityLevel { get; set; } // 1-10
        public string Category { get; set; }

        public virtual HealthCondition HealthCondition { get; set; }
    }

    public class Treatment
    {
        public int Id { get; set; }
        public int HealthConditionId { get; set; }
        public string TreatmentName { get; set; }
        public string Description { get; set; }
        public string MedicalAdvice { get; set; }
        public string LifestyleChanges { get; set; }
        public bool IsRecommended { get; set; }

        public virtual HealthCondition HealthCondition { get; set; }
    }

    public class Exercise
    {
        public int Id { get; set; }
        public int HealthConditionId { get; set; }
        public string ExerciseName { get; set; }
        public string Description { get; set; }
        public int DurationMinutes { get; set; }
        public int IntensityLevel { get; set; } // 1-5
        public string VideoUrl { get; set; }
        public string Instructions { get; set; }
        public string Modifications { get; set; }
        public bool IsSafeInPregnancy { get; set; }

        public virtual HealthCondition HealthCondition { get; set; }
    }

    public class DietPlan
    {
        public int Id { get; set; }
        public int HealthConditionId { get; set; }
        public string PlanName { get; set; }
        public string Description { get; set; }
        public string FoodsToInclude { get; set; }
        public string FoodsToAvoid { get; set; }
        public int CaloriesPerDay { get; set; }
        public string MealPlan { get; set; }

        public virtual HealthCondition HealthCondition { get; set; }
    }
}
