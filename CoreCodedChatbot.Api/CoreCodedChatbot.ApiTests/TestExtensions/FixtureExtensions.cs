using AutoFixture;

namespace CoreCodedChatbot.ApiTests.TestExtensions
{
    public static class FixtureExtensions
    {
        public static T Get<T>(this IFixture fixture)
        {
            return fixture.Build<T>().Create();
        }
    }
}