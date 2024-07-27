namespace BMSReporting.API.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options) { }
		public DbSet<Branch> Branchs => Set<Branch>();
		public DbSet<Client> Clients => Set<Client>();
		public DbSet<Service> Services => Set<Service>();
		public DbSet<Booking> Bookings => Set<Booking>();
		public DbSet<BookingService> BookingServices => Set<BookingService>();
		public DbSet<Transaction> Transactions => Set<Transaction>();

		protected override void OnModelCreating(ModelBuilder builder)
		{
			//builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(builder);
		}
	}
}
