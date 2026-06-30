using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YogaSakhi.Application.CQRS.Queries.Health;
using YogaSakhi.Domain.Interfaces;

namespace YogaSakhi.Application.CQRS.Handlers.Health
{
    public class GetAllHealthConditionsQueryHandler : IRequestHandler<GetAllHealthConditionsQuery, GetAllHealthConditionsResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllHealthConditionsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllHealthConditionsResponse> Handle(GetAllHealthConditionsQuery request, CancellationToken cancellationToken)
        {
            var conditions = await _unitOfWork.HealthConditions.FindAsync(c => c.IsActive);

            var response = new GetAllHealthConditionsResponse
            {
                HealthConditions = conditions.Select(c => new HealthConditionListDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IconUrl = c.IconUrl,
                    ExerciseCount = c.Exercises?.Count ?? 0,
                    SymptomCount = c.Symptoms?.Count ?? 0
                }).ToList()
            };

            return response;
        }
    }
}
