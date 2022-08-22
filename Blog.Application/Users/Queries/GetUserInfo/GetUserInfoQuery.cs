using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Blog.Application.Users.Queries.GetUserInfo
{
    public class GetUserInfoQuery : IRequest<UserInfoVm>
    {
        public Guid Id { get; set; }
    }
}
