using Blog.Application.Common.Mappings;
using Blog.Application.Users.Commands.EditUserInfo;

namespace Blog.WebApi.DTOs.UserDTOs
{
    public class EditUserInfoDTO : IMapWith<EditUserInfoCommand>
    {
        //public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AboutMe { get; set; }
    }
}
