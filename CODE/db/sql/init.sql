-- �û���
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
);
INSERT into Users(UserName,Password,Email,Phone,Address,Enabled) values('admin','0b4e7a0e5fe84ad35fb5f95b9ceeac79','751682472@163.com','88888888','�й�ɽ��',1); --aaaaaa
-- ��ɫ��
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
);