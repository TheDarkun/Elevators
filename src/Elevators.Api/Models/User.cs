using System.ComponentModel.DataAnnotations;

namespace Elevators.Api.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public required long UserId { get; set; }
    public required Guid SessionId { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public required int ExpiresIn { get; set; }
    public required DateTime IssuedAt { get; set; }
}