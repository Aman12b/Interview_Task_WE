using System;
using System.Collections.Generic;
using System.IO;
using DomainModel;
using Application.Ports;

namespace Infrastructure.Csv
{
    // Erwartetes Format: Id,Name,RequiresSmartMeter,PricePerUnit
    public class TariffCsvRepository : ITariffRepository
    {
        private readonly Dictionary<string, Tariff> _cache;

        public TariffCsvRepository(string csvPath)
        {
            _cache = LoadTariffs(csvPath);
        }

        private Dictionary<string, Tariff> LoadTariffs(string path)
        {
            var dict = new Dictionary<string, Tariff>(StringComparer.OrdinalIgnoreCase);

            if (!File.Exists(path))
                throw new FileNotFoundException("tariffs.csv not found", path);

            string[] lines = File.ReadAllLines(path);
            if (lines.Length <= 1)
                return dict;

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] p = line.Split(CsvConfig.Delimiter);
                if (p.Length < 4) continue;

                string id = p[0].Trim();
                string name = p[1].Trim();

                bool requires;
                if (!bool.TryParse(p[2].Trim(), out requires))
                    continue;

                decimal price;
                if (!decimal.TryParse(p[3].Trim(), out price))
                    continue;

                Tariff t = new Tariff(id, name, requires, price);
                if (!dict.ContainsKey(id))
                    dict.Add(id, t);
            }

            return dict;
        }

        public Tariff GetById(string id)
        {
            Tariff t;
            return _cache.TryGetValue(id, out t) ? t : null;
        }
    }
}
