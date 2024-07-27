using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BMSReporting.API.Controllers
{
	/// <summary>
	/// Revenue Report
	/// </summary>
	[Route("api/Revenue")]
	[ApiController]
	public class RevenueReportController(ApplicationDbContext _context) : ControllerBase
	{
		/// <summary>
		///this Inlcude One Result with all 
		///{
		///TotalRevenue,
		///RevenueByService,
		///RevenueByBranch,
		///RevenueByPaymentMethod
		///}
		/// </summary>
		/// <param name="startDate">(format: YYYY-MM-DD).</param>
		/// <param name="endDate">(format: YYYY-MM-DD).</param>
		[HttpGet("Get")]
		public async Task<IActionResult> GetRevenueReport(DateTime? startDate = null, DateTime? endDate = null, string branchName = null, string serviceName = null, string paymentMethod = null)
		{
			var query = _context.Transactions.AsNoTracking().AsQueryable();

			if (startDate.HasValue)
			{
				var SDate = DateOnly.FromDateTime(startDate.Value);
				query = query.Where(t => t.PaymentDate >= SDate);
			}

			if (endDate.HasValue)
			{
				var EDate = DateOnly.FromDateTime(endDate.Value);
				query = query.Where(t => t.PaymentDate <= EDate);
			}

			//if (branchId.HasValue)
			//	query = query.Include(t => t.Booking).Where(t => t.Booking.BranchId == branchId);

			if (!string.IsNullOrEmpty(branchName))
				query = query.Include(bs => bs.Booking).ThenInclude(r => r.Branch).Where(b => b.Booking.Branch.Name.ToLower().Contains(branchName.ToLower()));


			if (!string.IsNullOrEmpty(paymentMethod))
				query = query.Where(t => t.PaymentMethod.ToLower().Contains(paymentMethod.ToLower()));

			//if (serviceId.HasValue) query = query.Include(bs => bs.Booking)
			//		.ThenInclude(bs => bs.BookingServices)
			//		.Where(bs => bs.Booking.BookingServices.Any(e => e.ServiceId == serviceId.Value));

			if (!string.IsNullOrEmpty(serviceName))
				query = query.Include(bs => bs.Booking).ThenInclude(bs => bs.BookingServices).ThenInclude(bs => bs.Service)
					   .Where(bs => bs.Booking.BookingServices.Any(e => e.Service.Name.ToLower().Contains(serviceName.ToLower())));


			var totalRevenue = await query.SumAsync(t => t.Amount);

			var revenueByService = await _context.BookingServices.Include(bs => bs.Service)
				.Where(r => !string.IsNullOrEmpty(serviceName) ? r.Service.Name.ToLower().Contains(serviceName.ToLower()) : true)
				//.Where(r => serviceId.HasValue ? r.ServiceId == serviceId : true)
				.Include(bs => bs.Service)
				.Where(bs => query.Any(t => t.BookingId == bs.BookingId))
				.GroupBy(bs => bs.Service.Name)
				.Select(g => new { Service = g.Key, Revenue = g.Sum(bs => bs.Price) })
				.ToListAsync();

			var revenueByBranch = await query
				.Include(t => t.Booking)
				.ThenInclude(b => b.Branch)
				.GroupBy(t => t.Booking.Branch.Name)
				.Select(g => new { Branch = g.Key, Revenue = g.Sum(t => t.Amount) })
				.ToListAsync();

			var revenueByPaymentMethod = await query
				.GroupBy(t => t.PaymentMethod)
				.Select(g => new { PaymentMethod = g.Key, Revenue = g.Sum(t => t.Amount) })
				.ToListAsync();

			return Ok(new
			{
				TotalRevenue = totalRevenue,
				RevenueByService = revenueByService,
				RevenueByBranch = revenueByBranch,
				RevenueByPaymentMethod = revenueByPaymentMethod
			});
		}
		#region Every EndPoint Alone

		///// <param name="startDate">(format: YYYY-MM-DD).</param>
		///// <param name="endDate">(format: YYYY-MM-DD).</param>
		//[HttpGet("total")]
		//public async Task<IActionResult> GetTotalRevenue(DateTime? startDate = null, DateTime? endDate = null)
		//{
		//	var query = _context.Transactions.AsQueryable();

		//	if (startDate.HasValue)
		//	{
		//		var SDate = DateOnly.FromDateTime(startDate.Value);
		//		query = query.Where(t => t.PaymentDate >= SDate);
		//	}

		//	if (endDate.HasValue)
		//	{
		//		var EDate = DateOnly.FromDateTime(endDate.Value);
		//		query = query.Where(t => t.PaymentDate <= EDate);
		//	}

		//	var totalRevenue = await query.SumAsync(t => t.Amount);

		//	return Ok(new { TotalRevenue = totalRevenue });
		//}

		///// <param name="startDate">(format: YYYY-MM-DD).</param>
		///// <param name="endDate">(format: YYYY-MM-DD).</param>
		//[HttpGet("byService")]
		//public async Task<IActionResult> GetRevenueByService(DateTime? startDate = null, DateTime? endDate = null, int? serviceId = null)
		//{
		//	var query = _context.BookingServices.AsNoTracking().Where(r => r.Booking.Transactions.Any()).AsQueryable();

		//	if (startDate.HasValue)
		//	{
		//		var SDate = DateOnly.FromDateTime(startDate.Value);
		//		query = query.Where(bs => bs.Booking.Transactions.Any(t => t.PaymentDate >= SDate));
		//	}

		//	if (endDate.HasValue)
		//	{
		//		var EDate = DateOnly.FromDateTime(endDate.Value);
		//		query = query.Where(bs => bs.Booking.Transactions.Any(t => t.PaymentDate <= EDate));
		//	}


		//	if (serviceId.HasValue) query = query.Where(bs => bs.ServiceId == serviceId.Value);

		//	var revenueByService = await query
		//		.GroupBy(bs => bs.Service.Name)
		//		.Select(g => new
		//		{
		//			ServiceName = g.Key,
		//			Revenue = g.Sum(bs => bs.Price)
		//		})
		//		.ToListAsync();

		//	return Ok(revenueByService);
		//}

		///// <param name="startDate">(format: YYYY-MM-DD).</param>
		///// <param name="endDate">(format: YYYY-MM-DD).</param>
		//[HttpGet("byBranch")]
		//public async Task<IActionResult> GetRevenueByBranch(DateTime? startDate = null, DateTime? endDate = null, int? branchId = null)
		//{
		//	var query = _context.Transactions.AsQueryable();

		//	if (startDate.HasValue)
		//	{
		//		var SDate = DateOnly.FromDateTime(startDate.Value);
		//		query = query.Where(t => t.PaymentDate >= SDate);
		//	}

		//	if (endDate.HasValue)
		//	{
		//		var EDate = DateOnly.FromDateTime(endDate.Value);
		//		query = query.Where(t => t.PaymentDate <= EDate);
		//	}

		//	if (branchId.HasValue)
		//		query = query.Where(bs => bs.Booking.BranchId == branchId.Value);

		//	var revenueByBranch = await query
		//		.GroupBy(t => t.Booking.Branch.Name)
		//		.Select(g => new
		//		{
		//			BranchName = g.Key,
		//			Revenue = g.Sum(t => t.Amount)
		//		})
		//		.ToListAsync();

		//	return Ok(revenueByBranch);
		//}

		///// <param name="startDate">(format: YYYY-MM-DD).</param>
		///// <param name="endDate">(format: YYYY-MM-DD).</param>
		//[HttpGet("byPaymentMethod")]
		//public async Task<IActionResult> GetRevenueByPaymentMethod(DateTime? startDate = null, DateTime? endDate = null, string paymentMethod = null)
		//{
		//	var query = _context.Transactions.AsQueryable();

		//	if (startDate.HasValue)
		//	{
		//		var SDate = DateOnly.FromDateTime(startDate.Value);
		//		query = query.Where(t => t.PaymentDate >= SDate);
		//	}

		//	if (endDate.HasValue)
		//	{
		//		var EDate = DateOnly.FromDateTime(endDate.Value);
		//		query = query.Where(t => t.PaymentDate <= EDate);
		//	}

		//	if (!string.IsNullOrEmpty(paymentMethod)) query = query.Where(t => t.PaymentMethod.ToLower().Contains(paymentMethod.ToLower()));

		//	var revenueByPaymentMethod = await query
		//		.GroupBy(t => t.PaymentMethod)
		//		.Select(g => new
		//		{
		//			PaymentMethod = g.Key,
		//			Revenue = g.Sum(t => t.Amount)
		//		})
		//		.ToListAsync();

		//	return Ok(revenueByPaymentMethod);
		//}

		#endregion
	}
}

