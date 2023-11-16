using System.Text.RegularExpressions;

namespace CoreCodedChatbot.ApiApplication.Extensions;

public static class SolrQueryExtensions
{
    private static Regex _specialCharSearchRegex = new Regex("([-+!()\"~*?:;&|])");

    public static string ReplaceSpecialChars(this string searchTerm)
    {
        return _specialCharSearchRegex.Replace(searchTerm, "\\$1");
    }
}