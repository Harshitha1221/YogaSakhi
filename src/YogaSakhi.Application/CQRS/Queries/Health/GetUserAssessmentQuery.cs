using MediatR;
using System;
using System.Collections.Generic;

namespace YogaSakhi.Application.CQRS.Queries.Health
{
    public class GetUserAssessmentQuery : IRequest<GetUserAssessmentResponse>
    {
        public int UserId { get; set; }
    }

    public class GetUserAssessmentResponse
    {
        public List<AssessmentHistoryDto> Assessments { get; set; }
    }

    public class AssessmentHistoryDto
    {
        public int Id { get; set; }
        public int HealthConditionId { get; set; }
        public string ConditionName { get; set; }
        public int AssessmentScore { get; set; }
        public DateTime AssessmentDate { get; set; }
        public string AIRecommendation { get; set; }
    }
}
