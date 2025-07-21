using MediatR;

namespace GradTest.Application.Common.Contracts;

public interface ICommand<out TResponse> : IRequest<TResponse>;