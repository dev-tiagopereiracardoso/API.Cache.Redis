using API.Cache.Redis.Domain.Implementation.Interfaces;
using API.Cache.Redis.Models.Input;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace API.Cache.Redis.Service.Controllers
{
    [Route("v1/cache")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "cache")]
    public class CacheController : ControllerBase
    {
        private readonly IRedisService _redisService;

        public CacheController(
                IRedisService redisService
            )
        {
            _redisService = redisService;
        }

        [HttpGet("{key}")]
        [SwaggerOperation(Summary = "")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status417ExpectationFailed)]
        public IActionResult Search(string key)
        {
            var Data = _redisService.Get(key);
            return StatusCode((int)HttpStatusCode.OK, Data);
        }

        [HttpPost("{key}")]
        [SwaggerOperation(Summary = "")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(bool))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status417ExpectationFailed)]
        public IActionResult Search(string key, [FromBody] CacheInput cacheInput)
        {
            var Data = _redisService.Set(key, cacheInput.Value);
            return StatusCode(Data ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest);
        }

        [HttpDelete()]
        [SwaggerOperation(Summary = "")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(bool))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status417ExpectationFailed)]
        public IActionResult Delete()
        {
            _redisService.Delete();

            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}