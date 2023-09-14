namespace DiskodePro.WebApp.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string message)
        : base(message)
    {
    }
}

public class CommentNotFoundException : Exception
{
    public CommentNotFoundException(string message)
        : base(message)
    {
    }
}

public class PostNotFoundException : Exception
{
    public PostNotFoundException(string message)
        : base(message)
    {
    }
}