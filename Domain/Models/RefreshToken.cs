using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class RefreshToken
{
    [Key] public Guid Id { get; init; } = Guid.NewGuid();

    [Required] public Guid UserId { get; init; }

    [Required]
    [MaxLength(512)] 
    public string Token { get; set; }

    public DateTime ExpiresAt { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [ForeignKey("UserId")]
    public User User { get; set; }
}