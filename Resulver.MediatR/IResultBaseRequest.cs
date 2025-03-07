namespace Resulver.MediatR;

public interface IResultBaseRequest : IRequest<Result>;
public interface IResultBaseRequest<TResultContent> : IRequest<Result<TResultContent>>;