using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stargate.Server.Data.Models;
using Stargate.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stargate.Tests
{
    public class StarGateTestContext
    {
        public static StargateContext CreateInMemoryDatabase()
            => CreateInMemoryDatabase(DateTime.MinValue, DateTime.MaxValue);

        public static StargateContext CreateInMemoryDatabase(DateTime startTime, DateTime endTime)
        {
            var options = new DbContextOptionsBuilder<StargateContext>()
                .UseSqlite("DataSource=:memory:")
                .LogTo(Console.WriteLine, LogLevel.Information)
                .Options;
            var dbContext = new StargateContext(options);
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();

            var person = dbContext.People.Add(new Person
            {
                Name = "John Doe",
            });
            dbContext.SaveChanges();

            dbContext.PersonAstronauts.Add(new PersonAstronaut
            {
                PersonId = person.Entity.Id,
                Name = "John Doe",
                CurrentRank = "LT1",
                CurrentDutyTitle = "Commander",
                CareerStartDate = startTime,
                CareerEndDate = endTime,

            });

            dbContext.AstronautDetails.Add(new AstronautDetail
            {
                PersonId = person.Entity.Id,
                CurrentDutyTitle = "Captain",
                CurrentRank = "LT1",
                CareerStartDate = startTime,
                CareerEndDate = endTime,
            });

            dbContext.AstronautDuties.Add(new AstronautDuty
            {
                PersonId = person.Entity.Id,
                DutyStartDate = startTime,
                DutyEndDate = endTime,
            });

            dbContext.SaveChanges();

            return dbContext;
        }

    }
}
