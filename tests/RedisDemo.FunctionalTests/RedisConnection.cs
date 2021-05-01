using System;
using System.Threading;
using StackExchange.Redis;

namespace RedisDemo.FunctionalTests
{
	public static class RedisConnection
	{
		private static readonly Lazy<ConnectionMultiplexer> LazyConnection = new(GetConnection, LazyThreadSafetyMode.ExecutionAndPublication);

		public static ConnectionMultiplexer Redis => LazyConnection.Value;

		private static ConnectionMultiplexer GetConnection()
		{
			return ConnectionMultiplexer.Connect("localhost");
		}
	}
}
