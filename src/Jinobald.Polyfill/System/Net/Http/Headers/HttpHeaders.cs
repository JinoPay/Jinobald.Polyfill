using System.Collections;
using System.Text;

#if NETFRAMEWORK
namespace System.Net.Http.Headers;

/// <summary>
///     HTTP 헤더 이름 및 값의 컬렉션입니다.
/// </summary>
public abstract class HttpHeaders : IEnumerable<KeyValuePair<string, IEnumerable<string>>>
{
    private readonly Dictionary<string, List<string>> _headers;
    private readonly StringComparer _comparer;

    /// <summary>
    ///     <see cref="HttpHeaders" /> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    protected HttpHeaders()
    {
        _comparer = StringComparer.OrdinalIgnoreCase;
        _headers = new Dictionary<string, List<string>>(_comparer);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    ///     컬렉션을 반복하는 열거자를 반환합니다.
    /// </summary>
    public IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumerator()
    {
        foreach (KeyValuePair<string, List<string>> kvp in _headers)
        {
            yield return new KeyValuePair<string, IEnumerable<string>>(kvp.Key, kvp.Value);
        }
    }

    /// <summary>
    ///     지정된 헤더가 <see cref="HttpHeaders" /> 컬렉션에 있는지 여부를 반환합니다.
    /// </summary>
    /// <param name="name">헤더입니다.</param>
    /// <returns>지정된 헤더가 컬렉션에 있으면 <c>true</c>이고, 그렇지 않으면 <c>false</c>입니다.</returns>
    public bool Contains(string name)
    {
        return _headers.ContainsKey(name);
    }

    /// <summary>
    ///     <see cref="HttpHeaders" /> 컬렉션에서 지정된 헤더를 제거합니다.
    /// </summary>
    /// <param name="name">제거할 헤더의 이름입니다.</param>
    /// <returns>헤더가 성공적으로 제거되었으면 <c>true</c>이고, 그렇지 않으면 <c>false</c>입니다.</returns>
    public bool Remove(string name)
    {
        return _headers.Remove(name);
    }

    /// <summary>
    ///     구문 분석 없이 지정된 헤더 및 해당 값을 <see cref="HttpHeaders" /> 컬렉션에 추가합니다.
    /// </summary>
    /// <param name="name">컬렉션에 추가할 헤더입니다.</param>
    /// <param name="value">컬렉션에 추가할 헤더 값입니다.</param>
    /// <returns>헤더가 성공적으로 추가되었으면 <c>true</c>이고, 그렇지 않으면 <c>false</c>입니다.</returns>
    public bool TryAddWithoutValidation(string name, string? value)
    {
        try
        {
            Add(name, value);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    ///     구문 분석 없이 지정된 헤더 및 해당 값을 <see cref="HttpHeaders" /> 컬렉션에 추가합니다.
    /// </summary>
    /// <param name="name">컬렉션에 추가할 헤더입니다.</param>
    /// <param name="values">컬렉션에 추가할 헤더 값입니다.</param>
    /// <returns>헤더가 성공적으로 추가되었으면 <c>true</c>이고, 그렇지 않으면 <c>false</c>입니다.</returns>
    public bool TryAddWithoutValidation(string name, IEnumerable<string?> values)
    {
        try
        {
            Add(name, values);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    ///     <see cref="HttpHeaders" /> 컬렉션에 저장된 특정 헤더에 대한 모든 헤더 값을 가져오려고 시도합니다.
    /// </summary>
    /// <param name="name">헤더입니다.</param>
    /// <param name="values">지정된 헤더에 대한 헤더 값입니다.</param>
    /// <returns>지정된 헤더가 컬렉션에 있으면 <c>true</c>이고, 그렇지 않으면 <c>false</c>입니다.</returns>
    public bool TryGetValues(string name, out IEnumerable<string>? values)
    {
        if (_headers.TryGetValue(name, out List<string>? headerValues))
        {
            values = headerValues;
            return true;
        }

        values = null;
        return false;
    }

    /// <summary>
    ///     <see cref="HttpHeaders" /> 컬렉션에 저장된 특정 헤더에 대한 모든 헤더 값을 가져옵니다.
    /// </summary>
    /// <param name="name">헤더입니다.</param>
    /// <returns>지정된 헤더에 대한 헤더 값입니다.</returns>
    public IEnumerable<string> GetValues(string name)
    {
        if (!_headers.TryGetValue(name, out List<string>? values))
        {
            throw new InvalidOperationException($"지정된 헤더 '{name}'을(를) 찾을 수 없습니다.");
        }

        return values;
    }

    /// <summary>
    ///     헤더를 문자열로 반환합니다.
    /// </summary>
    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (KeyValuePair<string, List<string>> header in _headers)
        {
            sb.Append(header.Key);
            sb.Append(": ");
            sb.AppendLine(string.Join(", ", header.Value.ToArray()));
        }

        return sb.ToString();
    }

    /// <summary>
    ///     지정된 헤더 및 해당 값을 <see cref="HttpHeaders" /> 컬렉션에 추가합니다.
    /// </summary>
    /// <param name="name">컬렉션에 추가할 헤더입니다.</param>
    /// <param name="value">컬렉션에 추가할 헤더 값입니다.</param>
    public void Add(string name, string? value)
    {
        ValidateHeaderName(name);

        if (!_headers.TryGetValue(name, out List<string>? values))
        {
            values = new List<string>();
            _headers[name] = values;
        }

        if (value != null)
        {
            values.Add(value);
        }
    }

    /// <summary>
    ///     지정된 헤더 및 해당 값을 <see cref="HttpHeaders" /> 컬렉션에 추가합니다.
    /// </summary>
    /// <param name="name">컬렉션에 추가할 헤더입니다.</param>
    /// <param name="values">컬렉션에 추가할 헤더 값입니다.</param>
    public void Add(string name, IEnumerable<string?> values)
    {
        ValidateHeaderName(name);

        if (!_headers.TryGetValue(name, out List<string>? headerValues))
        {
            headerValues = new List<string>();
            _headers[name] = headerValues;
        }

        foreach (string? value in values)
        {
            if (value != null)
            {
                headerValues.Add(value);
            }
        }
    }

    /// <summary>
    ///     <see cref="HttpHeaders" /> 컬렉션에서 모든 헤더를 제거합니다.
    /// </summary>
    public void Clear()
    {
        _headers.Clear();
    }

    private static void ValidateHeaderName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("헤더 이름은 null이거나 빈 문자열일 수 없습니다.", nameof(name));
        }
    }
}

/// <summary>
///     요청 헤더의 컬렉션입니다.
/// </summary>
public sealed class HttpRequestHeaders : HttpHeaders
{
    /// <summary>
    ///     Accept 헤더의 값을 가져오거나 설정합니다.
    /// </summary>
    public string? Accept
    {
        get => TryGetValues("Accept", out IEnumerable<string>? values)
            ? string.Join(", ", ((List<string>)values!).ToArray())
            : null;
        set
        {
            Remove("Accept");
            if (value != null)
            {
                Add("Accept", value);
            }
        }
    }

    /// <summary>
    ///     Authorization 헤더의 값을 가져오거나 설정합니다.
    /// </summary>
    public string? Authorization
    {
        get => TryGetValues("Authorization", out IEnumerable<string>? values)
            ? string.Join(", ", ((List<string>)values!).ToArray())
            : null;
        set
        {
            Remove("Authorization");
            if (value != null)
            {
                Add("Authorization", value);
            }
        }
    }

    /// <summary>
    ///     Host 헤더의 값을 가져오거나 설정합니다.
    /// </summary>
    public string? Host
    {
        get => TryGetValues("Host", out IEnumerable<string>? values)
            ? string.Join(", ", ((List<string>)values!).ToArray())
            : null;
        set
        {
            Remove("Host");
            if (value != null)
            {
                Add("Host", value);
            }
        }
    }

    /// <summary>
    ///     User-Agent 헤더의 값을 가져오거나 설정합니다.
    /// </summary>
    public string? UserAgent
    {
        get => TryGetValues("User-Agent", out IEnumerable<string>? values)
            ? string.Join(" ", ((List<string>)values!).ToArray())
            : null;
        set
        {
            Remove("User-Agent");
            if (value != null)
            {
                Add("User-Agent", value);
            }
        }
    }
}

/// <summary>
///     응답 헤더의 컬렉션입니다.
/// </summary>
public sealed class HttpResponseHeaders : HttpHeaders
{
    /// <summary>
    ///     Server 헤더의 값을 가져오거나 설정합니다.
    /// </summary>
    public string? Server
    {
        get => TryGetValues("Server", out IEnumerable<string>? values)
            ? string.Join(" ", ((List<string>)values!).ToArray())
            : null;
        set
        {
            Remove("Server");
            if (value != null)
            {
                Add("Server", value);
            }
        }
    }

    /// <summary>
    ///     Location 헤더의 값을 가져오거나 설정합니다.
    /// </summary>
    public Uri? Location
    {
        get
        {
            if (TryGetValues("Location", out IEnumerable<string>? values))
            {
                string locationValue = string.Join(", ", ((List<string>)values!).ToArray());
                if (Uri.TryCreate(locationValue, UriKind.RelativeOrAbsolute, out Uri? uri))
                {
                    return uri;
                }
            }

            return null;
        }
        set
        {
            Remove("Location");
            if (value != null)
            {
                Add("Location", value.ToString());
            }
        }
    }
}

/// <summary>
///     콘텐츠 헤더의 컬렉션입니다.
/// </summary>
public sealed class HttpContentHeaders : HttpHeaders
{
    /// <summary>
    ///     Content-Length 헤더의 값을 가져오거나 설정합니다.
    /// </summary>
    public long? ContentLength
    {
        get
        {
            if (TryGetValues("Content-Length", out IEnumerable<string>? values))
            {
                string lengthValue = string.Join("", ((List<string>)values!).ToArray());
                if (long.TryParse(lengthValue, out long length))
                {
                    return length;
                }
            }

            return null;
        }
        set
        {
            Remove("Content-Length");
            if (value.HasValue)
            {
                Add("Content-Length", value.Value.ToString());
            }
        }
    }

    /// <summary>
    ///     Content-Disposition 헤더의 값을 가져오거나 설정합니다.
    /// </summary>
    public string? ContentDisposition
    {
        get => TryGetValues("Content-Disposition", out IEnumerable<string>? values)
            ? string.Join(", ", ((List<string>)values!).ToArray())
            : null;
        set
        {
            Remove("Content-Disposition");
            if (value != null)
            {
                Add("Content-Disposition", value);
            }
        }
    }

    /// <summary>
    ///     Content-Encoding 헤더의 값을 가져오거나 설정합니다.
    /// </summary>
    public string? ContentEncoding
    {
        get => TryGetValues("Content-Encoding", out IEnumerable<string>? values)
            ? string.Join(", ", ((List<string>)values!).ToArray())
            : null;
        set
        {
            Remove("Content-Encoding");
            if (value != null)
            {
                Add("Content-Encoding", value);
            }
        }
    }

    /// <summary>
    ///     Content-Type 헤더의 값을 가져오거나 설정합니다.
    /// </summary>
    public string? ContentType
    {
        get => TryGetValues("Content-Type", out IEnumerable<string>? values)
            ? string.Join(", ", ((List<string>)values!).ToArray())
            : null;
        set
        {
            Remove("Content-Type");
            if (value != null)
            {
                Add("Content-Type", value);
            }
        }
    }
}
#endif
