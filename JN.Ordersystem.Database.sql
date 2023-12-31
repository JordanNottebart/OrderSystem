USE [master]
GO
/****** Object:  Database [JN.Ordersystem]    Script Date: 19/06/2023 15:19:30 ******/
CREATE DATABASE [JN.Ordersystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'JN.Ordersystem', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\JN.Ordersystem.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'JN.Ordersystem_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\JN.Ordersystem_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [JN.Ordersystem] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [JN.Ordersystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [JN.Ordersystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [JN.Ordersystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [JN.Ordersystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET  DISABLE_BROKER 
GO
ALTER DATABASE [JN.Ordersystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [JN.Ordersystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [JN.Ordersystem] SET  MULTI_USER 
GO
ALTER DATABASE [JN.Ordersystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [JN.Ordersystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [JN.Ordersystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [JN.Ordersystem] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [JN.Ordersystem] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [JN.Ordersystem] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [JN.Ordersystem] SET QUERY_STORE = OFF
GO
USE [JN.Ordersystem]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 19/06/2023 15:19:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerLastName] [nvarchar](50) NOT NULL,
	[CustomerFirstName] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](70) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[PostalCode] [nvarchar](10) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 19/06/2023 15:19:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_OrderDetail] PRIMARY KEY CLUSTERED 
(
	[OrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 19/06/2023 15:19:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[OrderDate] [smalldatetime] NOT NULL,
	[CustomerID] [int] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 19/06/2023 15:19:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ItemName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Price] [decimal](20, 2) NOT NULL,
	[UnitsInStock] [int] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suppliers]    Script Date: 19/06/2023 15:19:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suppliers](
	[SupplierID] [int] IDENTITY(1,1) NOT NULL,
	[SupplierName] [nvarchar](50) NOT NULL,
	[ContactInfo] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Orders]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Product]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Customer] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Customer]
GO
USE [master]
GO
ALTER DATABASE [JN.Ordersystem] SET  READ_WRITE 
GO
