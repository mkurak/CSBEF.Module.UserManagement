using CSBEF.Core.Concretes;
using CSBEF.Core.Interfaces;
using CSBEF.Module.UserManagement.Poco;
using Microsoft.EntityFrameworkCore;

namespace CSBEF.Module.UserManagement
{
    public class ModulePocoBuilder : ICustomModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            switch (GlobalConfiguration.DbProvider)
            {
                case "mssql":
                    Build_MSSQL(modelBuilder);
                    break;

                case "postgresql":
                    Build_PostgreSQL(modelBuilder);
                    break;
            }
        }

        private void Build_MSSQL(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("UserManagement_Group");

                entity.Property(e => e.AddingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<GroupInRole>(entity =>
            {
                entity.ToTable("UserManagement_GroupInRole");

                entity.Property(e => e.AddingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupInRole)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserManagement_GroupInRole_UserManagement_Group");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.GroupInRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserManagement_GroupInRole_UserManagement_Role");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("UserManagement_Role");

                entity.Property(e => e.AddingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RoleName).IsRequired();

                entity.Property(e => e.RoleTitle).HasMaxLength(256);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("UserManagement_Token");

                entity.Property(e => e.AddingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Device)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.DeviceKey)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ExpiredDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TokenCode).IsRequired();

                entity.Property(e => e.UpdatingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Token)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserManagement_Token_UserManagement_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("UserManagement_User");

                entity.Property(e => e.AddingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.ProfileBgPic).HasMaxLength(256);

                entity.Property(e => e.ProfilePic).HasMaxLength(256);

                entity.Property(e => e.ProfileStatusMessage).HasMaxLength(512);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Surname).HasMaxLength(256);

                entity.Property(e => e.UpdatingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<UserInGroup>(entity =>
            {
                entity.ToTable("UserManagement_UserInGroup");

                entity.Property(e => e.AddingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.UserInGroup)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserManagement_UserInGroup_UserManagement_Group");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserInGroup)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserManagement_UserInGroup_UserManagement_User");
            });

            modelBuilder.Entity<UserInRole>(entity =>
            {
                entity.ToTable("UserManagement_UserInRole");

                entity.Property(e => e.AddingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserInRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserManagement_UserInRole_UserManagement_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserInRole)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserManagement_UserInRole_UserManagement_User");
            });

            modelBuilder.Entity<UserMessage>(entity =>
            {
                entity.ToTable("UserManagement_UserMessage");

                entity.HasIndex(e => e.FromUserId);

                entity.HasIndex(e => e.ToUserId);

                entity.Property(e => e.AddingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.UpdatingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ViewDate).HasColumnType("datetime");

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.UserMessageFromUser)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.UserMessageToUser)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }

        private void Build_PostgreSQL(ModelBuilder modelBuilder)
        {
        }
    }
}