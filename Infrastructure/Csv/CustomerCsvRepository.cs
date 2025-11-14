using System;
using System.Collections.Generic;
using System.IO;
using DomainModel;
using Application.Ports;

namespace Infrastructure.Csv
{
    // Erwartetes Format: Id,Name,HasUnpaidInvoices,SlaLevel,MeterType
    public class CustomerCsvRepository : ICustomerRepository
    {
        private readonly Dictionary<string, Customer> _cache;

        public CustomerCsvRepository(string csvPath)
        {
            _cache = LoadCustomers(csvPath);
        }

        private Dictionary<string, Customer> LoadCustomers(string path)
        {
            var dict = new Dictionary<string, Customer>(StringComparer.OrdinalIgnoreCase);

            if (!File.Exists(path))
                throw new FileNotFoundException("customers.csv not found", path);

            string[] lines = File.ReadAllLines(path);
            if (lines.Length <= 1)
                return dict;

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] p = line.Split(CsvConfig.Delimiter);
                if (p.Length < 5) continue;

                string id = p[0].Trim();
                string name = p[1].Trim();
                bool hasUnpaid;
                if (!bool.TryParse(p[2].Trim(), out hasUnpaid))
                    continue;

                SlaLevel sla;
                if (!Enum.TryParse<SlaLevel>(p[3].Trim(), true, out sla))
                    continue;

                MeterType meter;
                if (!Enum.TryParse<MeterType>(p[4].Trim(), true, out meter))
                    continue;

                Customer c = new Customer(id, name, hasUnpaid, sla, meter);
                if (!dict.ContainsKey(id))
                    dict.Add(id, c);
            }

            return dict;
        }

        public Customer GetById(string id)
        {
            Customer c;
            return _cache.TryGetValue(id, out c) ? c : null;
        }
    }
}
