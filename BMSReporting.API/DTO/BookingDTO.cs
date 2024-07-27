namespace BMSReporting.API.DTO
{
	public class BookingDTO
	{
		public int BookingId { get; set; }
		public int ClientId { get; set; }
		public string ClientName { get; set; }
		public int BranchId { get; set; }
		public string BranchName { get; set; }
		public DateOnly BookingDate { get; set; }
		public TimeSpan BookingTime { get; set; }
		public string Status { get; set; }

	}
}
