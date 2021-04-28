using System;
using System.Threading;
using System.Threading.Tasks;
using CodeFuller.Library.Bootstrap;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

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

		public async Task<int> Run(string[] args, CancellationToken cancellationToken)
		{
			try
			{
				await RunInternal();

				return 0;
			}
#pragma warning disable CA1031 // Do not catch general exception types
			catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
			{
				logger.LogCritical(e, "Application has failed");
				return e.HResult;
			}
		}

		private async Task RunInternal()
		{
			logger.LogInformation("Connecting to Redis ...");
			using var redis = await ConnectionMultiplexer.ConnectAsync("localhost");
			logger.LogInformation("Connected successfully");

			var database = redis.GetDatabase();

			const string stringKey = "TestStringKey";

			var value = await database.StringGetAsync(stringKey);
			logger.LogInformation("String value before set: {StringValue}", value);

			await database.StringSetAsync(stringKey, "Some Value");

			value = await database.StringGetAsync(stringKey);
			logger.LogInformation("String value after set: {StringValue}", value);
		}
	}
}
