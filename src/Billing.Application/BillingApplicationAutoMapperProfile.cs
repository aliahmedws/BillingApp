using AutoMapper;
using Billing.Blocks;
using Billing.ConsumerDocumentDetails;
using Billing.ConsumerDocuments;
using Billing.ConsumerPersonalInfos;
using Billing.FileAttachments;
using Billing.MeterInfos;
using Billing.Phases;
using Billing.PlotInfos;
using Billing.PlotSizes;

namespace Billing;

public class BillingApplicationAutoMapperProfile : Profile
{
    public BillingApplicationAutoMapperProfile()
    {
        CreateMap<Phase, PhaseDto>()
            .ForMember(d => d.CreatorName, opt => opt.Ignore())
            .ForMember(d => d.LastModifierName, opt => opt.Ignore());
        CreateMap<Block, BlockDto>()
            .ForMember(d => d.PhaseName, o => o.MapFrom(s => s.Phases != null ? s.Phases.PhaseName : null));
        CreateMap<PlotSize, PlotSizeDto>();
        CreateMap<ConsumerPersonalInfo, ConsumerPersonalInfoDto>();
        CreateMap<Address, AddressDto>();
        CreateMap<ConsumerDocument, ConsumerDocumentDto>()
            .ForMember(d => d.ConsumerDocumentDetails, opt => opt.MapFrom(s => s.ConsumerDocumentDetails));

        CreateMap<ConsumerDocumentDetail, ConsumerDocumentDetailDto>()
            .ForMember(d => d.ConsumerDocumentFile, opt => opt.MapFrom(s => s.ConsumerDocumentFile));

        CreateMap<FileAttachment, FileAttachmentDto>()
            .ForMember(d => d.FileBytes, opt => opt.Ignore());

        CreateMap<CreateConsumerDocumentDto, ConsumerDocument>();
        CreateMap<CreateConsumerDocumentDetailDto, ConsumerDocumentDetail>();
        CreateMap<FileAttachmentDto, FileAttachment>();

        CreateMap<PlotInfo, PlotInfoDto>()
            .ForMember(dest => dest.BlockName, opt => opt.MapFrom(src => src.Block != null ? src.Block.BlockName : null))
            .ForMember(dest => dest.PhaseName, opt => opt.MapFrom(src => src.Phase != null ? src.Phase.PhaseName : null))
            .ForMember(dest => dest.PlotSizeName, opt => opt.MapFrom(src => src.PlotSize != null ? src.PlotSize.SizeName : null));
        //.ForMember(dest => dest.ConsumerFullName, opt => opt.MapFrom(src => src.Con != null ? src.ConsumerFullName));

        CreateMap<MeterInfo, MeterInfoDto>()
            .ForMember(dest => dest.PhaseName, opt => opt.MapFrom(src => src.Phase != null ? src.Phase.PhaseName : null))
            .ForMember(dest => dest.PlotNo, opt => opt.MapFrom(src => src.Plot != null ? src.Plot.PlotNo : null));
    }
}
