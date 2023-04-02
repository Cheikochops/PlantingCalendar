Create or Alter Procedure plantbase.Calendar_Basic_Read
As
Begin

	Select Distinct
			CalendarId,
			CalendarName
		From
			plantbase.CalendarView c

End