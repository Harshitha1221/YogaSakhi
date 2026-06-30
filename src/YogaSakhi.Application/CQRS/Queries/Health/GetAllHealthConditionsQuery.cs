using MediatR;
using System.Collections.Generic;

namespace YogaSakhi.Application.CQRS.Queries.Health
{
    public class GetAllHealthConditionsQuery : IRequest<GetAllHealthConditionsResponse>
    {
    }

    public class GetAllHealthConditionsResponse
    {
        public List<HealthConditionListDto> HealthConditions { get; set; }
    }

    public class HealthConditionListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public int ExerciseCount { get; set; }
        public int SymptomCount { get; set; }
    }
}
