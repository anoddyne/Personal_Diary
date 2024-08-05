using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Personal_Diary.DB_Models;

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



		/// <summary>
		/// Retrieves the weekly activity data for the past four weeks.
		/// </summary>
		/// <returns>A list of WeeklyActivityData objects representing the activity data for each week.</returns>
		public async Task OnGetAsync()
		{
			// Get the current date and calculate the date four weeks ago
			var currentDate = DateTime.Now.Date;
			var fourWeeksAgo = currentDate.AddDays(-28);

			// Retrieve all completed tasks that occurred in the past four weeks
			var completedTasks = await _context.Tasks
				.Where(t => t.Status == Task_Status.Completed && t.EndDateTime >= fourWeeksAgo)
				.Include(t => t.TaskType)
				.ToListAsync();

			// Initialize the list to store the weekly activity data
			WeeklyActivityData = new List<WeeklyActivityData>();

			// Iterate over the past four weeks
			for (int i = 0; i < 4; i++)
			{
				// Calculate the start and end dates of the current week
				var weekStart = currentDate.AddDays(-7 * i);
				var weekEnd = weekStart.AddDays(7);

				// Calculate the activity data for the current week
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

				// Add the weekly activity data to the list
				WeeklyActivityData.Add(weeklyData);
			}

			// Reverse the order of the weekly activity data
			WeeklyActivityData.Reverse();
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

