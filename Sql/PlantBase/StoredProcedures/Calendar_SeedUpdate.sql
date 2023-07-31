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


		Delete From
				plantbase.Task
			Where
				FK_CalendarSeedId in
				(
					Select
							Id 
						From
							@RemovableCalendarSeedIds
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
		
		Insert Into
			plantbase.Task
			(
				FK_CalendarSeedId,
				Name,
				Description,
				IsRanged,
				RangeStartDate,
				RangeEndDate,
				SetDate,
				DisplayChar,
				DisplayColour,
				IsComplete,
				IsDisplay
			)
			Select
					cs.Id,
					sa.Name,
					sa.Description,
					case 
						when sa.StartDate = sa.EndDate then 0
						else 1
					end,
					case 
						when sa.StartDate = sa.EndDate then null
						else Parse(concat(c.Year, '-', right(sa.StartDate, 2), '-', left(sa.StartDate, 2)) as date)
					end,
					case 
						when sa.StartDate = sa.EndDate then null
						else Parse(concat(c.Year, '-', right(sa.EndDate, 2), '-', left(sa.EndDate, 2)) as date)
					end,
					case 
						when sa.StartDate = sa.EndDate then  Parse(concat(c.Year, '-', right(sa.StartDate, 2), '-', left(sa.StartDate, 2)) as date)
						else null
					end,
					sa.DisplayChar,
					sa.DisplayColour,
					0,
					sa.IsDisplay
				From
					plantbase.CalendarSeed cs
					join plantbase.SeedAction sa on cs.FK_SeedId = sa.FK_SeedId	
					join plantbase.Calendar c on cs.FK_CalendarId = c.Id and c.Id = @calendarId
					join @newCalendarSeedId n on cs.Id = n.Id
				Where
					coalesce(sa.StartDate, '') != ''
					and coalesce(sa.EndDate, '') != ''

End

