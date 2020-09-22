create database Testdb

use Testdb

CREATE TABLE Students
(
    Id int NOT NULL Identity(1,1) primary key,
    FirstName nvarchar(20) NULL,
	LastName nvarchar(20) NULL,
	Phone char(12),
	Email varchar(20)
);
alter table Students
alter column LastName nvarchar(30) not null

alter table Students
--add MName nvarchar(30) 
drop column MName

----DML
insert into Students
(FirstName, LastName, Phone, Email)
values 
(N'Александр',N'Македонский','(012)3456789','alex@imperium.com'),
(N'Диоген',N'Синопский',null,'sinop@pithos.com')

set identity_insert Students on
insert into Students
(Id, FirstName,LastName, Phone, Email)
values 
--(1,N'Александр',N'Македонский','(012)3456789','alex@imperium.com'),
--(2,N'Диоген',N'Синопский',null,'sinop@pithos.com'),
(3,null,N'Семирамиду','(012)3456789','assyria@imperium.com')
set identity_insert Students off

----select
select FirstName, Email from Students
where Id=1

----insert#2
CREATE TABLE StudentPhones
(
    --Id int NOT NULL identity primary key,
	Id int NOT NULL,
    LastName nvarchar(30),
	PhoneNumber char(12)
);

insert into StudentPhones
select Id, LastName, Phone from Students

----update
--#1
update Students
set Phone = null
where Id=1
--#2
update Students
set Phone = sp.PhoneNumber
from Students s 
join StudentPhones sp on s.Id = sp.Id

----delete
delete Students
where Id=1

----truncate - такжк сбрасывает identity у столбцов
truncate table Students
truncate table StudentPhones

----output
insert into Students
(FirstName, LastName, Phone, Email)
output inserted.*
values 
(N'Александр',N'Македонский','(012)3456789','alex@imperium.com'),
(N'Диоген',N'Синопский',null,'sinop@pithos.com'),
(null,N'Семирамиду','(012)3456789','assyria@imperium.com')

delete Students
output deleted.Id, deleted.LastName
where Id=1

update Students
set Phone = '(012)3456789'
output inserted.Id, inserted.LastName, inserted.Phone as [New Phone],
deleted.Phone as "Old Phone"
where Id=2

delete Students
output deleted.Id, deleted.LastName, deleted.Phone into StudentPhones

--table variable
declare @deleteTable table (Id int, LastName nvarchar(20))

delete from StudentPhones
output deleted.Id, deleted.LastName into @deleteTable

select * from @deleteTable