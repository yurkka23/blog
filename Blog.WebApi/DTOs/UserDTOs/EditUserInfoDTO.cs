using Blog.Application.Common.Mappings;
using Blog.Application.Users.Commands.EditUserInfo;
using System.ComponentModel.DataAnnotations;

namespace Blog.WebApi.DTOs.UserDTOs
{
    public class EditUserInfoDTO : IMapWith<EditUserInfoCommand>
    {
        //public Guid Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string AboutMe { get; set; }
    }
}
