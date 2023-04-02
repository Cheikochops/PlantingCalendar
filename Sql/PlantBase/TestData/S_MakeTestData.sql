Insert Into		
		plantbase.Seed
	(
		PlantType,
		Breed,
		Description,
		WaterRequirement,
		SunRequirement
	)
	VALUES
	(
		'Cucumber',
		'Masterpiece',
		'A large cucumber variety',
		'Consistent - 1 inch + per week',
		'Full Sun, 6 - 8 Hours'
	),
	(
		'Tomato',
		'Gardener''s Delight',
		'A high production small fruit, vine variety',
		'1 - 2 inches per week',
		'8 - 16 hours'
	),
	(
		'Courgette',
		'Ambassador F1 ',
		'An early variety with the production of dark green fruits',
		'1+ inch per week',
		'Full Sun, 6 - 8 hours'
	)

Insert Into
		plantbase.Calendar
	(
		Name,
		Year
	)
	VALUES
	(
		'Current Greenhouse Calendar',
		2023
	),
	(
		'A Previous Calendar',
		2021
	)


Insert Into
		plantbase.ActionType
	(
		Name,
		DisplayChar,
		DisplayColour
	)
	VALUES
	(
		'Sow',
		'S',
		'00FF00'
	),
	(
		'Harvest',
		'H',
		'0000FF'
	)


declare @tomatoSeedId bigint = (Select Top 1 Id from plantbase.Seed Where PlantType = 'Tomato')
declare @sowId bigint = (Select Top 1 Id from plantbase.ActionType Where Name = 'Sow')

Insert Into
		plantbase.SeedAction
	(
		FK_SeedId,
		FK_ActionTypeId,
		StartDate,
		EndDate
	)
	VALUES (
		@tomatoSeedId,
		@sowId,
		'2023-02-01',
		'2023-04-30'
	)

declare @calendarId bigint = (Select Top 1 Id from plantbase.Calendar Where Year = 2023)

Insert Into
		plantbase.CalendarSeed 
		(
			FK_CalendarId,
			FK_SeedId
		)
		VALUES (
			@calendarId,
			@tomatoSeedId
		);

Insert Into
		plantbase.RepeatableType
		(
			Name
		) 
		VALUES 
		(
			'Weekly'
		),
		(
			'Fortnightly'
		),
		(
			'Monthly'
		)

Insert Into
		plantbase.TaskType
		(
			Name,
			Description,
			StartDate,
			EndDate,
			DisplayChar,
			DisplayColour			
		)
		VALUES (
			'Sow',
			'Sow the plants in the ground',
			'2023-02-01',
			'2023-04-30',
			'S',
			'00FF00'
		),
		(
			'Harvest',
			'Harvest the crop',
			'2023-07-01',
			'2023-10-30',
			'H',
			'0000FF'
		)

declare @sowTaskTypeId bigint = (Select Top 1 Id from plantbase.TaskType Where Name = 'Sow')
declare @harvestTaskTypeId bigint = (Select Top 1 Id from plantbase.TaskType Where Name = 'Harvest')
declare @calendarSeedId bigint = (Select Top 1 Id from plantbase.CalendarSeed where FK_CalendarId = @calendarId)

Insert Into
		plantbase.Task
		(
			FK_TaskTypeId,
			FK_CalendarSeedId,
			TaskDate
		)
		VALUES
		(
			@harvestTaskTypeId,
			@calendarSeedId,
			'2023-09-10'
		)
