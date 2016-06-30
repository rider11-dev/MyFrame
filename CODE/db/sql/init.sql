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
);
INSERT into Users(UserName,Password,Email,Phone,Address,Enabled) values('admin','0b4e7a0e5fe84ad35fb5f95b9ceeac79','751682472@163.com','88888888','中国山东',1); --aaaaaa
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
);

-- 功能模块表
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
INSERT into Modules(Id,Code,Name,LinkUrl,Icon,IsMenu,HasChild,Enabled,SortOrder,IsSystem,ParentId)values(11,'RoleManage','用户管理','/RBAC/User/Index','fa-user',1,0,1,10,1,1);
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
