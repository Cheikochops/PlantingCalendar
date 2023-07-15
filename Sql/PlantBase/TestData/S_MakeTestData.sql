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
			Description,
			FK_SeedId,
			StartDate,
			EndDate,
			Enum_ActionTypeId,
			IsDisplay,
			DisplayColour,
			DisplayChar
		)
		VALUES (
			'Sow',
			'Sow the plant',
			@tomatoSeedId,
			'0102',
			'3004',
			1,
			1,
			'00000F',
			'S'
		),
		(
			'Harvest',
			'Harvest the plant',
			@tomatoSeedId,
			'1007',
			'2010',
			2,
			1,
			'111111',
			'H'
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

		declare @calendarSeedId bigint = (Select Top 1 Id from plantbase.CalendarSeed where FK_CalendarId = @calendarId)	
	
	Insert Into
			plantbase.Task
			(
				FK_CalendarSeedId,
				Name,
				Description,
				IsRanged,
				RangeStartDate,
				RangeEndDate,
				SetDate,
				IsDisplay,
				DisplayChar,
				DisplayColour,
				IsComplete		
			)
			VALUES (
				@calendarSeedId,
				'Sow',
				'Sow the plant',
				1,
				'2023-02-01',
				'2023-04-30',
				null,
				1,
				'S',
				'00FF00',
				0
			),
			(
				@calendarSeedId,
				'Harvest',
				'Harvest the plant',
				1,
				'2023-07-01',
				'2023-10-30',
				null,
				1,
				'H',
				'0000FF',
				0
			),
			(
				@calendarSeedId,
				'Trim',
				'Trim the bottom leaves off of plants',
				0,
				null,
				null,
				'2023-05-01',
				0,
				null,
				null,
				0
			)
