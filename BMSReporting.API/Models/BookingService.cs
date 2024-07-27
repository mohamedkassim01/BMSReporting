
using System.Text.Json.Serialization;

namespace BMSReporting.API.Models
{
	public class BookingService
	{
		[Key]
		public int BookingServiceId { get; set; } = default!;

		[ForeignKey("Booking")]
		public int BookingId { get; set; } = default!;
		[JsonIgnore]
		public Booking Booking { get; set; } = default!;

		[ForeignKey("Service")]
		public int ServiceId { get; set; } = default!;
		[JsonIgnore]
		public Service Service { get; set; } = default!;

		public decimal Price { get; set; } = default!;
	}
}
