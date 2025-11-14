using System;
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TariffSwitch.Tests.DomainModelTests
{
    [TestClass]
    public class FollowUpActionTests
    {
        [TestMethod]
        public void SetDue_SetsDueDate()
        {
            #region Arrange
            var f = FollowUpAction.Create(
                FollowUpAction.NewId(),
                FollowUpType.ScheduleMeterUpgrade,
                "R1",
                null);

            DateTimeOffset due = new DateTimeOffset(2025, 1, 1, 12, 0, 0, TimeSpan.Zero);
            #endregion

            #region Act
            f.SetDue(due);
            #endregion

            #region Assert
            Assert.IsTrue(f.DueAt.HasValue);
            Assert.AreEqual(due, f.DueAt.Value);
            #endregion
        }
    }
}
