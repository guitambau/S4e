USE [master]
GO

/****** Object:  Table [dbo].[empresa_associado]    Script Date: 05/01/2024 10:59:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[empresa_associado](
	[associado_id] [int] NOT NULL,
	[empresa_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[associado_id] ASC,
	[empresa_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[empresa_associado]  WITH CHECK ADD FOREIGN KEY([associado_id])
REFERENCES [dbo].[associado] ([id])
GO

ALTER TABLE [dbo].[empresa_associado]  WITH CHECK ADD FOREIGN KEY([empresa_id])
REFERENCES [dbo].[empresa] ([id])
GO

