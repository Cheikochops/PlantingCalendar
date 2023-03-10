IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'plantbase' 
                 AND  TABLE_NAME = 'Seed'))
BEGIN

	/*
		Table for storing seed details
	*/

    Create Table plantbase.Seed 
	(
		Id bigint IDENTITY(1,1),  
		PlantType varchar(50) not null,
		Breed varchar(50) null,
		Description varchar(1000) null,
		WaterRequirement varchar(100) null,
		SunRequirement varchar(100) null
		PRIMARY KEY (Id)
	)

END


IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'plantbase' 
                 AND  TABLE_NAME = 'ActionType'))
BEGIN

	/*
		Table for storing the action type and how it should be displayed on the calendar
	*/

    Create Table plantbase.ActionType 
	(
		Id bigint IDENTITY(1,1),  
		Name varchar(50) not null,
		DisplayChar char null,
		DisplayColour varchar(6) null --colour code
		PRIMARY KEY (Id)
	)

END


IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'plantbase' 
                 AND  TABLE_NAME = 'SeedAction'))
BEGIN

	/*
		Table for storing actions against seeds e.g Sow, Harvest e.t.c and the date range
		they can be performed in.
	*/

    Create Table plantbase.SeedAction 
	(
		Id bigint IDENTITY(1,1),  
		FK_SeedId bigint not null,
		FK_ActionTypeId bigint not null,
		StartDate date not null,
		EndDate date not null,
		PRIMARY KEY (Id),
		FOREIGN KEY (FK_SeedId) REFERENCES plantbase.Seed(Id),
		FOREIGN KEY (FK_ActionTypeId) REFERENCES plantbase.ActionType(Id)
	)

END

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'plantbase' 
                 AND  TABLE_NAME = 'RepeatableType'))
BEGIN

	/*
		Table for storing the type of repeatable task e.g Monthly, Weekly, Bi Monthly.
	*/

    Create Table plantbase.RepeatableType 
	(
		Id bigint IDENTITY(1,1),  
		Name varchar(50) not null,
		PRIMARY KEY (Id)
	)

END

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'plantbase' 
                 AND  TABLE_NAME = 'TaskType'))
BEGIN

	/*
		Table for storing a type of task
	*/

    Create Table plantbase.TaskType 
	(
		Id bigint IDENTITY(1,1),  
		Name varchar(50) not null,
		FK_RepeatableTypeId bigint not null,
		StartDate datetime null,
		EndDate datetime null,
		PRIMARY KEY (Id),
		FOREIGN KEY (FK_RepeatableTypeId) REFERENCES plantbase.RepeatableType(Id)
	)

END


IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'plantbase' 
                 AND  TABLE_NAME = 'Task'))
BEGIN

	/*
		Table for storing the tasks against a given calendar
	*/

    Create Table plantbase.Task 
	(
		Id bigint IDENTITY(1,1),  
		FK_TaskTypeId bigint,
		IsComplete bit default(0),
		PRIMARY KEY (Id),
		FOREIGN KEY (FK_TaskTypeId) REFERENCES plantbase.TaskType(Id)
	)

END


IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'plantbase' 
                 AND  TABLE_NAME = 'Calendar'))
BEGIN

	/*
		Table for storing Calendars
	*/

    Create Table plantbase.Calendar 
	(
		Id bigint IDENTITY(1,1),  
		Name varchar(50) not null,
		Year int not null,
		PRIMARY KEY (Id)
	)

END
