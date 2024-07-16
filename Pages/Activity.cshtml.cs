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

		public string ActivityDataJson { get; set; }

		/*public async Task OnGetAsync()
		{
			var tasks = await _context.Tasks
				.Include(t => t.TaskType)
				.Where(t => t.Status == Task_Status.Completed)
				.ToListAsync();

			var groupedData = tasks
				.GroupBy(t => new { Week = EF.Functions.DatePart("week", t.StartDateTime), t.TaskType.Name })
				.Select(g => new
				{
					Week = g.Key.Week,
					TaskType = g.Key.Name,
					Hours = g.Sum(t => (t.EndDateTime - t.StartDateTime).TotalHours)
				})
				.ToList();

			ActivityDataJson = JsonSerializer.Serialize(groupedData);
		}*/

		public void OnGet()
        {
        }
    }
}
