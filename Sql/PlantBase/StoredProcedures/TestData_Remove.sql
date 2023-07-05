Create or Alter Procedure plantbase.TestData_Remove
AS
BEGIN

	declare @TestCucumber bigint = (Select Top 1 Id From plantbase.Seed Where PlantType = 'TEST: Cucumber')
	declare @TestTomato bigint = (Select Top 1 Id From plantbase.Seed Where PlantType = 'TEST: Tomato')
	declare @TestCourgette bigint = (Select Top 1 Id From plantbase.Seed Where PlantType = 'TEST: Courgette')

	declare @TestCurrentCalendar bigint = (Select Top 1 Id From plantbase.Calendar Where Name = 'TEST: Current Greenhouse Calendar');
	declare @TestPreviousCalendar bigint = (Select Top 1 Id From plantbase.Calendar Where Name = 'TEST: A Previous Calendar');

	Declare @TaskTypeTable table (
		Id bigint
	)

	Insert Into
			@TaskTypeTable
			(
				tt.Id
			)
		Select
				t.Id
			From
				plantbase.Task t
				join plantbase.CalendarSeed cs on t.FK_CalendarSeedId = cs.Id
				join plantbase.TaskType tt on tt.Id = t.FK_TaskTypeId
			Where
				cs.FK_CalendarId = @TestCurrentCalendar
				or cs.FK_CalendarId = @TestPreviousCalendar
	
	
	Delete From
			plantbase.Task
		Where
			Id in
			(
				Select
						t.Id
					From
						plantbase.Task t
						join plantbase.CalendarSeed cs on t.FK_CalendarSeedId = cs.Id
					Where
						cs.FK_CalendarId = @TestCurrentCalendar
						or cs.FK_CalendarId = @TestPreviousCalendar

			)

	Delete From
			plantbase.TaskType
		Where
			Id in
			(
				Select
						Id
					From
						@TaskTypeTable
			)

	Delete From
			plantbase.CalendarSeed
		Where
			FK_SeedId = @TestTomato
			or FK_SeedId = @TestCucumber
			or FK_SeedId = @TestCourgette

	Delete From
			plantbase.Calendar
		Where
			Id = @TestCurrentCalendar
			or Id = @TestPreviousCalendar	

	Delete From
			plantbase.SeedAction
		Where
			FK_SeedId = @TestTomato

	Delete From
			plantbase.Seed
		Where
			Id = @TestTomato
			or Id = @TestCucumber
			or Id = @TestCourgette

END