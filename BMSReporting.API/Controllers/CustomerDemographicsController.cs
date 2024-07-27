using BMSReporting.API.DTO;
using Microsoft.AspNetCore.Mvc;
namespace BMSReporting.API.Controllers
{

	/// <summary>
	/// Customer Demographics
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerDemographicsController(ApplicationDbContext _context) : ControllerBase
	{
		/// <summary>
		///this Inlcude One Result with all 
		///{
		///TotalCustomers,
		///AgeGroups,
		///GenderGroups,
		///BranchGroups,
		///PaymentMethodGroups 
		///}
		/// </summary>
		/// <param name="startDate">(format: YYYY-MM-DD).</param>
		/// <param name="endDate">(format: YYYY-MM-DD).</param>
		[HttpGet("Get")]
		public async Task<IActionResult> GetCustomerDemographicsReportData(DateTime? startDate = null, DateTime? endDate = null, string branchName = null, string serviceName = null, string gender = null, string paymentMethod = null)
		{
			var query = _context.Clients.AsNoTracking().Include(bs => bs.Bookings).ThenInclude(b => b.Branch).AsQueryable();

			if (startDate.HasValue)
			{
				var SDate = DateOnly.FromDateTime(startDate.Value);
				query = query.Where(c => c.Bookings.Any(b => b.BookingDate >= SDate));
			}

			if (endDate.HasValue)
			{
				var EDate = DateOnly.FromDateTime(endDate.Value);
				query = query.Where(c => c.Bookings.Any(b => b.BookingDate <= EDate));

			}

			if (!string.IsNullOrEmpty(gender)) query = query.Where(c => c.Gender.ToLower().Contains(gender.ToLower()));

			if (!string.IsNullOrEmpty(branchName)) query = query.Where(c => c.Bookings.Any(b => b.ClientId == c.ClientId && b.Branch.Name.ToLower().Contains(branchName.ToLower())));

			//if (branchId.HasValue) query = query.Where(c => c.Bookings.Any(b => b.ClientId == c.ClientId && b.BranchId == branchId.Value));

			if (!string.IsNullOrEmpty(serviceName))
				query = query.Include(e => e.Bookings).ThenInclude(e => e.BookingServices).ThenInclude(s => s.Service)
					.Where(c => c.Bookings.Any(e => e.BookingServices.Any(q => q.Service.Name.ToLower().Contains(serviceName.ToLower()))));

			//if (serviceId.HasValue)
			//	query = query.Include(e => e.Bookings).ThenInclude(e => e.BookingServices)
			//		.Where(c => c.Bookings.Any(e => e.BookingServices.Any(q => q.ServiceId == serviceId.Value)));

			if (!string.IsNullOrEmpty(paymentMethod))
			{
				query = query.Include(e => e.Bookings).ThenInclude(e => e.BookingServices)
					.Where(c =>
						   c.Bookings.Any(q =>
										  q.Transactions.Any(t => t.BookingId == q.BookingId && t.PaymentMethod.ToLower().Contains(paymentMethod.ToLower()))));
			}

			var totalCustomers = await query.CountAsync();

			var ageGroups = query.AsEnumerable()
				.Select(c => new { AgeGroup = GetAgeGroup(DateTime.Now.Year - c.Birthdate.Year) })
				.GroupBy(g => g.AgeGroup)
				.Select(g => new CustomerAgeGroup
				{
					AgeRange = g.Key,
					Count = g.Count()
				})
				.ToList();

			var genderGroups = await query
				.GroupBy(c => c.Gender)
				.Select(g => new CustomerGenderGroup
				{
					Gender = g.Key,
					Count = g.Count()
				})
				.ToListAsync();

			var branchGroups = await query
				.Join(_context.Bookings,
					client => client.ClientId,
					booking => booking.ClientId,
					(client, booking) => new { client, booking })
				.GroupBy(cb => new { cb.booking.BranchId, cb.booking.Branch.Name })
				.Select(g => new CustomerBranchGroup
				{
					BranchName = g.Key.Name,
					Count = g.Count()
				})
				.ToListAsync();

			var paymentMethodGroups = await query
				.Join(_context.Bookings,
					client => client.ClientId,
					booking => booking.ClientId,
					(client, booking) => new { client, booking })
				.Join(_context.Transactions,
					cb => cb.booking.BookingId,
					transaction => transaction.BookingId,
					(cb, transaction) => new { cb.client, transaction })
				.GroupBy(cbt => cbt.transaction.PaymentMethod)
				.Select(g => new CustomerPaymentMethodGroup
				{
					PaymentMethod = g.Key,
					Count = g.Count()
				})
				.ToListAsync();

			return Ok(new CustomerDemographicsReport
			{
				TotalCustomers = totalCustomers,
				AgeGroups = ageGroups,
				GenderGroups = genderGroups,
				BranchGroups = branchGroups,
				PaymentMethodGroups = paymentMethodGroups
			});

		}

		private string GetAgeGroup(int age)
		{
			if (age <= 17) return "0-17";
			if (age <= 25) return "18-25";
			if (age <= 35) return "26-35";
			if (age <= 45) return "36-45";
			if (age <= 60) return "46-60";
			return "60+";
		}

	}


}
