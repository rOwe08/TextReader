using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TextReader.Models;

namespace TextReader.Sources
{
    public class FileTextSource : ITextSource
    {
        private readonly string _filePath;

        public FileTextSource(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<IEnumerable<string>> GetLinesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await File.ReadAllLinesAsync(_filePath, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new TextSourceException($"Error reading file {_filePath}", ex);
            }
        }
    }
}
