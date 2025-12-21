#if NET35 || NET40 || NET45 || NET451 || NET452
namespace System;

/// <summary>
/// 형식 문자열을 사용하여 형식을 지정할 수 있는 개체의 기본 클래스를 제공합니다.
/// </summary>
public abstract class FormattableString : IFormattable
{
    /// <summary>
    /// 복합 형식 문자열을 가져옵니다.
    /// </summary>
    public abstract string Format { get; }

    /// <summary>
    /// 복합 형식 문자열의 형식 항목 수를 가져옵니다.
    /// </summary>
    public abstract int ArgumentCount { get; }

    /// <summary>
    /// 고정 문화권에서 현재 개체의 문자열 표현을 반환합니다.
    /// </summary>
    /// <returns>고정 문화권에서 현재 개체의 문자열 표현입니다.</returns>
    public override string ToString()
    {
        return ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 지정된 문화권에서 현재 개체의 문자열 표현을 반환합니다.
    /// </summary>
    /// <param name="formatProvider">개체 형식을 지정할 때 사용할 형식 공급자입니다.</param>
    /// <returns>지정된 문화권에서 현재 개체의 문자열 표현입니다.</returns>
    public abstract string ToString(IFormatProvider formatProvider);

    /// <summary>
    /// 현재 개체의 인수 배열을 반환합니다.
    /// </summary>
    /// <returns>현재 개체의 인수 배열입니다.</returns>
    public abstract object[] GetArguments();

    /// <summary>
    /// 고정 문화권을 사용하여 지정된 FormattableString 개체의 문자열 표현을 반환합니다.
    /// </summary>
    /// <param name="formattableString">문자열로 변환할 FormattableString 개체입니다.</param>
    /// <returns>고정 문화권에서 FormattableString 개체의 문자열 표현입니다.</returns>
    public static string Invariant(FormattableString formattableString)
    {
        if (formattableString == null)
        {
            throw new ArgumentNullException(nameof(formattableString));
        }

        return formattableString.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    string IFormattable.ToString(string format, IFormatProvider formatProvider)
    {
        return ToString(formatProvider);
    }
}
#endif
