Create or Alter View plantbase.SeedView
AS
	Select
			s.Id,
			s.Breed,
			s.Description,
			s.PlantType,
			s.SunRequirement,
			s.WaterRequirement,
			s.ExpiryDate,
			ActionId = sa.Id,
			ActionType = sa.Name,
			sa.DisplayChar,
			sa.DisplayColour,
			sa.StartDate,
			sa.EndDate,
			IsExpired = cast(case when s.ExpiryDate is null or s.ExpiryDate > GETUTCDATE() then 0 else 1 end as bit),
			s.IsDeleted
		From
			plantbase.Seed s
			left join plantbase.SeedAction sa on sa.FK_SeedId = s.Id