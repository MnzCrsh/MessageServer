using MessageServer.Domain;
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
        await _ownerRepository.GetAsync(id);
        //TODO: Map Owner to Owner DTO
        return Ok();
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

    // [HttpPost]
    // [Route("[action]/{owner}/{pet}")]
    // public async Task<IActionResult> AddPetToOwnerAsync(Guid ownerId, Guid petId)
    // {
    //     var owner = await _ownerRepository.GetAsync(ownerId);
    //     var pet = _petRepository.GetAsync(petId);
    //     
    //     _ownerRepository.AddPet(owner, pet);
    // }
    //
    // [HttpPost]
    // [Route("[action]/{owner}/{pet}")]
    // public void RemovePetFromOwner(Owner owner, Pet pet)
    // {
    //     _ownerRepository.RemovePet(owner, pet);
    // }
}