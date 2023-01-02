namespace FluentSerializer.UseCase.Mavenlink.Models;

internal sealed class Request<TRequest>
{
	public TRequest Data { get; init; } = default!;
}