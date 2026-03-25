using Eghatha.Domain.Disasters;
using Eghatha.Domain.Volunteers;
using Eghatha.Domain.Volunteers.Equipments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Data.Configurations
{
    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            builder.ToTable("Equipments");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(e => e.Category)
                  .HasConversion(
                  v => v.Value,
                  v => EquipmentCategory.FromValue(v)).IsRequired();

            builder.Property(e => e.Quantity)
                .IsRequired();


            builder.Property(e => e.Status)
                .HasConversion(
                v => v.Value,
                v => EquipmentStatus.FromValue(v)).IsRequired();
           
            
            // Relationships
            builder.HasOne<Volunteer>()
                .WithMany(v => v.Equipments)
                .HasForeignKey("VolunteerId")
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            
            builder.HasIndex("VolunteerId");
        }
    }
}
