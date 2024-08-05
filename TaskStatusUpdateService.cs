using Microsoft.EntityFrameworkCore;
using Personal_Diary;
using Personal_Diary.DB_Models;

public class TaskStatusUpdateService : BackgroundService
{
	private readonly IServiceProvider _services;
	private readonly ILogger<TaskStatusUpdateService> _logger;

	public TaskStatusUpdateService(IServiceProvider services, ILogger<TaskStatusUpdateService> logger)
	{
		_services = services;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			await UpdateTaskStatuses();
			await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
		}
	}
	/// <summary>
	/// Updates the status of tasks based on their end date and current time.
	/// </summary>
	/// <returns>A task representing the asynchronous operation.</returns>
	private async Task UpdateTaskStatuses()
	{
		// Create a new scope to ensure proper disposal of resources
		using (var scope = _services.CreateScope())
		{
			// Get the required service from the service provider
			var dbContext = scope.ServiceProvider.GetRequiredService<PersonalDiaryDbContext>();

			// Get the list of tasks that are not yet completed
			var tasks = await dbContext.Tasks
				.Where(t => t.Status != Task_Status.Completed)
				.ToListAsync();

			// Get the current time
			var now = DateTime.Now;

			// Update the status of each task based on its end date and current time
			foreach (var task in tasks)
			{
				if (task.EndDateTime < now && task.Status != Task_Status.Completed)
				{
					// Mark the task as failed if its end date has passed and it is not already completed
					task.Status = Task_Status.Failed;
					_logger.LogInformation($"Task {task.Id} marked as Failed");
				}
				else if (task.StartDateTime <= now && task.EndDateTime > now)
				{
					// Mark the task as pending if its start date has passed and its end date has not yet passed
					task.Status = Task_Status.Pending;
					_logger.LogInformation($"Task {task.Id} marked as Pending");
				}
			}

			// Save the changes to the database
			await dbContext.SaveChangesAsync();
		}
	}

}