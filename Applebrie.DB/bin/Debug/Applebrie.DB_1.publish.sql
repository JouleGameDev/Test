﻿/*
Deployment script for Applebrie-db

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "Applebrie-db"
:setvar DefaultFilePrefix "Applebrie-db"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Starting rebuilding table [users].[Users]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [users].[tmp_ms_xx_Users] (
    [Id]      INT        IDENTITY (1, 1) NOT NULL,
    [Name]    NCHAR (25) NOT NULL,
    [Sex]     BIT        NOT NULL,
    [Role]    TINYINT    NOT NULL,
    [Status]  TINYINT    NOT NULL,
    [Created] DATETIME   NOT NULL,
    [Updated] DATETIME   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [users].[Users])
    BEGIN
        SET IDENTITY_INSERT [users].[tmp_ms_xx_Users] ON;
        INSERT INTO [users].[tmp_ms_xx_Users] ([Id], [Name], [Sex], [Role], [Status], [Created], [Updated])
        SELECT   [Id],
                 [Name],
                 [Sex],
                 [Role],
                 [Status],
                 [Created],
                 [Updated]
        FROM     [users].[Users]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [users].[tmp_ms_xx_Users] OFF;
    END

DROP TABLE [users].[Users];

EXECUTE sp_rename N'[users].[tmp_ms_xx_Users]', N'Users';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Refreshing Procedure [users].[CreateUser]...';


GO
EXECUTE sp_refreshsqlmodule N'[users].[CreateUser]';


GO
PRINT N'Refreshing Procedure [users].[GetUsers]...';


GO
EXECUTE sp_refreshsqlmodule N'[users].[GetUsers]';


GO
PRINT N'Refreshing Procedure [users].[UpsertUsers]...';


GO
EXECUTE sp_refreshsqlmodule N'[users].[UpsertUsers]';


GO
PRINT N'Update complete.';


GO
