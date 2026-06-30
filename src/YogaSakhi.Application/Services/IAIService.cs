using System.Threading.Tasks;

namespace YogaSakhi.Application.Services
{
    public interface IAIService
    {
        Task<string> GeneratePersonalizedRecommendationAsync(
            int userId, 
            int conditionId, 
            string additionalContext = null);

        Task<string> AnalyzeSymptomsAsync(
            int userId, 
            int conditionId, 
            string symptoms);

        Task<string> GenerateProgressReportAsync(int userId);
        Task<string> GenerateExercisePlanAsync(int userId, int conditionId);
        Task<string> GenerateDietPlanAsync(int userId, int conditionId);
    }
}
