using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Messenger
{
    public partial class MessengerContext : DbContext
    {
        public MessengerContext()
        {
        }

        public MessengerContext(DbContextOptions<MessengerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<ChatMember> ChatMembers { get; set; } = null!;
        public virtual DbSet<ChatMessege> ChatMesseges { get; set; } = null!;
        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<ClientAvatar> ClientAvatars { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Messenger;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>(entity =>
            {
                entity.HasKey(e => e.IdChat)
                    .HasName("PK__Chat__8307BCB35D216CED");

                entity.ToTable("Chat");

                entity.Property(e => e.IdChat)
                    .ValueGeneratedNever()
                    .HasColumnName("idChat");

                entity.Property(e => e.Name)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Type)
                    .IsUnicode(false)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<ChatMember>(entity =>
            {
                entity.HasKey(e => new { e.IdChat, e.IdClient });

                entity.ToTable("ChatMember");

                entity.Property(e => e.IdChat).HasColumnName("idChat");

                entity.Property(e => e.IdClient).HasColumnName("idClient");

                entity.Property(e => e.Role)
                    .IsUnicode(false)
                    .HasColumnName("role");
            });

            modelBuilder.Entity<ChatMessege>(async entity =>
            {
                entity.ToTable("ChatMessege");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Body)
                    .IsUnicode(false)
                    .HasColumnName("body");

                entity.Property(e => e.Datetime)
                    .HasColumnType("datetime")
                    .HasColumnName("datetime");

                entity.Property(e => e.IdChat).HasColumnName("idChat");

                entity.Property(e => e.IdClient).HasColumnName("idClient");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Phone)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Teg)
                    .IsUnicode(false)
                    .HasColumnName("teg");
            });

            modelBuilder.Entity<ClientAvatar>(entity =>
            {
                entity.ToTable("ClientAvatar");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdClient).HasColumnName("idClient");

                entity.Property(e => e.Photo).HasColumnName("photo");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
