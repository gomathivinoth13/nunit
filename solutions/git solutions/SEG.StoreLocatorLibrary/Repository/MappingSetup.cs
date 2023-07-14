using AutoMapper;
using SEG.StoreLocatorLibrary.Shared;
using SEG.StoreLocatorLibrary.Shared.ResponseModels;

namespace SEG.StoreLocatorLibrary.Repository
{
    public static class MappingSetup
    {
        private static IMapper _mapper;

        public static TDest Map<TSource, TDest>(TSource source)
        {
            var mapper = _mapper ?? CreateMap();
            return mapper.Map<TDest>(source);
        } 

        private static IMapper CreateMap()
        {
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Address, AddressResponse>();
                cfg.CreateMap<SimAddress, Address>(); 

                cfg.CreateMap<Pharmacy, PharmacyResponse>();
                cfg.CreateMap<SimPharmacy, Pharmacy>(); 

                cfg.CreateMap<Location, LocationResponse>();
                cfg.CreateMap<SimLocation, Location>(); //**** TODO: Verify Geo setting

                cfg.CreateMap<SimMediaLink, MediaLink>();
                cfg.CreateMap<SimPromotion, Promotion>();

                cfg.CreateMap<Store, StoreResponse>();
                cfg.CreateMap<SimStore, Store>() //
                    .ForMember(d => d.Timings, opt => opt.Ignore())
                    .ForMember(d => d.MediaLink, opt => opt.Ignore())
                    .ForMember(d => d.Promotion, opt => opt.Ignore())
                    .ForMember(d => d.TimeZone, opt => opt.Ignore())
                    .ForMember(d => d.Str_Trt_Desc, opt => opt.Ignore());

                cfg.CreateMap<Store, StoreAddressResponse>();
            });

            IMapper mapper = mapConfig.CreateMapper();
            _mapper = mapper;
            return mapper;
        }
    }
}
