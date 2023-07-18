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
				o.[Value]
			From
				OPENJSON(@SeedDetailsJson) o


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
				Where
					coalesce(sa.StartDate, '') != ''
					and coalesce(sa.EndDate, '') != ''


		select Id = @CalendarId

END