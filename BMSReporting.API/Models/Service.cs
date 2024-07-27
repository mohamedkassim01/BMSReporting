using System.Text.Json.Serialization;

namespace BMSReporting.API.Models
{
	public class Service
	{
		[Key]
		public int ServiceId { get; set; } = default!;
		public string Name { get; set; } = default!;
		public string Description { get; set; } = default!;
		public decimal Price { get; set; } = default!;
		public int Duration { get; set; } = default!;

		[JsonIgnore]
		public ICollection<BookingService> BookingServices { get; set; }
	}
}
