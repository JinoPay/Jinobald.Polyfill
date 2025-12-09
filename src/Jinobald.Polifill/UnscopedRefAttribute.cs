#if !NET7_0_OR_GREATER

namespace System.Diagnostics.CodeAnalysis;

using Targets = AttributeTargets;

/// <summary>
///     Used to indicate a byref escapes and is not scoped.
/// </summary>
[ExcludeFromCodeCoverage]
[DebuggerNonUserCode]
[AttributeUsage(Targets.Method | Targets.Property | Targets.Parameter, Inherited = false)]
public sealed class UnscopedRefAttribute : Attribute;
#else
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(System.Diagnostics.CodeAnalysis.UnscopedRefAttribute))]
#endif