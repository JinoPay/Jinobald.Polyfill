using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

#if !NET

namespace System.Runtime.InteropServices;

/// <summary>
///     An attribute used to indicate a GC transition should be skipped when making an unmanaged function call.
/// </summary>
[ExcludeFromCodeCoverage]
[DebuggerNonUserCode]
[AttributeUsage(
    AttributeTargets.Method,
    Inherited = false)]
//Link: https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.suppressgctransitionattribute?view=net-10.0
public sealed class SuppressGCTransitionAttribute : Attribute;
#else
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(System.Runtime.InteropServices.SuppressGCTransitionAttribute))]
#endif
