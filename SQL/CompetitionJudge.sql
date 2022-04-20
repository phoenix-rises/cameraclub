SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [competition].[CompetitionJudge](
	[CompetitionId] [int] NOT NULL,
	[JudgeId] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [competition].[CompetitionJudge] ADD  CONSTRAINT [PK_CompetitionJudge] PRIMARY KEY CLUSTERED 
(
	[CompetitionId] ASC,
	[JudgeId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [competition].[CompetitionJudge]  WITH CHECK ADD  CONSTRAINT [FK_CompetitionJudge_Competition] FOREIGN KEY([CompetitionId])
REFERENCES [competition].[Competition] ([Id])
GO
ALTER TABLE [competition].[CompetitionJudge] CHECK CONSTRAINT [FK_CompetitionJudge_Competition]
GO
ALTER TABLE [competition].[CompetitionJudge]  WITH CHECK ADD  CONSTRAINT [FK_CompetitionJudge_Judge] FOREIGN KEY([JudgeId])
REFERENCES [contact].[Judge] ([Id])
GO
ALTER TABLE [competition].[CompetitionJudge] CHECK CONSTRAINT [FK_CompetitionJudge_Judge]
GO
