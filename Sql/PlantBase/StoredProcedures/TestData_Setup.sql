Create or Alter Procedure plantbase.TestData_Setup
AS
BEGIN

	Insert Into		
			plantbase.Seed
		(
			PlantType,
			Breed,
			Description,
			WaterRequirement,
			SunRequirement,
			ExpiryDate
		)
		VALUES
		(
			'TEST: Cucumber',
			'Masterpiece',
			'A large cucumber variety',
			'Consistent - 1 inch + per week',
			'Full Sun, 6 - 8 Hours',
			GetDate()
		),
		(
			'TEST: Tomato',
			'Gardener''s Delight',
			'A high production small fruit, vine variety',
			'1 - 2 inches per week',
			'8 - 16 hours',
			'2023-05-10'
		),
		(
			'TEST: Courgette',
			'Ambassador F1 ',
			'An early variety with the production of dark green fruits',
			'1 + inch per week',
			'Full Sun, 6 - 8 hours',
			'2023-07-10'
		)
	
	Insert Into
			plantbase.Calendar
		(
			Name,
			Year
		)
		VALUES
		(
			'TEST: Current Greenhouse Calendar',
			2023
		),
		(
			'TEST: A Previous Calendar',
			2021
		)
	
	declare @tomatoSeedId bigint = (Select Top 1 Id from plantbase.Seed Where PlantType = 'TEST: Tomato')
	
	Insert Into
			plantbase.SeedAction
		(
			Name,
			FK_SeedId,
			StartDate,
			EndDate,
			Enum_ActionTypeId
		)
		VALUES (
			'TEST: Sow',
			@tomatoSeedId,
			'0102',
			'3004',
			1
		)
	
	Insert Into
			plantbase.SeedAction
		(
			Name,
			FK_SeedId,
			StartDate,
			EndDate,
			Enum_ActionTypeId
		)
		VALUES (
			'TEST: Harvest',
			@tomatoSeedId,
			'1007',
			'2010',
			2
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
			plantbase.TaskType
			(
				Name,
				StartDate,
				EndDate,
				DisplayChar,
				DisplayColour		
			)
			VALUES (
				'TEST: Sow',
				'2023-02-01',
				'2023-04-30',
				'S',
				'00FF00'
			),
			(
				'TEST: Harvest',
				'2023-07-01',
				'2023-10-30',
				'H',
				'0000FF'
			)
	
	declare @sowTaskTypeId bigint = (Select Top 1 Id from plantbase.TaskType Where Name = 'TEST: Sow')
	declare @harvestTaskTypeId bigint = (Select Top 1 Id from plantbase.TaskType Where Name = 'TEST: Harvest')
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
			),
			(
				@sowTaskTypeId,
				@calendarSeedId,
				null
			)

END