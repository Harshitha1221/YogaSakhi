using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using YogaSakhi.Application.CQRS.Commands.Auth;
using YogaSakhi.Domain.Entities;
using YogaSakhi.Domain.Interfaces;
using BCrypt.Net;

namespace YogaSakhi.Application.CQRS.Handlers.Auth
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
                if (existingUser != null)
                {
                    return new RegisterUserResponse
                    {
                        Success = false,
                        Message = "User with this email already exists"
                    };
                }

                // Register user
                var (success, token, message) = await _authService.RegisterAsync(
                    request.Email,
                    request.Password,
                    request.FullName,
                    request.Age);

                if (!success)
                {
                    return new RegisterUserResponse
                    {
                        Success = false,
                        Message = message
                    };
                }

                // Get the newly created user
                var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email == request.Email);

                return new RegisterUserResponse
                {
                    Success = true,
                    Message = "User registered successfully",
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
                return new RegisterUserResponse
                {
                    Success = false,
                    Message = $"Registration failed: {ex.Message}"
                };
            }
        }
    }
}
