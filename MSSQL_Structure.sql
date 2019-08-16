USE [master]
GO
/****** Object:  Database [CSBEF]    Script Date: 13.08.2019 05:59:41 ******/
CREATE DATABASE [CSBEF]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CSBEF', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\CSBEF.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CSBEF_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\CSBEF_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [CSBEF] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CSBEF].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CSBEF] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CSBEF] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CSBEF] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CSBEF] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CSBEF] SET ARITHABORT OFF 
GO
ALTER DATABASE [CSBEF] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CSBEF] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CSBEF] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CSBEF] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CSBEF] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CSBEF] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CSBEF] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CSBEF] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CSBEF] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CSBEF] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CSBEF] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CSBEF] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CSBEF] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CSBEF] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CSBEF] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CSBEF] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CSBEF] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CSBEF] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CSBEF] SET  MULTI_USER 
GO
ALTER DATABASE [CSBEF] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CSBEF] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CSBEF] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CSBEF] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CSBEF] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CSBEF] SET QUERY_STORE = OFF
GO
USE [CSBEF]
GO
/****** Object:  User [csbef_user]    Script Date: 13.08.2019 05:59:41 ******/
CREATE USER [csbef_user] FOR LOGIN [csbef_user] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [csbef_user]
GO
/****** Object:  Table [dbo].[UserManagement_Group]    Script Date: 13.08.2019 05:59:42 ******/
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
/****** Object:  Table [dbo].[UserManagement_GroupInRole]    Script Date: 13.08.2019 05:59:42 ******/
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
/****** Object:  Table [dbo].[UserManagement_Role]    Script Date: 13.08.2019 05:59:42 ******/
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
/****** Object:  Table [dbo].[UserManagement_Token]    Script Date: 13.08.2019 05:59:42 ******/
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
/****** Object:  Table [dbo].[UserManagement_User]    Script Date: 13.08.2019 05:59:42 ******/
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
/****** Object:  Table [dbo].[UserManagement_UserInGroup]    Script Date: 13.08.2019 05:59:42 ******/
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
/****** Object:  Table [dbo].[UserManagement_UserInRole]    Script Date: 13.08.2019 05:59:42 ******/
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
/****** Object:  Table [dbo].[UserManagement_UserMessage]    Script Date: 13.08.2019 05:59:42 ******/
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
USE [master]
GO
ALTER DATABASE [CSBEF] SET  READ_WRITE 
GO
