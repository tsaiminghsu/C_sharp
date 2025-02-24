use master
go
if exists(select 1 from master..sysdatabases where name='SMSystemDB')
	drop database SMSystemDB
create database SMSystemDB
go
USE SMSystemDB
GO

--角色权限表
create table tb_Role
(
	Id int primary key identity,
	RoleName nvarchar(50) null,
	PowerIds varchar(50) null,
	Remark nvarchar(100) null
)
insert into tb_Role values('超级管理员','1,2,3,4,5,6,7,8,9,','')
insert into tb_Role values('普通用户','1,2,3,','')
select * from tb_Role
--用户表(教师表)
create table tb_UserInfo
(
	Id int primary key identity,
	LoginName varchar(30) null,
	LoginPwd varchar(100) null,
	UserName nvarchar(10) null,
	RoleId int null
)
--密码123456经过MD5加密
insert into tb_UserInfo values('xuanboy','E10ADC3949BA59ABBE56E057F20F883E','轩啸',1)
insert into tb_UserInfo values('zhangsan','E10ADC3949BA59ABBE56E057F20F883E','张三',2)
select * from tb_UserInfo
--导航菜单表
create table tb_Menu
(
	Id int primary key identity,
	MenuName nvarchar(50) null,
	MenuUrl varchar(100) null,
	Remark nvarchar(100) null
)
insert into tb_Menu values('班级信息管理','ClassManage.aspx','')
insert into tb_Menu values('学生信息管理','StudentManage.aspx','')
insert into tb_Menu values('科目信息管理','SubjectManage.aspx','')
insert into tb_Menu values('学生成绩管理','StudentScore.aspx','')
insert into tb_Menu values('学生成绩统计','StuScoreCount.aspx','')
insert into tb_Menu values('用户信息管理','UserManage.aspx','')
insert into tb_Menu values('排期信息管理','ScheduleManage.aspx','')
insert into tb_Menu values('导航链接管理','AdminUrlManage.aspx','')
insert into tb_Menu values('权限信息管理','PowerManage.aspx','')
select * from tb_Menu
--班级表
create table tb_ClassInfo
(
	Id int primary key identity,
	ClassName nvarchar(20) null,
	Remark nvarchar(100) null
)
insert into tb_ClassInfo values('高三一班','实验班')
insert into tb_ClassInfo values('高三二班','平行班')
select * from tb_ClassInfo

--学生表
create table tb_StuInfo
(
	Id int primary key identity,
	StuNum varchar(20) null,
	StuPwd varchar(20) null,
	StuName varchar(20) null,
	Birthday datetime null,
	Sex bit null,
	ClassId int null,
	StartTime datetime null
)

insert into tb_StuInfo values('stu00001','123456','王勇','1992-05-05',1,1,GETDATE())
insert into tb_StuInfo values('stu00002','123456','张芳','1993-06-06',0,2,GETDATE())
select * from tb_StuInfo


--科目表
create table tb_Subject
(
	Id int primary key identity,
	SubjectName nvarchar(20) null,
	Remark nvarchar(100) null
)
insert into tb_Subject values('语文','')
insert into tb_Subject values('数学','')
insert into tb_Subject values('英语','')
select * from tb_Subject

--成绩排期表
create table tb_Schedule
(
	Id int primary key identity,
	ScheduleName nvarchar(100) null,
	Remark nvarchar(100) null
)
insert into tb_Schedule values('2014年1月期末考试','')
insert into tb_Schedule values('2014年4月期中考试','')
--成绩表
create table tb_Score
(
	Id int primary key identity,
	StuId int null,
	SubjectId int null,
	Score float null,
	ScheduleId int null
)
insert into tb_Score values(1,1,87,1)
insert into tb_Score values(1,2,97,1)
insert into tb_Score values(1,3,56,1)
insert into tb_Score values(2,1,90,1)
insert into tb_Score values(2,2,83,1)
insert into tb_Score values(2,3,89,1)
select * from tb_Score
--创建视图
--CREATE VIEW [dbo].[View_StuScore]
--AS
--SELECT     dbo.tb_Score.Id, dbo.tb_Score.Score,dbo.tb_Score.ScheduleId,dbo.tb_StuInfo.StuNum, dbo.tb_StuInfo.StuName,dbo.tb_StuInfo.ClassId, dbo.tb_Subject.SubjectName, dbo.tb_ClassInfo.ClassName
--FROM         dbo.tb_Score INNER JOIN
--                      dbo.tb_StuInfo ON dbo.tb_Score.StuId = dbo.tb_StuInfo.Id INNER JOIN
--                      dbo.tb_Subject ON dbo.tb_Score.SubjectId = dbo.tb_Subject.Id INNER JOIN
--                      dbo.tb_ClassInfo ON dbo.tb_StuInfo.ClassId = dbo.tb_ClassInfo.Id

--GO
