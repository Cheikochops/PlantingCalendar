Create or Alter Procedure plantbase.Calendar_Read
(
	@Id bigint = null
)
As
Begin

	Select
			CalendarId,
			CalendarName,
			[Year]
		From
			plantbase.CalendarView c
		Where
			(@Id is null or c.CalendarId = @Id)

End