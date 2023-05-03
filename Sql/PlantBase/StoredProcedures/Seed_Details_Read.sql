Create or Alter Procedure [plantbase].Seed_Details_Read
(
	@Id bigint
)
As
Begin

	Select Distinct
			Id,
			PlantType,
			Breed,
			Description,
			SunRequirement,
			WaterRequirement,
			ActionId,
			ActionType,
			DisplayChar,
			DisplayColour,
			StartDate,
			EndDate
		From
			plantbase.SeedView c
		Where
			c.Id = @Id

End
GO


