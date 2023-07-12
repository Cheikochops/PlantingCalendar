Create or Alter Procedure plantbase.Calendar_SeedUpdate	
(
	@CalendarId bigint,
	@SeedJson nvarchar(max)
)
As
Begin

	declare @seedIds table (
		Id bigint
	)

	declare @newCalendarSeedId table (
		Id bigint
	)
	
	declare @RemovableCalendarSeedIds table
	(
		Id bigint
	)

	Insert Into 
			@seedIds
		Select
				o.Value
			From
				Openjson(@SeedJson) o

	Insert Into
			@RemovableCalendarSeedIds
		Select
				cs.Id
			From
				plantbase.CalendarSeed cs
			Where
				cs.FK_CalendarId = @CalendarId
				and not exists (Select * from @seedIds where Id = cs.FK_SeedId)


		Declare @RemovableTaskTypeIds table
		(
			Id bigint
		)

		Merge 
			plantbase.Task as target
		Using (
			Select
					t.FK_TaskTypeId,
					t.Id
				From
					plantbase.CalendarSeed cs
					join @RemovableCalendarSeedIds rcs on cs.Id = rcs.Id
					join plantbase.Task t on cs.Id = t.FK_CalendarSeedId
		) as source (FK_TaskTypeId, Id) on target.Id = source.Id
		When Matched Then 
			Delete
		Output 
				source.FK_TaskTypeId
			Into 
				@RemovableTaskTypeIds;


		Delete From
				plantbase.TaskType
			Where
				Id in (
					Select	
							Id
						From
							@RemovableTaskTypeIds
				)

		Delete From
				plantbase.CalendarSeed
			Where
				Id in
				(
					Select
							Id 
						From
							@RemovableCalendarSeedIds
				)


	Merge 
		plantbase.CalendarSeed as target
		Using (
			Select
					Id
				From
					@seedIds
		) as source (Id) on target.FK_SeedId = source.Id and target.FK_CalendarId = @calendarId
		When Not Matched Then
			Insert
			(
				FK_CalendarId,
				FK_SeedId
			)
			VALUES
			(
				@CalendarId,
				source.Id
			)
		Output
			inserted.Id
		Into
			@newCalendarSeedId;

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
					Parse(concat(c.Year, '-', right(sa.StartDate, 2), '-', left(sa.StartDate, 2)) as date),
					Parse(concat(c.Year, '-', right(sa.EndDate, 2), '-', left(sa.EndDate, 2)) as date),
					sa.DisplayChar,
					sa.DisplayColour
				From
					plantbase.CalendarSeed cs
					join plantbase.SeedAction sa on cs.FK_SeedId = sa.FK_SeedId	
					join plantbase.Calendar c on cs.FK_CalendarId = c.Id and c.Id = @calendarId
					join @newCalendarSeedId ncs on cs.Id = ncs.Id
				Where
					coalesce(sa.StartDate, '') != ''
					and coalesce(sa.EndDate, '') != ''
		) as source (SeedId, ActionId, ActionName, StartDate, EndDate, DisplayChar, DisplayColour) on 1 = 2
		When Not Matched Then
			Insert
			(
				Name,
				StartDate,
				EndDate,
				DisplayChar,
				DisplayColour
			)
			Values (
				source.ActionName,
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
					cs.Id,
					null,
					0
				From
					@CalendarSeedTaskType c
					join plantbase.SeedAction sa on sa.Id = c.ActionId
					join plantbase.CalendarSeed cs on cs.FK_SeedId = c.SeedId and cs.FK_CalendarId = @CalendarId

End