using System;
using System.Threading;
using System.Threading.Tasks;
using CodeFuller.Library.Bootstrap;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RedisDemo
{
	internal class ApplicationLogic : IApplicationLogic
	{
		private readonly ILogger<ApplicationLogic> logger;

		private readonly ApplicationSettings settings;

		public ApplicationLogic(ILogger<ApplicationLogic> logger, IOptions<ApplicationSettings> options)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
		}

		public Task<int> Run(string[] args, CancellationToken cancellationToken)
		{
			try
			{
				RunInternal();

				return Task.FromResult(0);
			}
#pragma warning disable CA1031 // Do not catch general exception types
			catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
			{
				logger.LogCritical(e, "Application has failed");
				return Task.FromResult(e.HResult);
			}
		}

		private static void RunInternal()
		{
		}
	}
}
