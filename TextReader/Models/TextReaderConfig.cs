using System;

namespace TextReader.Models
{
    public static class TextReaderConfig
    {
        // Random text generation settings
        public const string RandomTextChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public const int MinRandomLineLength = 10;
        public const int MaxRandomLineLength = 100;

        // Line count settings
        public const int DefaultLineCount = 200;
        public const int MaxLineCount = 1000000;
        public const int MinLineCount = 1;

        // UI settings
        public const int StatusBarHeight = 22;
        public const int SearchBoxWidth = 300;
        public const int SearchBoxHeight = 100;
    }
} 