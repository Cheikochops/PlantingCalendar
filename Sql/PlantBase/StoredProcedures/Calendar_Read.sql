Create or Alter Procedure plantbase.Calendar_Read	
(
	@Id bigint
)
As
Begin

	Select
			CalendarId,
			CalendarName,
			[Year],
			TaskTypeName,
			TaskTypeDescription,
			TaskDate,
			TaskStartDate,
			TaskEndDate,
			TaskTypeId,
			TaskId,
			IsComplete,
			DisplayColour,
			DisplayChar
		From
			plantbase.CalendarView c

End