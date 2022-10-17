namespace Blog.Application.Common.Exceptions;

public class NotRightsException : Exception
{
    public NotRightsException(object id) : base($"User - {id} has not rights for do this action.") { }
}
