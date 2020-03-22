using System;
using System.Net;
using System.Text.RegularExpressions;

namespace CoreCodedChatbot.ApiApplication.Extensions
{
    public static class WebRequestExtensions
    {
        static Regex rxCookieParts = new Regex(@"(?<name>.*?)\=(?<value>.*?)\;|(?<name>\bsecure\b|\bhttponly\b)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        static Regex rxRemoveCommaFromDate = new Regex(@"\bexpires\b\=.*?(\;|$)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);

        public static bool GetHttpCookiesFromHeader(this string cookieHeader, out CookieCollection cookies)
        {
            cookies = new CookieCollection();

            try
            {

                string rawcookieString = rxRemoveCommaFromDate.Replace(cookieHeader, new MatchEvaluator(RemoveComma));

                string[] rawCookies = rawcookieString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (rawCookies.Length == 0)
                {
                    cookies.Add(rawcookieString.ToCookie());
                }
                else
                {
                    foreach (var rawCookie in rawCookies)
                    {
                        cookies.Add(rawCookie.ToCookie());
                    }
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Cookie ToCookie(this string rawCookie)
        {

            if (!rawCookie.EndsWith(";")) rawCookie += ";";

            MatchCollection maches = rxCookieParts.Matches(rawCookie);

            Cookie cookie = new Cookie(maches[0].Groups["name"].Value.Trim(), maches[0].Groups["value"].Value.Trim());

            for (int i = 1; i < maches.Count; i++)
            {
                switch (maches[i].Groups["name"].Value.ToLower().Trim())
                {
                    case "domain":
                        cookie.Domain = maches[i].Groups["value"].Value;
                        break;
                    case "expires":

                        DateTime dt;

                        if (DateTime.TryParse(maches[i].Groups["value"].Value, out dt))
                        {
                            cookie.Expires = dt;
                        }
                        else
                        {
                            cookie.Expires = DateTime.Now.AddDays(2);
                        }
                        break;
                    case "path":
                        cookie.Path = maches[i].Groups["value"].Value;
                        break;
                    case "secure":
                        cookie.Secure = true;
                        break;
                    case "httponly":
                        cookie.HttpOnly = true;
                        break;
                }
            }
            return cookie;
        }

        private static string RemoveComma(Match match)
        {
            return match.Value.Replace(',', ' ');
        }
    }
}