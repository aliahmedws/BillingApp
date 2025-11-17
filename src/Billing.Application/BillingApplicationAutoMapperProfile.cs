using AutoMapper;
using Billing.Blocks;
using Billing.ConsumerDocumentDetails;
using Billing.ConsumerDocuments;
using Billing.ConsumerPersonalInfos;
using Billing.FileAttachments;
using Billing.MeterDocuments;
using Billing.MeterInfos;
using Billing.Phases;
using Billing.PlotDocuments;
using Billing.PlotInfos;
using Billing.PlotSizes;
using Billing.PlotTransferHistories;

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

        CreateMap<ConsumerDocumentDetail, ConsumerDocumentDetailDto>();

        CreateMap<FileAttachment, FileAttachmentDto>();

        CreateMap<CreateConsumerDocumentDto, ConsumerDocument>();
        CreateMap<CreateConsumerDocumentDetailDto, ConsumerDocumentDetail>();
        CreateMap<FileAttachmentDto, FileAttachment>();

        CreateMap<PlotInfo, PlotInfoDto>()
            .ForMember(dest => dest.BlockName, opt => opt.MapFrom(src => src.Block != null ? src.Block.BlockName : null))
            .ForMember(dest => dest.PhaseName, opt => opt.MapFrom(src => src.Phase != null ? src.Phase.PhaseName : null))
            .ForMember(dest => dest.PlotSizeName, opt => opt.MapFrom(src => src.PlotSize != null ? src.PlotSize.SizeName : null))
            .ForMember(dest => dest.PlotDocuments, opt => opt.MapFrom(src => src.PlotDocuments));

        CreateMap<MeterInfo, MeterInfoDto>()
            .ForMember(dest => dest.PhaseName, opt => opt.MapFrom(src => src.Phase != null ? src.Phase.PhaseName : null))
            .ForMember(dest => dest.PlotNo, opt => opt.MapFrom(src => src.Plot != null ? src.Plot.PlotNo : null))
            .ForMember(dest => dest.MeterOwnerName, opt => opt.MapFrom(src => src.MeterOwner != null ? src.MeterOwner.FirstName + " " + src.MeterOwner.LastName : null))
            .ForMember(dest => dest.MeterDocuments, opt => opt.MapFrom(src => src.MeterDocuments));


        CreateMap<PlotTransferHistory, PlotTransferHistoryDto>()
            .ForMember(dest => dest.PlotNo, opt => opt.MapFrom(src => src.Plot != null ? src.Plot.PlotNo : null))
            .ForMember(dest => dest.FromConsumerName, opt => opt.MapFrom(src => src.FromConsumers != null ? src.FromConsumers.FirstName + " " + src.FromConsumers.LastName : null))
            .ForMember(dest => dest.ToConsumerName, opt => opt.MapFrom(src => src.Consumers != null ? src.Consumers.FirstName + " " + src.Consumers.LastName : null))
            .ForMember(dest => dest.ApprovedByUserName, opt => opt.MapFrom(src => src.ApprovedByUser != null ? src.ApprovedByUser.UserName : null))
            .ForMember(dest => dest.RejectByUserName, opt => opt.MapFrom(src => src.RejectByUser != null ? src.RejectByUser.UserName : null ));

        CreateMap<MeterDocument, MeterDocumentDto>()
            .ForMember(dest => dest.FileAttachments, opt => opt.MapFrom(src => src.FileAttachments));

        CreateMap<PlotDocument, PlotDocumentDto>()
            .ForMember(dest => dest.FileAttachments, opt => opt.MapFrom(src => src.FileAttachments));


    }
}
