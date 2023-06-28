Create or Alter Procedure plantbase.NewCalendar_Create
(
	@CalendarName varchar(200),
	@CalendarYear int,
	@SeedDetailsJson nvarchar(max)
)
AS
BEGIN

	Declare @CalendarIds table
	(
		Id bigint
	)

	Insert plantbase.Calendar
		(
			Name,
			Year
		)
		Output INSERTED.Id 
			Into @CalendarIds
		VALUES (
			@CalendarName,
			@CalendarYear
		)

	declare @CalendarId bigint = (Select Top 1 Id From @CalendarIds)

	Insert Into
		plantbase.CalendarSeed
		(
			FK_CalendarId,
			FK_SeedId
		)
		Select
				@CalendarId,
				o.Id
			From
				OPENJSON(@SeedDetailsJson) With (
					SeedId bigint
				) as o

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
					ActionDescription = null, --SORT THIS OUT
					FK_RepeatableTypeId = null, --SORT THIS OUT
					sa.StartDate,
					sa.EndDate,
					sa.DisplayChar,
					sa.DisplayColour
				From
					plantbase.CalendarSeed cs
					join plantbase.SeedAction sa on cs.FK_SeedId = sa.FK_SeedId	
		) as source (SeedId, ActionId, ActionName, ActionDescription, FK_RepeatableTypeId, StartDate, EndDate, DisplayChar, DisplayColour) on 1 = 2
		When Not Matched Then
			Insert
			(
				Name,
				Description,
				FK_RepeatableTypeId,
				StartDate,
				EndDate,
				DisplayChar,
				DisplayColour
			)
			Values (
				source.ActionName,
				source.ActionDescription,
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

END