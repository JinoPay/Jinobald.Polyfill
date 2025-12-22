namespace System;

#if NET35 || NET40 || NET45 || NET451 || NET452
public static class ArrayEx
{
    extension(Array)
    {
        public static T[] Empty<T>()
        {
            return [];
        }
    }
}
#endif
