Create or Alter Procedure plantbase.Seed_Display_Read	
As
Begin

	Select
			s.Id,
			s.PlantType,
			s.Breed,
			s.SunRequirement,
			s.WaterRequirement,
			IsExpired = case 
							when s.ExpiryDate is null then 0 
							when s.ExpiryDate < GetUtcDate() then 1
							else 0
						end
		From
			plantbase.SeedView s

End