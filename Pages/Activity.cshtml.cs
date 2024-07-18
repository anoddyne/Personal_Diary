using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Personal_Diary.DB_Models;
using System.Text.Json;

namespace Personal_Diary.Pages
{
	public class ActivityModel : PageModel
	{
		private readonly PersonalDiaryDbContext _context;
		public ActivityModel(PersonalDiaryDbContext context)
		{
			_context = context;
		}

		public List<WeeklyActivityData> WeeklyActivityData { get; set; }

		public async Task OnGetAsync()
		{
			var currentDate = DateTime.Now.Date;
			var fourWeeksAgo = currentDate.AddDays(-28);

			var completedTasks = await _context.Tasks
				.Where(t => t.Status == Task_Status.Completed && t.EndDateTime >= fourWeeksAgo)
				.Include(t => t.TaskType)
				.ToListAsync();

			WeeklyActivityData = new List<WeeklyActivityData>();

			for (int i = 0; i < 4; i++)
			{
				var weekStart = currentDate.AddDays(-7 * i);
				var weekEnd = weekStart.AddDays(7);

				var weeklyData = new WeeklyActivityData
				{
					WeekStart = weekStart,
					ActivityByType = completedTasks
						.Where(t => t.EndDateTime >= weekStart && t.EndDateTime < weekEnd)
						.GroupBy(t => t.TaskType)
						.Select(g => new ActivityByType
						{
							TypeName = g.Key.Name,
							Color = g.Key.Color,
							Hours = g.Sum(t => (t.EndDateTime - t.StartDateTime).TotalHours)
						})
						.ToList()
				};

				WeeklyActivityData.Add(weeklyData);
			}

			WeeklyActivityData.Reverse(); // Чтобы текущая неделя была последней
		}
	}

	public class WeeklyActivityData
	{
		public DateTime WeekStart { get; set; }
		public List<ActivityByType> ActivityByType { get; set; }
	}

	public class ActivityByType
	{
		public string TypeName { get; set; }
		public string Color { get; set; }
		public double Hours { get; set; }
	}

}

