CREATE TABLE [oNotifications] (
  [notId] integer PRIMARY KEY,
  [userId] integer,
  [notMessage] varchar(255),
  [notDescription] varchar(255),
  [notType] integer
)
GO

CREATE TABLE [oPermissions] (
  [user_id] integer,
  [id] integer PRIMARY KEY,
  [Type] varchar(255),
  [Description] text
)
GO

CREATE TABLE [oEmployee] (
  [empId] integer PRIMARY KEY,
  [empName] nvarchar(255),
  [empSurname] nvarchar(255),
  [empLegajo] nvarchar(255),
  [empCard] nvarchar(255),
  [empIdHikVision] bit,
  [empDelete] bit
)
GO

CREATE TABLE [oWorkShift] (
  [empId] integer,
  [turId] integer PRIMARY KEY,
  [turName] varchar(255),
  [turDesciption] varchar(255),
  [turInit] datetime,
  [turEnd] datetime,
  [turDelete] bit
)
GO

CREATE TABLE [oWorkShift_Segment] (
  [wsId] integer PRIMARY KEY,
  [turId] integer,
  [wsName] varchar(255),
  [wsDescription] varchar(255),
  [wsInit] datetime,
  [wsEnd] datetime,
  [wsDelete] bit
)
GO

CREATE TABLE [oMarcations] (
  [marId] integer PRIMARY KEY,
  [empId] integer,
  [marcCard] varchar(255),
  [marcHikId] varchar(255),
  [marcDirection] int,
  [marcDate] datetime,
  [marcEdited] bit,
  [marcEditedValue] datetime,
  [marcDescription] varchar(255),
  [marcDelete] bit,
  [devId] integer
)
GO

CREATE TABLE [oDevices] (
  [devId] integer PRIMARY KEY,
  [devHost] varchar(255),
  [devPort] varchar(255),
  [devUser] varchar(255),
  [devPassword] varchar(255),
  [devState] integer,
  [devDescription] varchar(255),
  [devDelete] bit
)
GO