using System.Text.Json.Serialization;

namespace BMSReporting.API.Models
{
	public class Client
	{
		[Key]
		public int ClientId { get; set; } = default!;
		public string FirstName { get; set; } = default!;
		public string LastName { get; set; } = default!;
		public string Gender { get; set; } = default!;
		public string Email { get; set; } = default!;
		public string Phone { get; set; } = default!;
		public string Address { get; set; } = default!;
		public string City { get; set; } = default!;
		public string Country { get; set; } = default!;
		public DateOnly Birthdate { get; set; } = default!;

		[JsonIgnore]
		public ICollection<Booking> Bookings { get; set; }
	}
}
