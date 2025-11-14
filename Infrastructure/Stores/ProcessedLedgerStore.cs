using System;
using System.Collections.Generic;
using System.IO;
using Application.Ports;

namespace Infrastructure.Stores
{
    // Sehr einfache Implementierung: jede Zeile = RequestId
    public class ProcessedLedgerStore : IProcessedLedgerStore
    {
        private readonly string _path;
        private readonly HashSet<string> _ids;

        public ProcessedLedgerStore(string path)
        {
            _path = path;
            _ids = Load(path);
        }

        private HashSet<string> Load(string path)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (!File.Exists(path))
                return set;

            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                string id = line.Trim();
                if (!string.IsNullOrEmpty(id))
                    set.Add(id);
            }

            return set;
        }

        public bool IsProcessed(string requestId)
        {
            return _ids.Contains(requestId);
        }

        public void MarkProcessed(string requestId)
        {
            if (_ids.Add(requestId))
            {
                // Append to file
                File.AppendAllLines(_path, new[] { requestId });
            }
        }
    }
}
