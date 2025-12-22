#if NET35
namespace System.Threading.Tasks;

/// <summary>
///     Parallel 클래스의 메서드 동작을 구성하는 옵션을 저장합니다.
/// </summary>
public class ParallelOptions
{
    private int _maxDegreeOfParallelism = -1;

    /// <summary>
    ///     ParallelOptions 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public ParallelOptions()
    {
        CancellationToken = CancellationToken.None;
    }

    /// <summary>
    ///     이 ParallelOptions 인스턴스와 연결된 CancellationToken을 가져오거나 설정합니다.
    /// </summary>
    public CancellationToken CancellationToken { get; set; }

    /// <summary>
    ///     이 ParallelOptions 인스턴스에서 사용할 수 있는 최대 동시 작업 수를 가져오거나 설정합니다.
    /// </summary>
    /// <remarks>
    ///     -1은 무제한을 의미하며, 시스템이 최적의 값을 결정합니다.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     값이 -1보다 작거나 0인 경우
    /// </exception>
    public int MaxDegreeOfParallelism
    {
        get => _maxDegreeOfParallelism;
        set
        {
            if (value < -1 || value == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "병렬 처리 최대 수준은 -1 또는 양수 정수여야 합니다.");
            }

            _maxDegreeOfParallelism = value;
        }
    }

    /// <summary>
    ///     유효한 병렬 처리 수준을 가져옵니다.
    /// </summary>
    internal int EffectiveMaxDegreeOfParallelism
    {
        get
        {
            if (_maxDegreeOfParallelism == -1)
            {
                return Environment.ProcessorCount;
            }

            return _maxDegreeOfParallelism;
        }
    }
}

#endif
