using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;

namespace RedisDemo.FunctionalTests
{
	// These tests use low-level StackExchange.Redis library.
	// https://stackexchange.github.io/StackExchange.Redis/
	[TestClass]
	public class StringTests
	{
		private static readonly ConnectionMultiplexer RedisConnection = ConnectionMultiplexer.Connect("localhost");

		[TestMethod]
		public async Task StringGetAsync_ForMissingKey_ReturnsNull()
		{
			// Arrange

			var database = RedisConnection.GetDatabase();

			// Act

			var value = await database.StringGetAsync("redis.demo:functional.tests:string.tests:missing.key");

			// Assert

			value.IsNull.Should().BeTrue();
		}

		[TestMethod]
		public async Task StringGetAsync_ForExistingKey_ReturnsCorrectValue()
		{
			// Arrange

			var database = RedisConnection.GetDatabase();

			var wasSet = await database.StringSetAsync("redis.demo:functional.tests:string.tests:existing.key", "Some Value");
			Assert.IsTrue(wasSet);

			// Act

			string value = await database.StringGetAsync("redis.demo:functional.tests:string.tests:existing.key");

			// Assert

			value.Should().Be("Some Value");
		}
	}
}
