Create or Alter Procedure plantbase.Seed_Display_Read	
As
Begin

	Select
			s.Id,
			s.PlantType,
			s.Breed,
			s.SunRequirement,
			s.WaterRequirement,
			s.IsExpired
		From
			plantbase.SeedView s
		Where
			s.IsDeleted = 0

End