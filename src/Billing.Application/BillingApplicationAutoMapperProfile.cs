using AutoMapper;
using Billing.Blocks;
using Billing.ConsumerPersonalInfos;
using Billing.Phases;
using Billing.PlotSizes;

namespace Billing;

public class BillingApplicationAutoMapperProfile : Profile
{
    public BillingApplicationAutoMapperProfile()
    {
        CreateMap<Phase, PhaseDto>();
        CreateMap<Block, BlockDto>()
            .ForMember(d => d.PhaseName, o => o.MapFrom(s => s.Phases != null ? s.Phases.PhaseName : null));
        CreateMap<PlotSize, PlotSizeDto>();
        CreateMap<ConsumerPersonalInfo, ConsumerPersonalInfo>();
    }
}
