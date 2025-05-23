using FluentValidation;
using Marten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Techs.Api.Techs.Services;

namespace Techs.Api.Techs;

public class TechsController(ITechRepository repository) : ControllerBase
{
    [HttpPost("/techs")]
    public async Task<ActionResult> AddTechAsync(
        [FromBody] TechCreateModel request,
        [FromServices] IValidator<TechCreateModel> validator
        )
    {
        if(validator.Validate(request).IsValid == false)
        {
            return BadRequest();
        }

        var response = await repository.AddTechAsync(request);
       
     
        return Created($"/techs/{response.Id}", response);
    }

    [HttpGet("/techs/{id:guid}")]
    public async Task<ActionResult> GetATech(Guid id, CancellationToken token)
    {
       
        var response = await repository.GetTechByIdAsync(id, token);
       

        return response switch
        {
            null => NotFound(),
            _ => Ok(response)
        };
    }

    //[Authorize(Policy = "SoftwareCenter")]
    [HttpGet("/software/techs/{sub}")]
    public async Task<ActionResult> GetASoftwareTechBySub(string sub, CancellationToken token)
    {
       
        var response = await repository.GetSoftwareTechBySubAsync(sub, token);
       

        return response switch
        {
            null => NotFound(),
            _ => Ok(response.FirstName + " " + response.LastName)
        };
    }

    //[Authorize(Policy = "SoftwareCenter")]
    [HttpGet("/software/techs/")]
    public async Task<ActionResult> GetSoftwareTechs(int pageNumber, int pageSize, CancellationToken token)
    {

        var response = await repository.GetSoftwareTechsAsync(pageNumber, pageSize, token);


        return response switch
        {
            null => NotFound(),
            _ => Ok(response)
        };
    }
}