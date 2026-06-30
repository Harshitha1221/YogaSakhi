using MediatR;
using System.Collections.Generic;

namespace YogaSakhi.Application.CQRS.Queries.Health
{
    public class GetHealthConditionQuery : IRequest<GetHealthConditionResponse>
    {
        public int HealthConditionId { get; set; }
    }

    public class GetHealthConditionResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Overview { get; set; }
        public List<SymptomDto> Symptoms { get; set; }
        public List<ExerciseDto> Exercises { get; set; }
        public List<DietPlanDto> DietPlans { get; set; }
        public List<TreatmentDto> Treatments { get; set; }
    }

    public class SymptomDto
    {
        public int Id { get; set; }
        public string SymptomName { get; set; }
        public string Description { get; set; }
        public int SeverityLevel { get; set; }
    }

    public class ExerciseDto
    {
        public int Id { get; set; }
        public string ExerciseName { get; set; }
        public string Description { get; set; }
        public int DurationMinutes { get; set; }
        public int IntensityLevel { get; set; }
        public string VideoUrl { get; set; }
        public bool IsSafeInPregnancy { get; set; }
    }

    public class DietPlanDto
    {
        public int Id { get; set; }
        public string PlanName { get; set; }
        public string FoodsToInclude { get; set; }
        public string FoodsToAvoid { get; set; }
    }

    public class TreatmentDto
    {
        public int Id { get; set; }
        public string TreatmentName { get; set; }
        public string Description { get; set; }
        public string MedicalAdvice { get; set; }
    }
}
