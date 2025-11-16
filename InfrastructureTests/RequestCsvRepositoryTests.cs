using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Application.Ports;
using DomainModel;
using Infrastructure.Csv;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TariffSwitch.Tests.InfrastructureTests
{
    [TestClass]
    public class RequestCsvRepositoryTests
    {
        private string _tempFile;

        [TestInitialize]
        public void SetUp()
        {
            _tempFile = Path.GetTempFileName();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_tempFile))
                File.Delete(_tempFile);
        }

        [TestMethod]
        public void ReadNew_ReadsValidRequestsFromCsv()
        {
            #region Arrange
            var lines = new[]
            {
                "RequestId,CustomerId,TariffId,RequestedAt",
                "R1;C1;T1;2025-01-01T08:00:00",
                "R2;C2;T2;2025-01-02T09:30:00"
            };
            File.WriteAllLines(_tempFile, lines);

            var repo = new RequestCsvRepository(_tempFile);
            #endregion

            #region Act
            var list = new List<Request>(repo.ReadNew());
            #endregion

            #region Assert
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("R1", list[0].RequestId);
            Assert.AreEqual("C1", list[0].CustomerId);
            Assert.AreEqual("T1", list[0].TargetTariffId);

            Assert.AreEqual("R2", list[1].RequestId);
            Assert.AreEqual("C2", list[1].CustomerId);
            Assert.AreEqual("T2", list[1].TargetTariffId);
            #endregion
        }
    }
}
