using AutoMapper;
using Billing.Phases;

namespace Billing;

public class BillingApplicationAutoMapperProfile : Profile
{
    public BillingApplicationAutoMapperProfile()
    {
        CreateMap<Phase, PhaseDto>();
    }
}
