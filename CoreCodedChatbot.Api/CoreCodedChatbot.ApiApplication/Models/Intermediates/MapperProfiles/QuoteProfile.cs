using AutoMapper;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Models.Intermediates.MapperProfiles;

public class QuoteProfile : Profile
{
    public QuoteProfile()
    {
        CreateMap<Quote, ApiContract.ResponseModels.Quotes.ChildModels.Quote>()
            .ForMember(q => q.QuoteId, i => i.MapFrom(src => src.QuoteId))
            .ForMember(q => q.QuoteText, i => i.MapFrom(src => src.QuoteText))
            .ForMember(q => q.CreatedBy, i => i.MapFrom(src => src.Username))
            .ForMember(q => q.Disabled, i => i.MapFrom(src => !src.Enabled))
            .ForMember(q => q.LastEditedBy, i => i.MapFrom(src => src.LastEditedBy))
            .ForMember(q => q.EditedAt, i => i.MapFrom(src => src.LastEdited))
            .ReverseMap();
    }
}