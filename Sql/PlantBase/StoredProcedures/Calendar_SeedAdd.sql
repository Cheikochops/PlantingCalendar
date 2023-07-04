Create or Alter Procedure plantbase.Calendar_SeedAdd	
(
	@CalendarId bigint,
	@SeedId bigint
)
As
Begin


	Insert Into
		plantbase.CalendarSeed
		(
			FK_CalendarId,
			FK_SeedId
		)
		VALUES
		(
			@CalendarId,
			@SeedId
		)

	Declare @CalendarSeedTaskType table
	(
		SeedId bigint,
		TaskTypeId bigint,
		ActionId bigint
	)

	Merge 
		plantbase.TaskType as target
		Using (
			Select
					SeedId = cs.FK_SeedId,
					ActionId = sa.Id,
					ActionName = sa.Name,
					FK_RepeatableTypeId = null, --SORT THIS OUT
					Parse(concat(c.Year, '-', right(sa.StartDate, 2), '-', left(sa.StartDate, 2)) as date),
					Parse(concat(c.Year, '-', right(sa.EndDate, 2), '-', left(sa.EndDate, 2)) as date),
					sa.DisplayChar,
					sa.DisplayColour
				From
					plantbase.CalendarSeed cs
					join plantbase.SeedAction sa on cs.FK_SeedId = sa.FK_SeedId	
					join plantbase.Calendar c on cs.FK_CalendarId = c.Id and c.Id = @calendarId
				Where
					coalesce(sa.StartDate, '') != ''
					and coalesce(sa.EndDate, '') != ''
					and cs.FK_SeedId = @seedId
		) as source (SeedId, ActionId, ActionName, FK_RepeatableTypeId, StartDate, EndDate, DisplayChar, DisplayColour) on 1 = 2
		When Not Matched Then
			Insert
			(
				Name,
				FK_RepeatableTypeId,
				StartDate,
				EndDate,
				DisplayChar,
				DisplayColour
			)
			Values (
				source.ActionName,
				source.FK_RepeatableTypeId,
				source.StartDate,
				source.EndDate,
				source.DisplayChar,
				source.DisplayColour
			)
		Output 
				source.SeedId,
				INSERTED.Id,
				source.ActionId
			Into @CalendarSeedTaskType;

		
		Insert Into
			plantbase.Task
			(
				FK_TaskTypeId,
				FK_CalendarSeedId,
				TaskDate,
				IsComplete
			)
			Select
					c.TaskTypeId,
					c.SeedId,
					null,
					0
				From
					@CalendarSeedTaskType c
					join plantbase.SeedAction sa on sa.Id = c.ActionId
					join plantbase.CalendarSeed cs on cs.FK_SeedId = c.SeedId and cs.FK_CalendarId = @CalendarId


End