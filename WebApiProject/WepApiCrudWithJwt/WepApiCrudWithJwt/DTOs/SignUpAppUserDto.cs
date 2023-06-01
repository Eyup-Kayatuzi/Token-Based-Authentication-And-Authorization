using System.ComponentModel.DataAnnotations;

namespace WepApiCrudWithJwt.DTOs
{
    public class SignUpAppUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Bu bilginin girilmesi zorunludur.")]
        public string Username { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Bu bilginin girilmesi zorunludur.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bu bilginin girilmesi zorunludur.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
