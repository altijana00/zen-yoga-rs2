using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZEN_Yoga.Models.Requests;
using ZEN_Yoga.Models.Responses;
using ZEN_Yoga.Services.Interfaces.Base;
using ZEN_Yoga.Services.Interfaces.Studio;
using ZEN_Yoga.Services.Services;
using ZEN_Yoga.Services.Services.Studio;

namespace ZEN_YogaWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class StudioController : ControllerBase
    {

        [HttpGet("getAll")]
        public async Task<ActionResult<List<StudioResponse>>> GetAll([FromServices] IGetStudioService getStudioService)
        {
            var studios = await getStudioService.GetAll();

            if (studios == null)
            {
                return NoContent();
            }
            return Ok(studios);
        }

        [Authorize(Roles = "1, 2")]
        [HttpGet("getByOwner")]
        public async Task<ActionResult<List<StudioResponse>>> GetByOwner([FromServices] IGetStudioService getStudioService, int ownerId)
        {
            var studios = await getStudioService.GetByOwner(ownerId);

            if (studios == null)
            {
                return NoContent();
            }
            return Ok(studios);
        }

        [Authorize(Roles = "1, 2")]
        [HttpGet("getById")]
        public async Task<ActionResult<StudioResponse>> GetById([FromServices] IGetStudioService getStudioService, int id)
        {
            var studio = await getStudioService.GetById(id);

            if (studio == null)
            {
                return NoContent();
            }
            return Ok(studio);
        }

        [Authorize(Roles = "1, 2")]
        [HttpPost("add")]
        public async Task<IActionResult> AddStudio([FromBody] AddStudio addStudio, [FromServices] IUpsertStudioService<AddStudio> upsertStudioService, [FromServices] IStudioValidatorService studioValidatorService)
        {
            if (addStudio == null) 
            {
                return BadRequest();
            
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();

                return BadRequest(new { Message = errors });
            }

            await studioValidatorService.ValidateName(addStudio.Name);

            await upsertStudioService.Add(addStudio);
            return Ok(new {Message = "Studio added successfully!"});
        }

        [Authorize(Roles = "1, 2")]
        [HttpPut("edit")]
        public async Task<IActionResult> EditStudio([FromBody] EditStudio editStudio, int id, [FromServices] IUpsertStudioService<AddStudio> upsertStudioService, [FromServices] IStudioValidatorService studioValidatorService )
        {
            if (editStudio == null)
            {
                return BadRequest();

            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();

                return BadRequest(new { Message = errors });
            }

            await studioValidatorService.ValidateStudio(id);

            await upsertStudioService.Edit(editStudio, id);
            return Ok(new { Message = "Changes saved successfully!" });
        }

        [Authorize(Roles = "1, 2")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id, [FromServices] IDeleteService deleteService)
        {
            if (await deleteService.Delete(id))
            {
                return Ok(new { Message = "Studio deleted"! });
            }
            return BadRequest(new { Message = "There is no studio with this ID!" });
        }

       
    }
}
