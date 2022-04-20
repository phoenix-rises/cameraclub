SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [competition].[Competition](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](32) NOT NULL,
	[Date] [date] NOT NULL,
	[HasDigital] [bit] NOT NULL,
	[HasPrint] [bit] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [competition].[Competition] ADD  CONSTRAINT [PK_Competition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [competition].[Competition] ADD  CONSTRAINT [DF_Competition_HasDigital]  DEFAULT ((0)) FOR [HasDigital]
GO
ALTER TABLE [competition].[Competition] ADD  CONSTRAINT [DF_Competition_HasPrint]  DEFAULT ((0)) FOR [HasPrint]
GO
