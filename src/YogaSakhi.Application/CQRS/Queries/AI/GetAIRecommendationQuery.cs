using MediatR;

namespace YogaSakhi.Application.CQRS.Queries.AI
{
    public class GetAIRecommendationQuery : IRequest<GetAIRecommendationResponse>
    {
        public int UserId { get; set; }
        public int HealthConditionId { get; set; }
    }

    public class GetAIRecommendationResponse
    {
        public string Recommendation { get; set; }
        public string ExercisePlan { get; set; }
        public string DietPlan { get; set; }
        public string LifestyleAdvice { get; set; }
        public string WarningNotes { get; set; }
    }
}
