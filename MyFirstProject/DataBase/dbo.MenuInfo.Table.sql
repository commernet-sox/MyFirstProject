USE [BI]
GO
/****** Object:  Table [dbo].[MenuInfo]    Script Date: 2020/5/22 16:16:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TitleId] [varchar](50) NOT NULL,
	[Title] [varchar](50) NOT NULL,
	[ContentId] [varchar](50) NOT NULL,
	[Content] [varchar](50) NOT NULL,
 CONSTRAINT [PK_MenuInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[MenuInfo] ON 

INSERT [dbo].[MenuInfo] ([Id], [TitleId], [Title], [ContentId], [Content]) VALUES (1, N'1', N'系统数据', N'1', N'菜单管理')
INSERT [dbo].[MenuInfo] ([Id], [TitleId], [Title], [ContentId], [Content]) VALUES (3, N'2', N'基础数据', N'1', N'企业基本信息')
INSERT [dbo].[MenuInfo] ([Id], [TitleId], [Title], [ContentId], [Content]) VALUES (5, N'2', N'基础数据', N'2', N'企业资质信息')
INSERT [dbo].[MenuInfo] ([Id], [TitleId], [Title], [ContentId], [Content]) VALUES (6, N'2', N'基础数据', N'3', N'建造师信息')
INSERT [dbo].[MenuInfo] ([Id], [TitleId], [Title], [ContentId], [Content]) VALUES (7, N'2', N'基础数据', N'4', N'企业全部信息')
INSERT [dbo].[MenuInfo] ([Id], [TitleId], [Title], [ContentId], [Content]) VALUES (8, N'1', N'系统数据', N'2', N'测试数据')
SET IDENTITY_INSERT [dbo].[MenuInfo] OFF
