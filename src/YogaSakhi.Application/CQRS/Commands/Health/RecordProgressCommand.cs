using MediatR;

namespace YogaSakhi.Application.CQRS.Commands.Health
{
    public class RecordProgressCommand : IRequest<RecordProgressResponse>
    {
        public int UserId { get; set; }
        public int HealthConditionId { get; set; }
        public int ProgressScore { get; set; }
        public string Notes { get; set; }
        public string SymptomImprovement { get; set; }
        public int ExercisesCompleted { get; set; }
        public int DietComplianceScore { get; set; }
    }

    public class RecordProgressResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int ProgressId { get; set; }
    }
}
