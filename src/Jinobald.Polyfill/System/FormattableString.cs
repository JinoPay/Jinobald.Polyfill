#if NET35 || NET40 || NET45 || NET451 || NET452
namespace System;

/// <summary>
/// Provides a base class for objects that can be formatted by using format strings.
/// </summary>
public abstract class FormattableString : IFormattable
{
    /// <summary>
    /// Gets a composite format string.
    /// </summary>
    public abstract string Format { get; }

    /// <summary>
    /// Gets the number of format items in the composite format string.
    /// </summary>
    public abstract int ArgumentCount { get; }

    /// <summary>
    /// Returns the string representation of the current object in the invariant culture.
    /// </summary>
    /// <returns>The string representation of the current object in the invariant culture.</returns>
    public override string ToString()
    {
        return ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Returns the string representation of the current object in the specified culture.
    /// </summary>
    /// <param name="formatProvider">The format provider to use when formatting the object.</param>
    /// <returns>The string representation of the current object in the specified culture.</returns>
    public abstract string ToString(IFormatProvider formatProvider);

    /// <summary>
    /// Returns an array of the arguments of the current object.
    /// </summary>
    /// <returns>An array of the arguments of the current object.</returns>
    public abstract object[] GetArguments();

    /// <summary>
    /// Returns the string representation of the specified FormattableString object using the invariant culture.
    /// </summary>
    /// <param name="formattableString">The FormattableString object to convert to a string.</param>
    /// <returns>The string representation of the FormattableString object in the invariant culture.</returns>
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
