using System.Text.Json.Serialization;

namespace BMSReporting.API.Models
{
	public class Transaction
	{
		[Key]
		public int TransactionId { get; set; } = default!;

		[ForeignKey("Booking")]
		public int BookingId { get; set; } = default!;
		[JsonIgnore]
		public Booking Booking { get; set; }

		public decimal Amount { get; set; } = default!;
		public string PaymentMethod { get; set; } = default!;
		public DateOnly PaymentDate { get; set; } = default!;
	}
}
