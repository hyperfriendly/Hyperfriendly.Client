using System;

namespace HyperFriendly.Client
{
    public class QueryStringException : Exception
    {
        public QueryStringException(string message) : base(message)
        {
        }
    }
}