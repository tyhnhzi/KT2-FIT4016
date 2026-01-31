using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PickleballClub.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
        public DbSet<Models.Member> Members { get; set; }
        public DbSet<Models.Court> Courts { get; set; }
        public DbSet<Models.TransactionCategory> TransactionCategories { get; set; }
        public DbSet<Models.Transaction> Transactions { get; set; }
        public DbSet<Models.Challenge> Challenges { get; set; }
        public DbSet<Models.Participant> Participants { get; set; }
        public DbSet<Models.Match> Matches { get; set; }
        public DbSet<Models.News> News { get; set; }
        public DbSet<Models.Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply Prefix 248_ to all business tables
            builder.Entity<Models.Member>().ToTable("248_Members");
            builder.Entity<Models.Court>().ToTable("248_Courts");
            builder.Entity<Models.TransactionCategory>().ToTable("248_TransactionCategories");
            builder.Entity<Models.Transaction>().ToTable("248_Transactions");
            builder.Entity<Models.Challenge>().ToTable("248_Challenges");
            builder.Entity<Models.Participant>().ToTable("248_Participants");
            builder.Entity<Models.Match>().ToTable("248_Matches");
            builder.Entity<Models.News>().ToTable("248_News");
            builder.Entity<Models.Booking>().ToTable("248_Bookings");

            // Configure decimal precision for Challenge entity
            builder.Entity<Models.Challenge>()
                .Property(c => c.EntryFee)
                .HasPrecision(18, 2);

            builder.Entity<Models.Challenge>()
                .Property(c => c.PrizePool)
                .HasPrecision(18, 2);

            // Configure decimal precision for Participant entity
            builder.Entity<Models.Participant>()
                .Property(p => p.EntryFeeAmount)
                .HasPrecision(18, 2);

            // Configure decimal precision for Transaction entity
            builder.Entity<Models.Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            // Configure Refactored Match Model
            builder.Entity<Models.Match>()
                .HasOne(m => m.Team1_Player1)
                .WithMany()
                .HasForeignKey(m => m.Team1_Player1Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Models.Match>()
                .HasOne(m => m.Team1_Player2)
                .WithMany()
                .HasForeignKey(m => m.Team1_Player2Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Models.Match>()
                .HasOne(m => m.Team2_Player1)
                .WithMany()
                .HasForeignKey(m => m.Team2_Player1Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Models.Match>()
                .HasOne(m => m.Team2_Player2)
                .WithMany()
                .HasForeignKey(m => m.Team2_Player2Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
