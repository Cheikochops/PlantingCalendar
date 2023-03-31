Create or Alter View plantbase.CalendarView
AS
	Select
			CalendarId = c.Id,
			CalendarName = c.Name,
			[Year] = c.[Year],
			TaskTypeName = tt.Name,
			TaskTypeDescription = tt.Description,
			TaskTypeId = tt.Id,
			TaskId = t.Id,
			t.IsComplete,
			t.TaskDate,
			TaskStartDate = tt.StartDate,
			TaskEndDate = tt.EndDate,
			tt.DisplayColour,
			tt.DisplayChar
		From
			plantbase.Calendar c
			left join plantbase.Task t on c.Id = t.FK_TaskTypeId
			left join plantbase.TaskType tt on t.FK_TaskTypeId = tt.Id
			