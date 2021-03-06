USE [master]
GO

/****** Object:  Database [dbEndososPartidos]    Script Date: 10/17/2015 10:19:36 AM ******/
CREATE DATABASE [dbEndososPartidos]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'dbEndososPartidos', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\dbEndososPartidos.mdf' , SIZE = 3264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'dbEndososPartidos_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\dbEndososPartidos.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [dbEndososPartidos] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [dbEndososPartidos].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [dbEndososPartidos] SET ANSI_NULL_DEFAULT ON 
GO

ALTER DATABASE [dbEndososPartidos] SET ANSI_NULLS ON 
GO

ALTER DATABASE [dbEndososPartidos] SET ANSI_PADDING ON 
GO

ALTER DATABASE [dbEndososPartidos] SET ANSI_WARNINGS ON 
GO

ALTER DATABASE [dbEndososPartidos] SET ARITHABORT ON 
GO

ALTER DATABASE [dbEndososPartidos] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [dbEndososPartidos] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [dbEndososPartidos] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [dbEndososPartidos] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [dbEndososPartidos] SET CURSOR_DEFAULT  LOCAL 
GO

ALTER DATABASE [dbEndososPartidos] SET CONCAT_NULL_YIELDS_NULL ON 
GO

ALTER DATABASE [dbEndososPartidos] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [dbEndososPartidos] SET QUOTED_IDENTIFIER ON 
GO

ALTER DATABASE [dbEndososPartidos] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [dbEndososPartidos] SET  DISABLE_BROKER 
GO

ALTER DATABASE [dbEndososPartidos] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [dbEndososPartidos] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [dbEndososPartidos] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [dbEndososPartidos] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [dbEndososPartidos] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [dbEndososPartidos] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [dbEndososPartidos] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [dbEndososPartidos] SET RECOVERY FULL 
GO

ALTER DATABASE [dbEndososPartidos] SET  MULTI_USER 
GO

ALTER DATABASE [dbEndososPartidos] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [dbEndososPartidos] SET DB_CHAINING OFF 
GO

ALTER DATABASE [dbEndososPartidos] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [dbEndososPartidos] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [dbEndososPartidos] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [dbEndososPartidos] SET  READ_WRITE 
GO


