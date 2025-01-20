using Microsoft.EntityFrameworkCore;
using Stargate.Server.Business.Commands;
using Stargate.Server.Business.Queries;
using Stargate.Server.Data.Models;

namespace Stargate.Tests
{
    [TestClass]
    public class AstronautDutyTests
    {
        [TestMethod]
        public async Task GetAstronautDutiesByName_NamePresent_AstronautReturned()
        {
            //Arrange
            var name = "John Doe";
            var startTime = DateTime.Now.AddYears(-1);
            var endTime = DateTime.Now.AddYears(1);
            var dbContext = StarGateTestContext.CreateInMemoryDatabase(startTime, endTime);

            GetAstronautDutiesByName query = new GetAstronautDutiesByName { Name = name };
            GetAstronautDutiesByNameHandler handler = new GetAstronautDutiesByNameHandler(dbContext);
            
            //Act 
            GetAstronautDutiesByNameResult result = await handler.Handle(query, CancellationToken.None);

            //Assert 
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.ResponseCode, 200);
            Assert.IsNotNull(result.Person);
            Assert.AreEqual(name, result.Person.Name);
            Assert.AreEqual(startTime, result.Person.CareerStartDate);
            Assert.AreEqual(endTime, result.Person.CareerEndDate);
            Assert.AreEqual("LT1", result.Person.CurrentRank);
            Assert.AreEqual("Commander", result.Person.CurrentDutyTitle);

            dbContext.Database.CloseConnection();
        }

        [TestMethod]
        public async Task GetAstronautDutiesByName_NameNotPresent_NoAstronautReturend()
        {
            //Arrange
            var name = "";
            var startTime = DateTime.Now.AddYears(-1);
            var endTime = DateTime.Now.AddYears(1);
            var dbContext = StarGateTestContext.CreateInMemoryDatabase(startTime, endTime);

            GetAstronautDutiesByName query = new GetAstronautDutiesByName { Name = name };
            GetAstronautDutiesByNameHandler handler = new GetAstronautDutiesByNameHandler(dbContext);

            //Act 
            GetAstronautDutiesByNameResult result = await handler.Handle(query, CancellationToken.None);

            //Assert 
            Assert.IsNotNull(result);
            Assert.IsNull(result.Person);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(200, result.ResponseCode);

            dbContext.Database.CloseConnection();
        }

        [TestMethod]
        public async Task CreateAstronautDuty_DutyUpdated_UpdatedDutyReturned()
        {
            // Arrange
            var name = "John Doe";
            var rank = "Rank Test";
            var dutyTitle = "Duty Title Test";
            var startTime = DateTime.Now.AddYears(-1);
            var endTime = DateTime.Now.AddYears(1);
            var dbContext = StarGateTestContext.CreateInMemoryDatabase(startTime, endTime);

            CreateAstronautDuty command = new CreateAstronautDuty
            {
                Name = name,
                Rank = rank,
                DutyTitle = dutyTitle,
                DutyStartDate = startTime,               
            };
            CreateAstronautDutyHandler handler = new CreateAstronautDutyHandler(dbContext);

            // Act
            CreateAstronautDutyResult result = await handler.Handle(command, CancellationToken.None);
            var updatedDuty = dbContext.AstronautDuties.OrderByDescending(x => x.Id).First();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(200, result.ResponseCode);
            Assert.IsNotNull(updatedDuty);
            Assert.IsNull(updatedDuty.DutyEndDate);
            Assert.AreEqual(startTime.Date, updatedDuty.DutyStartDate);
            Assert.AreEqual(rank, updatedDuty.Rank);
            Assert.AreEqual(dutyTitle, updatedDuty.DutyTitle);
        }

        [TestMethod]
        public async Task CreateAstronautDuty_NoDutyPresent_NewDutyReturned()
        {
            // Arrange
            var name = "Jane Doe";
            var rank = "Rank Test";
            var dutyTitle = "Duty Title Test";
            var startTime = DateTime.Now.AddYears(-1);
            var endTime = DateTime.Now.AddYears(1);
            var dbContext = StarGateTestContext.CreateInMemoryDatabase(startTime, endTime);
            dbContext.People.Add(new Person
            {
                Name = name,
            });
            dbContext.SaveChanges();
            CreateAstronautDuty command = new CreateAstronautDuty
            {
                Name = name,
                Rank = rank,
                DutyTitle = dutyTitle,
                DutyStartDate = startTime,
            };
            CreateAstronautDutyHandler handler = new CreateAstronautDutyHandler(dbContext);

            // Act
            CreateAstronautDutyResult result = await handler.Handle(command, CancellationToken.None);
            var updatedDuty = dbContext.AstronautDuties.OrderByDescending(x => x.Id).First();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(200, result.ResponseCode);
            Assert.IsNotNull(updatedDuty);
            Assert.IsNull(updatedDuty.DutyEndDate);
            Assert.AreEqual(startTime.Date, updatedDuty.DutyStartDate);
            Assert.AreEqual(rank, updatedDuty.Rank);
            Assert.AreEqual(dutyTitle, updatedDuty.DutyTitle);
        }
    }
}