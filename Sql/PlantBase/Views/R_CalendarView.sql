Create or Alter View plantbase.CalendarView
AS
	Select
			CalendarId = c.Id,
			CalendarName = c.Name,
			[Year] = c.[Year],
			PlantTypeName = s.PlantType,
			PlantBreed = s.Breed,
			SeedId = s.Id,
			TaskName = t.Name,
			TaskDescription = t.Description,
			TaskId = t.Id,
			t.IsComplete,
			t.IsRanged,
			t.SetDate,
			t.RangeStartDate,
			t.RangeEndDate,
			t.IsDisplay,
			t.DisplayColour,
			t.DisplayChar
		From
			plantbase.Calendar c
			left join plantbase.CalendarSeed cs on c.Id = cs.FK_CalendarId
			left join plantbase.Seed s on cs.FK_SeedId = s.Id
			left join plantbase.Task t on cs.Id = t.FK_CalendarSeedId
