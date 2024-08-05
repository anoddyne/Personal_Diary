namespace Personal_Diary.DB_Models
{
	public class TaskClass
	{
		public int Id { get; set; }
		public string TaskTitle { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public string? Description { get; set; }
		public int TaskTypeId { get; set; }
		public TaskType TaskType { get; set; }
		public Frequency Frequency { get; set; }
		public Task_Status Status { get; set; }

	}

	public enum Frequency
	{
		None,
		Daily,
		Weekly,
		Monthly,
		Yearly
	}

	public enum Task_Status
	{
		Completed,
		Pending,
		Failed
	}

}
