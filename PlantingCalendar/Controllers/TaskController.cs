using Microsoft.AspNetCore.Mvc;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;

[ApiController]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    private readonly ITaskHelper _taskHelper;
    public TaskController(ITaskHelper taskHelper)
    {
        _taskHelper = taskHelper;
    }

    [HttpPost("")]
    public async Task<ActionResult> SetTaskDate(UploadTaskDate task)
    {
        await _taskHelper.SetTaskDate(task);

        return Ok();
    }

    [HttpGet("types")]
    public async Task<ActionResult> GetTaskTypes()
    {
        var taskTypes = _taskHelper.GetRepeatableTypes();

        return Ok(taskTypes);
    }

    [HttpPost("")]
    public async Task<ActionResult> CreateNewTask([FromBody]UploadNewTask task)
    {
        await _taskHelper.CreateNewTask(task);
        return Ok();
    }

    [HttpDelete("")]
    public async Task<ActionResult> DeleteTask(long taskId)
    {
        await _taskHelper.DeleteTask(taskId);

        return Ok();
    }

    [HttpPut("complete")]
    public async Task<ActionResult> ToggleCompleteTask(long taskId)
    {
        await _taskHelper.ToggleCompleteTask(taskId);

        return Ok();
    }

}