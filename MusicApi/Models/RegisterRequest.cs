using System.ComponentModel.DataAnnotations;
namespace MusicApi.Models
{
    record class RegisterRequest(
        [Required, EmailAddress] string Email,
        [Required, MinLength(6)] string Password);
}
