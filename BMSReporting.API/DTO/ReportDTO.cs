namespace BMSReporting.API.DTO
{
	public class CustomerDemographicsReport
	{
		public int TotalCustomers { get; set; }
		public List<CustomerAgeGroup> AgeGroups { get; set; }
		public List<CustomerGenderGroup> GenderGroups { get; set; }
		public List<CustomerBranchGroup> BranchGroups { get; set; }
		public List<CustomerPaymentMethodGroup> PaymentMethodGroups { get; set; }
	}

	public class CustomerPaymentMethodGroup
	{
		public string PaymentMethod { get; set; }
		public int Count { get; set; }
	}

	public class CustomerAgeGroup
	{
		public string AgeRange { get; set; }
		public int Count { get; set; }
	}

	public class CustomerGenderGroup
	{
		public string Gender { get; set; }
		public int Count { get; set; }
	}

	public class CustomerBranchGroup
	{
		public string BranchName { get; set; }
		public int Count { get; set; }
	}

}
