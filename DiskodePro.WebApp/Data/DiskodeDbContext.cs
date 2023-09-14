using DiskodePro.WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiskodePro.WebApp.Data;

public class DiskodeDbContext : DbContext
{
    public DiskodeDbContext(DbContextOptions<DiskodeDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<PostTag> PostTags { get; set; } = null!;
    public DbSet<UserLikedPosts> UserLikedPosts { get; set; } = null!;
    public DbSet<UserLikedComments> UserLikedComments { get; set; } = null!;
    public DbSet<UsersFollowUsers> UsersFollowUsers { get; set; } = null!;
    public DbSet<UserSavedPosts> UserSavedPosts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiskodeDbContext).Assembly);
    }
}

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Specify primary key 
        builder
            .HasKey(user => user.UserId);

        // Email should be unique
        builder
            .HasAlternateKey(user => user.Email);

        builder.Property(user => user.Name)
            .HasMaxLength(50)
            .HasColumnType("varchar");

        builder.Property(user => user.Email)
            .HasMaxLength(50)
            .HasColumnType("varchar");

        // Specify relationships

        builder
            .HasMany<Post>(user => user.CreatedPosts)
            .WithOne(post => post.Creator)
            .HasForeignKey(post => post.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany<UserLikedPosts>(user => user.LikedPosts)
            .WithOne(ulp => ulp.User)
            .HasForeignKey(ulp => ulp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany<Comment>(user => user.CreatedComments)
            .WithOne(comment => comment.Creator)
            .HasForeignKey(comment => comment.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany<UserLikedComments>(user => user.LikedComments)
            .WithOne(ulc => ulc.User)
            .HasForeignKey(ulc => ulc.UserId)
            .OnDelete(DeleteBehavior.Cascade);


        builder
            .HasMany<UserSavedPosts>(user => user.SavedPosts)
            .WithOne(ufp => ufp.User)
            .HasForeignKey(ufp => ufp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany<UsersFollowUsers>(user => user.Followers)
            .WithOne(ufu => ufu.Followee)
            .HasForeignKey(ufu => ufu.FolloweeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany<UsersFollowUsers>(user => user.Following)
            .WithOne(ufu => ufu.Follower)
            .HasForeignKey(ufu => ufu.FollowerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        // Specify primary key 
        builder
            .HasKey(post => post.PostId);

        // Specify Relationships

        builder
            .HasOne<User>(post => post.Creator)
            .WithMany(user => user.CreatedPosts)
            .HasForeignKey(post => post.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany<Comment>(post => post.Comments)
            .WithOne(comment => comment.Post)
            .HasForeignKey(comment => comment.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany<PostTag>(post => post.Tags)
            .WithOne(pt => pt.Post)
            .HasForeignKey(pt => pt.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany<UserLikedPosts>(post => post.LikedByUsers)
            .WithOne(ulp => ulp.Post)
            .HasForeignKey(ulp => ulp.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        // Specify primary key 
        builder
            .HasKey(comment => comment.CommentId);

        // Specify Relationships

        builder
            .HasOne<Post>(comment => comment.Post)
            .WithMany(post => post.Comments)
            .HasForeignKey(comment => comment.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<User>(comment => comment.Creator)
            .WithMany(user => user.CreatedComments)
            .HasForeignKey(comment => comment.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<Comment>(comment => comment.ParentComment)
            .WithMany(pc => pc.Replies)
            .HasForeignKey(comment => comment.ParentCommentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany<Comment>(comment => comment.Replies)
            .WithOne(reply => reply.ParentComment)
            .HasForeignKey(reply => reply.ParentCommentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany<UserLikedComments>(comment => comment.LikedByUsers)
            .WithOne(ulc => ulc.Comment)
            .HasForeignKey(ulc => ulc.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class UserLikedPostsEntityTypeConfiguration : IEntityTypeConfiguration<UserLikedPosts>
{
    public void Configure(EntityTypeBuilder<UserLikedPosts> builder)
    {
        // Composite Primary Key
        builder
            .HasKey(x => new { x.UserId, x.PostId });

        // Specify Relationships

        builder
            .HasOne<User>(ulp => ulp.User)
            .WithMany(user => user.LikedPosts)
            .HasForeignKey(ulp => ulp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Post>(ulp => ulp.Post)
            .WithMany(post => post.LikedByUsers)
            .HasForeignKey(ulp => ulp.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class UserLikedCommentsEntityTypeConfiguration : IEntityTypeConfiguration<UserLikedComments>
{
    public void Configure(EntityTypeBuilder<UserLikedComments> builder)
    {
        // Composite Primary Key
        builder
            .HasKey(x => new { x.UserId, x.CommentId });

        // Configure relationships
        builder
            .HasOne<User>(ulc => ulc.User)
            .WithMany(user => user.LikedComments)
            .HasForeignKey(ulc => ulc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Comment>(ulc => ulc.Comment)
            .WithMany(comment => comment.LikedByUsers)
            .HasForeignKey(ulc => ulc.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class UsersFollowUsersEntityTypeConfiguration : IEntityTypeConfiguration<UsersFollowUsers>
{
    public void Configure(EntityTypeBuilder<UsersFollowUsers> builder)
    {
        // Composite Primary Key
        builder
            .HasKey(x => new { x.FollowerId, x.FolloweeId });

        // Configure relationships
        builder
            .HasOne<User>(ufu => ufu.Follower)
            .WithMany(user => user.Following)
            .HasForeignKey(ufu => ufu.FollowerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<User>(ufu => ufu.Followee)
            .WithMany(u => u.Followers)
            .HasForeignKey(ufu => ufu.FolloweeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class UserFavoritePostsEntityTypeConfiguration : IEntityTypeConfiguration<UserSavedPosts>
{
    public void Configure(EntityTypeBuilder<UserSavedPosts> builder)
    {
        // Composite Primary Key
        builder
            .HasKey(x => new { x.UserId, x.PostId });

        // Configure relationships
        builder
            .HasOne<User>(ufp => ufp.User)
            .WithMany(u => u.SavedPosts)
            .HasForeignKey(ufp => ufp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class PostTagEntityTypeConfiguration : IEntityTypeConfiguration<PostTag>
{
    public void Configure(EntityTypeBuilder<PostTag> builder)
    {
        // Composite Primary Key
        builder
            .HasKey(x => new { x.PostId, x.TagName });

        // Configure relationships

        builder
            .HasOne<Post>(pt => pt.Post)
            .WithMany(post => post.Tags)
            .HasForeignKey(pt => pt.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}