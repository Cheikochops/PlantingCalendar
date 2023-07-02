Create or Alter View plantbase.CalendarView
AS
	Select
			CalendarId = c.Id,
			CalendarName = c.Name,
			[Year] = c.[Year],
			PlantTypeName = s.PlantType,
			PlantBreed = s.Breed,
			SeedId = s.Id,
			TaskName = tt.Name,
			TaskTypeId = tt.Id,
			TaskId = t.Id,
			t.IsComplete,
			SetTaskDate = t.TaskDate,
			RangeTaskStartDate = tt.StartDate,
			RangeTaskEndDate = tt.EndDate,
			TaskDisplayColour = tt.DisplayColour,
			TaskDisplayChar = tt.DisplayChar
		From
			plantbase.Calendar c
			left join plantbase.CalendarSeed cs on c.Id = cs.FK_CalendarId
			left join plantbase.Seed s on cs.FK_SeedId = s.Id
			left join plantbase.Task t on cs.Id = t.FK_CalendarSeedId
			left join plantbase.TaskType tt on t.FK_TaskTypeId = tt.Id
