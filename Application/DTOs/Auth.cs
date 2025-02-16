namespace Application.DTOs;

public class AuthRequestDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class AuthResponseDTO
{
    public string Token { get; set; }
    public UserDTO User { get; set; }
}

public class RegisterRequestDTO
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserDTO
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}