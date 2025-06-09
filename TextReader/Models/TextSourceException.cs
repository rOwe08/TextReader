using System;

namespace TextReader.Models
{
    public class TextSourceException : Exception
    {
        public TextSourceException(string message) : base(message)
        {
        }

        public TextSourceException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
} 