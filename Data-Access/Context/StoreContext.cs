﻿using Data_Access.Models;
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
        public DbSet<UserAchievements> UserAchievements { get; set; }
        public DbSet<UserGames> UserGames { get; set; }
        public DbSet<Friends> Friends { get; set; }
        public DbSet<BlockedUsers> BlockedUsers { get; set; }

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
            mb.Entity<UserAchievements>()
                .HasKey(ua => new { ua.UserId, ua.AchievementId });
            mb.Entity<UserGames>()
                .HasKey(ug => new { ug.UserId, ug.GameId });
            mb.Entity<Friends>()
                .HasKey(f => new { f.UserId, f.FriendId });
            mb.Entity<BlockedUsers>()
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
                .HasOne(g => g.RecommendedRequirement) //recommended requirement to game
                .WithOne(r => r.Game)
                .HasForeignKey<Game>(g => g.RecommendedId)
                .OnDelete(DeleteBehavior.Cascade);
            mb.Entity<Game>()
                .HasOne(g => g.MinimumRequirement) //minimum requirement to game
                .WithOne(r => r.Game)
                .HasForeignKey<Game>(g => g.MinimumId)
                .OnDelete(DeleteBehavior.Cascade);
            mb.Entity<Game>()
                .HasMany(g=>g.GameGalleries) //game galleries to game
                .WithOne(gg => gg.Game)
                .HasForeignKey(gg => gg.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            //User
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
            mb.Entity<Friends>()
                .HasOne(f => f.User) //user friends to user
                .WithMany(u => u.Friends)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            mb.Entity<Friends>()
                .HasOne(f => f.Friend) //friend to user
                .WithMany()
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.NoAction);

            //BlockedUsers
            mb.Entity<BlockedUsers>()
                .HasOne(b=>b.User) //user blocked users to user
                .WithMany(u => u.BlockedUsers)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            mb.Entity<BlockedUsers>()
                .HasOne(b => b.BlockedUser) //blocked user to users
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
