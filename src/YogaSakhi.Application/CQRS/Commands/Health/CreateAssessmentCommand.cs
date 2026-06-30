using MediatR;

namespace YogaSakhi.Application.CQRS.Commands.Health
{
    public class CreateAssessmentCommand : IRequest<CreateAssessmentResponse>
    {
        public int UserId { get; set; }
        public int HealthConditionId { get; set; }
        public int AssessmentScore { get; set; }
        public string AssessmentDetails { get; set; }
        public string Symptoms { get; set; }
    }

    public class CreateAssessmentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int AssessmentId { get; set; }
        public string AIRecommendation { get; set; }
    }
}
