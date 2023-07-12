Create or ALTER Procedure [plantbase].[Task_Delete]
(
	@TaskId bigint
)
As
Begin

	declare @taskTypeId bigint = 
	(
		Select Top 1 
				tt.Id 
			From 
				plantbase.TaskType tt
				join plantbase.Task t on tt.Id = t.FK_TaskTypeId
			Where
				t.Id = @TaskId
	);

	Delete From 
			plantbase.Task
		Where
			FK_TaskTypeId = @taskTypeId

	Delete From 
			plantbase.TaskType
		Where
			Id = @taskTypeId

End
