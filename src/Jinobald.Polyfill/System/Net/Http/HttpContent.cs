using System.Net.Http.Headers;
using System.Text;

#if NETFRAMEWORK
namespace System.Net.Http;

/// <summary>
///     HTTP 엔터티 본문 및 콘텐츠 헤더를 나타내는 기본 클래스입니다.
/// </summary>
public abstract class HttpContent : IDisposable
{
    private bool _disposed;
    private byte[]? _bufferedContent;
    private HttpContentHeaders? _headers;

    /// <summary>
    ///     HTTP 콘텐츠 헤더를 가져옵니다.
    /// </summary>
    public HttpContentHeaders Headers
    {
        get
        {
            if (_headers == null)
            {
                _headers = new HttpContentHeaders();
            }

            return _headers;
        }
    }

    /// <summary>
    ///     현재 인스턴스에서 사용하는 관리되지 않는 리소스를 해제합니다.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     HTTP 콘텐츠를 바이트 배열로 읽습니다.
    /// </summary>
    /// <returns>HTTP 콘텐츠를 나타내는 바이트 배열입니다.</returns>
    public byte[] ReadAsByteArray()
    {
        CheckDisposed();
        LoadIntoBuffer();
        return _bufferedContent ?? new byte[0];
    }

    /// <summary>
    ///     HTTP 콘텐츠를 스트림으로 읽습니다.
    /// </summary>
    /// <returns>HTTP 콘텐츠를 나타내는 스트림입니다.</returns>
    public Stream ReadAsStream()
    {
        CheckDisposed();
        byte[] bytes = ReadAsByteArray();
        return new MemoryStream(bytes);
    }

    /// <summary>
    ///     HTTP 콘텐츠를 문자열로 읽습니다.
    /// </summary>
    /// <returns>HTTP 콘텐츠를 나타내는 문자열입니다.</returns>
    public string ReadAsString()
    {
        CheckDisposed();
        byte[] bytes = ReadAsByteArray();
        Encoding encoding = GetEncoding();
        return encoding.GetString(bytes);
    }

    /// <summary>
    ///     HTTP 콘텐츠를 바이트 스트림으로 직렬화하고 <paramref name="stream" /> 매개 변수로 제공되는 스트림 개체에 비동기적으로 복사합니다.
    /// </summary>
    /// <param name="stream">대상 스트림입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task CopyToAsync(Stream stream)
    {
        return CopyToAsync(stream, null);
    }

    /// <summary>
    ///     HTTP 콘텐츠를 바이트 스트림으로 직렬화하고 <paramref name="stream" /> 매개 변수로 제공되는 스트림 개체에 비동기적으로 복사합니다.
    /// </summary>
    /// <param name="stream">대상 스트림입니다.</param>
    /// <param name="context">전송 컨텍스트입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task CopyToAsync(Stream stream, TransportContext? context)
    {
        CheckDisposed();
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        return Task.Run(() => SerializeToStream(stream, context));
    }

    /// <summary>
    ///     HTTP 콘텐츠를 메모리 버퍼에 비동기적으로 로드합니다.
    /// </summary>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task LoadIntoBufferAsync()
    {
        return LoadIntoBufferAsync(int.MaxValue);
    }

    /// <summary>
    ///     HTTP 콘텐츠를 메모리 버퍼에 비동기적으로 로드합니다.
    /// </summary>
    /// <param name="maxBufferSize">사용할 최대 버퍼 크기(바이트)입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task LoadIntoBufferAsync(long maxBufferSize)
    {
        return Task.Run(() => LoadIntoBuffer(maxBufferSize));
    }

    /// <summary>
    ///     HTTP 콘텐츠를 바이트 배열로 비동기적으로 읽습니다.
    /// </summary>
    /// <returns>HTTP 콘텐츠를 나타내는 바이트 배열을 포함하는 작업 개체입니다.</returns>
    public Task<byte[]> ReadAsByteArrayAsync()
    {
        CheckDisposed();
        return Task.Run(() => ReadAsByteArray());
    }

    /// <summary>
    ///     HTTP 콘텐츠를 스트림으로 비동기적으로 읽습니다.
    /// </summary>
    /// <returns>HTTP 콘텐츠를 나타내는 스트림을 포함하는 작업 개체입니다.</returns>
    public Task<Stream> ReadAsStreamAsync()
    {
        CheckDisposed();
        return Task.Run(() => ReadAsStream());
    }

    /// <summary>
    ///     HTTP 콘텐츠를 문자열로 비동기적으로 읽습니다.
    /// </summary>
    /// <returns>HTTP 콘텐츠를 나타내는 문자열을 포함하는 작업 개체입니다.</returns>
    public Task<string> ReadAsStringAsync()
    {
        CheckDisposed();
        return Task.Run(() => ReadAsString());
    }

    /// <summary>
    ///     HTTP 콘텐츠를 바이트 스트림으로 직렬화하고 <paramref name="stream" /> 매개 변수로 제공되는 스트림 개체에 복사합니다.
    /// </summary>
    /// <param name="stream">대상 스트림입니다.</param>
    public void CopyTo(Stream stream)
    {
        CopyTo(stream, null);
    }

    /// <summary>
    ///     HTTP 콘텐츠를 바이트 스트림으로 직렬화하고 <paramref name="stream" /> 매개 변수로 제공되는 스트림 개체에 복사합니다.
    /// </summary>
    /// <param name="stream">대상 스트림입니다.</param>
    /// <param name="context">전송 컨텍스트입니다.</param>
    public void CopyTo(Stream stream, TransportContext? context)
    {
        CheckDisposed();
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        SerializeToStream(stream, context);
    }

    /// <summary>
    ///     HTTP 콘텐츠를 메모리 버퍼에 로드합니다.
    /// </summary>
    public void LoadIntoBuffer()
    {
        LoadIntoBuffer(int.MaxValue);
    }

    /// <summary>
    ///     HTTP 콘텐츠를 메모리 버퍼에 로드합니다.
    /// </summary>
    /// <param name="maxBufferSize">사용할 최대 버퍼 크기(바이트)입니다.</param>
    public void LoadIntoBuffer(long maxBufferSize)
    {
        CheckDisposed();

        if (_bufferedContent != null)
        {
            return;
        }

        using (var memoryStream = new MemoryStream())
        {
            SerializeToStream(memoryStream, null);
            if (memoryStream.Length > maxBufferSize)
            {
                throw new HttpRequestException($"콘텐츠 크기({memoryStream.Length})가 최대 버퍼 크기({maxBufferSize})를 초과합니다.");
            }

            _bufferedContent = memoryStream.ToArray();
        }
    }

    /// <summary>
    ///     <see cref="HttpContent" />에서 사용하는 관리되지 않는 리소스를 해제하고, 필요한 경우 관리되는 리소스도 삭제합니다.
    /// </summary>
    /// <param name="disposing">관리되는 리소스와 관리되지 않는 리소스를 모두 해제하려면 <c>true</c>이고, 관리되지 않는 리소스만 해제하려면 <c>false</c>입니다.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
    }

    /// <summary>
    ///     HTTP 콘텐츠를 스트림으로 직렬화합니다.
    /// </summary>
    /// <param name="stream">대상 스트림입니다.</param>
    /// <param name="context">전송 컨텍스트입니다.</param>
    protected abstract void SerializeToStream(Stream stream, TransportContext? context);

    private Encoding GetEncoding()
    {
        string? contentType = Headers.ContentType;
        if (contentType != null)
        {
            // charset 파싱 시도
            int charsetIndex = contentType.IndexOf("charset=", StringComparison.OrdinalIgnoreCase);
            if (charsetIndex >= 0)
            {
                int charsetStart = charsetIndex + 8;
                int charsetEnd = contentType.IndexOf(';', charsetStart);
                if (charsetEnd < 0)
                {
                    charsetEnd = contentType.Length;
                }

                string charset = contentType.Substring(charsetStart, charsetEnd - charsetStart).Trim().Trim('"');

                try
                {
                    return Encoding.GetEncoding(charset);
                }
                catch
                {
                    // 인코딩을 찾을 수 없는 경우 기본값 사용
                }
            }
        }

        return Encoding.UTF8;
    }

    private void CheckDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }

    /// <summary>
    ///     HTTP 콘텐츠의 바이트 길이를 계산하는지 여부를 결정합니다.
    /// </summary>
    /// <param name="length">콘텐츠의 길이(바이트)입니다.</param>
    /// <returns>길이가 유효한 값이면 <c>true</c>이고, 그렇지 않으면 <c>false</c>입니다.</returns>
    protected internal abstract bool TryComputeLength(out long length);
}

/// <summary>
///     전송 컨텍스트를 나타냅니다.
/// </summary>
public class TransportContext
{
}
#endif
