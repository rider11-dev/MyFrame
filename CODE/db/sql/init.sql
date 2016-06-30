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

-- ����ģ���
CREATE TABLE Modules(
	Id int identity(1,1) PRIMARY KEY,
	Code nvarchar(20) not null,
	Name nvarchar(20) not null,
	LinkUrl nvarchar(100),
	Icon nvarchar(50),
	IsMenu bit not null default 0,
	ParentId int ,
	HasChild bit not null default 0,
	Enabled bit not null default 0,
	IsSystem bit not null default 0,
	SortOrder int not null default 10,
	Remark nvarchar(255),
	Creator int,
	CreateTime date,
	LastModifier int,
	LastModifyTime date,
	IsDeleted bit not null default 0
);
set IDENTITY_INSERT Modules on;
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(11,'RoleManage','�û�����','/RBAC/User/Index','fa-user',1,0,1,10,1,1);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(12,'GroupManage','�û������','/RBAC/UserGroup/Index','fa-group',1,0,1,20,1,1);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(13,'RoleManage','��ɫ����','/RBAC/Role/Index','fa-bookmark',1,0,1,30,1,1);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(14,'AuthManage','Ȩ�޷���','/RBAC/Auth/Index','fa-eye',1,0,1,40,1,1);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(15,'ModuleManage','ģ�����','/RBAC/Module/Index','fa-cog',1,0,1,50,1,1);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem)values(1,'RBACManage','��Ȩ����',null,'fa-wrench',1,1,1,20,1);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(21,'UserExtManage','�ҵ���Ϣ','/RBAC/Account/Index','fa-edit',1,0,1,10,1,2);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(22,'ChangePwd','�޸�����','/RBAC/Account/ChangePwd','fa-key',1,0,1,20,1,2);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(23,'Logout','�˳���¼','/RBAC/Account/Logout','fa-sign-out',1,0,1,30,1,2);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem)values(2,'MyAccountManage','�ҵ��˻�',null,'fa-user',1,1,1,40,1);
set IDENTITY_INSERT Modules off;
