using System;

namespace CoreCodedChatbot.ApiApplication.Extensions;

public static class ObjectAssertions
{
    public static void AssertNonNull(this object obj)
    {
        switch (obj)
        {
            case string stringObj when string.IsNullOrWhiteSpace(stringObj):
                throw new ArgumentNullException(nameof(obj));
            case null:
                throw new ArgumentNullException(nameof(obj));
        }
    }
}