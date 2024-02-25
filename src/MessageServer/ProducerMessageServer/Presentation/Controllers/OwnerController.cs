using MessageServer.Domain;
using MessageServer.Infrastructure;
using MessageServer.Infrastructure.Repositories.Abstractions;
using MessageServer.Infrastructure.Repositories.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace MessageServer.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OwnerController : ControllerBase
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IPetRepository _petRepository;

    public OwnerController(IOwnerRepository ownerRepository,
                           IPetRepository petRepository)
    {
        _ownerRepository = ownerRepository;
        _petRepository = petRepository;
    }

    [HttpPost]
    [Route("[action]/{owner}")]
    public async Task<IActionResult> CreateNewOwnerAsync(Owner owner)
    {
        var result = await _ownerRepository.CreateAsync(owner);
        return Ok(result);
    }

    [HttpGet]
    [Route("[action]/{id:guid}")]
    public async Task<IActionResult> GetOwnerAsync(Guid id)
    {
        var result = await _ownerRepository.GetAsync(id);
        var resultDto = OwnerMapper.EntityToDto(result);
        
        return Ok(resultDto);
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetOwnersListAsync()
    {
        var result = await _ownerRepository.GetAllAsync();
        return Ok(result);
    }

    [HttpPost]
    [Route("[action]/{owner}")]
    public async Task<IActionResult> UpdateOwnerAsync(Owner owner)
    {
        await _ownerRepository.UpdateAsync(owner);
        return Ok();
    }

    [HttpDelete]
    [Route("[action]/{id:guid}/{deleteConfirmation:bool}")]
    public async Task<IActionResult> DeleteOwnerAsync(Guid id, bool deleteConfirmation)
    {
        await _ownerRepository.DeleteAsync(id, deleteConfirmation);
        return Ok();
    }

    [HttpPost]
    [Route("[action]/{ownerId:guid}/{petId:guid}")]
    public async Task<IActionResult> AddPetToOwnerAsync(Guid ownerId, Guid petId)
    {
        var owner = await _ownerRepository.GetAsync(ownerId);
        var pet = await _petRepository.GetAsync(petId);

        if (pet == null) return NotFound(petId);
        
        _ownerRepository.AddPet(owner, pet);
        return Ok();
    }
    
    [HttpPost]
    [Route("[action]/{ownerId:guid}/{petId:guid}")]
    public async Task<IActionResult> RemovePetFromOwner(Guid ownerId, Guid petId)
    {
        var owner = await _ownerRepository.GetAsync(ownerId);
        var pet = await _petRepository.GetAsync(petId);

        if (pet == null) return NotFound(petId);
        
        _ownerRepository.RemovePet(owner, pet);
        return Ok();

    }
}