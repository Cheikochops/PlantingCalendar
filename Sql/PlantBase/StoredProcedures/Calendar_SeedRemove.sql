Create or Alter Procedure plantbase.Calendar_SeedRemove	
(
	@CalendarId bigint,
	@SeedId bigint
)
As
Begin

	declare @calendarSeedId bigint = (Select Top 1 Id From plantbase.CalendarSeed Where FK_CalendarId = @CalendarId and FK_SeedId = @SeedId)

	Delete From	
			plantbase.TaskType
		Where
			Id in
		(
			Select
					tt.Id
				From
					plantbase.TaskType tt
					join plantbase.Task t on tt.Id = t.FK_TaskTypeId
				Where
					t.FK_CalendarSeedId = @calendarSeedId
		)

	Delete From
			plantbase.Task
		Where
			FK_CalendarSeedId = @calendarSeedId

	Delete From	
			plantbase.CalendarSeed
		Where
			FK_CalendarId = @CalendarId
			and FK_SeedId = @SeedId

End
