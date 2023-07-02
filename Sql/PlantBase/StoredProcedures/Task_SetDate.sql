Create or ALTER Procedure [plantbase].[Task_SetDate]
(
	@TaskId bigint,
	@Day int,
	@Month int
)
As
Begin

	declare @Year int = (Select 
							[Year] 
						From 
							plantbase.CalendarSeed cs
							join plantbase.Task t on cs.Id = t.FK_CalendarSeedId
							join plantbase.Calendar c on cs.FK_CalendarId = c.Id
						Where
							t.Id = @TaskId)

		Update
				plantbase.Task
			Set
				TaskDate = datefromparts(@Year, @Month, @Day)
			Where
				Id = @TaskId

End
