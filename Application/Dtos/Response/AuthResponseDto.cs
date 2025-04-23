namespace Application.Dtos.Response;

public record AuthResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
};