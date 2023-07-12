Create or ALTER Procedure [plantbase].[Task_Update]
(
	@TaskId bigint,
	@TaskDetailsJson nvarchar(max)
)
As
Begin

	Declare @TaskDetails table (
		TaskName varchar(max),
		TaskDescription varchar(max),
		DisplayChar char,
		DisplayColour varchar(max),
		TaskStartDate DateTime,
		TaskEndDate DateTime,
		TaskSetDate DateTime
	)

	Insert Into
		@TaskDetails
		(
			TaskName,
			TaskDescription,
			DisplayChar,
			DisplayColour,
			TaskStartDate,
			TaskEndDate,
			TaskSetDate
		)
		Select
				t.TaskName,
				t.TaskDescription,
				t.DisplayChar,
				t.DisplayColour,
				t.TaskStartDate,
				t.TaskEndDate,
				t.TaskSetDate
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
					TaskSetDate DateTime
				) as t

	Update
			plantbase.Task
		Set
			TaskDate = (Select TaskSetDate From @TaskDetails)

	declare @taskTypeId bigint = (Select Top 1 FK_TaskTypeId from plantbase.Task Where Id = @TaskId)

	Update
			tt
		Set
			Name = td.TaskName,
			Description = td.TaskDescription,
			StartDate = td.TaskStartDate,
			EndDate = td.TaskEndDate,
			DisplayChar = td.DisplayChar,
			DisplayColour = td.DisplayColour
		From
			@TaskDetails as td
			join plantbase.TaskType tt on tt.Id = @taskTypeId

	
End
