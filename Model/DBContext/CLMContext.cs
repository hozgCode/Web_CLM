using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CLMContext : DbContext
    {
        public CLMContext()
            : base("CLMContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CLMContext, Configuration>());
            DbConfiguration.SetConfiguration(new EFConfiguration());
        }

        /// <summary>
        /// 用户
        /// </summary>
        public virtual DbSet<User> Users { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public virtual DbSet<Customer> Customers { get; set; }
        /// <summary>
        /// 注册信息
        /// </summary>
        public virtual DbSet<Registration> Registrations { get; set; }
        /// <summary>
        /// 注册回执信息
        /// </summary>
        public virtual DbSet<Respond> Responds { get; set; }
    }
}
