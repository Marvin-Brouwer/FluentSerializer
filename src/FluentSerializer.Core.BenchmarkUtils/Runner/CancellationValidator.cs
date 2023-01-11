using BenchmarkDotNet.Validators;

using System.Collections.Generic;
using System.Threading;

namespace FluentSerializer.Core.BenchmarkUtils.Runner;

/// <summary>
/// This class is purely meant to invalidate new runs when --quick-exit mode is enabled.
/// This prevents GitHub Pipelines from running longer than necessary.
/// </summary>
public sealed class CancellationValidator : IValidator
{
	private readonly CancellationTokenSource _cancellationTokenSource;
	private readonly CancellationToken _cancellationToken;

	public bool TreatsWarningsAsErrors => true;

	public static readonly CancellationValidator Default = new(new CancellationTokenSource());
	private CancellationValidator(CancellationTokenSource cancellationTokenSource)
	{
		_cancellationToken = cancellationTokenSource.Token;
		_cancellationTokenSource = cancellationTokenSource;
	}

	public void RequestCancellation() => _cancellationTokenSource.Cancel();

	public IEnumerable<ValidationError> Validate(ValidationParameters validationParameters)
	{
		_ = validationParameters;

		if (_cancellationToken.IsCancellationRequested)
		{
			yield return new ValidationError(TreatsWarningsAsErrors, "Cancellation requested");
		}
	}
}
