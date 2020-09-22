use Testdb

--distinct
select distinct Department 
from Employees

--top
select top 10 * from Employees
select top 25 percent * from Employees

--orderby
select * from Employees
order by BirthDate

--with ties
select top 30 with ties FName, LName, Salary from Employees
order by Salary desc 

--select into
select ID, LName, FName, Salary 
into EmpSalaries
from Employees

--where
select * from Employees
where Salary = 80000

select * from Employees
where Department = 'Sales'

--comparison
select * from Employees
where BirthDate > '19800101'

----logic operators (all, any, and, between, exists, in, like, not, or, some)
select * from Employees
where Department = 'sales' and Salary >=100000

select * from Employees
where Salary>=150000 and ( Department = 'sales' or Department = 'supply')

--in
select * from Employees
where Department in ('sales','supply', 'law', 'logistics')

--betweeen
select * from Employees
where BirthDate between '19800101' and '19850101'

--like (%, _, [], [^])
select * from Employees
where Phone like '(8%'

select * from Employees
where ID like '_2'

select * from Employees
where ID like '[2,4]2'

select * from Employees
where ID like '[2-4]2'

select * from Employees
where ID like '[^2-5]2'

--null
select * from Employees
where Salary is not null

select * from Employees
where Salary in (80000,120000)
or Salary is null

--case
select ID, LName, Salary, 

case
	when Salary>=260000 then 'chief'
	when Salary>=100000 then 'manager'
	when Salary is null then 'unknown'
	else 'worker'
end as Position,

case
	when Salary>=260000 then 'chief'
	when Salary>=100000 then 'manager'
	when Salary is null then 'unknown'
end as Position2

from Employees

--поисковок выражение case
select ID, LName, Department, Salary,

case Department
	when 'FINANCE & ACCOUNTING' then '100%'
	when 'LAW' then '80%'
	when 'Logistics' then '70%'
	else '10%'
end as [Bonus%],

Salary/100*
case Department
	when 'FINANCE & ACCOUNTING' then 100
	when 'LAW' then 80
	when 'Logistics' then 70
	else 10
end as [Bonus],

(Salary/100*
case Department
	when 'FINANCE & ACCOUNTING' then 100
	when 'LAW' then 80
	when 'Logistics' then 70
	else 10
end) + Salary as [Bonus & Salary]

from Employees

--IIF
select ID, LName, Department, Salary,
	IIF(Salary >=100000, 'Manager', 'Worker') as Position
--case when Salary >=100000 then 'manager'	 else 'worker'
from Employees

alter table Employees
add Gender bit 

update Employees
set Gender = IIF(ID>50, 1, 0)

select ID, LName, 
IIF(Gender =0, 'female', 'male') as Gender
from Employees

--group by
select Department, Gender from Employees
group by Department, Gender


--having
select Department from Employees
group by Department 
having Department like 'L%'