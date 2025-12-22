using System.Text;

#if NETFRAMEWORK
namespace System.Net.Http;

/// <summary>
///     MIME 형식 application/x-www-form-urlencoded를 사용하여 인코딩된 이름/값 튜플의 컨테이너입니다.
/// </summary>
public class FormUrlEncodedContent : ByteArrayContent
{
    /// <summary>
    ///     <see cref="FormUrlEncodedContent" /> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="nameValueCollection">이름/값 쌍의 컬렉션입니다.</param>
    public FormUrlEncodedContent(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        : base(GetContentByteArray(nameValueCollection))
    {
        Headers.ContentType = "application/x-www-form-urlencoded";
    }

    private static byte[] GetContentByteArray(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
    {
        if (nameValueCollection == null)
        {
            throw new ArgumentNullException(nameof(nameValueCollection));
        }

        var sb = new StringBuilder();
        bool first = true;

        foreach (KeyValuePair<string, string> pair in nameValueCollection)
        {
            if (!first)
            {
                sb.Append('&');
            }

            sb.Append(Encode(pair.Key));
            sb.Append('=');
            sb.Append(Encode(pair.Value));

            first = false;
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    private static string Encode(string? data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return string.Empty;
        }

        return Uri.EscapeDataString(data)
            .Replace("%20", "+");
    }
}
#endif
