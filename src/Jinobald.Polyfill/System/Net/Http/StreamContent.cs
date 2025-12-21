#if NETFRAMEWORK
namespace System.Net.Http;

using System.IO;

/// <summary>
/// 스트림을 기반으로 HTTP 콘텐츠를 제공합니다.
/// </summary>
public class StreamContent : HttpContent
{
    private readonly Stream _content;
    private readonly int _bufferSize;
    private readonly long _startPosition;
    private bool _contentConsumed;

    /// <summary>
    /// <see cref="StreamContent"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="content">HTTP 콘텐츠로 사용할 스트림입니다.</param>
    public StreamContent(Stream content)
        : this(content, 4096)
    {
    }

    /// <summary>
    /// <see cref="StreamContent"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="content">HTTP 콘텐츠로 사용할 스트림입니다.</param>
    /// <param name="bufferSize">버퍼 크기(바이트)입니다.</param>
    public StreamContent(Stream content, int bufferSize)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));
        if (bufferSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(bufferSize));

        _content = content;
        _bufferSize = bufferSize;

        if (content.CanSeek)
            _startPosition = content.Position;
    }

    /// <summary>
    /// HTTP 콘텐츠를 스트림으로 직렬화합니다.
    /// </summary>
    /// <param name="stream">대상 스트림입니다.</param>
    /// <param name="context">전송 컨텍스트입니다.</param>
    protected override void SerializeToStream(Stream stream, TransportContext? context)
    {
        PrepareContent();

        var buffer = new byte[_bufferSize];
        int bytesRead;

        while ((bytesRead = _content.Read(buffer, 0, buffer.Length)) > 0)
        {
            stream.Write(buffer, 0, bytesRead);
        }
    }

    /// <summary>
    /// HTTP 콘텐츠의 바이트 길이를 계산합니다.
    /// </summary>
    /// <param name="length">콘텐츠의 길이(바이트)입니다.</param>
    /// <returns>길이를 계산할 수 있으면 <c>true</c>이고, 그렇지 않으면 <c>false</c>입니다.</returns>
    protected internal override bool TryComputeLength(out long length)
    {
        if (_content.CanSeek)
        {
            length = _content.Length - _startPosition;
            return true;
        }

        length = 0;
        return false;
    }

    /// <summary>
    /// <see cref="StreamContent"/>에서 사용하는 관리되지 않는 리소스를 해제합니다.
    /// </summary>
    /// <param name="disposing">관리되는 리소스와 관리되지 않는 리소스를 모두 해제하려면 <c>true</c>이고, 관리되지 않는 리소스만 해제하려면 <c>false</c>입니다.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _content.Dispose();
        }

        base.Dispose(disposing);
    }

    private void PrepareContent()
    {
        if (_contentConsumed)
        {
            if (_content.CanSeek)
            {
                _content.Position = _startPosition;
            }
            else
            {
                throw new InvalidOperationException("스트림이 이미 소비되었으며 탐색을 지원하지 않습니다.");
            }
        }

        _contentConsumed = true;
    }
}
#endif
