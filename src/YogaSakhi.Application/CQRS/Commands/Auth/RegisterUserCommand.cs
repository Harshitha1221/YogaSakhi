using MediatR;

namespace YogaSakhi.Application.CQRS.Commands.Auth
{
    public class RegisterUserCommand : IRequest<RegisterUserResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class RegisterUserResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public UserDto User { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
