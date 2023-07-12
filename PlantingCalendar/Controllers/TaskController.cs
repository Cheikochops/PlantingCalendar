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

    [HttpGet("types")]
    public async Task<ActionResult> GetTaskTypes()
    {
        var taskTypes = _taskHelper.GetRepeatableTypes();

        return Ok(taskTypes);
    }

    [HttpPost("new")]
    public async Task<ActionResult> CreateNewTask([FromBody]UploadNewTask task)
    {
        await _taskHelper.CreateNewTask(task);
        return Ok();
    }

    [HttpPost("")]
    public async Task<ActionResult> UpdateTask(long taskId, [FromBody]UploadTaskDetails task)
    {
        await _taskHelper.EditTask(taskId, task);
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