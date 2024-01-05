USE [master]
GO

/****** Object:  Table [dbo].[associado]    Script Date: 05/01/2024 11:12:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[associado](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nome] [varchar](200) NOT NULL,
	[cpf] [varchar](11) NOT NULL,
	[data_nascimento] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

