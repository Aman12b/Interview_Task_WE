using System;
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TariffSwitch.Tests.DomainModelTests
{
    [TestClass]
    public class DecisionTests
    {
        [TestMethod]
        public void Approved_Factory_CreatesApprovedDecision()
        {
            #region Arrange
            DateTimeOffset due = new DateTimeOffset(2025, 1, 1, 8, 0, 0, TimeSpan.Zero);
            #endregion

            #region Act
            Decision d = Decision.Approved(due);
            #endregion

            #region Assert
            Assert.AreEqual(DecisionKind.Approved, d.Kind);
            Assert.IsTrue(d.IsApproved);
            Assert.AreEqual(string.Empty, d.Reason);
            Assert.IsTrue(d.SlaDueAt.HasValue);
            Assert.AreEqual(due, d.SlaDueAt.Value);
            #endregion
        }

        [TestMethod]
        public void Rejected_Factory_SetsReason_AndKindRejected()
        {
            #region Arrange
            string reason = "Some error";
            #endregion

            #region Act
            Decision d = Decision.Rejected(reason);
            #endregion

            #region Assert
            Assert.AreEqual(DecisionKind.Rejected, d.Kind);
            Assert.AreEqual(reason, d.Reason);
            Assert.IsFalse(d.SlaDueAt.HasValue);
            Assert.IsFalse(d.IsApproved);
            #endregion
        }
    }
}
