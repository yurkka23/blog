using System;
using MediatR;

namespace Blog.Application.Users.Commands.EditUserInfo
{
    public class EditUserInfoCommand : IRequest
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string AboutMe { get; set; } = null!;
    }
}
