Create or Alter Procedure [plantbase].Seed_Save
(
	@SeedDetailsJson varchar(max)
)
As
Begin

declare @SeedTable table (
	id bigint
)
--Save or update seeds

	Merge 
			plantbase.Seed as target
			using (
				Select
						*
					From
						OPENJSON(@SeedDetailsJson)
					With (
						Id bigint,
						PlantType varchar(100),
						Breed varchar(100),
						SunRequirement varchar(100),
						WaterRequirement varchar(100),
						Description varchar(1000),
						ExpiryDate DateTime,
						Actions nvarchar(max)
					)
				) as source
			on target.Id = source.Id
		When Not matched by target then
			Insert (PlantType, Breed, Description, WaterRequirement, SunRequirement, ExpiryDate)
			Values (source.PlantType, source.Breed, source.Description, source.WaterRequirement, source.SunRequirement, source.ExpiryDate)
		When matched then 
			Update Set
				target.PlantType = source.PlantType,
				target.Breed = source.Breed,
				target.Description = source.Description,
				target.SunRequirement = source.SunRequirement,
				target.WaterRequirement = source.WaterRequirement,
				target.ExpiryDate = source.ExpiryDate
		Output 
			Inserted.Id
		into
			@SeedTable;

	declare @seedId bigint = (Select Top 1 Id from @SeedTable)
-- Save or update Actions
	
		Merge 
			plantbase.SeedAction as target
			using (
				Select
						a.*
					From
						OPENJSON(@SeedDetailsJson)
					With (
						Actions nvarchar(max)
						) as s
					outer apply (
						Select
								*
							From
								OPENJSON(s.Actions)
							with (
							    ActionType varchar(100),
							    ActionId bigint,
								DisplayChar char,
								DisplayColour varchar(10),
								StartDate DateTime,
								EndDate DateTime
							)
					) as a
				) as source
			on target.Id = source.ActionId
		When Not matched by target then
			Insert (FK_SeedId, Name, DisplayChar, DisplayColour, StartDate, EndDate)
			Values (@seedId, source.ActionType, source.DisplayChar, source.DisplayColour, source.StartDate, source.EndDate)
		When matched then 
			Update Set
				target.Name = source.ActionType,
				target.DisplayChar = source.DisplayChar,
				target.DisplayColour = source.DisplayColour,
				target.StartDate = source.StartDate,
				target.EndDate = source.EndDate;

End
GO
