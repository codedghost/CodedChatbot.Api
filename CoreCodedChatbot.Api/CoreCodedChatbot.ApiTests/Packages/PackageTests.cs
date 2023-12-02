using System;
using System.Linq;
using System.Reflection;
using CoreCodedChatbot.ApiApplication;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Packages;

[TestFixture]
public class PackageTests
{
    [Test]
    public void SuccessWhen_AllExistingServicesAreRegistered()
    {
        var newServiceCollection = new ServiceCollection();

        var baseServiceType = typeof(IBaseService);

        Assert.That(newServiceCollection.Count == 0);

        var services = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(t => t.IsAssignableTo(baseServiceType) && t != baseServiceType);

        newServiceCollection.AddApiServices();

        Assert.AreEqual(newServiceCollection.Count, services.Count());
    }
}