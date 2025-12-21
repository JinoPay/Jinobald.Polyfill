// Jinobald.Polyfill - Lookup 인터페이스
// 키를 IEnumerable<TElement> 값 시퀀스에 매핑하는 데이터 구조를 정의합니다.

#if NET20

using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>
    /// 키를 <see cref="IEnumerable{TElement}"/> 값 시퀀스에 매핑하는 데이터 구조에 대한
    /// 인덱서, 크기 속성 및 부울 검색 메서드를 정의합니다.
    /// </summary>
    /// <typeparam name="TKey"><see cref="ILookup{TKey, TElement}"/>의 키 형식입니다.</typeparam>
    /// <typeparam name="TElement"><see cref="ILookup{TKey, TElement}"/>의 값을 구성하는 <see cref="IEnumerable{TElement}"/> 시퀀스의 요소 형식입니다.</typeparam>
    public interface ILookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>
    {
        /// <summary>
        /// <see cref="ILookup{TKey, TElement}"/>에 있는 키/값 컬렉션 쌍의 수를 가져옵니다.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 지정된 키로 인덱싱된 값의 <see cref="IEnumerable{TElement}"/> 시퀀스를 가져옵니다.
        /// </summary>
        /// <param name="key">원하는 값 시퀀스의 키입니다.</param>
        /// <returns>지정된 키로 인덱싱된 값의 <see cref="IEnumerable{TElement}"/> 시퀀스입니다.</returns>
        IEnumerable<TElement> this[TKey key] { get; }

        /// <summary>
        /// 지정된 키가 <see cref="ILookup{TKey, TElement}"/>에 있는지 확인합니다.
        /// </summary>
        /// <param name="key"><see cref="ILookup{TKey, TElement}"/>에서 검색할 키입니다.</param>
        /// <returns><paramref name="key"/>가 <see cref="ILookup{TKey, TElement}"/>에 있으면 true이고, 그렇지 않으면 false입니다.</returns>
        bool Contains(TKey key);
    }
}

#endif
