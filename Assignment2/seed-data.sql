INSERT [dbo].[Purchases] ([WebsiteId], [ProductId], [UnitOfMeasureId], [PurchaseStatus])
VALUES (N'Store1', N'Apples', N'Box', N'InProgress')
GO
INSERT [dbo].[Purchases] ([WebsiteId], [ProductId], [UnitOfMeasureId], [PurchaseStatus])
VALUES (N'Store1', N'Apples', N'Piece', N'Paid')
GO
INSERT [dbo].[Purchases] ([WebsiteId], [ProductId], [UnitOfMeasureId], [PurchaseStatus])
VALUES (N'Store1', N'Bicycle', N'Piece', N'Paid')
GO
INSERT [dbo].[Purchases] ([WebsiteId], [ProductId], [UnitOfMeasureId], [PurchaseStatus])
VALUES (N'Store2', N'Apples', N'Piece', N'InProgress')
GO
INSERT [dbo].[Purchases] ([WebsiteId], [ProductId], [UnitOfMeasureId], [PurchaseStatus])
VALUES (N'Store2', N'Carrots', N'Box', N'Paid')
GO


INSERT [dbo].[PurchasesSnapshot] ([WebsiteId], [ProductId], [UnitOfMeasureId], [PurchaseStatus], [ProcessingDate])
VALUES (N'Store1', N'Apples', N'Box', N'InProgress', CAST(N'2025-04-15T11:40:18.327' AS DateTime))
GO
INSERT [dbo].[PurchasesSnapshot] ([WebsiteId], [ProductId], [UnitOfMeasureId], [PurchaseStatus], [ProcessingDate])
VALUES (N'Store1', N'Bicycle', N'Piece', N'Paid', CAST(N'2025-04-15T11:40:18.327' AS DateTime))
GO
INSERT [dbo].[PurchasesSnapshot] ([WebsiteId], [ProductId], [UnitOfMeasureId], [PurchaseStatus], [ProcessingDate])
VALUES (N'Store2', N'Apples', N'Piece', N'InProgress', CAST(N'2025-04-15T11:40:18.327' AS DateTime))
GO
