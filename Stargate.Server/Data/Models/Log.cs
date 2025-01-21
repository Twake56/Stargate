using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stargate.Server.Data.Models
{
    [Table("Log")]
    public class Log
    {
        public int Id { get; set; }

        public string LogLevel { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime Date { get; set; }

    }

    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
