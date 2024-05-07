
namespace LifeStyle.Domain.Exception
{
    public class AlreadyExistsException : DataValidationException
    {
        public AlreadyExistsException(string message) : base(message)
        {
        }
        public AlreadyExistsException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
