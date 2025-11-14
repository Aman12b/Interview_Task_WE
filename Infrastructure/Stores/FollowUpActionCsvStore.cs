using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Application.Ports;
using DomainModel;

namespace Infrastructure.Stores
{
    // Speicher FollowUps in CSV: Id,Type,RequestId,DueAt
    public class FollowUpActionCsvStore : IFollowUpActionStore
    {
        private readonly string _path;

        public FollowUpActionCsvStore(string path)
        {
            _path = path;
            EnsureHeader();
        }

        private void EnsureHeader()
        {
            if (!File.Exists(_path))
            {
                File.WriteAllText(_path, "Id,Type,RequestId,DueAt" + Environment.NewLine);
            }
        }

        public void SaveMany(IEnumerable<FollowUpAction> actions)
        {
            var lines = new List<string>();
            foreach (FollowUpAction a in actions)
            {
                string dueText = a.DueAt.HasValue
                    ? a.DueAt.Value.ToString("o", CultureInfo.InvariantCulture)
                    : string.Empty;

                string line = string.Format(
                    "{0},{1},{2},{3}",
                    a.Id,
                    a.Type,
                    a.RequestId,
                    dueText
                );
                lines.Add(line);
            }

            if (lines.Count > 0)
                File.AppendAllLines(_path, lines.ToArray());
        }
    }
}
