using Application.Ports;
using Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TariffSwitch.Tests.ApplicationTests
{
    internal class InMemoryLedger : IProcessedLedgerStore
    {
        private readonly System.Collections.Generic.HashSet<string> _set =
            new System.Collections.Generic.HashSet<string>(System.StringComparer.OrdinalIgnoreCase);

        public bool IsProcessed(string requestId)
        {
            return _set.Contains(requestId);
        }

        public void MarkProcessed(string requestId)
        {
            _set.Add(requestId);
        }
    }

    [TestClass]
    public class IdempotencyServiceTests
    {
        [TestMethod]
        public void ShouldProcess_True_ForNewId()
        {
            #region Arrange
            var ledger = new InMemoryLedger();
            var idem = new IdempotencyService(ledger);
            #endregion

            #region Act
            bool result = idem.ShouldProcess("R1");
            #endregion

            #region Assert
            Assert.IsTrue(result);
            #endregion
        }

        [TestMethod]
        public void ShouldProcess_False_ForAlreadyProcessedId()
        {
            #region Arrange
            var ledger = new InMemoryLedger();
            ledger.MarkProcessed("R1");
            var idem = new IdempotencyService(ledger);
            #endregion

            #region Act
            bool result = idem.ShouldProcess("R1");
            #endregion

            #region Assert
            Assert.IsFalse(result);
            #endregion
        }
    }
}
