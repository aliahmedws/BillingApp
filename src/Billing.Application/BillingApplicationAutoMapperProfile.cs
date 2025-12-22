using AutoMapper;
using Billing.Blocks;
using Billing.ConsumerDocuments;
using Billing.ConsumerPersonalInfos;
using Billing.ElectricityBills;
using Billing.ElectricityPaymentHistories;
using Billing.FileAttachments;
using Billing.GovtCharges;
using Billing.IescoCharges;
using Billing.MaintenanceBills;
using Billing.MaintenancePaymentHistories;
using Billing.MeterDocuments;
using Billing.MeterInfos;
using Billing.Phases;
using Billing.PlotDocuments;
using Billing.PlotInfos;
using Billing.PlotSizes;
using Billing.PlotTransferHistories;
using Billing.PlotTransferHistoryDocuments;
using Billing.SocietyCharges;
using Billing.TarrifSlabs;

namespace Billing;

public class BillingApplicationAutoMapperProfile : Profile
{
    public BillingApplicationAutoMapperProfile()
    {
        CreateMap<Phase, PhaseDto>();
        CreateMap<GovtCharge, GovtChargeDto>();
        CreateMap<IescoCharge, IescoChargeDto>();
        CreateMap<TarrifSlab, TarrifSlabDto>();
        CreateMap<SocietyCharge, SocietyChargeDto>()
            .ForMember(x => x.SizeName, opt => opt.MapFrom(src => src.PlotSizes != null ? src.PlotSizes.SizeName : null));
        CreateMap<Phase, PhaseDto>()
            .ForMember(d => d.CreatorName, opt => opt.Ignore())
            .ForMember(d => d.LastModifierName, opt => opt.Ignore());
        CreateMap<Block, BlockDto>()
            .ForMember(d => d.PhaseName, o => o.MapFrom(s => s.Phases != null ? s.Phases.PhaseName : null));
        CreateMap<PlotSize, PlotSizeDto>();
        CreateMap<ConsumerPersonalInfo, ConsumerPersonalInfoDto>()
            .ForMember(dest => dest.ConsumerDocuments, opt => opt.MapFrom(src => src.ConsumerDocuments));
        CreateMap<Address, AddressDto>();

        CreateMap<FileAttachment, FileAttachmentDto>();

        CreateMap<PlotInfo, PlotInfoDto>()
            .ForMember(dest => dest.BlockName, opt => opt.MapFrom(src => src.Block != null ? src.Block.BlockName : null))
            .ForMember(dest => dest.PhaseName, opt => opt.MapFrom(src => src.Phase != null ? src.Phase.PhaseName : null))
            .ForMember(dest => dest.PlotSizeName, opt => opt.MapFrom(src => src.PlotSize != null ? src.PlotSize.SizeName : null))
            .ForMember(dest => dest.PlotDocuments, opt => opt.MapFrom(src => src.PlotDocuments));

        CreateMap<MeterInfo, MeterInfoDto>()
            .ForMember(dest => dest.PhaseName, opt => opt.MapFrom(src => src.Phase != null ? src.Phase.PhaseName : null))
            .ForMember(dest => dest.PlotNo, opt => opt.MapFrom(src => src.Plot != null ? src.Plot.PlotNo : null))
            .ForMember(dest => dest.MeterOwnerName, opt => opt.MapFrom(src => src.MeterOwner != null ? src.MeterOwner.FirstName + " " + src.MeterOwner.LastName : null))
            .ForMember(dest => dest.MeterDocuments, opt => opt.MapFrom(src => src.MeterDocuments))
            .ForMember(dest => dest.PlotSizeName, opt => opt.MapFrom(src => src.Plot.PlotSize.SizeName));


        CreateMap<PlotTransferHistory, PlotTransferHistoryDto>()
            .ForMember(dest => dest.PlotNo, opt => opt.MapFrom(src => src.Plot != null ? src.Plot.PlotNo : null))
            .ForMember(dest => dest.FromConsumerName, opt => opt.MapFrom(src => src.FromConsumers != null ? src.FromConsumers.FirstName + " " + src.FromConsumers.LastName : null))
            .ForMember(dest => dest.ToConsumerName, opt => opt.MapFrom(src => src.Consumers != null ? src.Consumers.FirstName + " " + src.Consumers.LastName : null))
            .ForMember(dest => dest.ApprovedByUserName, opt => opt.MapFrom(src => src.ApprovedByUser != null ? src.ApprovedByUser.UserName : null))
            .ForMember(dest => dest.RejectByUserName, opt => opt.MapFrom(src => src.RejectByUser != null ? src.RejectByUser.UserName : null))
            .ForMember(dest => dest.PlotTransferHistoryDocuments, opt => opt.MapFrom(src => src.PlotTransferHistoryDocuments));

        CreateMap<MeterDocument, MeterDocumentDto>()
            .ForMember(dest => dest.FileAttachments, opt => opt.MapFrom(src => src.FileAttachments));

        CreateMap<PlotDocument, PlotDocumentDto>()
            .ForMember(dest => dest.FileAttachments, opt => opt.MapFrom(src => src.FileAttachments));

        CreateMap<ConsumerDocument, ConsumerDocumentDto>()
            .ForMember(dest => dest.FileAttachments, opt => opt.MapFrom(src => src.FileAttachments));

        CreateMap<PlotTransferHistoryDocument, PlotTransferHistoryDocumentDto>()
            .ForMember(dest => dest.FileAttachments, opt => opt.MapFrom(src => src.FileAttachments));

        CreateMap<MaintenanceBill, MaintenanceBillDto>()
           .ForMember(dest => dest.ConsumerFullName, opt => opt.MapFrom(src =>
                        src.ConsumerPersonalInfos != null
                        ? src.ConsumerPersonalInfos.FirstName + " " + src.ConsumerPersonalInfos.LastName
                        : string.Empty))
           .ForMember(dest => dest.PlotNo, opt => opt.MapFrom(src =>
                        src.PlotInfos != null
                        ? src.PlotInfos.PlotNo
                            + " / "
                            + (src.PlotInfos.Block != null ? src.PlotInfos.Block.BlockName : string.Empty)
                            + " / "
                            + (src.PlotInfos.Block != null && src.PlotInfos.Block.Phases != null
                                ? src.PlotInfos.Block.Phases.PhaseName
                                : string.Empty)
                        : string.Empty));

        CreateMap<ElectricityBill, ElectricityBillDto>()
            .ForMember(x => x.MeterNo, opt => opt.MapFrom(src => src.MeterInfos.MeterNo));

        CreateMap<MaintenancePaymentHistory, MaintenancePaymentHistoryDto>();
        CreateMap<ElectricityPaymentHistory, ElectricityPaymentHistoryDto>();
}
}
