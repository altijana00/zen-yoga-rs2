using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZEN_Yoga.Models.Responses;
using ZEN_Yoga.Services.Interfaces.Base;
using ZEN_Yoga.Services.Interfaces.UserClass;
using ZEN_Yoga.Services.Services;

namespace ZEN_YogaWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserClassController : ControllerBase
    {

        [Authorize(Roles = "1")]
        [HttpGet("getAll")]
        public async Task<ActionResult<List<UserClassesResponse>>> GetAll([FromServices] IGetUserClassService getUserClassService)
        {
            var classes = await getUserClassService.GetAll();

            if (classes == null)
            {
                return NoContent();
            }
            return Ok(classes);
        }


        [HttpGet("getById")]
        public async Task<ActionResult<List<UserClassesResponse>>> GetById([FromServices] IGetUserClassService getUserClassService, int id)
        {
            var userClasses = await getUserClassService.GetById(id);

            if (userClasses == null)
            {
                return NoContent();
            }
            return Ok(userClasses);
        }

        [HttpPost("join")]
        public async Task<IActionResult> Join(int classId, int userId, [FromServices] IUpsertUserClassService upsertUserClassService )
        {


            if(await upsertUserClassService.Join(classId, userId))
            {
                return Ok(new { Message = "Joined class successfully" });
            }
            return BadRequest(new { Message = "Class already exists!" });
           
            
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id, [FromServices] IDeleteService deleteService)
        {


            if (await deleteService.Delete(id))
            {
                return Ok(new { Message = "Class deleted!" });
            }
            return BadRequest();


        }
    }
}
