using System;
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TariffSwitch.Tests.DomainModelTests
{
    [TestClass]
    public class RequestTests
    {
        [TestMethod]
        public void Decide_Rejected_WhenCustomerIsNull()
        {
            #region Arrange
            var request = new Request("R1", "C1", "T1", DateTimeOffset.Now);
            var tariff = new Tariff("T1", "Tariff1", false, 10m);
            #endregion

            #region Act
            PreliminaryDecision result = request.Decide(null, tariff);
            #endregion

            #region Assert
            Assert.AreEqual(DecisionKind.Rejected, result.Decision.Kind);
            Assert.AreEqual("Customer not found", result.Decision.Reason);
            #endregion
        }

        [TestMethod]
        public void Decide_Rejected_WhenTariffIsNull()
        {
            #region Arrange
            var request = new Request("R1", "C1", "T1", DateTimeOffset.Now);
            var customer = new Customer("C1", "Alice", false, SlaLevel.Standard, MeterType.Classic);
            #endregion

            #region Act
            PreliminaryDecision result = request.Decide(customer, null);
            #endregion

            #region Assert
            Assert.AreEqual(DecisionKind.Rejected, result.Decision.Kind);
            Assert.AreEqual("Tariff not found", result.Decision.Reason);
            #endregion
        }

        [TestMethod]
        public void Decide_Rejected_WhenCustomerHasUnpaidInvoices()
        {
            #region Arrange
            var request = new Request("R1", "C1", "T1", DateTimeOffset.Now);
            var customer = new Customer("C1", "Alice", true, SlaLevel.Standard, MeterType.Classic);
            var tariff = new Tariff("T1", "Tariff1", false, 15m);
            #endregion

            #region Act
            PreliminaryDecision result = request.Decide(customer, tariff);
            #endregion

            #region Assert
            Assert.AreEqual(DecisionKind.Rejected, result.Decision.Kind);
            Assert.AreEqual("Customer has unpaid invoices", result.Decision.Reason);
            #endregion
        }

        [TestMethod]
        public void Decide_ApprovedWithFollowUp_WhenSmartMeterRequired_AndCustomerHasClassic()
        {
            #region Arrange
            var request = new Request("R1", "C1", "T_SMART", DateTimeOffset.Now);
            var customer = new Customer("C1", "Alice", false, SlaLevel.Standard, MeterType.Classic);
            var tariff = new Tariff("T_SMART", "SmartTariff", true, 20m);
            #endregion

            #region Act
            PreliminaryDecision result = request.Decide(customer, tariff);
            #endregion

            #region Assert
            Assert.AreEqual(DecisionKind.Approved, result.Decision.Kind);
            Assert.AreEqual(1, result.FollowUps.Count);

            var f = result.FollowUps[0];
            Assert.AreEqual(FollowUpType.ScheduleMeterUpgrade, f.Type);
            Assert.AreEqual("R1", f.RequestId);
            Assert.IsFalse(f.DueAt.HasValue); // wird erst im UseCase gesetzt
            #endregion
        }

        [TestMethod]
        public void Decide_Approved_NoFollowUp_WhenNoSmartMeterRequired()
        {
            #region Arrange
            var request = new Request("R1", "C1", "T_REG", DateTimeOffset.Now);
            var customer = new Customer("C1", "Alice", false, SlaLevel.Standard, MeterType.Smart);
            var tariff = new Tariff("T_REG", "RegularTariff", false, 12m);
            #endregion

            #region Act
            PreliminaryDecision result = request.Decide(customer, tariff);
            #endregion

            #region Assert
            Assert.AreEqual(DecisionKind.Approved, result.Decision.Kind);
            Assert.AreEqual(0, result.FollowUps.Count);
            #endregion
        }
    }
}
