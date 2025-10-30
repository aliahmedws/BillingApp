using AutoMapper;
using Billing.GovtCharges;
using Billing.IescoCharges;
using Billing.Phases;
using Billing.SocietyCharges;
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
        CreateMap<GovtCharge, GovtChargeDto>();
        CreateMap<IescoCharge, IescoChargeDto>();
        CreateMap<SocietyCharge, SocietyChargeDto>();
        CreateMap<Phase, PhaseDto>()
            .ForMember(d => d.CreatorName, opt => opt.Ignore())
            .ForMember(d => d.LastModifierName, opt => opt.Ignore());
        CreateMap<Block, BlockDto>()
            .ForMember(d => d.PhaseName, o => o.MapFrom(s => s.Phases != null ? s.Phases.PhaseName : null));
        CreateMap<PlotSize, PlotSizeDto>();
        CreateMap<ConsumerPersonalInfo, ConsumerPersonalInfoDto>();
        CreateMap<Address, AddressDto>();
    }
}
