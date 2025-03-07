namespace Resulver.MediatR;

public interface IResultBaseRequestHandler<in TRequest, TResultContent> :
    IRequestHandler<TRequest, Result<TResultContent>>
    where TRequest : IResultBaseRequest<TResultContent>;

public interface IResultBaseRequestHandler<in TRequest> :
    IRequestHandler<TRequest, Result>
    where TRequest : IResultBaseRequest;