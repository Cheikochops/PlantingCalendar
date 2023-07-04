Create or ALTER Procedure [plantbase].[Task_ToggleComplete]
(
	@TaskId bigint
)
As
Begin

	Update
			plantbase.Task
		Set
			IsComplete = 1^IsComplete
		Where
			Id = @TaskId

End
