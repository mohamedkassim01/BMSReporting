using BMSReporting.API.Enums;

namespace BMSReporting.API.Data.Extensions
{
	internal class InitialData
	{
		public static IEnumerable<Client> Clients =>
				new List<Client>
				{
						new Client { ClientId = 1, FirstName = "Mohamed", LastName = "Ahmed", Gender = "Male", Email = "C1@repo.com", Phone = "01123455678", Address = "Hosary", City = "6 October", Country = "Egypt", Birthdate = DateOnly.Parse("01-27-1991") },
						new Client { ClientId = 2, FirstName = "Mustafa", LastName = "Yousif", Gender = "Male", Email = "C2@repo.com", Phone = "01223455222", Address = "23 street", City = "6 October", Country = "Egypt", Birthdate = DateOnly.Parse("02-25-1993") },
						new Client { ClientId = 3, FirstName = "Eman", LastName = "Osama", Gender = "Female", Email = "C3@repo.com", Phone = "01333455333", Address = "45 street", City = "6 October", Country = "Egypt", Birthdate = DateOnly.Parse("05-14-1980") },
						new Client { ClientId = 4, FirstName = "Mahmoud", LastName = "Emad", Gender = "Male", Email = "C4@repo.com", Phone = "01333455333", Address = "2 street", City = "6 October", Country = "Egypt", Birthdate = DateOnly.Parse("09-18-1985") },
						new Client { ClientId = 5, FirstName = "Fatima", LastName = "Al-Khatib", Gender = "Female", Email = "FA@repo.com", Phone = "01456745673", Address = "King Hussein Street", City = "Amman", Country = "Jordan", Birthdate = DateOnly.Parse("07-10-1988") },
				};
		public static IEnumerable<Branch> Branchs =>
			new List<Branch>
			{
					new Branch { BranchId = 100, Name = "Alex", Address = "Alexandria 45st", City = "Alexandria", Country = "EG", Phone = "03999000" },
					new Branch { BranchId = 101, Name = "Cairo", Address = "Cairo 20st", City = "Cairo", Country = "EG", Phone = "02222133" },
			};
		public static IEnumerable<Service> Services =>
			new List<Service>
			{
					new Service{ServiceId = 11,Name="Fly To KSA",Description="Fly To KSA",Price= 4000,Duration=1},
					new Service{ServiceId = 12,Name="Fly To UAE",Description="Fly To UAE",Price= 3030.80M,Duration=2},
					new Service{ServiceId = 13,Name="Fly To KW",Description="Fly To KW",Price= 3450,Duration=3},
			};
		public static IEnumerable<Booking> BookingsWithServicesAndTransaction
		{
			get
			{
				var bookingList = new List<Booking>();
				for (int i = 0; i < 50; i++)
				{
					Random rnd = new Random();
					var Sid = rnd.Next(11, 14);
					var Bid = rnd.Next(100, 102);
					var Cid = rnd.Next(1, 6);
					var service = Services.First(r => r.ServiceId == Sid);
					var branch = Branchs.First(r => r.BranchId == Bid);
					var client = Clients.First(r => r.ClientId == Cid);
					var BookingDate = DateOnly.Parse(rnd.Next(1, 13) + "-" + rnd.Next(1, 27) + "-2024");

					Booking booking = new Booking()
					{
						ClientId = client.ClientId,
						BranchId = branch.BranchId,
						BookingDate = DateOnly.Parse(rnd.Next(1, 13) + "-" + rnd.Next(1, 27) + "-2024"),
						BookingTime = TimeSpan.Parse(rnd.Next(1, 24) + ":" + rnd.Next(1, 50) + ":" + rnd.Next(1, 50)),
						Status = Enum.GetName(typeof(BookingStatus), rnd.Next(1, 4))
					};
					booking.BookingServices = new List<BookingService>() {
																			new BookingService
																			{
																				ServiceId = service.ServiceId,
																				Price = service.Price
																			},
																		};
					booking.Transactions = new List<Transaction>() {
													new Transaction
													{
														Amount = service.Price,
														PaymentMethod = Enum.GetName(typeof(PaymentMethod), rnd.Next(1, 4)),
														PaymentDate = BookingDate.AddDays(1)
													}
												};
					bookingList.Add(booking);
				}

				return bookingList;
			}
		}

	}
}

#region InitDataManual

//Booking booking1 = new Booking()
//{
//	ClientId = 1,
//	BranchId = 101,
//	BookingDate = DateOnly.Parse("01-14-2024"),
//	BookingTime = TimeSpan.Parse("13:33:22"),
//	Status = Enum.GetName(typeof(BookingStatus), 1)
//};
//booking1.BookingServices = new List<BookingService>() {
//														new BookingService
//														{
//															ServiceId = 11,
//															Price = 4000
//														}
//													};
//booking1.Transactions = new List<Transaction>() {
//								new Transaction
//								{
//									Amount = 4000,
//									PaymentMethod = "Cash",
//									PaymentDate = DateOnly.Parse("01-14-2024")
//								}
//							};


//Booking booking2 = new()
//{
//	ClientId = 2,
//	BranchId = 101,
//	BookingDate = DateOnly.Parse("01-15-2024"),
//	BookingTime = TimeSpan.Parse("15:33:22"),
//	Status = Enum.GetName(typeof(BookingStatus), 2)
//};
//booking2.BookingServices = new List<BookingService>() {
//														new BookingService
//														{
//															ServiceId = 12,
//															Price = 3030.80M
//														}
//													};
//booking2.Transactions = new List<Transaction>() {
//								new Transaction
//								{
//									Amount = 3030.80M,
//									PaymentMethod = "Credit Card",
//									PaymentDate = DateOnly.Parse("01-15-2024")
//								}
//							};

//Booking booking3 = new Booking()
//{
//	ClientId = 3,
//	BranchId = 101,
//	BookingDate = DateOnly.Parse("01-19-2024"),
//	BookingTime = TimeSpan.Parse("17:33:22"),
//	Status = Enum.GetName(typeof(BookingStatus), 1)
//};
//booking3.BookingServices = new List<BookingService>() {
//														new BookingService
//														{
//															ServiceId = 13,
//															Price = 3450
//														}
//													};
//booking3.Transactions = new List<Transaction>() {
//								new Transaction
//								{
//									Amount = 3450,
//									PaymentMethod = "Cash",
//									PaymentDate = DateOnly.Parse("01-20-2024")
//								}
//							};

//Booking booking4 = new Booking()
//{
//	ClientId = 3,
//	BranchId = 100,
//	BookingDate = DateOnly.Parse("01-20-2024"),
//	BookingTime = TimeSpan.Parse("16:33:22"),
//	Status = Enum.GetName(typeof(BookingStatus), 1)
//};
//booking4.BookingServices = new List<BookingService>() {
//														new BookingService
//														{
//															ServiceId = 12,
//															Price = 3030.80M
//														}
//													};
//booking4.Transactions = new List<Transaction>() {
//								new Transaction
//								{
//									Amount = 3030.80M,
//									PaymentMethod = "Visa",
//									PaymentDate = DateOnly.Parse("01-20-2024")
//								}
//							};


//Booking booking5 = new Booking()
//{
//	ClientId = 2,
//	BranchId = 100,
//	BookingDate = DateOnly.Parse("02-06-2024"),
//	BookingTime = TimeSpan.Parse("12:33:22"),
//	Status = Enum.GetName(typeof(BookingStatus), 1)
//};
//booking5.BookingServices = new List<BookingService>() {
//														new BookingService
//														{
//															ServiceId = 11,
//															Price = 4000
//														},
//													};
//booking5.Transactions = new List<Transaction>() {
//								new Transaction
//								{
//									Amount = 4000,
//									PaymentMethod = "Visa",
//									PaymentDate = DateOnly.Parse("02-05-2024")
//								}
//							};



//Booking booking6 = new Booking()
//{
//	ClientId = 2,
//	BranchId = 100,
//	BookingDate = DateOnly.Parse("02-06-2024"),
//	BookingTime = TimeSpan.Parse("12:33:22"),
//	Status = Enum.GetName(typeof(BookingStatus), 1)
//};
//booking5.BookingServices = new List<BookingService>() {
//														new BookingService
//														{
//															ServiceId = 11,
//															Price = 4000
//														},
//													};
//booking5.Transactions = new List<Transaction>() {
//								new Transaction
//								{
//									Amount = 4000,
//									PaymentMethod = "Visa",
//									PaymentDate = DateOnly.Parse("02-05-2024")
//								}
//							};

//return new List<Booking> { booking1, booking2, booking3, booking4, booking5 };

#endregion