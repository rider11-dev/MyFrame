-- 用户表
CREATE TABLE Users(
	Id int IDENTITY(1,1) primary key,
	UserName nvarchar(20) NOT NULL,
	Password nvarchar(32) NOT NULL,
	Email nvarchar(50) NULL,
	Phone nvarchar(50) NULL,
	Address nvarchar(300) NULL,
	Creator int NULL,
	CreateTime datetime NULL,
	LastModifier int NULL,
	LastModifyTime datetime NULL,
	Enabled bit NOT NULL default 0,
	Remark nvarchar(255) NULL
);
ALTER TABLE [Users] ADD  CONSTRAINT [UK_Users_UserName] UNIQUE NONCLUSTERED ( [UserName] ASC );

INSERT into Users(UserName,Password,Email,Phone,Address,Enabled) values('admin','0b4e7a0e5fe84ad35fb5f95b9ceeac79','751682472@163.com','88888888','中国山东',1); --aaaaaa
-- 角色表
create table Roles(
	Id int identity(1,1) primary key,
	RoleName nvarchar(20) not null,
	Remark nvarchar(255),
	[Enabled] bit not null default 0,
	IsSystem bit not null default 0,
	SortOrder int null,
	Creator int NULL,
	CreateTime datetime NULL,
	LastModifier int NULL,
	LastModifyTime datetime NULL
);
ALTER TABLE [Roles] ADD  CONSTRAINT [UK_Roles_RoleName] UNIQUE NONCLUSTERED ( [RoleName] ASC );

-- 功能模块表
CREATE TABLE Modules(
	Id int identity(1,1) PRIMARY KEY,
	Code nvarchar(20) not null,
	Name nvarchar(20) not null,
	LinkUrl nvarchar(100),
	Icon nvarchar(50),
	ParentId int ,
	[Enabled] bit not null default 0,
	IsSystem bit not null default 0,
	SortOrder int,
	Remark nvarchar(255),
	Creator int,
	CreateTime datetime,
	LastModifier int,
	LastModifyTime datetime
);
ALTER TABLE [Modules] ADD  CONSTRAINT [UK_Module_Code] UNIQUE NONCLUSTERED ( [Code] ASC );

set IDENTITY_INSERT Modules on;
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(11,'UserManage','用户管理','/RBAC/User/Index','fa-user',1,0,1,10,1,1);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(12,'GroupManage','用户组管理','/RBAC/UserGroup/Index','fa-group',1,0,1,20,1,1);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(13,'RoleManage','角色管理','/RBAC/Role/Index','fa-bookmark',1,0,1,30,1,1);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(14,'AuthManage','权限分配','/RBAC/Auth/Index','fa-eye',1,0,1,40,1,1);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(15,'ModuleManage','模块管理','/RBAC/Module/Index','fa-cog',1,0,1,50,1,1);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem)values(1,'RBACManage','授权管理',null,'fa-wrench',1,1,1,20,1);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(21,'UserExtManage','我的信息','/RBAC/Account/Index','fa-edit',1,0,1,10,1,2);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(22,'ChangePwd','修改密码','/RBAC/Account/ChangePwd','fa-key',1,0,1,20,1,2);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(23,'Logout','退出登录','/RBAC/Account/Logout','fa-sign-out',1,0,1,30,1,2);
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem)values(2,'MyAccountManage','我的账户',null,'fa-user',1,1,1,40,1);
set IDENTITY_INSERT Modules off;


--用户角色表
Create Table UserRoleRelation(
	Id int IDENTITY(1,1) PRIMARY KEY,
	UserID int not null,
	RoleID int not null
);
ALTER TABLE [UserRoleRelation] ADD  CONSTRAINT [UK_UserRoleRelation_Rel] UNIQUE NONCLUSTERED ( [UserID] ASC,[RoleID] ASC );

-- 操作表
CREATE TABLE Operations(
	Id int identity(1,1) PRIMARY KEY,
	OptCode nvarchar(20) not null,
	OptName nvarchar(20) not null,
	SubmitUrl nvarchar(100),
	Icon nvarchar(50),
	ModuleId int not null,
	Controller nvarchar(100) not null,
	Tag nvarchar(100),
	ClickFunc nvarchar(255) not null,
	SortOrder int,
	[Enabled] bit not null default 0,
	IsSystem bit not null default 0,
	Remark nvarchar(255)
);
ALTER TABLE [Operations] ADD  CONSTRAINT [UK_Operations_ModuleOptCode] UNIQUE NONCLUSTERED ( [ModuleId] ASC ,[OptCode] ASC);

--角色权限表
CREATE TABLE [RolePermission](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[PermissionId] [int] NOT NULL,
	[PerType] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY];

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限类型：0 功能权限，1 操作权限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RolePermission', @level2type=N'COLUMN',@level2name=N'PerType';














