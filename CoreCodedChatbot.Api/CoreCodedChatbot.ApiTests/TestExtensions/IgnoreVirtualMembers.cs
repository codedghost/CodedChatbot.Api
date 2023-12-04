using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace CoreCodedChatbot.ApiTests.TestExtensions;

public class IgnoreVirtualMembers : ISpecimenBuilder
{
    public Type ReflectedType { get; }

    public object Create(object request, ISpecimenContext context)
    {
        var pi = request as PropertyInfo;
        if (pi != null)
        {
            if (this.ReflectedType == null ||
                this.ReflectedType == pi.ReflectedType)
            {
                if (pi.GetGetMethod().IsVirtual)
                {
                    return new OmitSpecimen();
                }
            }
        }

        return new NoSpecimen();
    }
}