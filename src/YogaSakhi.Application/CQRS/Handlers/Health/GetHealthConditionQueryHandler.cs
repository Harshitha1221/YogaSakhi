using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YogaSakhi.Application.CQRS.Queries.Health;
using YogaSakhi.Domain.Interfaces;

namespace YogaSakhi.Application.CQRS.Handlers.Health
{
    public class GetHealthConditionQueryHandler : IRequestHandler<GetHealthConditionQuery, GetHealthConditionResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetHealthConditionQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetHealthConditionResponse> Handle(GetHealthConditionQuery request, CancellationToken cancellationToken)
        {
            var condition = await _unitOfWork.HealthConditions.GetByIdAsync(request.HealthConditionId);

            if (condition == null)
            {
                return null;
            }

            var symptoms = await _unitOfWork.Symptoms.FindAsync(s => s.HealthConditionId == request.HealthConditionId);
            var exercises = await _unitOfWork.Exercises.FindAsync(e => e.HealthConditionId == request.HealthConditionId);
            var dietPlans = await _unitOfWork.DietPlans.FindAsync(d => d.HealthConditionId == request.HealthConditionId);
            var treatments = await _unitOfWork.Treatments.FindAsync(t => t.HealthConditionId == request.HealthConditionId);

            return new GetHealthConditionResponse
            {
                Id = condition.Id,
                Name = condition.Name,
                Description = condition.Description,
                Overview = condition.Overview,
                Symptoms = symptoms.Select(s => new SymptomDto
                {
                    Id = s.Id,
                    SymptomName = s.SymptomName,
                    Description = s.Description,
                    SeverityLevel = s.SeverityLevel
                }).ToList(),
                Exercises = exercises.Select(e => new ExerciseDto
                {
                    Id = e.Id,
                    ExerciseName = e.ExerciseName,
                    Description = e.Description,
                    DurationMinutes = e.DurationMinutes,
                    IntensityLevel = e.IntensityLevel,
                    VideoUrl = e.VideoUrl,
                    IsSafeInPregnancy = e.IsSafeInPregnancy
                }).ToList(),
                DietPlans = dietPlans.Select(d => new DietPlanDto
                {
                    Id = d.Id,
                    PlanName = d.PlanName,
                    FoodsToInclude = d.FoodsToInclude,
                    FoodsToAvoid = d.FoodsToAvoid
                }).ToList(),
                Treatments = treatments.Select(t => new TreatmentDto
                {
                    Id = t.Id,
                    TreatmentName = t.TreatmentName,
                    Description = t.Description,
                    MedicalAdvice = t.MedicalAdvice
                }).ToList()
            };
        }
    }
}
