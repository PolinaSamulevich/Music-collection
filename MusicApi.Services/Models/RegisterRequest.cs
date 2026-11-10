using System.ComponentModel.DataAnnotations;
namespace MusicApi.Services.Models
{
    public record class RegisterRequest(
        [Required, EmailAddress] string Email,
        [Required, MinLength(6)] string Password);
}
