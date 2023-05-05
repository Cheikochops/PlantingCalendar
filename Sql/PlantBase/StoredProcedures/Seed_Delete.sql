Create or Alter Procedure [plantbase].Seed_Delete
(
	@Id bigint
)
As
Begin


	Update 
			plantbase.Seed
		Set
			IsDeleted = 1
		Where
			Id = @Id

End
GO


