namespace BMSReporting.API
{
	public static class DatabaseServiceCollection
	{
		public static IServiceCollection AddDBContextServices(this IServiceCollection services, IConfiguration configuration)
		{
			Boolean.TryParse(configuration["InMemoryDataBase"].ToString(), out bool result);
			if (!result)
				services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SQLDataBase")));
			else services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("BMSReporting"));

			return services;
		}
	}
}
