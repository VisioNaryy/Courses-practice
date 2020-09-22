-------scalar functions
----System functions
--@@ERROR
declare @myInt int;
set @myInt = 'ABC'
go

--@@IDENTITY - outputs the last identity value
insert Students 
values
('Alex','Li',null,null)
go
select @@IDENTITY
go

--@@ROWCOUNT
update Students
set LastName = 'Po'
where Id=4
go
select @@ROWCOUNT
go

truncate table Students
go
select @@ROWCOUNT
go

--NEWID()
declare @newId uniqueidentifier
set @newId = NEWID()
print @newId

--ISNUMERIC()

-- ISNULL() - если Salary = null, то заменяем это значение на 0.0
select ID, LName, ISNULL(Salary, 0.0) from Employees

declare @myInt int;
select 3 + @myInt, 3+ ISNULL(@myInt, 0)

--conversion functions
select 10/3,
10./3,
cast(10 as decimal) / 3,
convert(decimal, 10) / 3

select convert(datetime, '20200122 13:45')
--TRY_PARSE, TRY_CONVERT, TRY_CAST
