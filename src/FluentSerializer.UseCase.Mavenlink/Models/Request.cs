namespace FluentSerializer.UseCase.Mavenlink.Models
{
    internal class Request<TRequest>
    {
        public TRequest Data { get; init; } = default!;
    }
}
