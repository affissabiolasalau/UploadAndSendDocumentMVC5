using Microsoft.EntityFrameworkCore;
using MyDocuments.Models;

namespace MyDocuments.Models
{
    public class ConnectionStrings : DbContext
    {
        public ConnectionStrings(DbContextOptions<ConnectionStrings> options) : base(options)
        {

        }

        public DbSet<UsersModel> Users { get; set; }
        public DbSet<TransactionsModel> Transactions { get; set; }
        public DbSet<ErrorLogsModel> ErrorLogs { get; set; }
        public DbSet<FilesModel> Files { get; set; }
    }
}
