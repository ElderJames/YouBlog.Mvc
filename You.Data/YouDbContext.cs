using You.Models;
using System.Data.Entity;

namespace You.Data
{
    public class YouDbContext : DbContext
    {
        #region 内容
        public DbSet<Category> Categories { get; set; }
        public DbSet<CommonModel> CommonModels { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        //public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
      
        public DbSet<Menu> Menus { get; set; }
        #endregion

     

        #region 用户
        public DbSet<User> User { get; set; }

        public DbSet<Message> Messages { get; set; }
        #endregion



        public YouDbContext()
            : base("name=YouConnection")
        {
            Database.CreateIfNotExists();
        }
    }
}
