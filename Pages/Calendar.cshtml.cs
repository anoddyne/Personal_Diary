using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Personal_Diary.DB_Models;


namespace Personal_Diary.Pages
{
	public class CalendarModel : PageModel
	{
		private readonly PersonalDiaryDbContext _context;

		public CalendarModel(PersonalDiaryDbContext context)
		{
			_context = context;
		}

		public List<TaskType> TaskTypes { get; set; }

		public async Task OnGetAsync()
		{
			TaskTypes = await _context.TaskTypes.ToListAsync();
		}

		/// <summary>
		/// Retrieves all tasks from the database and converts them into a list of objects with properties for each task's details.
		/// If a task has a non-None frequency, it will also add additional events for each occurrence of that task in the next month.
		/// </summary>
		/// <returns>A JSON result containing the list of events.</returns>
		public async Task<IActionResult> OnGetTasksAsync()
		{
			// Retrieve all tasks from the database, including their associated task types
			var tasks = await _context.Tasks
				.Include(t => t.TaskType)
				.ToListAsync();

			// Create a list to store the events
			var events = new List<object>();

			// Iterate over each task
			foreach (var task in tasks)
			{
				// Add the task as an event
				events.Add(new
				{
					id = task.Id, // The ID of the task
					title = task.TaskTitle, // The title of the task
					start = task.StartDateTime, // The start date and time of the task
					end = task.EndDateTime, // The end date and time of the task
					description = task.Description, // The description of the task
					taskTypeId = task.TaskTypeId, // The ID of the task type
					color = task.TaskType.Color, // The color of the task type
					frequency = task.Frequency, // The frequency of the task
					status = GetTaskStatus(task, task.StartDateTime) // The status of the task
				});

				// If the task has a non-None frequency, add additional events for each occurrence in the next month
				if (task.Frequency != Frequency.None)
				{
					DateTime nextOccurrence = task.StartDateTime;
					while (nextOccurrence < DateTime.Now.AddMonths(1))
					{
						nextOccurrence = GetNextOccurrence(nextOccurrence, task.Frequency);
						if (nextOccurrence != DateTime.MinValue)
						{
							events.Add(new
							{
								id = task.Id, // The ID of the task
								title = task.TaskTitle, // The title of the task
								start = nextOccurrence, // The start date and time of the occurrence
								end = nextOccurrence.Add(task.EndDateTime - task.StartDateTime), // The end date and time of the occurrence
								description = task.Description, // The description of the task
								taskTypeId = task.TaskTypeId, // The ID of the task type
								color = task.TaskType.Color, // The color of the task type
								frequency = task.Frequency, // The frequency of the task
								status = GetTaskStatus(task, task.StartDateTime) // The status of the task
							});
						}
					}
				}
			}

			// Return the list of events as a JSON result
			return new JsonResult(events);
		}
		/// <summary>
		/// Returns the next occurrence of a given frequency after a specified date.
		/// </summary>
		/// <param name="current">The current date.</param>
		/// <param name="frequency">The frequency of the occurrence.</param>
		/// <returns>The next occurrence of the frequency after the current date, or DateTime.MinValue if the frequency is not supported.</returns>
		private DateTime GetNextOccurrence(DateTime current, Frequency frequency)
		{
			// Use pattern matching to determine the next occurrence based on the frequency
			return frequency switch
			{
				Frequency.Daily => current.AddDays(1), // Add one day to the current date
				Frequency.Weekly => current.AddDays(7), // Add seven days to the current date
				Frequency.Monthly => current.AddMonths(1), // Add one month to the current date
				Frequency.Yearly => current.AddYears(1), // Add one year to the current date
				_ => DateTime.MinValue // Return DateTime.MinValue if the frequency is not supported
			};
		}


		private string GetTaskStatus(TaskClass task, DateTime eventStart)
		{
			if (task.Status == Task_Status.Completed)
				return "Выполнено";

			if (eventStart > DateTime.Now)
				return "Ожидает выполнения";

			if (eventStart.Date < DateTime.Now.Date && task.Status != Task_Status.Completed)
				return "Провалено";

			return "Ожидает выполнения";
		}

		public async Task<IActionResult> OnPostCompleteTaskAsync([FromBody] TaskClass taskToComplete)
		{
			var task = await _context.Tasks.FindAsync(taskToComplete.Id);
			if (task == null)
			{
				return NotFound();
			}
			try
			{
				task.Status = Task_Status.Completed;
				await _context.SaveChangesAsync();
				return new JsonResult(new { success = true });
			}
			catch (Exception ex)
			{
				return new JsonResult(new { success = false, error = ex });
			}
		}


		public async Task<IActionResult> OnPostSaveTaskAsync([FromBody] TaskClass newTask)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var taskToSave = new TaskClass
			{
				TaskTitle = newTask.TaskTitle,
				StartDateTime = newTask.StartDateTime,
				EndDateTime = newTask.EndDateTime,
				Description = newTask.Description,
				TaskTypeId = newTask.TaskTypeId,
				Frequency = newTask.Frequency,
				Status = Task_Status.Pending
			};

			try
			{
				_context.Tasks.Add(taskToSave);
				await _context.SaveChangesAsync();
				return new JsonResult(new { success = true });
			}
			catch (Exception ex)
			{
				return BadRequest(new { success = false, error = ex.Message });
			}
		}

		public async Task<IActionResult> OnPostDeleteTaskAsync([FromBody] TaskClass taskToDelete)
		{
			var task = await _context.Tasks.FindAsync(taskToDelete.Id);
			if (task == null)
			{
				return NotFound();
			}

			try
			{
				_context.Tasks.Remove(task);
				await _context.SaveChangesAsync();
				return new JsonResult(new { success = true });
			}
			catch (Exception ex)
			{
				return BadRequest(new { success = false, error = ex.Message });
			}
		}

		public async Task<IActionResult> OnPostUpdateTaskAsync([FromBody] TaskClass updatedTask)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var existingTask = await _context.Tasks.FindAsync(updatedTask.Id);
			if (existingTask == null)
			{
				return NotFound();
			}

			existingTask.TaskTitle = updatedTask.TaskTitle;
			existingTask.StartDateTime = updatedTask.StartDateTime;
			existingTask.EndDateTime = updatedTask.EndDateTime;
			existingTask.Description = updatedTask.Description;
			existingTask.TaskTypeId = updatedTask.TaskTypeId;
			existingTask.Frequency = updatedTask.Frequency;
			existingTask.Status = updatedTask.Status;


			try
			{
				await _context.SaveChangesAsync();
				return new JsonResult(new { success = true });
			}
			catch (Exception ex)
			{
				return BadRequest(new { success = false, error = ex.Message });
			}
		}
	}
}