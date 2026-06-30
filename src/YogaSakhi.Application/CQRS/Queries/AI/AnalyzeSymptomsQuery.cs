using MediatR;
using System.Collections.Generic;

namespace YogaSakhi.Application.CQRS.Queries.AI
{
    public class AnalyzeSymptomsQuery : IRequest<AnalyzeSymptomsResponse>
    {
        public int UserId { get; set; }
        public int HealthConditionId { get; set; }
        public string Symptoms { get; set; }
    }

    public class AnalyzeSymptomsResponse
    {
        public string AnalysisResult { get; set; }
        public int SeverityScore { get; set; }
        public List<string> MatchedSymptoms { get; set; }
        public string RecommendedAction { get; set; }
        public bool RequiresDoctorConsultation { get; set; }
    }
}
