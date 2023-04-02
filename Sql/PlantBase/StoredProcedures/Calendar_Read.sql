Create or Alter Procedure plantbase.Calendar_Read	
(
	@Id bigint
)
As
Begin

	Select
			*
		From
			plantbase.CalendarView c
		Where
			c.CalendarId = @Id

End