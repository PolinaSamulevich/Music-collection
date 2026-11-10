using System.ComponentModel.DataAnnotations;
namespace MusicApi.Services.Models
{
    public record class LoginRequest(
        [Required, EmailAddress] string Email,
        [Required] string Password);
}
