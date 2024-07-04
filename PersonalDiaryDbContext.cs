using Microsoft.EntityFrameworkCore;
using Personal_Diary.DB_Models;

namespace Personal_Diary
{
    public class PersonalDiaryDbContext : DbContext
    {
        public PersonalDiaryDbContext(DbContextOptions<PersonalDiaryDbContext> options) : base(options) { }
        public DbSet<TaskClass> Tasks { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
    }
}
