Create or Alter View plantbase.CalendarView
AS
	Select
			CalendarId = c.Id,
			CalendarName = c.Name,
			[Year] = c.[Year]
		From
			plantbase.Calendar c