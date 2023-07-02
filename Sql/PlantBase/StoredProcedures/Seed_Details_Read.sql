Create or Alter Procedure [plantbase].Seed_Details_Read
(
	@Id bigint,
	@IncludeDeleted bit
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
			ExpiryDate,
			ActionId,
			ActionName,
			ActionType,
			DisplayChar,
			DisplayColour,
			StartDate,
			EndDate
		From
			plantbase.SeedView c
		Where
			c.Id = @Id
			and ((@IncludeDeleted = 0 and IsDeleted = 0) or @IncludeDeleted = 1)

End
GO


