
using System.Text.Json.Serialization;

namespace BMSReporting.API.Models
{
	public class Branch
	{
		[Key]
		public int BranchId { get; set; } = default!;
		public string Name { get; set; } = default!;
		public string Address { get; set; } = default!;
		public string City { get; set; } = default!;
		public string Country { get; set; } = default!;
		public string Phone { get; set; } = default!;

		[JsonIgnore]
		public ICollection<Booking> Bookings { get; set; }
	}
}
