Create or Alter View plantbase.SeedView
AS
	Select
			s.Id,
			s.Breed,
			s.Description,
			s.PlantType,
			s.SunRequirement,
			s.WaterRequirement,
			ActionId = sa.Id,
			ActionType = a.Name,
			a.DisplayChar,
			a.DisplayColour,
			sa.StartDate,
			sa.EndDate			
		From
			plantbase.Seed s
			left join plantbase.SeedAction sa on sa.FK_SeedId = s.Id
			left join plantbase.ActionType a on sa.FK_ActionTypeId = a.Id