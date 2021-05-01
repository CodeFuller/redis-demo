using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static RedisDemo.FunctionalTests.RedisConnection;

namespace RedisDemo.FunctionalTests
{
	[TestClass]
	public class StringTests
	{
		[TestMethod]
		public async Task StringGetAsync_ForMissingKey_ReturnsNull()
		{
			// Arrange

			var database = Redis.GetDatabase();

			// Act

			var value = await database.StringGetAsync("RedisDemo.FunctionalTests.StringTests.Missing Key");

			// Assert

			value.IsNull.Should().BeTrue();
		}

		[TestMethod]
		public async Task StringGetAsync_ForExistingKey_ReturnsCorrectValue()
		{
			// Arrange

			var database = Redis.GetDatabase();

			var wasSet = await database.StringSetAsync("RedisDemo.FunctionalTests.StringTests.Existing Key", "Some Value");
			Assert.IsTrue(wasSet);

			// Act

			string value = await database.StringGetAsync("RedisDemo.FunctionalTests.StringTests.Existing Key");

			// Assert

			value.Should().Be("Some Value");
		}
	}
}
