using AutoMapper;
using BMSReporting.API.DTO;

namespace BMSReporting.API.Helper
{
	public class AutoMappingProfiles : Profile
	{
		public AutoMappingProfiles()
		{

			CreateMap<Booking, BookingDTO>()
				.ForMember(des => des.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
				.ForMember(des => des.ClientName, opt => opt.MapFrom(src => src.Client.FirstName + " " + src.Client.LastName))
				.ReverseMap();
		}



	}
}
