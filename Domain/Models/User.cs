using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class User
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [Required]
    [EmailAddress(ErrorMessage = "invalid Email")]
    [MaxLength(254)]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(256)]
    public string PasswordHash { get; set; }
    
    [Required]
    public int RoleId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [MaxLength(50)]
    public string? SecondName { get; set; }
    
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public RefreshToken Token { get; init; }
    
    [ForeignKey("RoleId")]
    public Role Role { get; init; }
    
    public ICollection<Task> Tasks { get; init; } = new List<Task>();
}