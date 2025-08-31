using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZEN_Yoga.Models;
using ZEN_Yoga.Models.Requests;
using ZEN_Yoga.Models.Responses;
using ZEN_Yoga.Services.Interfaces.Base;
using ZEN_Yoga.Services.Interfaces.City;
using ZEN_Yoga.Services.Interfaces.Role;
using ZEN_Yoga.Services.Interfaces.User;
using ZEN_Yoga.Services.Services;

namespace ZEN_YogaWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        [Authorize(Roles = "1")]
        [HttpGet("getAll")]        
        public async Task<ActionResult<List<UserResponse>>> GetAll([FromServices] IGetUserService getUserService)
        {
            var users = await getUserService.GetAll();

            if (users == null)
            {
                return NoContent();
            }
            return Ok(users);
        }


        [Authorize(Roles = "1")]
        [HttpGet("getById")]
        public async Task<ActionResult<List<UserResponse>>> GetById([FromServices] IGetUserService getUserService, int id)
        {
            var user = await getUserService.GetById(id);

            if (user == null) 
            {
                return NoContent();
            }

            return Ok(user);
        }

        [Authorize(Roles = "1")]
        [HttpGet("getByEmail")]
        public async Task<ActionResult<List<UserResponse>>> GetByEmail([FromServices] IGetUserService getUserService, string email)
        {
            var user = await getUserService.GetByEmail(email);

            if (user == null)
            {
                return NoContent();
            }
            return Ok(user);
        }

        [Authorize(Roles = "1")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] RegisterUser registerUser, 
                                            [FromServices] IUpsertUserService<RegisterUser> upsertUserService,
                                            [FromServices] IUserValidatorService userValidatorService,
                                            [FromServices] IRoleValidatorService roleValidatorService,
                                            [FromServices] ICityValidatorService cityValidatorService)
        {
            if (registerUser == null) {
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

            await upsertUserService.Add(userValidatorService, roleValidatorService, cityValidatorService, registerUser);

            return Ok(new { Message = "User registered!" });
        }

        
        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody] EditUser editUser, int id, [FromServices] IUpsertUserService<RegisterUser> upsertUserService, [FromServices] IUserValidatorService userValidatorService)
        {
            if (editUser == null)
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

            await userValidatorService.ValidateUserId(id);

            await upsertUserService.Edit(editUser, id);

            return Ok(new { Message = "Changes saved successfully!" });
        }

        [Authorize(Roles = "1")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery]int id, [FromServices] IDeleteService deleteService)
        {
            if (await deleteService.Delete(id))
            {
                return Ok(new { Message = "User deleted"! });
            }
            return BadRequest(new { Message = "There is no user with this ID!" });
        }
    }
}
