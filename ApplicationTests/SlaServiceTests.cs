// =======================================================
// File: ApplicationTests/SlaServiceTests.cs
// =======================================================
using System;
using Application.Ports;
using Application.Services;
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TariffSwitch.Tests.ApplicationTests
{
    [TestClass]
    public class SlaServiceTests
    {
        [TestMethod]
        public void CalculateDue_Standard_NoUpgrade_Adds48Hours()
        {
            #region Arrange
            var slaOptions = new SlaOptions
            {
                StandardHours = 48,
                PremiumHours = 24,
                SmartMeterUpgradeExtraHours = 12
            };

            var baseTime = new DateTimeOffset(2025, 1, 1, 8, 0, 0, TimeSpan.Zero);
            var slaService = new SlaService(slaOptions);

            var customer = new Customer("C1", "Alice", false, SlaLevel.Standard, MeterType.Smart);
            var request = new Request("R1", "C1", "T1", baseTime);
            #endregion

            #region Act
            DateTimeOffset due = slaService.CalculateDue(customer, request, false);
            #endregion

            #region Assert
            Assert.AreEqual(baseTime.AddHours(48), due);
            #endregion
        }

        [TestMethod]
        public void CalculateDue_Premium_NoUpgrade_Adds24Hours()
        {
            #region Arrange
            var slaOptions = new SlaOptions
            {
                StandardHours = 48,
                PremiumHours = 24,
                SmartMeterUpgradeExtraHours = 12
            };

            var baseTime = new DateTimeOffset(2025, 1, 1, 8, 0, 0, TimeSpan.Zero);
            var slaService = new SlaService(slaOptions);

            var customer = new Customer("C1", "Alice", false, SlaLevel.Premium, MeterType.Smart);
            var request = new Request("R1", "C1", "T1", baseTime);
            #endregion

            #region Act
            DateTimeOffset due = slaService.CalculateDue(customer, request, false);
            #endregion

            #region Assert
            Assert.AreEqual(baseTime.AddHours(24), due);
            #endregion
        }

        [TestMethod]
        public void CalculateDue_Standard_WithUpgrade_Adds48Plus12Hours()
        {
            #region Arrange
            var slaOptions = new SlaOptions
            {
                StandardHours = 48,
                PremiumHours = 24,
                SmartMeterUpgradeExtraHours = 12
            };

            var baseTime = new DateTimeOffset(2025, 1, 1, 8, 0, 0, TimeSpan.Zero);
            var slaService = new SlaService(slaOptions);

            var customer = new Customer("C1", "Alice", false, SlaLevel.Standard, MeterType.Classic);
            var request = new Request("R1", "C1", "T1", baseTime);
            #endregion

            #region Act
            DateTimeOffset due = slaService.CalculateDue(customer, request, true);
            #endregion

            #region Assert
            Assert.AreEqual(baseTime.AddHours(60), due); // 48 + 12
            #endregion
        }
    }
}
