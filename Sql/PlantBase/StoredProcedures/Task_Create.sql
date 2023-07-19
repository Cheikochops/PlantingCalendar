Create or ALTER Procedure [plantbase].[Task_Create]
(
	@CalendarId bigint,
	@TaskDetailsJson nvarchar(max)
)
As
Begin

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
			t.Name,
			t.Description,
			t.IsRanged,
			t.RangeStartDate,
			t.RangeEndDate,
			t.SetDate,
			t.DisplayChar,
			t.DisplayColour,
			0,
			t.IsDisplay
		From
			OPENJSON(@TaskDetailsJson)
			With
			(
				SeedId bigint,
				Name varchar(max),
				Description varchar(max),
				IsRanged bit,
				RangeStartDate DateTime,
				RangeEndDate DateTime,
				SetDate DateTime,
				DisplayChar char,
				DisplayColour varchar(6),
				IsDisplay bit
			) t
			join plantbase.CalendarSeed cs on cs.FK_CalendarId = @CalendarId and cs.FK_SeedId = t.SeedId
End

