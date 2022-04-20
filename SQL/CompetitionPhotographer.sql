SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [competition].[CompetitionPhotographer](
	[CompetitionId] [int] NOT NULL,
	[PhotographerId] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [competition].[CompetitionPhotographer] ADD  CONSTRAINT [PK_CompetitionPhotographer] PRIMARY KEY CLUSTERED 
(
	[CompetitionId] ASC,
	[PhotographerId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [competition].[CompetitionPhotographer]  WITH CHECK ADD  CONSTRAINT [FK_CompetitionPhotographer_Competition] FOREIGN KEY([CompetitionId])
REFERENCES [competition].[Competition] ([Id])
GO
ALTER TABLE [competition].[CompetitionPhotographer] CHECK CONSTRAINT [FK_CompetitionPhotographer_Competition]
GO
ALTER TABLE [competition].[CompetitionPhotographer]  WITH CHECK ADD  CONSTRAINT [FK_CompetitionPhotographer_Photographer] FOREIGN KEY([PhotographerId])
REFERENCES [contact].[Photographer] ([Id])
GO
ALTER TABLE [competition].[CompetitionPhotographer] CHECK CONSTRAINT [FK_CompetitionPhotographer_Photographer]
GO
