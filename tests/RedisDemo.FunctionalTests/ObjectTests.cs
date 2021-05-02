using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace RedisDemo.FunctionalTests
{
	[TestClass]
	public class ObjectTests
	{
		private class NestedObject
		{
			public decimal DecimalProperty { get; init; }
		}

		private class SomeObject
		{
			public string StringProperty { get; init; }

			public int NumericProperty { get; init; }

			public NestedObject NestedObject { get; init; }
		}

		[TestMethod]
		public async Task GetObject_ForExistingObject_ReturnsCorrectValue()
		{
			// Arrange

			var redisConfiguration = new RedisConfiguration
			{
				KeyPrefix = "redis.demo:functional.tests:",
				Hosts = new[]
				{
					new RedisHost { Host = "localhost", Port = 6379 },
				},
			};

			var services = new ServiceCollection();
			services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);

			await using var serviceProvider = services.BuildServiceProvider();
			var cacheClient = serviceProvider.GetRequiredService<IRedisCacheClient>();

			var originalObject = new SomeObject
			{
				StringProperty = "Some String",
				NumericProperty = 12345,
				NestedObject = new NestedObject
				{
					DecimalProperty = 123.456789M,
				},
			};

			// Act

			var wasSet = await cacheClient.Db0.AddAsync("object.tests:some.object", originalObject);
			Assert.IsTrue(wasSet);

			var objectFromCache = await cacheClient.Db0.GetAsync<SomeObject>("object.tests:some.object");

			// Assert

			objectFromCache.Should().NotBeSameAs(originalObject);
			objectFromCache.Should().BeEquivalentTo(originalObject);
		}
	}
}
