﻿CREATE TABLE [user_types]
(
    [Id] INT IDENTITY(1,1) PRIMARY KEY, 
    [name] VARCHAR(30) NOT NULL UNIQUE
)
