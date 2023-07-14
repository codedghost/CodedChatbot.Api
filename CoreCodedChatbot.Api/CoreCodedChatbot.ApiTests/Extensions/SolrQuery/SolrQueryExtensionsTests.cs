using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Extensions;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Extensions.SolrQuery
{
    [TestFixture]
    public class SolrQueryExtensionsTests
    {
        [TestCase("abcd", "abcd")]
        [TestCase("Test: Song", @"Test\: Song")]
        [TestCase(":;()-+", @"\:\;\(\)\-\+")]
        public void ReplaceSpecialCharsTest(string searchTerm, string expectedResult)
        {
            var result = searchTerm.ReplaceSpecialChars();
            Assert.AreEqual(expectedResult, result);
        }

    }
}