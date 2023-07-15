Create or ALTER Procedure [plantbase].[Task_Update]
(
	@TaskId bigint,
	@TaskDetailsJson nvarchar(max)
)
As
Begin

	Update
			plantbase.Task
		Set
			Name = td.TaskName,
			Description = td.TaskDescription,
			IsRanged = case when td.TaskStartDate is null then 0 else 1 end,
			RangeEndDate = td.TaskEndDate,
			RangeStartDate = td.TaskStartDate,
			SetDate = td.TaskSetDate,
			DisplayChar = td.DisplayChar,
			DisplayColour = td.DisplayColour,
			IsComplete = 0,
			IsDisplay = td.IsDisplay
		From
		(
			Select
					t.TaskName,
					t.TaskDescription,
					t.DisplayChar,
					t.DisplayColour,
					t.TaskStartDate,
					t.TaskEndDate,
					t.TaskSetDate,
					t.IsDisplay
				From
					OPENJSON(@TaskDetailsJson) 
					With 
					(
						TaskName varchar(max),
						TaskDescription varchar(max),
						DisplayChar char,
						DisplayColour varchar(max),
						TaskStartDate DateTime,
						TaskEndDate DateTime,
						TaskSetDate DateTime,
						IsDisplay bit
					) as t
		) as td
		Where
			Id = @TaskId

	
End
