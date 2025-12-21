namespace System.Runtime.CompilerServices;

/// <summary>
/// Provides factory methods for creating FormattableString instances.
/// </summary>
public static class FormattableStringFactory
{
    /// <summary>
    /// Creates a FormattableString instance using the specified format string and arguments.
    /// </summary>
    /// <param name="format">The format string.</param>
    /// <param name="args">The arguments to be formatted.</param>
    /// <returns>A FormattableString instance representing the formatted string.</returns>
    public static FormattableString Create(string format, params object[] args)
    {
        if (format == null)
        {
            throw new ArgumentNullException(nameof(format));
        }

        return new ConcreteFormattableString(format, args);
    }

    private sealed class ConcreteFormattableString : FormattableString
    {
        private readonly string _format;
        private readonly object[] _args;

        public ConcreteFormattableString(string format, object[] args)
        {
            _format = format;
            _args = args ?? [];
        }

        public override string Format => _format;

        public override int ArgumentCount => _args.Length;

        public override object[] GetArguments()
        {
            return (object[])_args.Clone();
        }

        public override string ToString(IFormatProvider formatProvider)
        {
            return string.Format(formatProvider, _format, _args);
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, _format, _args);
        }
    }
}
