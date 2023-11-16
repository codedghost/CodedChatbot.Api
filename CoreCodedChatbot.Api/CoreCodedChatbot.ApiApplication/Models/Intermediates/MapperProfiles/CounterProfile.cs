using AutoMapper;
using CoreCodedChatbot.Database.Context.Models;
using ContractCounter = CoreCodedChatbot.ApiContract.ResponseModels.Counters.ChildModels.Counter;

namespace CoreCodedChatbot.ApiApplication.Models.Intermediates.MapperProfiles;

public class CounterProfile : Profile
{
    public CounterProfile()
    {
        CreateMap<Counter, ContractCounter>()
            .ForMember(c => c.CounterName, i => i.MapFrom(src => src.CounterName))
            .ForMember(c => c.CounterPreText, i => i.MapFrom(src => src.CounterSuffix))
            .ForMember(c => c.CounterValue, i => i.MapFrom(src => src.CounterValue))
            .ReverseMap();
    }
}