#if NET35
namespace System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

/// <summary>
/// 응용 프로그램 실행 중에 발생하는 하나 이상의 오류를 나타냅니다.
/// </summary>
public class AggregateException : Exception
{
    private readonly ReadOnlyCollection<Exception> _innerExceptions;

    /// <summary>
    /// 현재 예외를 발생시킨 Exception 인스턴스의 읽기 전용 컬렉션을 가져옵니다.
    /// </summary>
    public ReadOnlyCollection<Exception> InnerExceptions
    {
        get { return _innerExceptions; }
    }

    /// <summary>
    /// 오류를 설명하는 시스템 제공 메시지로 AggregateException 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public AggregateException()
        : base("One or more errors occurred.")
    {
        _innerExceptions = new ReadOnlyCollection<Exception>(new Exception[0]);
    }

    /// <summary>
    /// 오류를 설명하는 지정된 메시지로 AggregateException 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public AggregateException(string? message)
        : base(message ?? "One or more errors occurred.")
    {
        _innerExceptions = new ReadOnlyCollection<Exception>(new Exception[0]);
    }

    /// <summary>
    /// 이 예외의 원인인 내부 예외에 대한 참조로 AggregateException 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public AggregateException(params Exception[] innerExceptions)
        : this("One or more errors occurred.", innerExceptions)
    {
    }

    /// <summary>
    /// 이 예외의 원인인 내부 예외에 대한 참조로 AggregateException 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public AggregateException(IEnumerable<Exception> innerExceptions)
        : this("One or more errors occurred.", innerExceptions)
    {
    }

    /// <summary>
    /// 지정된 오류 메시지 및 이 예외의 원인인 내부 예외에 대한 참조로 AggregateException 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public AggregateException(string? message, params Exception[] innerExceptions)
        : base(message ?? "One or more errors occurred.", innerExceptions != null && innerExceptions.Length > 0 ? innerExceptions[0] : null)
    {
        if (innerExceptions == null)
            throw new ArgumentNullException(nameof(innerExceptions));

        var exceptions = new List<Exception>();
        foreach (var exception in innerExceptions)
        {
            if (exception == null)
                throw new ArgumentException("An element of innerExceptions is null.");
            exceptions.Add(exception);
        }

        _innerExceptions = new ReadOnlyCollection<Exception>(exceptions);
    }

    /// <summary>
    /// 지정된 오류 메시지 및 이 예외의 원인인 내부 예외에 대한 참조로 AggregateException 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public AggregateException(string? message, IEnumerable<Exception> innerExceptions)
        : this(message, innerExceptions != null ? innerExceptions.ToArray() : null!)
    {
    }

    /// <summary>
    /// 지정된 오류 메시지 및 이 예외의 원인인 내부 예외에 대한 참조로 AggregateException 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public AggregateException(string? message, Exception innerException)
        : base(message ?? "One or more errors occurred.", innerException)
    {
        if (innerException == null)
            throw new ArgumentNullException(nameof(innerException));

        _innerExceptions = new ReadOnlyCollection<Exception>(new[] { innerException });
    }

    /// <summary>
    /// AggregateException 인스턴스를 단일 새 인스턴스로 평면화합니다.
    /// </summary>
    public AggregateException Flatten()
    {
        var flattenedExceptions = new List<Exception>();
        var exceptionsToFlatten = new List<Exception>();
        exceptionsToFlatten.Add(this);

        while (exceptionsToFlatten.Count > 0)
        {
            var currentException = exceptionsToFlatten[0];
            exceptionsToFlatten.RemoveAt(0);

            var aggregateException = currentException as AggregateException;
            if (aggregateException != null)
            {
                foreach (var innerException in aggregateException.InnerExceptions)
                {
                    exceptionsToFlatten.Add(innerException);
                }
            }
            else
            {
                flattenedExceptions.Add(currentException);
            }
        }

        return new AggregateException(Message, flattenedExceptions);
    }

    /// <summary>
    /// 이 AggregateException에 포함된 각 예외에 대해 처리기를 호출합니다.
    /// </summary>
    public void Handle(Func<Exception, bool> predicate)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        var unhandledExceptions = new List<Exception>();

        foreach (var exception in InnerExceptions)
        {
            if (!predicate(exception))
            {
                unhandledExceptions.Add(exception);
            }
        }

        if (unhandledExceptions.Count > 0)
        {
            throw new AggregateException(Message, unhandledExceptions);
        }
    }

    /// <summary>
    /// 현재 AggregateException의 문자열 표현을 만들고 반환합니다.
    /// </summary>
    public override string ToString()
    {
        var text = base.ToString();

        for (int i = 0; i < InnerExceptions.Count; i++)
        {
            text = string.Format(
                "{0}{1}---> (Inner Exception #{2}) {3}{4}{5}",
                text,
                Environment.NewLine,
                i,
                InnerExceptions[i].ToString(),
                "<---",
                Environment.NewLine);
        }

        return text;
    }

    /// <summary>
    /// 이 예외의 근본 원인인 AggregateException을 반환합니다.
    /// </summary>
    public override Exception GetBaseException()
    {
        Exception? back = this;
        AggregateException? backAsAggregate = this;

        while (backAsAggregate != null && backAsAggregate.InnerExceptions.Count == 1)
        {
            back = back.InnerException;
            backAsAggregate = back as AggregateException;
        }

        return back ?? this;
    }
}

#endif
