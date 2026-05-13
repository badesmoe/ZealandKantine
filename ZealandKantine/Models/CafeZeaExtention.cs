using Microsoft.EntityFrameworkCore;

namespace ZealandKantine.Models
{
    public partial class CafeZea
    {
        public virtual DbSet<MenuDay> MenuDays { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuDay>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(d => d.WeekMenu)
                    .WithMany(w => w.MenuDays)
                    .HasForeignKey(d => d.WeekMenuId)
                    .HasConstraintName("FK_MenuDay_WeekMenu");
            });
        }
    }
}
