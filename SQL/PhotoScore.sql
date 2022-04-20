SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [competition].[PhotoScore](
	[PhotoId] [int] NOT NULL,
	[JudgeId] [int] NOT NULL,
	[Round] [int] NOT NULL,
	[Score] [varchar](16) NOT NULL,
	[Rank] [varchar](32) NULL
) ON [PRIMARY]
GO
ALTER TABLE [competition].[PhotoScore] ADD  CONSTRAINT [PK_PhotoScore] PRIMARY KEY CLUSTERED 
(
	[PhotoId] ASC,
	[JudgeId] ASC,
	[Round] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [competition].[PhotoScore]  WITH CHECK ADD  CONSTRAINT [FK_PhotoScore_Judge] FOREIGN KEY([JudgeId])
REFERENCES [contact].[Judge] ([Id])
GO
ALTER TABLE [competition].[PhotoScore] CHECK CONSTRAINT [FK_PhotoScore_Judge]
GO
ALTER TABLE [competition].[PhotoScore]  WITH CHECK ADD  CONSTRAINT [FK_PhotoScore_Photo] FOREIGN KEY([PhotoId])
REFERENCES [competition].[Photo] ([Id])
GO
ALTER TABLE [competition].[PhotoScore] CHECK CONSTRAINT [FK_PhotoScore_Photo]
GO
