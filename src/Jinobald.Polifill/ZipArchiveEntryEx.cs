using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.IO.Compression;

[ExcludeFromCodeCoverage]
[DebuggerNonUserCode]
public static partial class ZipArchiveEntryEx
{
    extension(ZipArchiveEntry target)
    {
#if !NET10
        /// <summary>
        ///     OS and application specific file attributes.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchiveentry.externalattributes?view=net-10.0
        public int ExternalAttributes
        {
            get => 0;
            set { }
        }
#endif
    }
}