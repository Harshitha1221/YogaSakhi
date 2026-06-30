using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using YogaSakhi.Application.CQRS.Commands.Health;
using YogaSakhi.Domain.Entities;
using YogaSakhi.Domain.Interfaces;
using YogaSakhi.Application.Services;

namespace YogaSakhi.Application.CQRS.Handlers.Health
{
    public class CreateAssessmentCommandHandler : IRequestHandler<CreateAssessmentCommand, CreateAssessmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAIService _aiService;

        public CreateAssessmentCommandHandler(IUnitOfWork unitOfWork, IAIService aiService)
        {
            _unitOfWork = unitOfWork;
            _aiService = aiService;
        }

        public async Task<CreateAssessmentResponse> Handle(CreateAssessmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Verify user exists
                var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    return new CreateAssessmentResponse
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                // Generate AI recommendation
                var aiRecommendation = await _aiService.GeneratePersonalizedRecommendationAsync(
                    request.UserId,
                    request.HealthConditionId,
                    request.Symptoms);

                // Create assessment
                var assessment = new UserAssessment
                {
                    UserId = request.UserId,
                    HealthConditionId = request.HealthConditionId,
                    AssessmentScore = request.AssessmentScore,
                    AssessmentDetails = request.AssessmentDetails,
                    AssessmentDate = DateTime.UtcNow,
                    AIRecommendation = aiRecommendation,
                    NeedsDoctorReview = request.AssessmentScore >= 8
                };

                await _unitOfWork.UserAssessments.AddAsync(assessment);
                await _unitOfWork.SaveChangesAsync();

                return new CreateAssessmentResponse
                {
                    Success = true,
                    Message = "Assessment created successfully",
                    AssessmentId = assessment.Id,
                    AIRecommendation = aiRecommendation
                };
            }
            catch (Exception ex)
            {
                return new CreateAssessmentResponse
                {
                    Success = false,
                    Message = $"Assessment creation failed: {ex.Message}"
                };
            }
        }
    }
}
