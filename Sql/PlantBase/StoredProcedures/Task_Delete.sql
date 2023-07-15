Create or ALTER Procedure [plantbase].[Task_Delete]
(
	@TaskId bigint
)
As
Begin

	Delete From 
			plantbase.Task
		Where
			Id = @TaskId

End
