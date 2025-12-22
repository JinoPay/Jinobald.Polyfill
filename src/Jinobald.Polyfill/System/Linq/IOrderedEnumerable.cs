// Jinobald.Polyfill - 정렬된 시퀀스 인터페이스
// 정렬된 시퀀스를 나타내며 후속 정렬 작업을 지원합니다.

#if NET20
using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>
    /// 정렬된 시퀀스를 나타냅니다.
    /// </summary>
    /// <typeparam name="TElement">시퀀스 요소의 형식입니다.</typeparam>
    public interface IOrderedEnumerable<out TElement> : IEnumerable<TElement>
    {
        /// <summary>
        /// <see cref="IOrderedEnumerable{TElement}"/>의 요소에 대해 키를 기준으로 후속 정렬을 수행합니다.
        /// </summary>
        /// <typeparam name="TKey"><paramref name="keySelector"/>에서 생성하는 키의 형식입니다.</typeparam>
        /// <param name="keySelector">각 요소의 키를 추출하는 함수입니다.</param>
        /// <param name="comparer">반환된 시퀀스에서 배치를 위해 키를 비교하는 <see cref="IComparer{TKey}"/>입니다.</param>
        /// <param name="descending">요소를 내림차순으로 정렬하려면 true이고, 오름차순으로 정렬하려면 false입니다.</param>
        /// <returns>요소가 키에 따라 정렬된 <see cref="IOrderedEnumerable{TElement}"/>입니다.</returns>
        IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(
            Func<TElement, TKey> keySelector,
            IComparer<TKey>? comparer,
            bool descending);
    }
}

#endif
