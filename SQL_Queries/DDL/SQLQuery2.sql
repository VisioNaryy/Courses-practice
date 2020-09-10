create database coursedb
go

use coursedb
--exec sp_rename 'Customers','customers'
create table Customers
(
Id int primary key identity,
Age int default 18,
FirstName nvarchar(20) not null,
LastName nvarchar(20) not null,
Email varchar(30) constraint UQ_Customer_Email unique,
Phone varchar(30),
check((Age > 0) and (Age <100) and (Email!='') and(Phone!='')),
constraint UQ_Customer_Phone unique (Phone)
);

Create table Orders
(
Id int primary key identity,
CustomerId int,
--CustomerId int references Customers(Id),
--or: foreign key (CustomerId) references Customers(Id)
CreatedAt date,
constraint FK_Orders_To_Customers foreign key (CustomerId)  references Customers (Id) on delete cascade
);