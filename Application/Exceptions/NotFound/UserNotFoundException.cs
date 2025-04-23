namespace Application.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string email) 
        : base("Пользователь не найден.")
    {
    }

    public UserNotFoundException(string message, string email) 
        : base(message)
    {
    }
    
    public UserNotFoundException(string message, Guid userId) 
        : base(message)
    {
    }

    public UserNotFoundException(string message, Exception innerException, string email) 
        : base(message, innerException)
    {
    }

    public Guid? UserId { get; set; }
    public string? Email { get; set; }

    public UserNotFoundException WithUserId(Guid userId)
    {
        UserId = userId;
        return this;
    }

    public UserNotFoundException WithEmail(string email)
    {
        Email = email;
        return this;
    }
}