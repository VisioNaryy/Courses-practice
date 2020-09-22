use Testdb
go

IF OBJECT_ID('Employees') is not null
drop table Employees
go

create table Employees
(
Id int identity primary key not null,
Fname varchar(50),
Lname varchar(50),
Phone varchar(15) constraint CK_Employees_Phone check (Phone LIKE '([0-9][0-9][0-9]) [0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]'),
Salary decimal(10,4),
Bonus decimal(10,4),
Sex varchar(6) constraint CK_Employees_Sex check(Sex in('male','female','m','f')),
StartDate date constraint DF_Employees_StartDate default GETDATE() 
)

insert into Employees
(Fname, Lname, Phone, Salary, Bonus, Sex)
values
('Alex', 'Po', '(903) 356-44-44', 80000,7000, 'male')
go

truncate table Employees

alter table Employees
add constraint CK_Employees_Bonus 
check(Bonus <=Salary*0.1) 
go

--отключить ограничение
alter table Employees
drop constraint CK_Employees_Sex
--check constraint CK_Employees_Sex - включить ограничение
go

alter table Employees
nocheck constraint CK_Employees_Bonu
go

alter table Employees
add constraint DF_Employees_Bonus 
default 0 for Bonus
go


create table Employees
(
Id int identity not null constraint PK_Employees_Id primary key, --clustered/nonclustered
Fname varchar(50),
Lname varchar(50),
Phone varchar(15) --constraint CK_Employees_Phone check (Phone LIKE '([0-9][0-9][0-9]) [0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]'),
)

insert into Employees
values
('Alex', 'Po', '(903) 356-44-44')
go

insert into Employees
values
('Robert', 'Smith', '(910) 111-11-11')
go

alter table Employees
add constraint CK_Employees_Fname_Lname check (Fname != Lname)
go

--unique
alter table Employees
add constraint UQ_Employees_Phone unique(Phone)
go

----references
--FOREIGN KEY
--ONE TO MANY
IF OBJECT_ID('Employees') is not null
drop table Employees
go

create table Employees
(
Id int identity primary key not null,
Fname varchar(50),
Lname varchar(50),
Phone varchar(15)
)

IF OBJECT_ID('Orders') is not null
drop table Orders
go

create table Orders
(
Id int identity primary key not null,
[Date] date default GETDATE(),
TotalSum decimal(10,4) default 0,
EmployeeId int constraint FK_Orders_Employees foreign key 
	references Employees(Id) 
	--on delete no action
	--on update no action
)

insert into Employees
values
('Alex', 'Po', '(903) 356-44-44'),
('Robert', 'Smith', '(910) 111-11-11'),
('Mary', 'Lee', '(924) 222-22-22')
go

insert Orders (EmployeeId)
values(1), (3), (2)

--отключить ограничение foreign key
alter table Orders
nocheck constraint FK_Orders_Employees
go

alter table Orders
drop constraint FK_Orders_Employees
go

alter table Orders
add constraint FK_Orders_Employees foreign key (EmployeeId)
references Employees(Id)
go

----RELATIONSHIPS
--ONE TO ONE
create table Products
(
Id int identity primary key not null,
[Name] varchar(30)
)
go

create table Product_Desc
(
Id int unique foreign key references Products(Id),
Color varchar(20),
[Weight] decimal (10,4)
)
go

create table Stock
(
Id int identity primary key not null,
Qty int,
ProductId int unique foreign key references Products(Id)
)

insert into Products values
('Product1'),('Product2'),('Product3')
go

insert into Product_Desc values
(1,'red',2.2),(2,'black', 3.1),(3, 'yellow', 1.7)
go

insert into Stock values
(117,1), (124,2),(86,3)

--MANY TO MANY
create table OrderProducts
(
OrderId int not null,
ProductId int not null,
PRIMARY KEY (OrderId, ProductId),
FOREIGN KEY(OrderId) REFERENCES Orders(Id),
FOREIGN KEY(ProductId) REFERENCES Products(Id)
)
go

insert into OrderProducts values
(1,1),(1,3),(2,3), (3,2)
go

--Self referencing

alter table Employees 
add EmployeeId int FOREIGN KEY REFERENCES Employees(Id)
go


--SET DEFAULT
alter table Orders
drop constraint FK_Orders_Employees
go

--change actions to delete Employees
alter table Orders
add constraint FK_Orders_Employees foreign key (EmployeeId)
references Employees(Id)
on delete set default
go

delete from Employees
where Id=1


--SET NULL
alter table Orders
drop constraint FK_Orders_Employees
go

--change actions to delete Employees
alter table Orders
add constraint FK_Orders_Employees foreign key (EmployeeId)
references Employees(Id)
on delete set null
go

delete from Employees
where Id=1

--CASCADE
--Родительская таблица - это таблица, на которую ссылаются (в данном случае это Employees), 
--Дочерняя таблица - это таблица, с которой ссылаются на родительскую (Orders)
--Перед изменением Foreign key в таблице Orders, нужно также удалить ключ из таблицы OrderProducts для избежания конфликта. т.к. там тоже есть внешний ключ
alter table Orders
drop constraint FK_Orders_Employees
go

alter table OrderProducts
drop constraint FK__OrderProd__Order__70DDC3D8
go

--change actions to delete Employees
alter table Orders
add constraint FK_Orders_Employees foreign key (EmployeeId)
references Employees(Id)
on delete cascade
go

delete from Employees
where Id=2