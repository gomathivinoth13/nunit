using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SEG.StoreLocatorLibrary.Shared.Types
{
    public class Option<T> : IEnumerable<T>
    {
        public int StatusCode { get; }
        public string Message { get; }
        public bool HasValue => _value.Any();
        public Exception Exception { get; }
        public bool IsException => Exception != null;

        private readonly IEnumerable<T> _value;

        private Option(IEnumerable<T> items, int statusCode, string message, Exception exception = null)
        {
            StatusCode = statusCode;
            Message = message;
            _value = items;
            Exception = exception;
        }

        public static Option<T> Create(T item, int statusCode, string message) => item != null
            ? new Option<T>(new[] { item }, statusCode, message)
            : CreateEmpty(400, "Null Argument");

        public static Option<T> Create(T item) =>
            item != null ? new Option<T>(new[] { item }, 200, "") : CreateEmpty(400, "Null Argument");

        public static Option<T> CreateEmpty(int statusCode, string message) =>
            new Option<T>(new T[0], statusCode, message);
        public static Option<T> CreateEmpty() => new Option<T>(new T[0], 204, "No Content");

        public static Option<T> CreateException(Exception e, string logMessage = null)
        {
            if (!int.TryParse(e.Data["ErrorCode"]?.ToString(), out var errorCode))
                errorCode = 500;

            return new Option<T>(new T[0], errorCode, logMessage ?? e.Message, e);
        }

        //public bool HasValue() => _value.Count() > 0;

        public static implicit operator T(Option<T> obj) => obj.Value;
        public T Value => _value.FirstOrDefault();

        public IEnumerator<T> GetEnumerator() => _value.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
