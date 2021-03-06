USE [BI]
GO
/****** Object:  Table [dbo].[UserMenuRole]    Script Date: 2020/5/22 16:16:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserMenuRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Creator] [varchar](50) NOT NULL,
	[Content] [varchar](50) NOT NULL,
	[UserId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_UserMenuRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[UserMenuRole] ON 

INSERT [dbo].[UserMenuRole] ([Id], [Creator], [Content], [UserId]) VALUES (11, N'123', N'8', N'1')
INSERT [dbo].[UserMenuRole] ([Id], [Creator], [Content], [UserId]) VALUES (12, N'123', N'3', N'1')
INSERT [dbo].[UserMenuRole] ([Id], [Creator], [Content], [UserId]) VALUES (13, N'123', N'5', N'1')
INSERT [dbo].[UserMenuRole] ([Id], [Creator], [Content], [UserId]) VALUES (14, N'123', N'6', N'1')
INSERT [dbo].[UserMenuRole] ([Id], [Creator], [Content], [UserId]) VALUES (15, N'123', N'7', N'1')
INSERT [dbo].[UserMenuRole] ([Id], [Creator], [Content], [UserId]) VALUES (16, N'123', N'1', N'1')
INSERT [dbo].[UserMenuRole] ([Id], [Creator], [Content], [UserId]) VALUES (17, N'123', N'3', N'3')
INSERT [dbo].[UserMenuRole] ([Id], [Creator], [Content], [UserId]) VALUES (18, N'123', N'5', N'3')
INSERT [dbo].[UserMenuRole] ([Id], [Creator], [Content], [UserId]) VALUES (19, N'123', N'6', N'3')
INSERT [dbo].[UserMenuRole] ([Id], [Creator], [Content], [UserId]) VALUES (20, N'123', N'7', N'3')
INSERT [dbo].[UserMenuRole] ([Id], [Creator], [Content], [UserId]) VALUES (21, N'123', N'8', N'3')
INSERT [dbo].[UserMenuRole] ([Id], [Creator], [Content], [UserId]) VALUES (22, N'123', N'1', N'3')
SET IDENTITY_INSERT [dbo].[UserMenuRole] OFF
