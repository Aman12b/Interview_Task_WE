using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DomainModel;
using Application.Ports;

namespace Infrastructure.Csv
{
    // Erwartetes Format: RequestId,CustomerId,TariffId,RequestedAt (ISO oder culture invariant)
    public class RequestCsvRepository : IRequestReader
    {
        private readonly string _path;

        public RequestCsvRepository(string csvPath)
        {
            _path = csvPath;
        }

        public IEnumerable<Request> ReadNew()
        {
            if (!File.Exists(_path))
                yield break;

            string[] lines = File.ReadAllLines(_path);
            if (lines.Length <= 1)
                yield break;

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] p = line.Split(CsvConfig.Delimiter);
                if (p.Length < 4) continue;

                string id = p[0].Trim();
                string custId = p[1].Trim();
                string tariffId = p[2].Trim();
                string ts = p[3].Trim();

                DateTimeOffset requestedAt;
                if (!DateTimeOffset.TryParse(ts, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out requestedAt))
                {
                    continue;
                }

                yield return new Request(id, custId, tariffId, requestedAt);
            }
        }
    }
}
