using System.ComponentModel.DataAnnotations;

namespace WepApiCrudWithJwt.DTOs
{
    public class SignInAppUserDto
    {
        [Required(ErrorMessage = "Bu bilginin girilmesi zorunludur.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Bu bilginin girilmesi zorunludur.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
