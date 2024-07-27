using System.Text.Json.Serialization;

namespace BMSReporting.API.Models
{
	public class Booking
	{
		[Key]
		public int BookingId { get; set; } = default!;

		[ForeignKey("Client")]
		public int ClientId { get; set; } = default!;
		[JsonIgnore]
		public Client Client { get; set; }

		[ForeignKey("Branch")]
		public int BranchId { get; set; } = default!;
		[JsonIgnore]
		public Branch Branch { get; set; }

		public DateOnly BookingDate { get; set; } = default!;
		public TimeSpan BookingTime { get; set; } = default!;
		public string Status { get; set; } = default!;

		[JsonIgnore]
		public ICollection<BookingService> BookingServices { get; set; }
		[JsonIgnore]
		public ICollection<Transaction> Transactions { get; set; }
	}
}
