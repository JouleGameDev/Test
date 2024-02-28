﻿CREATE TABLE [users].[Users]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [Name] NCHAR(25) NOT NULL, 
    [Sex] TINYINT NOT NULL, 
    [Role] TINYINT NOT NULL, 
    [Status] TINYINT NOT NULL, 
    [Created] DATETIME NOT NULL, 
    [Updated] DATETIME NOT NULL
)
