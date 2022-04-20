SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [contact].[Photographer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](32) NOT NULL,
	[LastName] [varchar](32) NOT NULL,
	[CompetitionNumber] [varchar](64) NULL,
	[Email] [varchar](256) NULL,
	[ClubNumber] [varchar](64) NULL
) ON [PRIMARY]
GO
ALTER TABLE [contact].[Photographer] ADD  CONSTRAINT [PK_Photographer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
