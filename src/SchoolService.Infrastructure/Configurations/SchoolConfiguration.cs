using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolService.Domain.Entities;


namespace SchoolService.Infrastructure.Configurations
{
    public class SchoolConfiguration : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.ToTable("School");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.SchoolCode)
                .IsUnique();

            builder.Property(x => x.SchoolCode)
                .HasMaxLength(20)
                .HasColumnType("VARCHAR(20)")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .HasColumnType("NVARCHAR(100)")
                .IsRequired();

            builder.Property(x => x.Email)
                .HasMaxLength(150)
                .HasColumnType("VARCHAR(150)");

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(30)
                .HasColumnType("VARCHAR(30)");

            builder.Property(x => x.Address)
                .HasMaxLength(250)
                .HasColumnType("NVARCHAR(250)");

            builder.Property(x => x.City)
                .HasMaxLength(50)
                .HasColumnType("NVARCHAR(50)");

            builder.Property(x => x.Region)
                .HasMaxLength(50)
                .HasColumnType("NVARCHAR(50)");

            builder.Property(x => x.PostalCode)
                .HasMaxLength(10)
                .HasColumnType("VARCHAR(10)");

            builder.Property(x => x.Country)
                .HasMaxLength(50)
                .HasColumnType("NVARCHAR(50)");

            builder.Property(x => x.IsActive)
                .HasColumnType("BIT")
                .HasDefaultValue(true)
                .IsRequired();
        }
    }
}
