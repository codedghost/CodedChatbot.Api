using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.Settings;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Settings;
using CoreCodedChatbot.ApiApplication.Services;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Services;

[TestFixture]
public class SettingsServiceTests
{
    private Mock<IUpdateSettingsCommand> _updateSettingsCommand;

    private SettingsService _subject;

    [SetUp]
    public void Setup()
    {
        _updateSettingsCommand = new Mock<IUpdateSettingsCommand>();

        _subject = new SettingsService(_updateSettingsCommand.Object);
    }

    [Test, AutoData]
    public void SuccessWhen_SettingsCommandCalledWithCorrectValues(string key, string value)
    {
        _subject.Update(key, value);

        _updateSettingsCommand.Verify(c => c.Update(key, value), Times.Once);
    }
}