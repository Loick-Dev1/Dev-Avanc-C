using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Route("api/providers")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class ProviderController : ControllerBase
    {
        private readonly ProviderService _providerService;

        public ProviderController(ProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProviderDto>>> GetAll()
        {
            var providers = await _providerService.GetAllAsync();
            return Ok(providers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProviderDto>> GetById(Guid id)
        {
            var provider = await _providerService.GetByIdAsync(id);
            if (provider == null) return NotFound();
            return Ok(provider);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProviderRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _providerService.CreateProviderAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }
    }
}
