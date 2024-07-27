namespace BMSReporting.API.Data.Extensions
{
	public static class DatabaseExtentions
	{
		public static async Task InitaliseDatabaseAsync(this WebApplication app, IConfiguration configuration)
		{
			using var scope = app.Services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			Boolean.TryParse(configuration["InMemoryDataBase"], out bool IsInMemory);
			if (!IsInMemory)
				context.Database.MigrateAsync().GetAwaiter().GetResult();
			await SeedAsync(context, IsInMemory);
		}

		private static async Task SeedAsync(ApplicationDbContext context, bool IsInMemory)
		{
			if (!IsInMemory) context.Database.OpenConnection();
			await SeedClientAsync(context, IsInMemory);
			await SeedBranchAsync(context, IsInMemory);
			await SeedServiceAsync(context, IsInMemory);
			await SeedBookingAsync(context);
			if (!IsInMemory) context.Database.CloseConnection();
		}

		private static async Task SeedBookingAsync(ApplicationDbContext context)
		{
			if (!await context.Bookings.AsNoTracking().AnyAsync())
			{
				await context.Bookings.AddRangeAsync(InitialData.BookingsWithServicesAndTransaction);
				await context.SaveChangesAsync();
			}
		}
		private static async Task SeedBranchAsync(ApplicationDbContext context, bool IsInMemory)
		{
			if (!await context.Branchs.AsNoTracking().AnyAsync())
			{
				if (!IsInMemory)
					context.Database.ExecuteSql($"SET IDENTITY_INSERT dbo.Branchs ON");
				await context.Branchs.AddRangeAsync(InitialData.Branchs);
				await context.SaveChangesAsync();
				if (!IsInMemory)
					context.Database.ExecuteSql($"SET IDENTITY_INSERT dbo.Branchs OFF");
			}
		}

		private static async Task SeedServiceAsync(ApplicationDbContext context, bool IsInMemory)
		{
			if (!await context.Services.AsNoTracking().AnyAsync())
			{
				if (!IsInMemory)
					context.Database.ExecuteSql($"SET IDENTITY_INSERT dbo.Services ON");
				await context.Services.AddRangeAsync(InitialData.Services);
				await context.SaveChangesAsync();
				if (!IsInMemory)
					context.Database.ExecuteSql($"SET IDENTITY_INSERT dbo.Services OFF");
			}
		}

		private static async Task SeedClientAsync(ApplicationDbContext context, bool IsInMemory)
		{
			if (!await context.Clients.AsNoTracking().AnyAsync())
			{
				if (!IsInMemory)
					context.Database.ExecuteSql($"SET IDENTITY_INSERT [dbo].[Clients] ON");
				await context.Clients.AddRangeAsync(InitialData.Clients);
				await context.SaveChangesAsync();
				if (!IsInMemory)
					context.Database.ExecuteSql($"SET IDENTITY_INSERT dbo.Clients OFF");
			}
		}
	}
}
