using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Personal_Diary.DB_Models;

namespace Personal_Diary.Pages
{
	public class TaskTypeSettingsModel : PageModel
	{
		private readonly PersonalDiaryDbContext _context;
		private readonly ILogger<TaskTypeSettingsModel> _logger;

		public TaskTypeSettingsModel(PersonalDiaryDbContext context, ILogger<TaskTypeSettingsModel> logger)
		{
			_context = context;
			_logger = logger;
		}

		public IList<TaskType> TaskTypes { get; set; }

		public async void OnGetAsync()
		{
			TaskTypes = await _context.TaskTypes.ToListAsync();

		}

		public async Task<IActionResult> OnPostCreateTaskTypeAsync([FromBody] TaskType newTaskType)
		{
			try
			{
				_context.TaskTypes.Add(newTaskType);
				await _context.SaveChangesAsync();
				return new JsonResult(new { success = true });
			} catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating task type");
				return BadRequest(new { success = false, error = ex.Message });
			}
		}


		public async Task<IActionResult> OnPostUpdateTaskTypeAsync([FromBody] TaskType updatedTaskType)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var taskType = await _context.TaskTypes.FindAsync(updatedTaskType.Id);
			if (taskType == null)
			{
				return NotFound();
			}

			taskType.Name = updatedTaskType.Name;
			taskType.Color = updatedTaskType.Color;

			try
			{
				await _context.SaveChangesAsync();
				return new JsonResult(new { success = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating task type");
				return BadRequest(new { success = false, error = ex.Message });
			}
		}



		public async Task<IActionResult> OnPostDeleteTaskTypeAsync([FromBody] TaskType model)
		{
			var taskType = await _context.TaskTypes.FindAsync(model.Id);
			if (taskType == null)
			{
				return NotFound();
			}
			try
			{
				_context.TaskTypes.Remove(taskType);
				await _context.SaveChangesAsync();

				return new JsonResult(new { success = true });
			} catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting task type");
				return BadRequest(new { success = false, error = ex.Message });
			}
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
