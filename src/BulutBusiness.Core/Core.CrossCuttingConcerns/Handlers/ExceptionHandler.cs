using BulutBusiness.Core.Core.CrossCuttingConcerns.Types;

namespace BulutBusiness.Core.Core.CrossCuttingConcerns.Handlers;

public abstract class ExceptionHandler
{
    public abstract Task HandleException(BusinessException businessException);
    public abstract Task HandleException(ValidationException validationException);
    public abstract Task HandleException(AuthorizationException authorizationException);
    public abstract Task HandleException(NotFoundException notFoundException);
    public abstract Task HandleException(System.Exception exception);
}
