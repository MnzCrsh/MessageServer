using MessageServer.Domain;
using MessageServer.Infrastructure;
using MessageServer.Infrastructure.Repositories.Abstractions;
using MessageServer.Infrastructure.Repositories.Implementations;
using MessageServer.Presentation.ControllerFilters;
using Microsoft.AspNetCore.Mvc;

namespace MessageServer.Presentation.Controllers;

[ApiController, ExceptionInterceptor]
[Route("api/[controller]/[action]")]
public class PetController : ControllerBase
{
    private readonly IPetRepository _repository;

    public PetController(IPetRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    [Route("{pet}")]
    public async Task<IActionResult> CreatePet(Pet pet)
    {
        var result = await _repository.CreateAsync(pet);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetPet(Guid id)
    {
        var result = await _repository.GetAsync(id);
        if (result == null) return NotFound(id);

        var resultDto = PetMapper.EntityToDto(result);
        
        return Ok(resultDto);
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllPets(CancellationToken ct)
    {
        var result = await _repository.GetAllAsync(ct);
        return Ok(result);
    }

    [HttpPost]
    [Route("{pet}")]
    public async Task<IActionResult> UpdatePet(Pet pet)
    {
        await _repository.UpdateAsync(pet);
        return Ok();
    }

    [HttpDelete]
    [Route("{id:guid}/{confirmDelete:bool}")]
    public async Task<IActionResult> DeletePet(Guid id, bool confirmDelete)
    {
        await _repository.DeleteAsync(id, confirmDelete);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetPetsByOwnerId(Guid ownerId)
    {
        var result = await _repository.GetPetsByOwnerAsync(ownerId);
        
        return Ok(result);
    }
}