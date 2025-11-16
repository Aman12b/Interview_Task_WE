using System;
using System.Collections.Generic;
using Application.Ports;
using Application.Services;
using Application.UseCases;
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TariffSwitch.Tests.ApplicationTests
{
    #region Fakes

    internal class FakeCustomerRepo : ICustomerRepository
    {
        private readonly Dictionary<string, Customer> _data;

        public FakeCustomerRepo(Dictionary<string, Customer> data)
        {
            _data = data;
        }

        public Customer GetById(string id)
        {
            Customer c;
            _data.TryGetValue(id, out c);
            return c;
        }
    }

    internal class FakeTariffRepo : ITariffRepository
    {
        private readonly Dictionary<string, Tariff> _data;

        public FakeTariffRepo(Dictionary<string, Tariff> data)
        {
            _data = data;
        }

        public Tariff GetById(string id)
        {
            Tariff t;
            _data.TryGetValue(id, out t);
            return t;
        }
    }

    internal class FakeRequestReader : IRequestReader
    {
        private readonly IList<Request> _requests;

        public FakeRequestReader(IList<Request> requests)
        {
            _requests = requests;
        }

        public IEnumerable<Request> ReadNew()
        {
            return _requests;
        }
    }

    internal class FakeLedgerStore : IProcessedLedgerStore
    {
        private readonly HashSet<string> _ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public bool IsProcessed(string requestId)
        {
            return _ids.Contains(requestId);
        }

        public void MarkProcessed(string requestId)
        {
            _ids.Add(requestId);
        }
    }

    internal class FakeFollowUpStore : IFollowUpActionStore
    {
        public readonly List<FollowUpAction> Stored = new List<FollowUpAction>();

        public void SaveMany(IEnumerable<FollowUpAction> actions)
        {
            Stored.AddRange(actions);
        }
    }

    internal class FakeOutput : IOutput
    {
        public int ResultCount = 0;
        public int LastTotal;
        public int LastApproved;
        public int LastRejected;
        public int LastSkipped;
        public int LastSkippedAlready;

        public void WriteResult(Request request, Decision decision, IEnumerable<FollowUpAction> followUps)
        {
            ResultCount++;
        }

        public void WriteSummary(int total, int approved, int rejected, int skipped, int skippedAlreadyProcessed)
        {
            LastTotal = total;
            LastApproved = approved;
            LastRejected = rejected;
            LastSkipped = skipped;
            LastSkippedAlready = skippedAlreadyProcessed;
        }
    }

    internal class FixedTimeProvider : ITimeProvider
    {
        private readonly DateTimeOffset _now;

        public FixedTimeProvider(DateTimeOffset now)
        {
            _now = now;
        }

        public DateTimeOffset Now()
        {
            return _now;
        }
    }

    #endregion

    [TestClass]
    public class ProcessTariffSwitchRequestsUseCaseTests
    {
        [TestMethod]
        public void Execute_ProcessesRequests_AndCreatesFollowUps()
        {
            #region Arrange
            var baseTime = new DateTimeOffset(2025, 1, 1, 8, 0, 0, TimeSpan.Zero);

            var customers = new Dictionary<string, Customer>
            {
                { "C1", new Customer("C1", "Alice", false, SlaLevel.Standard, MeterType.Classic) },
                { "C2", new Customer("C2", "Bob",   false, SlaLevel.Premium,  MeterType.Smart) }
            };

            var tariffs = new Dictionary<string, Tariff>
            {
                { "T_SMART", new Tariff("T_SMART", "SmartTariff",   true,  20m) },
                { "T_REG",   new Tariff("T_REG",   "RegularTariff", false, 10m) }
            };

            var requests = new List<Request>
            {
                new Request("R1", "C1", "T_SMART", baseTime),
                new Request("R2", "C2", "T_REG",   baseTime)
            };
            var slaOptions = new SlaOptions
            {
                StandardHours = 48,
                PremiumHours = 24,
                SmartMeterUpgradeExtraHours = 12
            };

            var customerRepo = new FakeCustomerRepo(customers);
            var tariffRepo = new FakeTariffRepo(tariffs);
            var requestReader = new FakeRequestReader(requests);
            var ledger = new FakeLedgerStore();
            var followUps = new FakeFollowUpStore();
            var timeProvider = new FixedTimeProvider(baseTime);
            var slaService = new SlaService(timeProvider, slaOptions);
            var idemService = new IdempotencyService(ledger);
            var output = new FakeOutput();

            var useCase = new ProcessTariffSwitchRequestsUseCase(
                customerRepo,
                tariffRepo,
                requestReader,
                ledger,
                followUps,
                output,
                slaService,
                idemService
            );
            #endregion

            #region Act
            useCase.Execute();
            #endregion

            #region Assert
            Assert.AreEqual(2, output.ResultCount);
            Assert.AreEqual(2, output.LastTotal);
            Assert.AreEqual(2, output.LastApproved);
            Assert.AreEqual(0, output.LastRejected);
            Assert.AreEqual(0, output.LastSkipped);
            Assert.AreEqual(0, output.LastSkippedAlready);

            Assert.AreEqual(1, followUps.Stored.Count);
            Assert.AreEqual("R1", followUps.Stored[0].RequestId);
            Assert.AreEqual(FollowUpType.ScheduleMeterUpgrade, followUps.Stored[0].Type);
            #endregion
        }
    }
}
