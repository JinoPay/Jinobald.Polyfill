namespace System.Runtime.CompilerServices;

/// <summary>
///     Indicates that the use of a value tuple on a member is meant to be treated as a tuple with element names.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property |
                AttributeTargets.ReturnValue | AttributeTargets.Class | AttributeTargets.Struct |
                AttributeTargets.Event)]
public sealed class TupleElementNamesAttribute : Attribute
{
#if NET35 || NET40
    public string[] TransformNames { get; }

    public TupleElementNamesAttribute(string[] transformNames)
    {
        TransformNames = transformNames;
    }
#else
        public string?[] TransformNames { get; }

        public TupleElementNamesAttribute(string?[] transformNames)
        {
            TransformNames = transformNames;
        }
#endif
}
