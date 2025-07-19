
using GradTest.Application.Common.Contracts;
using GradTest.Application.Common.Services;
using MediatR;
using GradTest.Shared.Tracing;

namespace GradTest.Application.Common.Pipelines;

public class UnitOfWorkPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : IResult
    where TRequest : ICommand<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;


    public UnitOfWorkPipelineBehavior(
        IUnitOfWork unitOfWork, 
        IEventPublisher eventPublisher)
    {
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using var activity = ActivityProvider.UnitOfWork();

        var handlerResult = await next();
        
        if (handlerResult.IsError)
        {
            return handlerResult;
        }
        
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var domainEvents = _unitOfWork.GetAndClearDomainEvents();
        await _eventPublisher.PublishEventsAsync(domainEvents);

        var saveChangesResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (saveChangesResult.IsError)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return (TResponse)(object) Result.Error(saveChangesResult.ErrorValue);
        }
        
        await _unitOfWork.CommitTransactionAsync(cancellationToken);
        return handlerResult;
    }
}