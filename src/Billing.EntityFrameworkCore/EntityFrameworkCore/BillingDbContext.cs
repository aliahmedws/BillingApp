using Billing.Blocks;
using Billing.ConsumerDocumentDetails;
using Billing.ConsumerDocuments;
using Billing.ConsumerPersonalInfos;
using Billing.MeterInfos;
using Billing.Phases;
using Billing.PlotInfos;
using Billing.PlotSizes;
using Billing.PlotTransferHistories;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Billing.Phases;
using Billing.GovtCharges;
using Billing.IescoCharges;
using Billing.SocietyCharges;
using Billing.TarrifSlabs;

namespace Billing.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class BillingDbContext : AbpDbContext<BillingDbContext>, ITenantManagementDbContext, IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    public DbSet<Phase> Phases { get; set; }
    public DbSet<GovtCharge> GovtCharges { get; set; }
    public DbSet<IescoCharge> IescoCharges { get; set; }
    public DbSet<SocietyCharge> SocietyCharges { get; set; }
    public DbSet<Block> Blocks { get; set; }
    public DbSet<PlotSize> PlotSizes { get; set; }
    public DbSet<ConsumerPersonalInfo> ConsumerPersonalInfos { get; set; }
    public DbSet<TarrifSlab> TarrifSlabs { get; set; }
    public DbSet<ConsumerDocument> ConsumerDocuments { get; set; }
    public DbSet<ConsumerDocumentDetail> ConsumerDocumentDetails { get; set; }
    public DbSet<PlotInfo> PlotInfos { get; set; }
    public DbSet<MeterInfo> MeterInfos { get; set; }
    public DbSet<PlotTransferHistory> PlotTransferHistories { get; set; }

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public BillingDbContext(DbContextOptions<BillingDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();
        builder.ConfigureBlobStoring();

        /* Configure your own tables/entities inside here */

        builder.Entity<Phase>(b =>
        {
            b.ToTable(BillingConsts.DbTablePrefix + "Phases", BillingConsts.DbSchema);

            b.ConfigureByConvention();

            // Properties
            b.Property(x => x.PhaseCode)
                .IsRequired()
                .HasMaxLength(PhaseConsts.MaxPhaseCodeLength);

            b.Property(x => x.PhaseName)
                .IsRequired()
                .HasMaxLength(PhaseConsts.MaxPhaseNameLength);

            b.Property(x => x.Description)
                .HasMaxLength(PhaseConsts.MaxDescriptionLength);

            b.Property(x => x.IsActive)
                .IsRequired();

            b.Property(x => x.TenantId)
                .HasColumnName(nameof(Phase.TenantId))
                .IsRequired(false);

            // Indexes
            b.HasIndex(x => x.PhaseName);
            b.HasIndex(x => x.PhaseCode);
            b.HasIndex(x => x.TenantId);
        });

        builder.Entity<GovtCharge>(b =>
        {
            b.ToTable(BillingConsts.DbTablePrefix + "GovtCharges", BillingConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasIndex(x => x.CreationTime);
        });

        builder.Entity<IescoCharge>(b =>
        {
            b.ToTable(BillingConsts.DbTablePrefix + "IescoCharges", BillingConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasIndex(x => x.CreationTime);

        });

        builder.Entity<SocietyCharge>(b =>
        {
            b.ToTable(BillingConsts.DbTablePrefix + "SocietyCharges", BillingConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasIndex(x => x.CreationTime);
        });

        builder.Entity<Block>(b =>
        {
            b.ToTable(BillingConsts.DbTablePrefix + "Blocks", BillingConsts.DbSchema);

            b.ConfigureByConvention();

            // Properties
            b.Property(x => x.BlockCode)
                .IsRequired()
                .HasMaxLength(BlockConsts.MaxBlockCodeLength);

            b.Property(x => x.BlockName)
                .IsRequired()
                .HasMaxLength(BlockConsts.MaxBlockNameLength);

            b.Property(x => x.Description)
                .HasMaxLength(BlockConsts.MaxDescriptionLength);

            b.Property(x => x.IsActive)
                .IsRequired();

            b.Property(x => x.TenantId)
               .HasColumnName(nameof(Block.TenantId))
               .IsRequired(false);

            // Indexes
            b.HasIndex(x => x.BlockName);
            b.HasIndex(x => x.BlockCode);
            b.HasIndex(x => x.TenantId);
        });

        builder.Entity<PlotSize>(b =>
        {
            b.ToTable(BillingConsts.DbTablePrefix + "PlotSizes", BillingConsts.DbSchema);
            b.ConfigureByConvention();

            // Properties
            b.Property(x => x.SizeName)
                .IsRequired()
                .HasMaxLength(PlotSizeConsts.MaxSizeNameLength);

            b.Property(x => x.Area)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            b.Property(x => x.Length)
                .HasColumnType("decimal(10,2)");

            b.Property(x => x.Width)
                .HasColumnType("decimal(10,2)");

            b.Property(x => x.Description)
                .HasMaxLength(PlotSizeConsts.MaxDescriptionLength);

            b.Property(x => x.IsActive)
                .IsRequired();

            b.Property(x => x.TenantId)
               .HasColumnName(nameof(PlotSize.TenantId))
               .IsRequired(false);

            // Indexes
            b.HasIndex(x => x.SizeName);
            b.HasIndex(x => x.Unit);
            b.HasIndex(x => x.TenantId);
        });

        builder.Entity<ConsumerPersonalInfo>(b =>
        {
            b.ToTable(BillingConsts.DbTablePrefix + "ConsumerPersonalInfos", BillingConsts.DbSchema);
            b.ConfigureByConvention();

            // --- Basic Info ---
            b.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(ConsumerPersonalInfoConsts.MaxFirstNameLength);

            b.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(ConsumerPersonalInfoConsts.MaxLastNameLength);

            b.Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(ConsumerPersonalInfoConsts.MaxPhoneLength);

            b.Property(x => x.CNIC)
                .IsRequired()
                .HasMaxLength(ConsumerPersonalInfoConsts.MaxCnicLength);

            b.Property(x => x.Email)
                .HasMaxLength(ConsumerPersonalInfoConsts.MaxEmailLength);

            b.Property(x => x.Gender)
                .IsRequired()
                .HasConversion<int>();

            b.Property(x => x.DOB)
                .IsRequired();

            // --- Guardian Info ---
            b.Property(x => x.AlternativePersonName)
                .HasMaxLength(ConsumerPersonalInfoConsts.MaxAlternativePersonNameLength);

            b.Property(x => x.AlternativePersonPhone)
                .HasMaxLength(ConsumerPersonalInfoConsts.MaxAlternativePersonPhoneLength);

            b.Property(x => x.AlternativePersonEmail)
                .HasMaxLength(ConsumerPersonalInfoConsts.MaxAlternativePersonEmailLength);

            b.Property(x => x.AlternativePersonCNIC)
                .HasMaxLength(ConsumerPersonalInfoConsts.MaxAlternativePersonCnicLength);

            // --- Value Object (Address) ---
            b.OwnsOne(x => x.Address, a =>
            {
                a.Property(p => p.Street)
                    .HasColumnName(nameof(Address.Street))
                    .HasMaxLength(AddressConsts.MaxStreetLength);

                a.Property(p => p.City)
                    .HasColumnName(nameof(Address.City))
                    .HasMaxLength(AddressConsts.MaxCityLength);

                a.Property(p => p.State)
                    .HasColumnName(nameof(Address.State))
                    .HasMaxLength(AddressConsts.MaxStateLength);

                a.Property(p => p.Country)
                    .HasColumnName(nameof(Address.Country))
                    .HasConversion<int>(); // enum → int

                a.Property(p => p.PostalCode)
                    .HasColumnName(nameof(Address.PostalCode))
                    .HasMaxLength(AddressConsts.MaxPostalCodeLength);
            });

            // --- Indexes ---
            b.HasIndex(x => x.CNIC);
            b.HasIndex(x => x.Phone);
        });
                        //TARRIF SLAB

        builder.Entity<TarrifSlab>(b =>
        {
            b.ToTable(BillingConsts.DbTablePrefix + "TarrifSlabs", BillingConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.LowerSlab).IsRequired();
            b.Property(x => x.UpperSlab);
            b.Property(x => x.UnitPrice).IsRequired();
           // b.Property(x => x.TenantId).IsRequired(false);
            b.HasIndex(x => x.TenantId);
        });

        builder.Entity<ConsumerDocument>(b =>
        {
            b.ToTable(BillingConsts.DbTablePrefix + "ConsumerDocuments", BillingConsts.DbSchema);
            b.ConfigureByConvention();

            b.HasOne(x => x.Consumers)
                .WithMany(x => x.ConsumerDocuments)
                .HasForeignKey(x => x.ConsumerId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(x => x.ConsumerDocumentDetails)
                .WithOne(x => x.ConsumerDocument)
                .HasForeignKey(x => x.ConsumerDocumentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ConsumerDocumentDetail>(b =>
        {
            b.ToTable(BillingConsts.DbTablePrefix + "ConsumerDocumentDetails", BillingConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Description)
                .HasMaxLength(ConsumerDocumentDetailConsts.DescriptionMaxLength)
                .IsRequired(false);

            b.HasOne(x => x.ConsumerDocument)
                .WithMany(x => x.ConsumerDocumentDetails)
                .HasForeignKey(x => x.ConsumerDocumentId)
                .OnDelete(DeleteBehavior.Cascade);

            // FileAttachment (owned type)
            b.OwnsOne(x => x.ConsumerDocumentFile, fa =>
            {
                fa.Property(f => f.Name).HasColumnName("FileName").HasMaxLength(256);
                fa.Property(f => f.BlobName).HasColumnName("BlobName").HasMaxLength(256);
                fa.Property(f => f.Path).HasColumnName("FilePath").HasMaxLength(512);
                fa.Property(f => f.SizeInBytes).HasColumnName("FileSize");
            });
        });

        builder.Entity<PlotInfo>(b =>
        {
            b.ToTable(BillingConsts.DbTablePrefix + "PlotInfos", BillingConsts.DbSchema);
            b.ConfigureByConvention();

            // --- Properties ---
            b.Property(x => x.PlotNo).IsRequired().HasMaxLength(PlotInfoConsts.MaxPlotNoLength);

            b.Property(x => x.PlotType).IsRequired();

            b.Property(x => x.StreetNo).IsRequired().HasMaxLength(PlotInfoConsts.MaxStreetNoLength);

            b.Property(x => x.PlotSizeId).IsRequired();

            b.Property(x => x.Status).IsRequired().HasConversion<int>(); // Enum → Int

            b.Property(x => x.BlockId).IsRequired();

            b.Property(x => x.ConsumerId).IsRequired(false);

            b.Property(x => x.PhaseId).IsRequired();

            b.Property(x => x.Remarks).HasMaxLength(PlotInfoConsts.MaxRemarksLength);

            b.Property(x => x.TenantId)
                .HasColumnName(nameof(PlotInfo.TenantId))
                .IsRequired(false);


            // --- Relationships ---
            b.HasOne(x => x.Block)
                .WithMany()
                .HasForeignKey(x => x.BlockId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Phase)
                .WithMany()
                .HasForeignKey(x => x.PhaseId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.PlotSize)
                .WithMany()
                .HasForeignKey(x => x.PlotSizeId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.ConsumerPersonaInfo)
                .WithMany()
                .HasForeignKey(x => x.ConsumerId)
                .OnDelete(DeleteBehavior.SetNull);

            // --- Indexes ---
            b.HasIndex(x => x.PlotNo);
            b.HasIndex(x => new { x.BlockId, x.PhaseId });
            b.HasIndex(x => x.ConsumerId);
            b.HasIndex(x => x.TenantId);
        });

        builder.Entity<MeterInfo>(b =>
        {
            b.ToTable(BillingConsts.DbTablePrefix + "MeterInfos", BillingConsts.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.MeterNo).IsRequired().HasMaxLength(MeterInfoConsts.MaxMeterNoLength);

            b.Property(x => x.MeterType).IsRequired().HasConversion<int>();
            b.Property(x => x.MeterCategory).IsRequired().HasConversion<int>();

            b.Property(x => x.MeterStatus).IsRequired().HasConversion<int>();

            b.Property(x => x.InstallationDate).IsRequired();

            b.Property(x => x.InitialReading).HasPrecision(18, 2).IsRequired();

            b.Property(x => x.Remarks).HasMaxLength(MeterInfoConsts.MaxRemarksLength);

            b.Property(x => x.PhaseId).IsRequired();

            b.Property(x => x.PlotId).IsRequired();
            b.Property(x => x.MeterOwnerId).IsRequired();

            b.Property(x => x.TenantId)
                .HasColumnName(nameof(MeterInfo.TenantId))
                .IsRequired(false);


            b.HasOne(x => x.Phase).WithMany(x => x.MeterInfos).HasForeignKey(x => x.PhaseId).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(x => x.Plot).WithMany(x => x.MeterInfos).HasForeignKey(x => x.PlotId).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(x => x.MeterOwner).WithMany(x => x.MeterInfos).HasForeignKey(x => x.MeterOwnerId).OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => x.MeterNo).IsUnique();
            b.HasIndex(x => x.MeterStatus);
            b.HasIndex(x => x.MeterType);
            b.HasIndex(x => x.PlotId);
            b.HasIndex(x => x.PhaseId);
            b.HasIndex(x => x.MeterOwnerId);
            b.HasIndex(x => x.TenantId);
        });

        builder.Entity<PlotTransferHistory>(b =>
        {
            b.ToTable(BillingConsts.DbTablePrefix + "PlotTransferHistories", BillingConsts.DbSchema);

            b.ConfigureByConvention();

            // Properties
            b.Property(x => x.TransferDate).IsRequired();

            b.Property(x => x.TransferType).IsRequired().HasConversion<int>();

            b.Property(x => x.RegistryNo).IsRequired().HasMaxLength(PlotTransferHistoryConsts.MaxRegistryNoLength);

            b.Property(x => x.ConsiderationAmount).HasPrecision(18, 2).IsRequired();

            b.Property(x => x.Remarks).HasMaxLength(PlotTransferHistoryConsts.MaxRemarksLength);

            b.Property(x => x.IsApproved).IsRequired().HasDefaultValue(false);

            b.Property(x => x.ApprovedByUserId).IsRequired(false);
            b.Property(x => x.RejectByUserId).IsRequired(false);

            b.Property(x => x.ApprovedAt).IsRequired(false);
            b.Property(x => x.TenantId)
                .HasColumnName(nameof(PlotTransferHistory.TenantId))
                .IsRequired(false);


            // Relationships
            b.HasOne(x => x.Plot).WithMany().HasForeignKey(x => x.PlotId).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(x => x.Consumers).WithMany().HasForeignKey(x => x.ToConsumerId).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(x => x.FromConsumers).WithMany().HasForeignKey(x => x.FromConsumerId).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(x => x.ApprovedByUser).WithMany().HasForeignKey(x => x.ApprovedByUserId).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(x => x.RejectByUser).WithMany().HasForeignKey(x => x.RejectByUserId).OnDelete(DeleteBehavior.Restrict);

            // Indexes
            b.HasIndex(x => x.RegistryNo).IsUnique();
            b.HasIndex(x => x.TransferDate);
            b.HasIndex(x => x.TransferType);
            b.HasIndex(x => x.PlotId);
            b.HasIndex(x => x.ToConsumerId);
            b.HasIndex(x => x.TenantId);
        });

    }

}

