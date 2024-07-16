using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Personal_Diary.DB_Models;
using System.Text.Json;
using System.Threading.Tasks;

namespace Personal_Diary.Pages
{
    public class CalendarModel : PageModel
    {
        private readonly PersonalDiaryDbContext _context;

        public CalendarModel(PersonalDiaryDbContext context)
        {
            _context = context;
        }

        public string TasksJson { get; set; }
		public List<TaskType> TaskTypes { get; set; }

        public async Task OnGetAsync()
        {
            var tasks = await _context.Tasks
                .Include(t => t.TaskType)
                .ToListAsync();

			var calendarTasks = tasks.Select(t => new
            {
				id = t.Id,
				title = t.TaskTitle,
				start = t.StartDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
				end = t.EndDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
				color = t.TaskType.Color,
				description = t.Description,
				taskTypeId = t.TaskTypeId,
				repeatFrequency = t.Frequency,
				status = t.Status
			}).ToList();

            TasksJson = JsonSerializer.Serialize(calendarTasks);
			TaskTypes = await _context.TaskTypes.ToListAsync();
		}
		


		public async Task<IActionResult> OnPostCreateTaskAsync(string title, DateTime startDateTime, DateTime endDateTime, string description, int taskTypeId, int repeatFrequency)
		{
			var task = new TaskClass
			{
				TaskTitle = title,
				StartDateTime = startDateTime,
				EndDateTime = endDateTime,
				Description = description,
				TaskTypeId = taskTypeId,
				Frequency = (Frequency)repeatFrequency,
				Status = Task_Status.Pending
			};


			_context.Tasks.Add(task);
			await _context.SaveChangesAsync();

			return new JsonResult(new { success = true });
		}

		public async Task<IActionResult> OnPostUpdateTaskAsync(int id, string title, DateTime startDateTime, DateTime endDateTime, string description, int taskTypeId, int repeatFrequency)
		{
			var task = await _context.Tasks.FindAsync(id);
			if (task == null)
			{
				return NotFound();
			}

			task.TaskTitle = title;
			task.StartDateTime = startDateTime;
			task.EndDateTime = endDateTime;
			task.Description = description;
			task.TaskTypeId = taskTypeId;
			task.Frequency = (Frequency)repeatFrequency;


			await _context.SaveChangesAsync();

			return new JsonResult(new { success = true });
		}

		public async Task<IActionResult> OnPostDeleteTaskAsync(int id)
		{
			var task = await _context.Tasks.FindAsync(id);
			if (task == null)
			{
				return NotFound();
			}

			_context.Tasks.Remove(task);
			await _context.SaveChangesAsync();

			return new JsonResult(new { success = true });
		}

	}
}
