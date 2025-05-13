using System.Text.RegularExpressions;

public static class HtmlHelper
{
    public static string RemoveHtmlComments(string html)
    {
        if (string.IsNullOrEmpty(html))
            return html;

        return Regex.Replace(html, @"<!--(.*?)-->", string.Empty, RegexOptions.Singleline);
    }
}