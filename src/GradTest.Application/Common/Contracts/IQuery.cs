using MediatR;

namespace GradTest.Application.Common.Contracts;

public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse: IResult;
