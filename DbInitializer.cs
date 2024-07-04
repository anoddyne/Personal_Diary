using Personal_Diary.DB_Models;

namespace Personal_Diary
{
	public static class DbInitializer
	{
		public static void Initialize(PersonalDiaryDbContext context)
		{
			context.Database.EnsureCreated();

			if (context.TaskTypes.Any())
			{
				return;
			}

			var taskTypes = new TaskType[]
			{
				new TaskType {Name = "Работа", Color = "#0000ff"},
				new TaskType {Name = "Учёба", Color = "#ffa500"},
				new TaskType {Name = "Личное", Color = "#008000"}
			};

			foreach (var t in taskTypes)
			{
				context.TaskTypes.Add(t);
			}

			context.SaveChanges();

			var tasks = new TaskClass[]
			{
				new TaskClass {
					TaskTitle = "Завершить проект",
					StartDateTime = DateTime.Now,
					EndDateTime = DateTime.Now.AddHours(2),
					TaskTypeId = 1,
					Frequency = Frequency.None,
					Status = Task_Status.Pending },
				new TaskClass {
					TaskTitle = "Пойти в зал",
					StartDateTime = DateTime.Now.AddHours(3),
					EndDateTime = DateTime.Now.AddHours(4),
					TaskTypeId = 3,
					Frequency = Frequency.Daily,
					Status = Task_Status.Pending }

			};

			foreach (var t in tasks)
			{
				context.Tasks.Add(t);
			}

			context.SaveChanges();
		}
	}
}
