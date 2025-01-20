using Microsoft.EntityFrameworkCore;
using Stargate.Server.Business.Commands;
using Stargate.Server.Business.Queries;
using Stargate.Server.Data.Models;
namespace Stargate.Tests
{
    [TestClass]
    public class PersonTests
    {
        [TestMethod]
        public async Task GetPersonByName_NamePresent_PersonReturned()
        {
            //Arrange
            var name = "John Doe";
            var startTime = DateTime.Now.AddYears(-1);
            var endTime = DateTime.Now.AddYears(1);
            var dbContext = StarGateTestContext.CreateInMemoryDatabase(startTime, endTime);

            GetPersonByName query = new GetPersonByName { Name = name };
            GetPersonByNameHandler handler = new GetPersonByNameHandler(dbContext);
            
            //Act 
            GetPersonByNameResult result = await handler.Handle(query, CancellationToken.None);

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
        public async Task GetPersonByName_PersonNotPresent_NoPersonReturend()
        {
            //Arrange
            var name = "";
            var startTime = DateTime.Now.AddYears(-1);
            var endTime = DateTime.Now.AddYears(1);
            var dbContext = StarGateTestContext.CreateInMemoryDatabase(startTime, endTime);

            GetPersonByName query = new GetPersonByName { Name= name };
            GetPersonByNameHandler handler = new GetPersonByNameHandler(dbContext);

            //Act 
            GetPersonByNameResult result = await handler.Handle(query, CancellationToken.None);

            //Assert 
            Assert.IsNotNull(result);
            Assert.IsNull(result.Person);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(200, result.ResponseCode);

            dbContext.Database.CloseConnection();
        }

        [TestMethod]
        public async Task CreatePerson_PersonAdded_NewPersonReturned()
        {
            // Arrange
            var name = "Jane Doe";
            var dbContext = StarGateTestContext.CreateInMemoryDatabase();

            CreatePerson command = new CreatePerson { Name = name };
            CreatePersonHandler handler = new CreatePersonHandler(dbContext);

            // Act
            CreatePersonResult result = await handler.Handle(command, CancellationToken.None);
            var addedPerson = dbContext.People.OrderByDescending(x => x.Id).First();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(200, result.ResponseCode);
            Assert.IsNotNull(addedPerson);
            Assert.AreEqual(name, addedPerson.Name);
        }

        [TestMethod]
        public async Task GetPeople_PeoplePresent_PeopleReturned()
        {
            // Arrange
            var startTime = DateTime.Now.AddYears(-1);
            var endTime = DateTime.Now.AddYears(1);
            var dbContext = StarGateTestContext.CreateInMemoryDatabase(startTime, endTime);
            dbContext.SaveChanges();
            GetPeople query = new GetPeople();
            GetPeopleHandler handler = new GetPeopleHandler(dbContext);

            // Act
            GetPeopleResult result = await handler.Handle(query, CancellationToken.None);
            var returnedPerson = result.People.First();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(200, result.ResponseCode);
            Assert.IsNotNull(returnedPerson);
            Assert.AreEqual(endTime, returnedPerson.CareerEndDate);
            Assert.AreEqual(startTime, returnedPerson.CareerStartDate);
            Assert.AreEqual("LT1", returnedPerson.CurrentRank);
            Assert.AreEqual("Commander", returnedPerson.CurrentDutyTitle);
        }
    }
}