Create or ALTER Procedure [plantbase].[Seed_Save]
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
				Select distinct
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
						ExpiryDate DateTime
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
						Actions nvarchar(max) as JSON 
						) as s
					outer apply (
						Select distinct
								*
							From
								OPENJSON(s.Actions)
							with (
								ActionName varchar(100),
								ActionDescription varchar(max),
							    ActionId bigint,
								IsDisplay bit,
								ActionType int,
								DisplayChar char,
								DisplayColour varchar(10),
								StartDate varchar(4),
								EndDate varchar(4)
							)
					) as a
				) as source
			on target.Id = source.ActionId
		When Not matched by target then
			Insert (Name, Description, FK_SeedId, Enum_ActionTypeId, IsDisplay, DisplayChar, DisplayColour, StartDate, EndDate)
			Values (source.ActionName, source.ActionDescription, @seedId, source.ActionType, source.IsDisplay, source.DisplayChar, source.DisplayColour, source.StartDate, source.EndDate)
		When matched then 
			Update Set
				target.Name = source.ActionName,
				target.Description = source.ActionDescription,
				target.DisplayChar = source.DisplayChar,
				target.DisplayColour = source.DisplayColour,
				target.StartDate = source.StartDate,
				target.EndDate = source.EndDate
		WHEN NOT MATCHED BY SOURCE then
			Delete;
		
End
