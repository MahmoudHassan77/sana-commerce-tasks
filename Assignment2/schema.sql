CREATE TABLE [dbo].[Purchases](
    [WebsiteId] [nvarchar](50) NOT NULL,
    [ProductId] [nvarchar](50) NOT NULL,
    [UnitOfMeasureId] [nvarchar](50) NOT NULL,
    [PurchaseStatus] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_Purchases] PRIMARY KEY CLUSTERED
    (
        [WebsiteId] ASC,
        [ProductId] ASC,
        [UnitOfMeasureId] ASC
    ) WITH (
        PAD_INDEX = OFF,
        STATISTICS_NORECOMPUTE = OFF,
        IGNORE_DUP_KEY = OFF,
        ALLOW_ROW_LOCKS = ON,
        ALLOW_PAGE_LOCKS = ON,
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
    ) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PurchasesSnapshot](
    [WebsiteId] [nvarchar](50) NOT NULL,
    [ProductId] [nvarchar](50) NOT NULL,
    [UnitOfMeasureId] [nvarchar](50) NOT NULL,
    [PurchaseStatus] [nvarchar](50) NOT NULL,
    [ProcessingDate] [datetime] NOT NULL,
    CONSTRAINT [PK_PurchasesSnapshot] PRIMARY KEY CLUSTERED
    (
        [WebsiteId] ASC,
        [ProductId] ASC,
        [UnitOfMeasureId] ASC
    ) WITH (
        PAD_INDEX = OFF,
        STATISTICS_NORECOMPUTE = OFF,
        IGNORE_DUP_KEY = OFF,
        ALLOW_ROW_LOCKS = ON,
        ALLOW_PAGE_LOCKS = ON,
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
    ) ON [PRIMARY]
) ON [PRIMARY]
GO
