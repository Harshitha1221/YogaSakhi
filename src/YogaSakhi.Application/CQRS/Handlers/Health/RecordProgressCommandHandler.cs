using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using YogaSakhi.Application.CQRS.Commands.Health;
using YogaSakhi.Domain.Entities;
using YogaSakhi.Domain.Interfaces;

namespace YogaSakhi.Application.CQRS.Handlers.Health
{
    public class RecordProgressCommandHandler : IRequestHandler<RecordProgressCommand, RecordProgressResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecordProgressCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RecordProgressResponse> Handle(RecordProgressCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var progress = new UserProgress
                {
                    UserId = request.UserId,
                    HealthConditionId = request.HealthConditionId,
                    ProgressDate = DateTime.UtcNow,
                    ProgressScore = request.ProgressScore,
                    Notes = request.Notes,
                    SymptomImprovement = request.SymptomImprovement,
                    ExercisesCompleted = request.ExercisesCompleted,
                    DietComplianceScore = request.DietComplianceScore
                };

                await _unitOfWork.UserProgresses.AddAsync(progress);
                await _unitOfWork.SaveChangesAsync();

                return new RecordProgressResponse
                {
                    Success = true,
                    Message = "Progress recorded successfully",
                    ProgressId = progress.Id
                };
            }
            catch (Exception ex)
            {
                return new RecordProgressResponse
                {
                    Success = false,
                    Message = $"Progress recording failed: {ex.Message}"
                };
            }
        }
    }
}
