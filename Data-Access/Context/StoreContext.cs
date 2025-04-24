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
        public DbSet<Friend> Friends { get; set; }
        public DbSet<BlockedUser> BlockedUser { get; set; }

        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }

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
            mb.Entity<Friend>()
                .HasKey(f => new { f.UserId, f.FriendId });
            mb.Entity<BlockedUser>()
                .HasKey(b => new { b.UserId, b.BlockedUserId });


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

            //Friends
            mb.Entity<Friend>()
                .HasOne(f => f.User) //user friends to user
                .WithMany(u => u.Friends)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            mb.Entity<Friend>()
                .HasOne(f => f.FriendUser) //friend to user
                .WithMany()
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.NoAction);

            //BlockedUsers
            mb.Entity<BlockedUser>()
                .HasOne(b=>b.User) //user blocked users to user
                .WithMany(u => u.BlockedUsers)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            mb.Entity<BlockedUser>()
                .HasOne(b => b.Blocked) //blocked user to users
                .WithMany()
                .HasForeignKey(b => b.BlockedUserId)
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
