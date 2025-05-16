using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Context
{
    public class StoreContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<GameGallery> GameGalleries { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<RecommendedRequirement> RecommendedRequirements { get; set; }
        public DbSet<MinimumRequirement> MinimumRequirements { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<Relationship> Relationships { get; set; } 

        public StoreContext(DbContextOptions<StoreContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            // Configuring the primary keys 
            mb.Entity<User>()
                .HasKey(u => u.Id);
            mb.Entity<Game>()
                .HasKey(g => g.Id);
            mb.Entity<Review>()
                .HasKey(r => r.Id);
            mb.Entity<Achievement>()
                .HasKey(a => a.Id);
            mb.Entity<GameGallery>()
                .HasKey(g => g.Id);
            mb.Entity<News>()
                .HasKey(n => n.Id);
            mb.Entity<RecommendedRequirement>()
                .HasKey(r => r.Id);
            mb.Entity<MinimumRequirement>()
                .HasKey(r => r.Id);
            mb.Entity<Wishlist>()
                 .HasKey(w => new { w.UserId,w.GameId });
            mb.Entity<UserAchievement>()
                .HasKey(ua => new { ua.UserId, ua.AchievementId });
            mb.Entity<UserGame>()
                .HasKey(ug => new { ug.UserId, ug.GameId });
            mb.Entity<Relationship>()
                .HasKey(r => new { r.UserId, r.FriendId });

            // Configuring the relationships

            // Game
            mb.Entity<Game>()
                .HasMany(g => g.Reviews) //game reviews to game
                .WithOne(r => r.Game)
                .HasForeignKey(r => r.GameId)
                .OnDelete(DeleteBehavior.Cascade);
            mb.Entity<Game>()
                .HasMany(g => g.Achievements) //game achievements to game
                .WithOne(a => a.Game)
                .HasForeignKey(a => a.GameId)
                .OnDelete(DeleteBehavior.Cascade);
            mb.Entity<Game>()
                .HasOne(g => g.RecommendedRequirement)
                .WithMany()
                .HasForeignKey(g => g.RecommendedRequirementId)
                .OnDelete(DeleteBehavior.SetNull);

            mb.Entity<Game>()
                .HasOne(g => g.MinimumRequirement)
                .WithMany()
                .HasForeignKey(g => g.MinimumRequirementId)
                .OnDelete(DeleteBehavior.SetNull);

            mb.Entity<Game>()
                .HasMany(g=>g.GameGalleries) //game galleries to game
                .WithOne(gg => gg.Game)
                .HasForeignKey(gg => gg.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<Game>()
                .Property(mb => mb.Price)
                .HasPrecision(18, 2);

            //User
            mb.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            mb.Entity<User>()
                .HasMany(u=>u.UserAchievements) //user achievements to user
                .WithOne(ua=>ua.User)
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            mb.Entity<User>()
                .HasMany(u=>u.UserGames) //user games to user
                .WithOne(ug => ug.User)
                .HasForeignKey(ug => ug.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            mb.Entity<User>()
                .HasMany(u=>u.Wishlist) //user wishlist to user
                .WithOne(w => w.User)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //Relationship
            mb.Entity<Relationship>()
                .ToTable("Relationships"); 
            mb.Entity<Relationship>()
                .HasOne(r => r.User) 
                .WithMany(u => u.Relationships)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            mb.Entity<Relationship>()
                .HasOne(r => r.FriendUser) 
                .WithMany()
                .HasForeignKey(r => r.FriendId)
                .OnDelete(DeleteBehavior.NoAction);


            //Achievement
            mb.Entity<Achievement>()
                .HasMany(a => a.UserAchievements) //user achievements to achievement
                .WithOne(ua => ua.Achievement)
                .HasForeignKey(ua => ua.AchievementId)
                .OnDelete(DeleteBehavior.Cascade);

            //Review
            mb.Entity<Review>()
                .HasOne(r => r.User) //user reviews to user
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
