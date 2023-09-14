namespace DiskodePro.WebApp.Exceptions;

public class RepositoryException : Exception
{
    public RepositoryException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

public class DuplicateEmailException : Exception
{
    public DuplicateEmailException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}