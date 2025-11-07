using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Billing.Migrations
{
    /// <inheritdoc />
    public partial class AddedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppConsumerPersonalInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CNIC = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AlternativePersonName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AlternativePersonPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AlternativePersonEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AlternativePersonCNIC = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<int>(type: "int", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppConsumerPersonalInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppGovtCharges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TvFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GST = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IncomeTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtraTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FurtherTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NjSurcharge = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalesTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FcSurcharge = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TrSurcharge = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxOnFpa = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalTaxes = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppGovtCharges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppIescoCharges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalEnergyCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IescoFixCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ServiceRent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VarFpa = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QtrTariffAdj = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalIescoCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppIescoCharges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPhases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhaseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhaseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPhases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPlotSizes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SizeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Area = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Width = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlotSizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppTarrifSlabs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RateRangeOne = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RateRangeTwo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RateRangeThree = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RateRangeFour = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RateRangeFive = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RateRangeSix = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RateRangeSeven = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RateRangeEight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTarrifSlabs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppConsumerDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsumerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppConsumerDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppConsumerDocuments_AppConsumerPersonalInfos_ConsumerId",
                        column: x => x.ConsumerId,
                        principalTable: "AppConsumerPersonalInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppBlocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BlockName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    PhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppBlocks_AppPhases_PhaseId",
                        column: x => x.PhaseId,
                        principalTable: "AppPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppSocietyCharges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlotSizeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SecurityCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaintenanceCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WaterCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalSocietyCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSocietyCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSocietyCharges_AppPlotSizes_PlotSizeId",
                        column: x => x.PlotSizeId,
                        principalTable: "AppPlotSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppConsumerDocumentDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsumerDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    BlobName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppConsumerDocumentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppConsumerDocumentDetails_AppConsumerDocuments_ConsumerDocumentId",
                        column: x => x.ConsumerDocumentId,
                        principalTable: "AppConsumerDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppPlotInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlotNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    PlotType = table.Column<int>(type: "int", nullable: false),
                    StreetNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    PlotSizeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    BlockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsumerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BlockId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConsumerPersonalInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PhaseId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlotSizeId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlotInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPlotInfos_AppBlocks_BlockId",
                        column: x => x.BlockId,
                        principalTable: "AppBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppPlotInfos_AppBlocks_BlockId1",
                        column: x => x.BlockId1,
                        principalTable: "AppBlocks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppPlotInfos_AppConsumerPersonalInfos_ConsumerId",
                        column: x => x.ConsumerId,
                        principalTable: "AppConsumerPersonalInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AppPlotInfos_AppConsumerPersonalInfos_ConsumerPersonalInfoId",
                        column: x => x.ConsumerPersonalInfoId,
                        principalTable: "AppConsumerPersonalInfos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppPlotInfos_AppPhases_PhaseId",
                        column: x => x.PhaseId,
                        principalTable: "AppPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppPlotInfos_AppPhases_PhaseId1",
                        column: x => x.PhaseId1,
                        principalTable: "AppPhases",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppPlotInfos_AppPlotSizes_PlotSizeId",
                        column: x => x.PlotSizeId,
                        principalTable: "AppPlotSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppPlotInfos_AppPlotSizes_PlotSizeId1",
                        column: x => x.PlotSizeId1,
                        principalTable: "AppPlotSizes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppMeterInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeterNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MeterType = table.Column<int>(type: "int", nullable: false),
                    MeterCategory = table.Column<int>(type: "int", nullable: false),
                    MeterStatus = table.Column<int>(type: "int", nullable: false),
                    InstallationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InitialReading = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeterOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppMeterInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppMeterInfos_AppConsumerPersonalInfos_MeterOwnerId",
                        column: x => x.MeterOwnerId,
                        principalTable: "AppConsumerPersonalInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppMeterInfos_AppPhases_PhaseId",
                        column: x => x.PhaseId,
                        principalTable: "AppPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppMeterInfos_AppPlotInfos_PlotId",
                        column: x => x.PlotId,
                        principalTable: "AppPlotInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppPlotTransferHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromConsumerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToConsumerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransferDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransferType = table.Column<int>(type: "int", nullable: false),
                    RegistryNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConsiderationAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RejectReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RejectByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConsumerPersonalInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlotInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlotTransferHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPlotTransferHistories_AbpUsers_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppPlotTransferHistories_AbpUsers_RejectByUserId",
                        column: x => x.RejectByUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppPlotTransferHistories_AppConsumerPersonalInfos_ConsumerPersonalInfoId",
                        column: x => x.ConsumerPersonalInfoId,
                        principalTable: "AppConsumerPersonalInfos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppPlotTransferHistories_AppConsumerPersonalInfos_FromConsumerId",
                        column: x => x.FromConsumerId,
                        principalTable: "AppConsumerPersonalInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppPlotTransferHistories_AppConsumerPersonalInfos_ToConsumerId",
                        column: x => x.ToConsumerId,
                        principalTable: "AppConsumerPersonalInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppPlotTransferHistories_AppPlotInfos_PlotId",
                        column: x => x.PlotId,
                        principalTable: "AppPlotInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppPlotTransferHistories_AppPlotInfos_PlotInfoId",
                        column: x => x.PlotInfoId,
                        principalTable: "AppPlotInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppBlocks_BlockCode",
                table: "AppBlocks",
                column: "BlockCode");

            migrationBuilder.CreateIndex(
                name: "IX_AppBlocks_BlockName",
                table: "AppBlocks",
                column: "BlockName");

            migrationBuilder.CreateIndex(
                name: "IX_AppBlocks_PhaseId",
                table: "AppBlocks",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_AppBlocks_TenantId",
                table: "AppBlocks",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AppConsumerDocumentDetails_ConsumerDocumentId",
                table: "AppConsumerDocumentDetails",
                column: "ConsumerDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppConsumerDocuments_ConsumerId",
                table: "AppConsumerDocuments",
                column: "ConsumerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppConsumerPersonalInfos_CNIC",
                table: "AppConsumerPersonalInfos",
                column: "CNIC");

            migrationBuilder.CreateIndex(
                name: "IX_AppConsumerPersonalInfos_Phone",
                table: "AppConsumerPersonalInfos",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_AppGovtCharges_CreationTime",
                table: "AppGovtCharges",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_AppIescoCharges_CreationTime",
                table: "AppIescoCharges",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_AppMeterInfos_MeterNo",
                table: "AppMeterInfos",
                column: "MeterNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppMeterInfos_MeterOwnerId",
                table: "AppMeterInfos",
                column: "MeterOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppMeterInfos_MeterStatus",
                table: "AppMeterInfos",
                column: "MeterStatus");

            migrationBuilder.CreateIndex(
                name: "IX_AppMeterInfos_MeterType",
                table: "AppMeterInfos",
                column: "MeterType");

            migrationBuilder.CreateIndex(
                name: "IX_AppMeterInfos_PhaseId",
                table: "AppMeterInfos",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_AppMeterInfos_PlotId",
                table: "AppMeterInfos",
                column: "PlotId");

            migrationBuilder.CreateIndex(
                name: "IX_AppMeterInfos_TenantId",
                table: "AppMeterInfos",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPhases_PhaseCode",
                table: "AppPhases",
                column: "PhaseCode");

            migrationBuilder.CreateIndex(
                name: "IX_AppPhases_PhaseName",
                table: "AppPhases",
                column: "PhaseName");

            migrationBuilder.CreateIndex(
                name: "IX_AppPhases_TenantId",
                table: "AppPhases",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotInfos_BlockId_PhaseId",
                table: "AppPlotInfos",
                columns: new[] { "BlockId", "PhaseId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotInfos_BlockId1",
                table: "AppPlotInfos",
                column: "BlockId1");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotInfos_ConsumerId",
                table: "AppPlotInfos",
                column: "ConsumerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotInfos_ConsumerPersonalInfoId",
                table: "AppPlotInfos",
                column: "ConsumerPersonalInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotInfos_PhaseId",
                table: "AppPlotInfos",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotInfos_PhaseId1",
                table: "AppPlotInfos",
                column: "PhaseId1");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotInfos_PlotNo",
                table: "AppPlotInfos",
                column: "PlotNo");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotInfos_PlotSizeId",
                table: "AppPlotInfos",
                column: "PlotSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotInfos_PlotSizeId1",
                table: "AppPlotInfos",
                column: "PlotSizeId1");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotInfos_TenantId",
                table: "AppPlotInfos",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotSizes_SizeName",
                table: "AppPlotSizes",
                column: "SizeName");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotSizes_TenantId",
                table: "AppPlotSizes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotSizes_Unit",
                table: "AppPlotSizes",
                column: "Unit");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotTransferHistories_ApprovedByUserId",
                table: "AppPlotTransferHistories",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotTransferHistories_ConsumerPersonalInfoId",
                table: "AppPlotTransferHistories",
                column: "ConsumerPersonalInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotTransferHistories_FromConsumerId",
                table: "AppPlotTransferHistories",
                column: "FromConsumerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotTransferHistories_PlotId",
                table: "AppPlotTransferHistories",
                column: "PlotId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotTransferHistories_PlotInfoId",
                table: "AppPlotTransferHistories",
                column: "PlotInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotTransferHistories_RegistryNo",
                table: "AppPlotTransferHistories",
                column: "RegistryNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotTransferHistories_RejectByUserId",
                table: "AppPlotTransferHistories",
                column: "RejectByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotTransferHistories_TenantId",
                table: "AppPlotTransferHistories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotTransferHistories_ToConsumerId",
                table: "AppPlotTransferHistories",
                column: "ToConsumerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotTransferHistories_TransferDate",
                table: "AppPlotTransferHistories",
                column: "TransferDate");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlotTransferHistories_TransferType",
                table: "AppPlotTransferHistories",
                column: "TransferType");

            migrationBuilder.CreateIndex(
                name: "IX_AppSocietyCharges_CreationTime",
                table: "AppSocietyCharges",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_AppSocietyCharges_PlotSizeId",
                table: "AppSocietyCharges",
                column: "PlotSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppTarrifSlabs_CreationTime",
                table: "AppTarrifSlabs",
                column: "CreationTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppConsumerDocumentDetails");

            migrationBuilder.DropTable(
                name: "AppGovtCharges");

            migrationBuilder.DropTable(
                name: "AppIescoCharges");

            migrationBuilder.DropTable(
                name: "AppMeterInfos");

            migrationBuilder.DropTable(
                name: "AppPlotTransferHistories");

            migrationBuilder.DropTable(
                name: "AppSocietyCharges");

            migrationBuilder.DropTable(
                name: "AppTarrifSlabs");

            migrationBuilder.DropTable(
                name: "AppConsumerDocuments");

            migrationBuilder.DropTable(
                name: "AppPlotInfos");

            migrationBuilder.DropTable(
                name: "AppBlocks");

            migrationBuilder.DropTable(
                name: "AppConsumerPersonalInfos");

            migrationBuilder.DropTable(
                name: "AppPlotSizes");

            migrationBuilder.DropTable(
                name: "AppPhases");
        }
    }
}
