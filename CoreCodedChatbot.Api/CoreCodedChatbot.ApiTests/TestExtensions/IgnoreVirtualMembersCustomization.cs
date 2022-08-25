using AutoFixture;

namespace CoreCodedChatbot.ApiTests.TestExtensions
{
    public class IgnoreVirtualMembersCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoreVirtualMembers());
        }
    }
}