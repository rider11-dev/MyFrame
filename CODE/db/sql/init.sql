USE MyFrame
GO
-- 用户表
CREATE TABLE Users(
	Id int IDENTITY(1,1) primary key,
	UserName nvarchar(20) NOT NULL,
	Password nvarchar(32) NOT NULL,
	Email nvarchar(50) NOT NULL,
	Phone nvarchar(50) NULL,
	Address nvarchar(300) NULL,
	Creator int NULL,
	CreateTime datetime NULL,
	LastModifier int NULL,
	LastModifyTime datetime NULL,
	Enabled bit NOT NULL default 0,
	Remark nvarchar(255) NULL,
	IsDeleted bit NOT NULL default 0
)
GO

-- 角色表
create table Roles(
	Id int identity(1,1) primary key,
	RoleName nvarchar(20) not null,
	Remark nvarchar(100),
	[Enabled] bit not null default 0,
	SortOrder int null,
	Creator int NULL,
	CreateTime datetime NULL,
	LastModifier int NULL,
	LastModifyTime datetime NULL,
	IsDeleted bit NOT NULL default 0
)
go