using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaSakhi.Domain.Interfaces;

namespace YogaSakhi.Application.Services
{
    public class AIService : IAIService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AIService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GeneratePersonalizedRecommendationAsync(
            int userId, 
            int conditionId, 
            string additionalContext = null)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            var condition = await _unitOfWork.HealthConditions.GetByIdAsync(conditionId);
            var userProfile = await _unitOfWork.UserProfiles.SingleOrDefaultAsync(p => p.UserId == userId);
            var assessments = await _unitOfWork.UserAssessments.FindAsync(
                a => a.UserId == userId && a.HealthConditionId == conditionId);

            var latestAssessment = assessments.OrderByDescending(a => a.AssessmentDate).FirstOrDefault();

            var recommendation = new StringBuilder();
            recommendation.AppendLine($"\n🌺 Personalized Wellness Plan for {condition.Name}");
            recommendation.AppendLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
            recommendation.AppendLine($"Dear {user.FullName},\n");

            // Current Status
            if (latestAssessment != null)
            {
                recommendation.AppendLine($"📊 Current Assessment Score: {latestAssessment.AssessmentScore}/10");
                recommendation.AppendLine($"📅 Last Assessment: {latestAssessment.AssessmentDate:MMMM dd, yyyy}\n");
            }

            // Pregnancy considerations
            if (userProfile?.PregnancyStatus == "Pregnant")
            {
                recommendation.AppendLine($"🤰 Pregnancy Status: {userProfile.WeeksPregnant} weeks");
                recommendation.AppendLine("⚠️  Pregnancy-safe modifications will be applied to all recommendations.\n");
            }

            // Exercise Plan
            recommendation.AppendLine("💪 RECOMMENDED EXERCISE PLAN:");
            recommendation.AppendLine("─────────────────────────────────────────────────────────────────────────");
            var exercises = await _unitOfWork.Exercises.FindAsync(
                e => e.HealthConditionId == conditionId);
            
            var safeExercises = userProfile?.PregnancyStatus == "Pregnant" 
                ? exercises.Where(e => e.IsSafeInPregnancy).ToList()
                : exercises.OrderBy(e => e.IntensityLevel).ToList();

            foreach (var exercise in safeExercises.Take(5))
            {
                recommendation.AppendLine($"• {exercise.ExerciseName} ({exercise.DurationMinutes} mins - Level {exercise.IntensityLevel}/5)");
                recommendation.AppendLine($"  Instructions: {exercise.Instructions}");
                if (exercise.Modifications != null)
                {
                    recommendation.AppendLine($"  Modifications: {exercise.Modifications}");
                }
            }

            recommendation.AppendLine();

            // Diet Plan
            recommendation.AppendLine("🥗 NUTRITIONAL RECOMMENDATIONS:");
            recommendation.AppendLine("─────────────────────────────────────────────────────────────────────────");
            var dietPlans = await _unitOfWork.DietPlans.FindAsync(
                d => d.HealthConditionId == conditionId);
            
            var dietPlan = dietPlans.FirstOrDefault();
            if (dietPlan != null)
            {
                recommendation.AppendLine($"Plan: {dietPlan.PlanName}");
                recommendation.AppendLine($"Daily Calorie Target: {dietPlan.CaloriesPerDay} calories");
                recommendation.AppendLine($"Foods to Include: {dietPlan.FoodsToInclude}");
                recommendation.AppendLine($"Foods to Avoid: {dietPlan.FoodsToAvoid}");
                if (dietPlan.MealPlan != null)
                {
                    recommendation.AppendLine($"Sample Meal Plan: {dietPlan.MealPlan}");
                }
            }

            recommendation.AppendLine();

            // Lifestyle Tips
            recommendation.AppendLine("🧘 LIFESTYLE RECOMMENDATIONS:");
            recommendation.AppendLine("─────────────────────────────────────────────────────────────────────────");
            var treatments = await _unitOfWork.Treatments.FindAsync(
                t => t.HealthConditionId == conditionId && t.IsRecommended);
            
            foreach (var treatment in treatments.Take(3))
            {
                recommendation.AppendLine($"• {treatment.TreatmentName}: {treatment.LifestyleChanges}");
            }

            recommendation.AppendLine();
            recommendation.AppendLine("⚠️  IMPORTANT DISCLAIMER:");
            recommendation.AppendLine("This is AI-assisted guidance for educational purposes only.");
            recommendation.AppendLine("Always consult with a qualified healthcare professional for medical advice.");
            recommendation.AppendLine("\nYour wellness journey is unique. Stay consistent, listen to your body, and celebrate small victories! 💖");

            return recommendation.ToString();
        }

        public async Task<string> AnalyzeSymptomsAsync(
            int userId, 
            int conditionId, 
            string symptoms)
        {
            var condition = await _unitOfWork.HealthConditions.GetByIdAsync(conditionId);
            var userSymptomList = symptoms.ToLower().Split(new[] { ',', ';', '.' }, 
                StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToList();

            var conditionSymptoms = await _unitOfWork.Symptoms.FindAsync(
                s => s.HealthConditionId == conditionId);

            var matchedSymptoms = conditionSymptoms
                .Where(s => userSymptomList.Any(us => s.SymptomName.ToLower().Contains(us) || us.Contains(s.SymptomName.ToLower())))
                .ToList();

            var analysis = new StringBuilder();
            analysis.AppendLine($"\n📋 SYMPTOM ANALYSIS - {condition.Name}");
            analysis.AppendLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
            analysis.AppendLine();

            if (matchedSymptoms.Any())
            {
                analysis.AppendLine($"✓ Matched {matchedSymptoms.Count} symptoms related to {condition.Name}:");
                analysis.AppendLine();
                
                var totalSeverity = 0;
                foreach (var symptom in matchedSymptoms)
                {
                    analysis.AppendLine($"• {symptom.SymptomName}");
                    analysis.AppendLine($"  Severity Level: {symptom.SeverityLevel}/10");
                    analysis.AppendLine($"  Description: {symptom.Description}");
                    analysis.AppendLine();
                    totalSeverity += symptom.SeverityLevel;
                }

                var averageSeverity = totalSeverity / matchedSymptoms.Count;
                analysis.AppendLine($"📊 Average Severity Score: {averageSeverity}/10");

                if (averageSeverity >= 8)
                {
                    analysis.AppendLine("🚨 RECOMMENDATION: Please consult a healthcare professional urgently.");
                }
                else if (averageSeverity >= 5)
                {
                    analysis.AppendLine("⚠️  RECOMMENDATION: Consider scheduling a consultation with your healthcare provider.");
                }
                else
                {
                    analysis.AppendLine("✓ RECOMMENDATION: Monitor symptoms and follow recommended lifestyle changes.");
                }
            }
            else
            {
                analysis.AppendLine($"⚠️  No direct match found with typical {condition.Name} symptoms.");
                analysis.AppendLine($"However, we recommend consulting a healthcare professional for proper diagnosis.");
            }

            analysis.AppendLine();
            analysis.AppendLine("⚕️  DISCLAIMER: This analysis is for informational purposes only.");
            analysis.AppendLine("Professional medical evaluation is essential for accurate diagnosis and treatment.");

            return analysis.ToString();
        }

        public async Task<string> GenerateProgressReportAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            var progresses = await _unitOfWork.UserProgresses.FindAsync(p => p.UserId == userId);
            var recentProgresses = progresses.OrderByDescending(p => p.ProgressDate).Take(10).ToList();

            var report = new StringBuilder();
            report.AppendLine($"\n📈 PROGRESS REPORT - {user.FullName}");
            report.AppendLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
            report.AppendLine($"Report Generated: {DateTime.UtcNow:MMMM dd, yyyy}");
            report.AppendLine();

            if (recentProgresses.Any())
            {
                var averageScore = recentProgresses.Average(p => p.ProgressScore);
                var averageExercises = (int)recentProgresses.Average(p => p.ExercisesCompleted);
                var averageDiet = (int)recentProgresses.Average(p => p.DietComplianceScore);

                report.AppendLine("📊 OVERALL STATISTICS:");
                report.AppendLine("─────────────────────────────────────────────────");
                report.AppendLine($"Average Health Score: {averageScore:F1}/10");
                report.AppendLine($"Avg Exercises/Day: {averageExercises}");
                report.AppendLine($"Avg Diet Compliance: {averageDiet}%");
                report.AppendLine();

                report.AppendLine("📅 RECENT PROGRESS ENTRIES:");
                report.AppendLine("─────────────────────────────────────────────────────────");
                foreach (var progress in recentProgresses)
                {
                    report.AppendLine($"Date: {progress.ProgressDate:MMMM dd, yyyy}");
                    report.AppendLine($"  Health Score: {progress.ProgressScore}/10");
                    report.AppendLine($"  Exercises: {progress.ExercisesCompleted}");
                    report.AppendLine($"  Diet Compliance: {progress.DietComplianceScore}%");
                    report.AppendLine($"  Notes: {progress.SymptomImprovement}");
                    report.AppendLine();
                }

                // Trend analysis
                report.AppendLine("📈 TREND ANALYSIS:");
                report.AppendLine("──────────────────────────────────────────────");
                if (recentProgresses.Count > 1)
                {
                    var firstScore = recentProgresses.Last().ProgressScore;
                    var lastScore = recentProgresses.First().ProgressScore;
                    var improvement = lastScore - firstScore;

                    if (improvement > 0)
                    {
                        report.AppendLine($"✓ Positive Trend: +{improvement} point(s) improvement!");
                    }
                    else if (improvement < 0)
                    {
                        report.AppendLine($"⚠️  Declining Trend: {improvement} point(s) decrease. Consider reviewing your routine.");
                    }
                    else
                    {
                        report.AppendLine("→ Stable Progress: Keep up with your current routine.");
                    }
                }
            }
            else
            {
                report.AppendLine("No progress records found. Start tracking your wellness journey!");
            }

            return report.ToString();
        }

        public async Task<string> GenerateExercisePlanAsync(int userId, int conditionId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            var userProfile = await _unitOfWork.UserProfiles.SingleOrDefaultAsync(p => p.UserId == userId);
            var exercises = await _unitOfWork.Exercises.FindAsync(e => e.HealthConditionId == conditionId);

            var plan = new StringBuilder();
            plan.AppendLine($"\n💪 PERSONALIZED EXERCISE PLAN");
            plan.AppendLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
            plan.AppendLine($"Fitness Level: {userProfile?.FitnessLevel ?? "Beginner"}");
            plan.AppendLine();

            if (userProfile?.PregnancyStatus == "Pregnant")
            {
                exercises = exercises.Where(e => e.IsSafeInPregnancy).ToList();
                plan.AppendLine("🤰 Pregnancy-Safe Exercises Selected");
                plan.AppendLine();
            }

            var exercisesByLevel = exercises.GroupBy(e => e.IntensityLevel);
            foreach (var levelGroup in exercisesByLevel.OrderBy(g => g.Key))
            {
                plan.AppendLine($"Level {levelGroup.Key} - " + (levelGroup.Key switch
                {
                    1 => "Gentle",
                    2 => "Easy",
                    3 => "Moderate",
                    4 => "Challenging",
                    5 => "Advanced",
                    _ => "Unknown"
                }));
                plan.AppendLine("──────────────────────────────────────────────────────────────────────");

                foreach (var exercise in levelGroup)
                {
                    plan.AppendLine($"• {exercise.ExerciseName} - {exercise.DurationMinutes} minutes");
                    plan.AppendLine($"  Description: {exercise.Description}");
                    if (!string.IsNullOrEmpty(exercise.VideoUrl))
                    {
                        plan.AppendLine($"  Video: {exercise.VideoUrl}");
                    }
                }
                plan.AppendLine();
            }

            return plan.ToString();
        }

        public async Task<string> GenerateDietPlanAsync(int userId, int conditionId)
        {
            var dietPlans = await _unitOfWork.DietPlans.FindAsync(d => d.HealthConditionId == conditionId);
            var dietPlan = dietPlans.FirstOrDefault();

            if (dietPlan == null)
            {
                return "No diet plan available for this condition."; 
            }

            var plan = new StringBuilder();
            plan.AppendLine($"\n🥗 {dietPlan.PlanName}");
            plan.AppendLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
            plan.AppendLine(dietPlan.Description);
            plan.AppendLine();
            plan.AppendLine($"Daily Calorie Target: {dietPlan.CaloriesPerDay} calories");
            plan.AppendLine();
            plan.AppendLine("✓ FOODS TO INCLUDE:");
            plan.AppendLine("──────────────────────────────────────────────────────────────────────");
            plan.AppendLine(dietPlan.FoodsToInclude);
            plan.AppendLine();
            plan.AppendLine("✗ FOODS TO AVOID:");
            plan.AppendLine("──────────────────────────────────────────────────────────────────────");
            plan.AppendLine(dietPlan.FoodsToAvoid);
            plan.AppendLine();
            
            if (!string.IsNullOrEmpty(dietPlan.MealPlan))
            {
                plan.AppendLine("🍽️  SAMPLE MEAL PLAN:");
                plan.AppendLine("──────────────────────────────────────────────────────────────────────");
                plan.AppendLine(dietPlan.MealPlan);
            }

            return plan.ToString();
        }
    }
}
