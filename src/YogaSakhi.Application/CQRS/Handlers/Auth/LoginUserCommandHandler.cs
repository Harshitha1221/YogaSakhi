using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using YogaSakhi.Application.CQRS.Commands.Auth;
using YogaSakhi.Domain.Interfaces;

namespace YogaSakhi.Application.CQRS.Handlers.Auth
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public LoginUserCommandHandler(IAuthService authService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var (success, token, message) = await _authService.LoginAsync(request.Email, request.Password);

                if (!success)
                {
                    return new LoginUserResponse
                    {
                        Success = false,
                        Message = message
                    };
                }

                var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
                user.LastLoginDate = DateTime.UtcNow;
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync();

                return new LoginUserResponse
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    User = new UserDto
                    {
                        Id = user.Id,
                        UserId = user.UserId,
                        FullName = user.FullName,
                        Email = user.Email,
                        Role = user.Role.ToString()
                    }
                };
            }
            catch (Exception ex)
            {
                return new LoginUserResponse
                {
                    Success = false,
                    Message = $"Login failed: {ex.Message}"
                };
            }
        }
    }
}
