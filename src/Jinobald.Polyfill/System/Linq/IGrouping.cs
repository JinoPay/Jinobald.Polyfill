// Jinobald.Polyfill - LINQ 그룹화 인터페이스
// 공통 키를 가진 요소 컬렉션을 나타냅니다.

#if NET20

using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>
    /// 공통 키를 가진 개체의 컬렉션을 나타냅니다.
    /// </summary>
    /// <typeparam name="TKey"><see cref="IGrouping{TKey, TElement}"/>의 키 형식입니다.</typeparam>
    /// <typeparam name="TElement"><see cref="IGrouping{TKey, TElement}"/>의 값 형식입니다.</typeparam>
    public interface IGrouping<out TKey, out TElement> : IEnumerable<TElement>
    {
        /// <summary>
        /// <see cref="IGrouping{TKey, TElement}"/>의 키를 가져옵니다.
        /// </summary>
        TKey Key { get; }
    }
}

#endif
