using Microsoft.AspNetCore.Mvc;

namespace BMSReporting.API.Controllers
{

	/// <summary>
	///  Appointment Report
	/// </summary>
	[Route("api/Appointment")]
	[ApiController]
	public class AppointmentReportController(ApplicationDbContext _context) : ControllerBase
	{
		/// <summary>
		///this Inlcude One Result with all 
		///{
		///TotalAppointments,
		///AppointmentsByService,
		///AppointmentsByBranch,
		///AppointmentsByStatus
		///}
		/// </summary>
		/// <param name="startDate">(format: YYYY-MM-DD).</param>
		/// <param name="endDate">(format: YYYY-MM-DD).</param>
		[HttpGet("Get")]
		public async Task<IActionResult> GetAppointmentReport(DateTime? startDate = null, DateTime? endDate = null, string serviceName = null, string branchName = null, string status = null)
		{
			var query = _context.Bookings.AsNoTracking().AsQueryable();

			if (startDate.HasValue)
			{
				var SDate = DateOnly.FromDateTime(startDate.Value);
				query = query.Where(bs => bs.BookingDate >= SDate);
			}

			if (endDate.HasValue)
			{
				var EDate = DateOnly.FromDateTime(endDate.Value);
				query = query.Where(bs => bs.BookingDate <= EDate);
			}


			if (!string.IsNullOrEmpty(branchName))
				query = query.Include(bs => bs.Branch).Where(b => b.Branch.Name.ToLower().Contains(branchName.ToLower()));

			//if (branchId.HasValue)
			//	query = query.Where(b => b.BranchId == branchId);

			if (!string.IsNullOrEmpty(status))
				query = query.Where(b => b.Status.ToLower().Contains(status.ToLower()));



			if (!string.IsNullOrEmpty(serviceName))
				query = query.Include(bs => bs.BookingServices).ThenInclude(bs => bs.Service)
					   .Where(bs => bs.BookingServices.Any(e => e.Service.Name.ToLower().Contains(serviceName.ToLower())));

			//if (serviceId.HasValue) query = query
			//		.Include(bs => bs.BookingServices)
			//		.Where(bs => bs.BookingServices.Any(e => e.ServiceId == serviceId.Value));

			var totalAppointments = await query.CountAsync();

			var appointmentsByService = await _context.BookingServices.Include(bs => bs.Service)
				.Where(r => !string.IsNullOrEmpty(serviceName) ? r.Service.Name.ToLower().Contains(serviceName.ToLower()) : true)
				//.Where(r => serviceId.HasValue ? r.ServiceId == serviceId : true)
				.Include(bs => bs.Service)
				.Where(bs => query.Any(b => b.BookingId == bs.BookingId))
				.GroupBy(bs => new { bs.Service.Name, bs.ServiceId })
				.Select(g => new
				{
					Service = g.Key.Name,
					AppointmentsCount = g.Count(),
					Appointments = query.Include(c => c.Client).Where(b => b.BookingServices.Any(w => w.ServiceId == g.Key.ServiceId)).Select(r => new
					{
						BranchName = r.Branch.Name,
						ClientName = r.Client.FirstName + " " + r.Client.LastName,
						r.BookingDate,
						r.BookingTime,
						r.Status
					}).ToList(),
				})
				.ToListAsync();

			var appointmentsByBranch = await query
				.Include(b => b.Branch)
				.GroupBy(b => new { b.Branch.Name, b.BranchId })
				.Select(g => new
				{
					Branch = g.Key.Name,
					AppointmentsCount = g.Count(),
					Appointments = query.Include(c => c.Client).Where(b => b.BranchId == g.Key.BranchId).Select(r => new
					{
						BranchName = r.Branch.Name,
						ClientName = r.Client.FirstName + " " + r.Client.LastName,
						r.BookingDate,
						r.BookingTime,
						r.Status
					}).ToList(),
				})
				.ToListAsync();

			var appointmentsByStatus = await query
				.GroupBy(b => b.Status)
				.Select(g => new
				{
					Status = g.Key,
					AppointmentsCount = g.Count(),
					Appointments = query.Include(c => c.Client).Where(b => b.Status == g.Key).Select(r => new
					{
						BranchName = r.Branch.Name,
						ClientName = r.Client.FirstName + " " + r.Client.LastName,
						r.BookingDate,
						r.BookingTime,
						r.Status
					}).ToList(),
				})
				.ToListAsync();

			return Ok(new
			{
				TotalAppointments = totalAppointments,
				AppointmentsByService = appointmentsByService,
				AppointmentsByBranch = appointmentsByBranch,
				AppointmentsByStatus = appointmentsByStatus
			});
		}

		#region Every EndPoint Alone

		///// <summary>
		///// </summary>
		///// <param name="startDate">(format: YYYY-MM-DD).</param>
		///// <param name="endDate">(format: YYYY-MM-DD).</param>
		//[HttpGet("total")]
		//public async Task<IActionResult> GetTotalAppointments(DateTime? startDate = null, DateTime? endDate = null)
		//{
		//	var query = _context.Bookings.AsQueryable();

		//	if (startDate.HasValue)
		//	{
		//		var SDate = DateOnly.FromDateTime(startDate.Value);
		//		query = query.Where(bs => bs.BookingDate >= SDate);
		//	}

		//	if (endDate.HasValue)
		//	{
		//		var EDate = DateOnly.FromDateTime(endDate.Value);
		//		query = query.Where(bs => bs.BookingDate <= EDate);
		//	}

		//	return Ok(new { totalAppointments = await query.CountAsync() });
		//}


		///// <summary>
		///// </summary>
		///// <param name="startDate">(format: YYYY-MM-DD).</param>
		///// <param name="endDate">(format: YYYY-MM-DD).</param>
		//[HttpGet("byService")]
		//public async Task<IActionResult> GetAppointmentsByService(DateTime? startDate = null, DateTime? endDate = null, int? serviceId = null)
		//{
		//	var query = _context.BookingServices.AsQueryable();

		//	if (startDate.HasValue)
		//	{
		//		var SDate = DateOnly.FromDateTime(startDate.Value);
		//		query = query.Where(bs => bs.Booking.BookingDate >= SDate);
		//	}

		//	if (endDate.HasValue)
		//	{
		//		var EDate = DateOnly.FromDateTime(endDate.Value);
		//		query = query.Where(bs => bs.Booking.BookingDate <= EDate);
		//	}


		//	if (serviceId.HasValue) query = query.Where(bs => bs.ServiceId == serviceId.Value);

		//	var appointmentsByService = await query
		//		.GroupBy(bs => bs.Service.Name)
		//		.Select(g => new
		//		{
		//			ServiceName = g.Key,
		//			NumberOfAppointments = g.Count()
		//		})
		//		.ToListAsync();

		//	return Ok(appointmentsByService);
		//}

		///// <summary>
		///// </summary>
		///// <param name="startDate">(format: YYYY-MM-DD).</param>
		///// <param name="endDate">(format: YYYY-MM-DD).</param>
		//[HttpGet("byBranch")]
		//public async Task<IActionResult> GetAppointmentsByBranch(DateTime? startDate = null, DateTime? endDate = null, int? branchId = null)
		//{
		//	var query = _context.Bookings.AsQueryable();

		//	if (startDate.HasValue)
		//	{
		//		var SDate = DateOnly.FromDateTime(startDate.Value);
		//		query = query.Where(bs => bs.BookingDate >= SDate);
		//	}

		//	if (endDate.HasValue)
		//	{
		//		var EDate = DateOnly.FromDateTime(endDate.Value);
		//		query = query.Where(bs => bs.BookingDate <= EDate);
		//	}

		//	if (branchId.HasValue) query = query.Where(bs => bs.BranchId == branchId.Value);

		//	var appointmentsByBranch = await query
		//		.GroupBy(b => b.Branch.Name)
		//		.Select(g => new
		//		{
		//			BranchName = g.Key,
		//			NumberOfAppointments = g.Count()
		//		})
		//		.ToListAsync();

		//	return Ok(appointmentsByBranch);
		//}


		///// <summary>
		///// </summary>
		///// <param name="startDate">(format: YYYY-MM-DD).</param>
		///// <param name="endDate">(format: YYYY-MM-DD).</param>
		//[HttpGet("byStatus")]
		//public async Task<IActionResult> GetAppointmentsByStatus(DateTime? startDate = null, DateTime? endDate = null, string status = null)
		//{
		//	var query = _context.Bookings.AsQueryable();

		//	if (startDate.HasValue)
		//	{
		//		var SDate = DateOnly.FromDateTime(startDate.Value);
		//		query = query.Where(bs => bs.BookingDate >= SDate);
		//	}

		//	if (endDate.HasValue)
		//	{
		//		var EDate = DateOnly.FromDateTime(endDate.Value);
		//		query = query.Where(bs => bs.BookingDate <= EDate);
		//	}

		//	if (!string.IsNullOrEmpty(status)) query = query.Where(bs => bs.Status.ToLower().Contains(status.ToLower()));

		//	var appointmentsByStatus = await query
		//		.GroupBy(b => b.Status)
		//		.Select(g => new
		//		{
		//			Status = g.Key,
		//			NumberOfAppointments = g.Count()
		//		})
		//		.ToListAsync();

		//	return Ok(appointmentsByStatus);
		//}

		#endregion

	}
}
