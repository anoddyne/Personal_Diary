using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Personal_Diary.DB_Models;

namespace Personal_Diary.Pages
{
    public class TaskTypeSettingsModel : PageModel
    {
		private readonly PersonalDiaryDbContext _context;

		public TaskTypeSettingsModel(PersonalDiaryDbContext context)
		{
			_context = context;
		}

		public IList<TaskType> TaskTypes { get; set; }

		public async void OnGetAsync()
		{
			TaskTypes = await _context.TaskTypes.ToListAsync();

		}

		public async Task<IActionResult> OnPostCreateTaskTypeAsync(string name, string color)
		{
			var taskType = new TaskType
			{
				Name = name,
				Color = color
			};

			_context.TaskTypes.Add(taskType);
			await _context.SaveChangesAsync();

			return new JsonResult(new { success = true });
		}

		public async Task<IActionResult> OnPostUpdateTaskTypeAsync(int id, string name, string color)
		{
			var taskType = await _context.TaskTypes.FindAsync(id);
			if (taskType == null)
			{
				return NotFound();
			}

			taskType.Name = name;
			taskType.Color = color;

			await _context.SaveChangesAsync();

			return new JsonResult(new { success = true });
		}



		public async Task<IActionResult> OnPostDeleteTaskTypeAsync(int id)
		{
			var taskType = await _context.TaskTypes.FindAsync(id);
			if (taskType == null)
			{
				return NotFound();
			}

			_context.TaskTypes.Remove(taskType);
			await _context.SaveChangesAsync();

			return new JsonResult(new { success = true });
		}
		public async Task<IActionResult> OnGetTaskTypeAsync(int id)
		{
			var taskType = await _context.TaskTypes.FindAsync(id);
			if (taskType == null)
			{
				return NotFound();
			}

			return new JsonResult(new
			{
				id = taskType.Id,
				name = taskType.Name,
				color = taskType.Color
			});
		}


	}
}
