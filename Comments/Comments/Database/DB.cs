namespace Comments.Database
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DB : DbContext
    {
        public DB()
            : base("name=DB")
        {
        }

        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<IpAddres> IpAddres { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasMany(e => e.Comment1)
                .WithOptional(e => e.Comment2)
                .HasForeignKey(e => e.Parent);
        }
    }
}
