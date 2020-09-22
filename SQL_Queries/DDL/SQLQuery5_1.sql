create table Orders
(
Id int identity primary key not null,
Date datetime default GETDATE() not null,
ProductId int not null,
Qty int not null,
Price decimal(6,4) not null,
City nvarchar(50),
)

Insert into Orders
values
('20170117', 1, 2, 12.400, 'Alabama'),
('20170118', 2, 5, 7.8, 'Houston'),
('20170119', 1, 3, 12.350, 'NJ'),
('20170120', 3, 1, 23.100, null),
('20170121', 1, 7, 12.300, 'Texas'),
('20170122', 4, 1, 45.100, 'NJ'),
('20170123', 4, 2, 45.100, null),
('20170124', 4, 3, 45.100, 'Washington'),
('20170125', 3, 4, 22.900, 'NYC'),
('20170125', 2, 6, 7.800, 'Toronto'),
('20170125', 1, 1, 12.400, null)

----aggregate functions
--sum, min, max, avg, count
select sum(Qty) [Total_Qty] from Orders

select ProductId, sum(Qty) as [Total_Qty] from Orders
where City is not null 
group by ProductId
having sum(Qty)>10

select ProductId, min(Price) as Min_price, max(Price) as Max_Price from Orders
where City is not null
group by ProductId

select ProductId, 
min(Price) as Min_price, 
max(Price) as Max_Price,
SUM(Qty*Price)/SUM(Qty) as Avg_Price
from Orders
where City is not null
group by ProductId

select count(distinct City) from Orders

select
count(distinct ProductId) as Products,
 count(distinct City) as Cities
from Orders

select  City, count(*) as Sales from Orders
where City is not null
group by City

select ProductId, 
COUNT(Id),
SUM(Qty) as Total_Qty,
SUM(Qty*Price) as Total_Price,
MIN(Price) as Min_price, 
MAX(Price) as Max_Price,
SUM(Qty*Price)/SUM(Qty) as Avg_Price
from Orders
where City is not null
group by ProductId
--order by COUNT(Id)

select City,
AVG(Price*Qty) as AVG_Qty
from Orders
where City is not null
group by City