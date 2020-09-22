/****************************************************************************
************************   T R A N S A C T - S Q L   ************************
*****************************************************************************
*****  Lesson VIII  ****            Joins            ************************
*****************************************************************************
****************************************************************************/

USE master
GO

CREATE DATABASE GrandSlamDB
GO

USE GrandSlamDB
GO

--CREATE TABLES
CREATE TABLE dbo.Players
(
    Id int NOT NULL IDENTITY PRIMARY KEY,
    FName varchar(50),
	LName varchar(50),
	[Rank] int
); 
GO

CREATE TABLE dbo.PlayerInfos
(
    Id int NOT NULL IDENTITY PRIMARY KEY,
    PlayerId int UNIQUE FOREIGN KEY REFERENCES Players (Id) ON DELETE CASCADE,
	BirthDate date,
	[Weight] smallint,
	Height smallint,
	Country varchar(50),
	BirthPlace varchar(50),
	Residence varchar(50)
); 
GO

CREATE TABLE dbo.Tournaments
(
    Id int NOT NULL IDENTITY PRIMARY KEY,
    Name varchar(50)
); 
GO

CREATE TABLE dbo.Courts
(
    Id int NOT NULL IDENTITY PRIMARY KEY,
    Name varchar(50),
	City varchar(50),
	Capacity int,
	Surface char(5) CHECK (Surface IN ('hard', 'clay', 'grass'))
); 
GO

CREATE TABLE dbo.Matches
(
    Id int NOT NULL IDENTITY PRIMARY KEY,
    TournamentId int FOREIGN KEY REFERENCES Tournaments (Id) ON DELETE SET NULL,
	[Round] varchar(15) CHECK ([Round] IN ('FINALS', 'SEMI-FINALS','QUARTER-FINALS')),
	[Date] date,
	[Time] time(0),
	CourtId int FOREIGN KEY REFERENCES Courts (Id) ON DELETE SET NULL,
); 
GO

CREATE TABLE dbo.PlayerStats
(
    Id int NOT NULL IDENTITY PRIMARY KEY,
    MatchId int FOREIGN KEY REFERENCES Matches (Id) ON DELETE CASCADE,
	PlayerId int FOREIGN KEY REFERENCES Players (Id) ON DELETE CASCADE,
	Win bit,
	Aces smallint
); 
GO

--INSERT DATAS
INSERT Players VALUES
    ('Stan'		   ,'Wawrinka'      ,3 ),
	('Dominic'	   ,'Thiem' 		,9 ),
	('Novak'	   ,'Djokovic'		,2 ),
	('Andy'        ,'Murray'		,1 ),
	('Richard'     ,'Gasquet'		,22),
	('Tomas'       ,'Berdych'		,13),
	('Albert'      ,'Ramos-Vinolas' ,24),
	('David'       ,'Goffin'		,14),
	('Roger'       ,'Federer'		,4 ),
	('Rafael'      ,'Nadal'			,5 ),
	('Grigor'      ,'Dimitrov'		,12),
	('Milos'       ,'Raonic'		,6 ),
	('Jo-Wilfried' ,'Tsonga'		,10),
	('Mischa'      ,'Zverev'		,20),
	('Gael'        ,'Monfils'		,11),
	('Kei'         ,'Nishikori'		,7 ),
	('Juan Martin' ,'del Potro'     ,35),
	('Lucas'       ,'Pouille'		,17),
	('Marin'       ,'Cilic'			,8 ),
	('Sam'         ,'Querrey'		,25);
GO

INSERT PlayerInfos 
	(PlayerId,	Country,			BirthDate,		BirthPlace,				Residence,		[Weight], Height)
VALUES
	( 1,		'Switzerland'		,'19850328',	'Switzerland'			,'Switzerland'	,81,	183),
	( 2,		'Austria'			,'19930903',	'Austria'				,'Austria'		,82,	185),
	( 3,		'Serbia'			,'19870522',	'Serbia'				,'Monaco'		,75,	188),
	( 4,		'GBR'				,'19870515',	'Scotland'				,'England'		,84,	191),
	( 5,		'France'			,'19860618',	'France'				,'Switzerland'	,75,	185),
	( 6,		'Czech Republic'	,'19850917',	'Czech Republic'		,'Monaco'		,91,	196),
	( 7,		'Spain'				,'19880117',	'Spain'					,'Spain'		,80,	188),
	( 8,		'Belgium'			,'19901207',	'Belgium'				,'Monaco'		,68,	180),
	( 9,		'Switzerland'		,'19810808',	'Switzerland'			,'Switzerland'	,85,	185),
	(10,		'Spain'				,'19860603',	'Spain'					,'Spain'		,85,	185),
	(11,		'Bulgaria'			,'19910516',	'Bulgaria'				,'Monaco'		,80,	191),
	(12,		'Canada'			,'19901227',	'Montenegro'			,'Monaco'		,98,	196),
	(13,		'France'			,'19850417',	'France'				,'Switzerland'	,91,	188),
	(14,		'Germany'			,'19870822',	'Russia'				,'Monaco'		,81,	191),
	(15,		'France'			,'19860901',	'France'				,'Switzerland'	,80,	193),
	(16,		'Japan'				,'19891229',	'Japan'					,'USA'			,75,	178),
	(17,		'Argentina'			,'19880923',	'Argentina'				,'Argentina'	,97,	198),
	(18,		'France'			,'19940223',	'France'				,'U.A.E.'		,81,	185),
	(19,		'Croatia'			,'19880928',	'Bosnia-Herzegovina'	,'Monaco'		,89,	198),
	(20,		'USA'				,'19871007',	'USA'					,'USA'			,95,	198);
GO

INSERT Tournaments VALUES
('Australian Open'),
('Roland Garros'),
('Wimbledon'),
('US Open');
GO

INSERT Courts
VALUES
	('Philippe-Chatrier Court'	,'Paris'		,14840	,'Clay'	),
	('Suzanne-Lenglen Court' 	,'Paris'		,10068	,'Clay'	),
	('Rod Laver Arena'			,'Melbourne'	,14820	,'Hard'	),
	('Margaret Court Arena'		,'Melbourne'	,7500	,'Hard'	),
	('Arthur Ashe Stadium'		,'New York'		,23771	,'Hard'	),
	('Louis Armstrong Stadium'	,'New York'		,14000	,'Hard'	),
	('Grandstand'				,'New York'		,8125	,'Hard'	),
	('Centre Court'				,'London'		,15916	,'Grass'),
	('No. 1 Court'				,'London'		,11360	,'Grass'),
	('No. 2 Court'				,'London'		,4000	,'Grass');
GO

INSERT Matches
(TournamentId,	CourtId,	[Date],		[Time],		[Round])
VALUES
--Roland Garros
(2,				1,			'20160605',	'15:00',	'FINALS'		),
(2,				2, 			'20160603',	'15:00',	'SEMI-FINALS'	),
(2,				1,			'20160603',	'15:00',	'SEMI-FINALS'	),
(2,				1,			'20160602',	'13:00',	'QUARTER-FINALS'),
(2,				1,			'20160601',	'14:00',	'QUARTER-FINALS'),
(2,				2, 			'20160601',	'14:00',	'QUARTER-FINALS'),
(2,				2, 			'20160602',	'13:00',	'QUARTER-FINALS'),
					
--Australian Open				
(1,				3,			'20170129',	'19:30',	'FINALS'		),
(1,				3,			'20170126',	'19:30',	'SEMI-FINALS'	),
(1,				3,			'20170127',	'19:30',	'SEMI-FINALS'	),
(1,				3,			'20170125',	'19:30',	'QUARTER-FINALS'),
(1,				3,			'20170124',	'15:30',	'QUARTER-FINALS'),
(1,				3,			'20170125',	'15:30',	'QUARTER-FINALS'),
(1,				3,			'20170124',	'19:00',	'QUARTER-FINALS'),
					
--US Open		
(4,				5,			'20160911',	'16:00',	'FINALS'		),
(4,				5,			'20160909',	'15:00',	'SEMI-FINALS'	),
(4,				5,			'20160909',	'16:30',	'SEMI-FINALS'	),
(4,				5,			'20160906',	'20:00',	'QUARTER-FINALS'),
(4,				5,			'20160907',	'13:00',	'QUARTER-FINALS'),
(4,				5,			'20160907',	'20:00',	'QUARTER-FINALS'),
(4,				5,			'20160906',	'13:00',	'QUARTER-FINALS'),
					
--Wimbledon				
(3,				8,			'20160710',	'14:00',	'FINALS'		),
(3,				8,			'20160708',	'15:00',	'SEMI-FINALS'	),
(3,				8,			'20160708',	'13:00',	'SEMI-FINALS'	),
(3,				8,			'20160706',	'15:00',	'QUARTER-FINALS'),
(3,				8,			'20160706',	'13:00',	'QUARTER-FINALS'),
(3,				9,			'20160706',	'13:00',	'QUARTER-FINALS'),
(3,				9,			'20160706',	'15:00',	'QUARTER-FINALS');
GO

INSERT PlayerStats
VALUES
	
( 1,	03			,1, 4 ),
( 2,	03			,1, 2 ),
( 3,	04			,1, 3 ),
( 4,	03			,1, 0 ),
( 5,	04			,1, 11),
( 6,	01			,1, 8 ),
( 7,	02			,1, 2 ),
  								 	  
( 8,	09			,1, 20),
( 9,	09			,1, 11),
(10,	10			,1, 8 ),
(11,	10			,1, 5 ),
(12,	01			,1, 4 ),
(13,	11			,1, 9 ),
(14,	09			,1, 9 ),
								 	  
(15,	01			,1, 9 ),
(16,	03			,1, 2 ),
(17,	01			,1, 10),
(18,	03			,1, 1 ),
(19,	16			,1, 1 ),
(20,	01			,1, 10),
(21,	15			,1, 12),
									 
(22,	04			,1, 7 ),
(23,	04			,1, 7 ),
(24,	12			,1, 23),
(25,	04			,1, 14),
(26,	09			,1, 27),
(27,	12			,1, 13),
(28,	06			,1, 10),
									 
( 1,	04			,0, 5 ),
( 2,	02			,0, 2 ),
( 3,	01			,0, 1 ),
( 4,	06			,0, 4 ),
( 5,	05			,0, 2 ),
( 6,	07			,0, 2 ),
( 7,	08			,0, 3 ),	
									  
( 8,	10			,0, 4 ),
( 9,	01			,0, 10),
(10,	11			,0, 22),
(11,	12			,0, 14),
(12,	13			,0, 7 ),
(13,	08			,0, 7 ),
(14,	14			,0, 5 ),	
									
(15,	03			,0, 5 ),
(16,	15			,0, 12),
(17,	16			,0, 3 ),
(18,	13			,0, 3 ),
(19,	04			,0, 8 ),
(20,	17			,0, 3 ),
(21,	18			,0, 5 ),
									
(22,	12			,0, 8 ),
(23,	06			,0, 5 ),
(24,	09			,0, 16),
(25,	13			,0, 15),
(26,	19			,0, 23),
(27,	20			,0, 15),
(28,	18			,0, 9 );
GO

SELECT p.Rank, p.FName + ' ' + p.LName Name, pli.BirthDate, pli.Weight, pli.Height 
FROM Players p
	JOIN PlayerInfos pli
	ON p.Id = pli.PlayerId
ORDER BY p.Rank

SELECT * 
FROM Courts c
	LEFT JOIN Matches m
	ON c.Id = m.CourtId
	LEFT JOIN Tournaments t
	ON m.TournamentId = t.Id

SELECT Name, Date, Time, Winner, wA Aces, Loser, lA Aces FROM
(SELECT m.Id, ps.Aces wA, c.Name, m.Date, m.Time, p.FName + ' ' + p.LName Winner  
FROM Matches m
		JOIN PlayerStats ps
			ON m.Id = ps.MatchId AND ps.Win = 1
		JOIN Players p 
			ON p.Id = ps.PlayerId
		JOIN Courts c ON c.Id = m.CourtId) winner 
JOIN
(SELECT m.Id, ps.Aces lA, p.FName + ' ' + p.LName Loser  
FROM Matches m
		JOIN PlayerStats ps
			ON m.Id = ps.MatchId AND ps.Win = 0
		JOIN Players p 
			ON p.Id = ps.PlayerId) loser
ON winner.Id = loser.Id

-- SELF JOIN
SELECT c.Name, m1.Date, m1.Time, p1.FName + ' ' + p1.LName Winner, ps1.Aces, p2.FName + ' ' + p2.LName Loser, ps2.Aces
		FROM Matches m1
		--JOIN Matches m2 
			--ON m1.Id=m2.Id
		JOIN PlayerStats ps1
			ON m1.Id = ps1.MatchId AND ps1.Win = 1
		JOIN PlayerStats ps2
			ON m1.Id = ps2.MatchId AND ps2.Win = 0
		JOIN Players p1 
			ON p1.Id = ps1.PlayerId
		JOIN Players p2
			ON p2.Id = ps2.PlayerId
		JOIN Courts c ON c.Id = m1.CourtId

SELECT p1.FName + ' ' + p1.LName Player1, p2.FName + ' ' + p2.LName Player2 
FROM PlayerStats ps1 
	JOIN PlayerStats ps2
	ON ps1.MatchId = ps2.MatchId
	AND ps1.Win = 1 AND ps2.Win = 0
	JOIN Players p1 ON ps1.PlayerId = p1.Id
	JOIN Players p2 ON ps2.PlayerId = p2.Id

	
-- FULL JOIN
SELECT * FROM Matches m
	FULL JOIN Courts c 
	ON c.Id = m.CourtId


--CROSS JOIN
SELECT p.LName, c.Name, ISNULL(t.Wins, 0) Wins FROM Players p
		CROSS JOIN Courts c
		LEFT JOIN 
		(SELECT ps.PlayerId, COUNT(ps.Win) Wins, m.CourtId FROM PlayerStats ps 
				JOIN Matches m 
				ON ps.MatchId = m.Id AND ps.Win = 1 
				GROUP BY ps.PlayerId, m.CourtId) t
		ON t.PlayerId = p.Id AND t.CourtId = c.Id
		ORDER BY p.LName

SELECT p.LName, t.Name, ISNULL(t.Wins, 0) Wins FROM Players p
		CROSS JOIN Tournaments t
		LEFT JOIN 
		(SELECT ps.PlayerId, COUNT(ps.Win) Wins, m.TournamentId FROM PlayerStats ps 
				JOIN Matches m 
				ON ps.MatchId = m.Id AND ps.Win = 1 
				GROUP BY ps.PlayerId, m.TournamentId) tb
		ON tb.PlayerId = p.Id AND tb.TournamentId = t.Id
		ORDER BY p.LName
