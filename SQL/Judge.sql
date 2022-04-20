SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [contact].[Judge](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](32) NOT NULL,
	[LastName] [varchar](32) NOT NULL,
	[Email] [varchar](256) NULL,
	[Bio] [varchar](512) NULL,
	[PhoneNumber] [varchar](11) NULL
) ON [PRIMARY]
GO
ALTER TABLE [contact].[Judge] ADD  CONSTRAINT [PK_Judge] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
