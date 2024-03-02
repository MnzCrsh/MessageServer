using MessageServer.Application.Abstractions;
using MessageServer.Domain;
using MessageServer.Infrastructure;
using MessageServer.Infrastructure.Repositories.Abstractions;
using MessageServer.Infrastructure.Repositories.Implementations;
using MessageServer.Presentation.ControllerFilters;
using Microsoft.AspNetCore.Mvc;

namespace MessageServer.Presentation.Controllers;

[ApiController, ExceptionInterceptor]
[Route("api/[controller]/[action]")]
public class OwnerController(IOwnerRepository ownerRepository,
                             IPetRepository petRepository,
                             IRabbitMqService rabbitMq) : ControllerBase
{
    [HttpPost]
    [Route("{owner}")]
    public async Task<IActionResult> CreateOwnerAsync(Owner owner)
    {
        var result = await ownerRepository.CreateAsync(owner);
        
        rabbitMq.SendMessage(result);
        return Ok(result);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetOwnerAsync(Guid id)
    {
        var result = await ownerRepository.GetAsync(id);
        var resultDto = OwnerMapper.EntityToDto(result);
        
        return Ok(resultDto);
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetOwnersListAsync()
    {
        var result = await ownerRepository.GetAllAsync();
        return Ok(result);
    }

    [HttpPost]
    [Route("{owner}")]
    public async Task<IActionResult> UpdateOwnerAsync(Owner owner)
    {
        await ownerRepository.UpdateAsync(owner);
        return Ok();
    }

    [HttpDelete]
    [Route("{id:guid}/{deleteConfirmation:bool}")]
    public async Task<IActionResult> DeleteOwnerAsync(Guid id, bool deleteConfirmation)
    {
        await ownerRepository.DeleteAsync(id, deleteConfirmation);
        return Ok();
    }

    [HttpPost]
    [Route("{ownerId:guid}/{petId:guid}")]
    public async Task<IActionResult> AddPetToOwnerAsync(Guid ownerId, Guid petId)
    {
        var owner = await ownerRepository.GetAsync(ownerId);
        var pet = await petRepository.GetAsync(petId);

        if (pet == null) return NotFound(petId);
        
        ownerRepository.AddPet(owner, pet);
        return Ok();
    }
    
    [HttpPost]
    [Route("{ownerId:guid}/{petId:guid}")]
    public async Task<IActionResult> RemovePetFromOwner(Guid ownerId, Guid petId)
    {
        var owner = await ownerRepository.GetAsync(ownerId);
        var pet = await petRepository.GetAsync(petId);

        if (pet == null) return NotFound(petId);
        
        ownerRepository.RemovePet(owner, pet);
        return Ok();

    }
}