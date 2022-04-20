SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [competition].[Photo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompetitionId] [int] NOT NULL,
	[PhotographerId] [int] NOT NULL,
	[Title] [varchar](64) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[StorageId] [uniqueidentifier] NULL,
	[FileName] [varchar](512) NULL
) ON [PRIMARY]
GO
ALTER TABLE [competition].[Photo] ADD  CONSTRAINT [PK_Photo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [competition].[Photo]  WITH CHECK ADD  CONSTRAINT [FK_Photo_Category] FOREIGN KEY([CategoryId])
REFERENCES [competition].[Category] ([Id])
GO
ALTER TABLE [competition].[Photo] CHECK CONSTRAINT [FK_Photo_Category]
GO
ALTER TABLE [competition].[Photo]  WITH CHECK ADD  CONSTRAINT [FK_Photo_Competition] FOREIGN KEY([CompetitionId])
REFERENCES [competition].[Competition] ([Id])
GO
ALTER TABLE [competition].[Photo] CHECK CONSTRAINT [FK_Photo_Competition]
GO
ALTER TABLE [competition].[Photo]  WITH CHECK ADD  CONSTRAINT [FK_Photo_Photographer] FOREIGN KEY([PhotographerId])
REFERENCES [contact].[Photographer] ([Id])
GO
ALTER TABLE [competition].[Photo] CHECK CONSTRAINT [FK_Photo_Photographer]
GO
