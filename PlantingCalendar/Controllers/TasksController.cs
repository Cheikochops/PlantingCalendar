using Microsoft.AspNetCore.Mvc;
using PlantingCalendar.DataAccess;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using PlantingCalendar.Pages;
//using System.Web.Http;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskDataAccess _dataAccess;
    public TasksController(ITaskDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    [HttpPost("")]
    public async Task<ActionResult> SetTaskDate(UploadTaskDate task)
    {
        await _dataAccess.SetTaskDate(task);

        return Ok();
    }

    [HttpGet("types")]
    public async Task<ActionResult> TaskTypes()
    {
        var taskTypes = await _dataAccess.GetRepeatableTypes();

        return Ok(taskTypes);
    }

    [HttpPost("")]
    public async Task<ActionResult> NewTask(UploadNewTask task)
    {

        return Ok();
    }

    [HttpDelete("")]
    public async Task<ActionResult> DeleteTask(long taskId)
    {

        return Ok();
    }

    [HttpPut("complete")]
    public async Task<ActionResult> CompleteTask(long taskId)
    {

        return Ok();
    }

}