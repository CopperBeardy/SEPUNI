using AutoMapper;
using CordEstates.Areas.Identity.Data;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Models.DTOs;
using System.Collections.Generic;

namespace CordEstates.Profiles
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {

            CreateMap<Listing, LandingListingDTO>().ForMember(des => des.ImageUrl, o => o.MapFrom(src => src.Image.ImageLink));
            CreateMap<Listing,ListingManagementDTO>()
                .ForMember(des => des.Image, o => o.MapFrom(src => src.Image))
                  .ForMember(des => des.Address, o => o.MapFrom(src => src.Address)).ReverseMap() ;
  
            CreateMap<List<Listing>, ListingManagementDTO>()
             
                .ReverseMap();
            CreateMap<Listing, ListingDTO>().ForMember(desc => desc.Url, o => o.MapFrom(src => src.Image.ImageLink));
            CreateMap<Listing, ListingDetailDTO>().ForMember(desc => desc.Url, o => o.MapFrom(src => src.Image.ImageLink));
            CreateMap<Listing, ExtendedListingDTO>().ForMember(desc => desc.Url, o => o.MapFrom(src => src.Image.ImageLink));


            CreateMap<Address, AddressDTO>().ReverseMap();
  
            CreateMap<Appointment, CreateAppointmentDTO>()
                .ForMember(desc => desc.Time, o =>o.MapFrom(src => src.Time))
                .ReverseMap();        
            CreateMap<Appointment, AppointmentDTO>().ReverseMap();
            CreateMap<Appointment, AppointmentManagementDTO>().ReverseMap();
            CreateMap<Appointment, EditAppointmentManagementDTO>().ReverseMap();


            CreateMap<Ticket, CreateTicketDTO>().ReverseMap();          
            CreateMap<Ticket, TicketManagementDTO>().ReverseMap();

            CreateMap<Photo, PhotoDTO>().ReverseMap();
            CreateMap<List<Photo>, PhotoDTO>().ReverseMap();


            CreateMap<Service, ServiceDTO>();


            CreateMap<Event, EventDTO>().ForMember(des => des.ImagePath, o => o.MapFrom(src => src.Photo.ImageLink));
            CreateMap<Event, UploadEventDTO>()
                .ForMember(desc => desc.Time, o => o.MapFrom(src => src.Time))
             .ForMember(des => des.ImagePath, o => o.MapFrom(src => src.Photo.ImageLink)).ReverseMap();
            CreateMap<List<Event>, EventDTO>();
            CreateMap<Event, EventManagementDTO>().ReverseMap();
             

            CreateMap<ApplicationUser, UserManagementDTO > ()
                    .ForMember(des => des.HeadShot, o => o.MapFrom(src => src.HeadShot))
                  
            .ReverseMap();
            CreateMap<List<ApplicationUser>, UserManagementDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(des => des.FirstName, o => o.MapFrom(src => src.FirstName))
                .ForMember(des => des.LastName, o => o.MapFrom(src => src.LastName))
                .ForMember(des => des.Bio, o => o.MapFrom(src => src.Bio))
                .ForMember(des => des.HeadShotUrl, o => o.MapFrom(src => src.HeadShot.ImageLink));

        }
    }
}
