/****** Object:  Table [dbo].[UserManagement_Group]    Script Date: 17.05.2020 05:19:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserManagement_Group](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [nvarchar](256) NOT NULL,
	[Status] [bit] NULL,
	[AddingDate] [datetime] NOT NULL,
	[UpdatingDate] [datetime] NOT NULL,
	[AddingUserId] [int] NULL,
	[UpdatingUserId] [int] NULL,
 CONSTRAINT [PK_UserManagement_Group] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserManagement_GroupInRole]    Script Date: 17.05.2020 05:19:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserManagement_GroupInRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[Status] [bit] NULL,
	[AddingDate] [datetime] NOT NULL,
	[UpdatingDate] [datetime] NOT NULL,
	[AddingUserId] [int] NULL,
	[UpdatingUserId] [int] NULL,
 CONSTRAINT [PK_UserManagement_GroupInRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserManagement_Role]    Script Date: 17.05.2020 05:19:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserManagement_Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](max) NOT NULL,
	[RoleTitle] [nvarchar](256) NULL,
	[RoleDescription] [nvarchar](max) NULL,
	[Status] [bit] NULL,
	[AddingDate] [datetime] NOT NULL,
	[UpdatingDate] [datetime] NOT NULL,
	[AddingUserId] [int] NULL,
	[UpdatingUserId] [int] NULL,
 CONSTRAINT [PK_UserManagement_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserManagement_Token]    Script Date: 17.05.2020 05:19:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserManagement_Token](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[NotificationToken] [nvarchar](256) NULL,
	[TokenCode] [nvarchar](max) NOT NULL,
	[ExpiredDate] [datetime] NOT NULL,
	[Device] [nvarchar](256) NOT NULL,
	[DeviceKey] [nvarchar](256) NOT NULL,
	[Status] [bit] NULL,
	[AddingDate] [datetime] NOT NULL,
	[UpdatingDate] [datetime] NOT NULL,
	[AddingUserId] [int] NULL,
	[UpdatingUserId] [int] NULL,
 CONSTRAINT [PK_UserManagement_Token] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserManagement_User]    Script Date: 17.05.2020 05:19:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserManagement_User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Surname] [nvarchar](256) NULL,
	[ProfilePic] [nvarchar](256) NULL,
	[ProfileBgPic] [nvarchar](256) NULL,
	[ProfileStatusMessage] [nvarchar](512) NULL,
	[UserName] [nvarchar](32) NULL,
	[Email] [nvarchar](256) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Status] [bit] NULL,
	[AddingDate] [datetime] NOT NULL,
	[UpdatingDate] [datetime] NOT NULL,
	[AddingUserId] [int] NULL,
	[UpdatingUserId] [int] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserManagement_UserInGroup]    Script Date: 17.05.2020 05:19:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserManagement_UserInGroup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[GroupId] [int] NOT NULL,
	[Status] [bit] NULL,
	[AddingDate] [datetime] NOT NULL,
	[UpdatingDate] [datetime] NOT NULL,
	[AddingUserId] [int] NULL,
	[UpdatingUserId] [int] NULL,
 CONSTRAINT [PK_UserManagement_UserInGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserManagement_UserInRole]    Script Date: 17.05.2020 05:19:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserManagement_UserInRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[Status] [bit] NULL,
	[AddingDate] [datetime] NOT NULL,
	[UpdatingDate] [datetime] NOT NULL,
	[AddingUserId] [int] NULL,
	[UpdatingUserId] [int] NULL,
 CONSTRAINT [PK_UserManagement_UserInRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserManagement_UserMessage]    Script Date: 17.05.2020 05:19:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserManagement_UserMessage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FromUserId] [int] NOT NULL,
	[ToUserId] [int] NOT NULL,
	[Message] [nvarchar](4000) NOT NULL,
	[ViewStatus] [bit] NOT NULL,
	[ViewDate] [datetime] NULL,
	[Status] [bit] NULL,
	[AddingDate] [datetime] NOT NULL,
	[UpdatingDate] [datetime] NOT NULL,
	[AddingUserId] [int] NULL,
	[UpdatingUserId] [int] NULL,
 CONSTRAINT [PK_UserManagement_UserMessage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[UserManagement_Group] ON 
GO
INSERT [dbo].[UserManagement_Group] ([Id], [GroupName], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (1, N'Administrators', 1, CAST(N'2020-05-15T05:33:30.177' AS DateTime), CAST(N'2020-05-15T05:33:30.177' AS DateTime), 1, 1)
GO
SET IDENTITY_INSERT [dbo].[UserManagement_Group] OFF
GO
SET IDENTITY_INSERT [dbo].[UserManagement_GroupInRole] ON 
GO
INSERT [dbo].[UserManagement_GroupInRole] ([Id], [GroupId], [RoleId], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (1, 1, 1, 1, CAST(N'2020-05-15T05:34:20.670' AS DateTime), CAST(N'2020-05-15T05:34:20.670' AS DateTime), 1, 1)
GO
SET IDENTITY_INSERT [dbo].[UserManagement_GroupInRole] OFF
GO
SET IDENTITY_INSERT [dbo].[UserManagement_Role] ON 
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (1, N'Root', N'i18n:rolesDefinitions.Root.title', N'i18n:rolesDefinitions.Root.desc', 1, CAST(N'2019-09-01T14:44:13.250' AS DateTime), CAST(N'2019-09-01T14:44:13.250' AS DateTime), 1, 1)
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (2, N'Root.UserManagement', N'i18n:rolesDefinitions.Root_UserManagement.title', N'i18n:rolesDefinitions.Root_UserManagement.desc', 1, CAST(N'2019-09-01T14:44:27.243' AS DateTime), CAST(N'2019-09-01T14:44:27.243' AS DateTime), 1, 1)
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (3, N'Root.UserManagement.User', N'i18n:rolesDefinitions.Root_UserManagement_User.title', N'i18n:rolesDefinitions.Root_UserManagement_User.desc', 1, CAST(N'2019-09-01T14:44:43.483' AS DateTime), CAST(N'2019-09-01T14:44:43.483' AS DateTime), 1, 1)
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (4, N'Root.UserManagement.User.List', N'i18n:rolesDefinitions.Root_UserManagement_User_List.title', N'i18n:rolesDefinitions.Root_UserManagement_User_List.desc', 1, CAST(N'2019-09-01T14:45:02.440' AS DateTime), CAST(N'2019-09-01T14:45:02.440' AS DateTime), 1, 1)
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (5, N'Root.UserManagement.User.Add', N'i18n:rolesDefinitions.Root_UserManagement_User_Add.title', N'i18n:rolesDefinitions.Root_UserManagement_User_Add.desc', 1, CAST(N'2019-09-01T14:45:13.667' AS DateTime), CAST(N'2019-09-01T14:45:13.667' AS DateTime), 1, 1)
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (6, N'Root.UserManagement.User.Update', N'i18n:rolesDefinitions.Root_UserManagement_User_Update.title', N'i18n:rolesDefinitions.Root_UserManagement_User_Update.desc', 1, CAST(N'2019-09-01T14:45:25.590' AS DateTime), CAST(N'2019-09-01T14:45:25.590' AS DateTime), 1, 1)
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (7, N'Root.UserManagement.User.ChangeStatus', N'i18n:rolesDefinitions.Root_UserManagement_User_ChangeStatus.title', N'i18n:rolesDefinitions.Root_UserManagement_User_ChangeStatus.desc', 1, CAST(N'2019-09-01T14:45:38.100' AS DateTime), CAST(N'2019-09-01T14:45:38.100' AS DateTime), 1, 1)
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (8, N'Root.UserManagement.User.ChangeRoles', N'i18n:rolesDefinitions.Root_UserManagement_User_ChangeRoles.title', N'i18n:rolesDefinitions.Root_UserManagement_User_ChangeRoles.desc', 1, CAST(N'2019-09-01T14:45:51.653' AS DateTime), CAST(N'2019-09-01T14:45:51.653' AS DateTime), 1, 1)
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (9, N'Root.UserManagement.User.UserInGroup', N'i18n:rolesDefinitions.Root_UserManagement_User_UserInGroup.title', N'i18n:rolesDefinitions.Root_UserManagement_User_UserInGroup.desc', 1, CAST(N'2019-09-01T14:46:08.357' AS DateTime), CAST(N'2019-09-01T14:46:08.357' AS DateTime), 1, 1)
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (10, N'Root.UserManagement.Group', N'i18n:rolesDefinitions.Root_UserManagement_Group.title', N'i18n:rolesDefinitions.Root_UserManagement_Group.desc', 1, CAST(N'2019-09-01T14:46:23.440' AS DateTime), CAST(N'2019-09-01T14:46:23.440' AS DateTime), 1, 1)
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (11, N'Root.UserManagement.Group.List', N'i18n:rolesDefinitions.Root_UserManagement_Group_List.title', N'i18n:rolesDefinitions.Root_UserManagement_Group_List.desc', 1, CAST(N'2019-09-01T14:46:34.310' AS DateTime), CAST(N'2019-09-01T14:46:34.310' AS DateTime), 1, 1)
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (12, N'Root.UserManagement.Group.Add', N'i18n:rolesDefinitions.Root_UserManagement_Group_Add.title', N'i18n:rolesDefinitions.Root_UserManagement_Group_Add.desc', 1, CAST(N'2019-09-01T14:46:44.510' AS DateTime), CAST(N'2019-09-01T14:46:44.510' AS DateTime), 1, 1)
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (13, N'Root.UserManagement.Group.Update', N'i18n:rolesDefinitions.Root_UserManagement_Group_Update.title', N'i18n:rolesDefinitions.Root_UserManagement_Group_Update.desc', 1, CAST(N'2019-09-01T14:46:54.753' AS DateTime), CAST(N'2019-09-01T14:46:54.753' AS DateTime), 1, 1)
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (14, N'Root.UserManagement.Group.ChangeStatus', N'i18n:rolesDefinitions.Root_UserManagement_Group_ChangeStatus.title', N'i18n:rolesDefinitions.Root_UserManagement_Group_ChangeStatus.desc', 1, CAST(N'2019-09-01T14:47:04.647' AS DateTime), CAST(N'2019-09-01T14:47:04.647' AS DateTime), 1, 1)
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (15, N'Root.UserManagement.Group.ChangeRoles', N'i18n:rolesDefinitions.Root_UserManagement_Group_ChangeRoles.title', N'i18n:rolesDefinitions.Root_UserManagement_Group_ChangeRoles.desc', 1, CAST(N'2019-09-01T14:47:32.800' AS DateTime), CAST(N'2019-09-01T14:47:32.800' AS DateTime), 1, 1)
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (16, N'Root.UserManagement.Role', N'i18n:rolesDefinitions.Root_UserManagement_Role.title', N'i18n:rolesDefinitions.Root_UserManagement_Role.desc', 1, CAST(N'2019-09-01T14:47:46.273' AS DateTime), CAST(N'2019-09-01T14:47:46.273' AS DateTime), 1, 1)
GO
INSERT [dbo].[UserManagement_Role] ([Id], [RoleName], [RoleTitle], [RoleDescription], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (17, N'Root.UserManagement.Role.List', N'i18n:rolesDefinitions.Root_UserManagement_Role_List.title', N'i18n:rolesDefinitions.Root_UserManagement_Role_List.desc', 1, CAST(N'2019-09-01T14:47:57.763' AS DateTime), CAST(N'2019-09-01T14:47:57.763' AS DateTime), 1, 1)
GO
SET IDENTITY_INSERT [dbo].[UserManagement_Role] OFF
GO
SET IDENTITY_INSERT [dbo].[UserManagement_Token] ON 
GO
INSERT [dbo].[UserManagement_Token] ([Id], [UserId], [NotificationToken], [TokenCode], [ExpiredDate], [Device], [DeviceKey], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (1, 1, NULL, N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEiLCJlbWFpbCI6InRlc3R1c2VyQGNvZGVzY2llbnRpZmljLmNvbSIsImdpdmVuX25hbWUiOiJFa29Gb29kVnVlQXBwIiwiY2VydHNlcmlhbG51bWJlciI6IkVrb0Zvb2RWdWVBcHAiLCJVc2VyTmFtZSI6IlRlc3QiLCJVc2VyU3VybmFtZSI6IlVzZXIiLCJQcm9maWxlUGljIjoiZGVmYXVsdC5qcGciLCJQcm9maWxlQmdQaWMiOiJkZWZhdWx0LmpwZyIsIlByb2ZpbGVTdGF0dXNNZXNzYWdlIjoiSGVsbG8gV29ybGQhIiwiVG9rZW5JZCI6IjEiLCJyb2xlIjoiUm9vdCIsIm5iZiI6MTU4OTUxMjA5OCwiZXhwIjoxNjIxMDQ4MDk4LCJpYXQiOjE1ODk1MTIwOTgsImlzcyI6IkpXVC5DU0JFRiIsImF1ZCI6IkNTQkVGIn0.zAclF2O-ANJHdt_aRyN0-L1wce2mDg3sXrW8STSABDo', CAST(N'2021-05-15T03:08:18.487' AS DateTime), N'EkoFoodVueApp', N'EkoFoodVueApp', 1, CAST(N'2020-05-15T06:08:18.487' AS DateTime), CAST(N'2020-05-15T06:08:18.487' AS DateTime), 1, 1)
GO
SET IDENTITY_INSERT [dbo].[UserManagement_Token] OFF
GO
SET IDENTITY_INSERT [dbo].[UserManagement_User] ON 
GO
INSERT [dbo].[UserManagement_User] ([Id], [Name], [Surname], [ProfilePic], [ProfileBgPic], [ProfileStatusMessage], [UserName], [Email], [Password], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (1, N'Test', N'User', N'default.jpg', N'default.jpg', N'Hello World!', N'testuser', N'testuser@codescientific.com', N'53FF9CE839143638049B9C83D9F1500F2D929154', 1, CAST(N'2020-05-15T05:33:07.930' AS DateTime), CAST(N'2020-05-15T05:33:07.930' AS DateTime), 1, 1)
GO
SET IDENTITY_INSERT [dbo].[UserManagement_User] OFF
GO
SET IDENTITY_INSERT [dbo].[UserManagement_UserInGroup] ON 
GO
INSERT [dbo].[UserManagement_UserInGroup] ([Id], [UserId], [GroupId], [Status], [AddingDate], [UpdatingDate], [AddingUserId], [UpdatingUserId]) VALUES (1, 1, 1, 1, CAST(N'2020-05-15T05:34:33.830' AS DateTime), CAST(N'2020-05-15T05:34:33.830' AS DateTime), 1, 1)
GO
SET IDENTITY_INSERT [dbo].[UserManagement_UserInGroup] OFF
GO
/****** Object:  Index [IX_UserManagement_UserMessage_FromUserId]    Script Date: 17.05.2020 05:19:27 ******/
CREATE NONCLUSTERED INDEX [IX_UserManagement_UserMessage_FromUserId] ON [dbo].[UserManagement_UserMessage]
(
	[FromUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserManagement_UserMessage_ToUserId]    Script Date: 17.05.2020 05:19:27 ******/
CREATE NONCLUSTERED INDEX [IX_UserManagement_UserMessage_ToUserId] ON [dbo].[UserManagement_UserMessage]
(
	[ToUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserManagement_Group] ADD  CONSTRAINT [DF_UserManagement_Group_Status_1]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[UserManagement_Group] ADD  CONSTRAINT [DF_UserManagement_Group_AddingDate_1]  DEFAULT (getdate()) FOR [AddingDate]
GO
ALTER TABLE [dbo].[UserManagement_Group] ADD  CONSTRAINT [DF_UserManagement_Group_UpdatingDate_1]  DEFAULT (getdate()) FOR [UpdatingDate]
GO
ALTER TABLE [dbo].[UserManagement_GroupInRole] ADD  CONSTRAINT [DF_UserManagement_GroupInRole_Status_1]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[UserManagement_GroupInRole] ADD  CONSTRAINT [DF_UserManagement_GroupInRole_AddingDate_1]  DEFAULT (getdate()) FOR [AddingDate]
GO
ALTER TABLE [dbo].[UserManagement_GroupInRole] ADD  CONSTRAINT [DF_UserManagement_GroupInRole_UpdatingDate_1]  DEFAULT (getdate()) FOR [UpdatingDate]
GO
ALTER TABLE [dbo].[UserManagement_Role] ADD  CONSTRAINT [DF_UserManagement_Role_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[UserManagement_Role] ADD  CONSTRAINT [DF_UserManagement_Role_AddingDate]  DEFAULT (getdate()) FOR [AddingDate]
GO
ALTER TABLE [dbo].[UserManagement_Role] ADD  CONSTRAINT [DF_UserManagement_Role_UpdatingDate]  DEFAULT (getdate()) FOR [UpdatingDate]
GO
ALTER TABLE [dbo].[UserManagement_Token] ADD  CONSTRAINT [DF_UserManagement_Token_Status_1]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[UserManagement_Token] ADD  CONSTRAINT [DF_UserManagement_Token_AddingDate_1]  DEFAULT (getdate()) FOR [AddingDate]
GO
ALTER TABLE [dbo].[UserManagement_Token] ADD  CONSTRAINT [DF_UserManagement_Token_UpdatingDate_1]  DEFAULT (getdate()) FOR [UpdatingDate]
GO
ALTER TABLE [dbo].[UserManagement_User] ADD  CONSTRAINT [DF_User_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[UserManagement_User] ADD  CONSTRAINT [DF_User_AddingDate]  DEFAULT (getdate()) FOR [AddingDate]
GO
ALTER TABLE [dbo].[UserManagement_User] ADD  CONSTRAINT [DF_UserManagement_User_UpdatingDate]  DEFAULT (getdate()) FOR [UpdatingDate]
GO
ALTER TABLE [dbo].[UserManagement_UserInGroup] ADD  CONSTRAINT [DF_UserManagement_UserInGroup_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[UserManagement_UserInGroup] ADD  CONSTRAINT [DF_UserManagement_UserInGroup_AddingDate]  DEFAULT (getdate()) FOR [AddingDate]
GO
ALTER TABLE [dbo].[UserManagement_UserInGroup] ADD  CONSTRAINT [DF_UserManagement_UserInGroup_UpdatingDate]  DEFAULT (getdate()) FOR [UpdatingDate]
GO
ALTER TABLE [dbo].[UserManagement_UserInRole] ADD  CONSTRAINT [DF_UserManagement_UserInRole_Status_1]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[UserManagement_UserInRole] ADD  CONSTRAINT [DF_UserManagement_UserInRole_AddingDate_1]  DEFAULT (getdate()) FOR [AddingDate]
GO
ALTER TABLE [dbo].[UserManagement_UserInRole] ADD  CONSTRAINT [DF_UserManagement_UserInRole_UpdatingDate_1]  DEFAULT (getdate()) FOR [UpdatingDate]
GO
ALTER TABLE [dbo].[UserManagement_UserMessage] ADD  CONSTRAINT [DF_UserManagement_UserMessage_ViewStatus]  DEFAULT ((0)) FOR [ViewStatus]
GO
ALTER TABLE [dbo].[UserManagement_UserMessage] ADD  CONSTRAINT [DF_UserManagement_UserMessage_Status_1]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[UserManagement_UserMessage] ADD  CONSTRAINT [DF_UserManagement_UserMessage_AddingDate_1]  DEFAULT (getdate()) FOR [AddingDate]
GO
ALTER TABLE [dbo].[UserManagement_UserMessage] ADD  CONSTRAINT [DF_UserManagement_UserMessage_UpdatingDate_1]  DEFAULT (getdate()) FOR [UpdatingDate]
GO
ALTER TABLE [dbo].[UserManagement_GroupInRole]  WITH CHECK ADD  CONSTRAINT [FK_UserManagement_GroupInRole_UserManagement_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[UserManagement_Group] ([Id])
GO
ALTER TABLE [dbo].[UserManagement_GroupInRole] CHECK CONSTRAINT [FK_UserManagement_GroupInRole_UserManagement_Group]
GO
ALTER TABLE [dbo].[UserManagement_GroupInRole]  WITH CHECK ADD  CONSTRAINT [FK_UserManagement_GroupInRole_UserManagement_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[UserManagement_Role] ([Id])
GO
ALTER TABLE [dbo].[UserManagement_GroupInRole] CHECK CONSTRAINT [FK_UserManagement_GroupInRole_UserManagement_Role]
GO
ALTER TABLE [dbo].[UserManagement_Token]  WITH CHECK ADD  CONSTRAINT [FK_UserManagement_Token_UserManagement_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserManagement_User] ([Id])
GO
ALTER TABLE [dbo].[UserManagement_Token] CHECK CONSTRAINT [FK_UserManagement_Token_UserManagement_User]
GO
ALTER TABLE [dbo].[UserManagement_UserInGroup]  WITH CHECK ADD  CONSTRAINT [FK_UserManagement_UserInGroup_UserManagement_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[UserManagement_Group] ([Id])
GO
ALTER TABLE [dbo].[UserManagement_UserInGroup] CHECK CONSTRAINT [FK_UserManagement_UserInGroup_UserManagement_Group]
GO
ALTER TABLE [dbo].[UserManagement_UserInGroup]  WITH CHECK ADD  CONSTRAINT [FK_UserManagement_UserInGroup_UserManagement_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserManagement_User] ([Id])
GO
ALTER TABLE [dbo].[UserManagement_UserInGroup] CHECK CONSTRAINT [FK_UserManagement_UserInGroup_UserManagement_User]
GO
ALTER TABLE [dbo].[UserManagement_UserInRole]  WITH CHECK ADD  CONSTRAINT [FK_UserManagement_UserInRole_UserManagement_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[UserManagement_Role] ([Id])
GO
ALTER TABLE [dbo].[UserManagement_UserInRole] CHECK CONSTRAINT [FK_UserManagement_UserInRole_UserManagement_Role]
GO
ALTER TABLE [dbo].[UserManagement_UserInRole]  WITH CHECK ADD  CONSTRAINT [FK_UserManagement_UserInRole_UserManagement_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserManagement_User] ([Id])
GO
ALTER TABLE [dbo].[UserManagement_UserInRole] CHECK CONSTRAINT [FK_UserManagement_UserInRole_UserManagement_User]
GO
ALTER TABLE [dbo].[UserManagement_UserMessage]  WITH CHECK ADD  CONSTRAINT [FK_UserManagement_UserMessage_UserManagement_User_FromUserId] FOREIGN KEY([FromUserId])
REFERENCES [dbo].[UserManagement_User] ([Id])
GO
ALTER TABLE [dbo].[UserManagement_UserMessage] CHECK CONSTRAINT [FK_UserManagement_UserMessage_UserManagement_User_FromUserId]
GO
ALTER TABLE [dbo].[UserManagement_UserMessage]  WITH CHECK ADD  CONSTRAINT [FK_UserManagement_UserMessage_UserManagement_User_ToUserId] FOREIGN KEY([ToUserId])
REFERENCES [dbo].[UserManagement_User] ([Id])
GO
ALTER TABLE [dbo].[UserManagement_UserMessage] CHECK CONSTRAINT [FK_UserManagement_UserMessage_UserManagement_User_ToUserId]
GO
