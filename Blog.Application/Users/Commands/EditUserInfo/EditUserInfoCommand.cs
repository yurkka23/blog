using System;
using MediatR;

namespace Blog.Application.Users.Commands.EditUserInfo
{
    public class EditUserInfoCommand : IRequest
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AboutMe { get; set; }
    }
}
