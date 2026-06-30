using MediatR;

namespace YogaSakhi.Application.CQRS.Commands.Auth
{
    public class LoginUserCommand : IRequest<LoginUserResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}
