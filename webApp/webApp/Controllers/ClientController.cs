using System.Transactions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApp.Data;
using webApp.DTOs;
using webApp.Repositories;

namespace webApp.Controllers;
[ApiController]
[Route("api/")]
public class ClientController: ControllerBase
{
    private readonly IClientTripRepository _clientTripRepository;

    public ClientController(IClientTripRepository clientTripRepository)
    {
        _clientTripRepository = clientTripRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTrips()
    {
        // var trips = await _context.Trips.Select(e => new
        // {
        //     Name = e.Name,
        //     Countries = e.IdCountries.Select(c => new
        //     {
        //         Name = c.Name
        //     })
        // }).ToListAsync();
        
        return Ok();
    }
    [HttpPost]
    [Route("trips/clients{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        var isClientExist = _clientTripRepository.DoesClientExist(idClient);
        if (!await isClientExist) 
        {
            return NotFound("Client with given id doesn't exist");
        }


        var isClientHasTrip = _clientTripRepository.IsClientHasTrip(idClient);
        if ( await isClientHasTrip)
        {
            return BadRequest("Client has associated trips and cannot be deleted");
        }
        
        await _clientTripRepository.removeClient(idClient);
        
        return Ok("Client has been removed");
    }
    [HttpPost]
    [Route("trips/clients")]
    public async Task<IActionResult> AddClientToTrip(AddClient addClient)
    {
        var clientWithPesel = await _clientTripRepository.IsClientExistWithGivenPesel(addClient.Pesel);
        
        if (clientWithPesel)
        {
            return BadRequest("Client with given Pesel already exist");
        }
        
        var clientHasTrip = _clientTripRepository.IsClientExistWithGivenPeselInTrips(addClient.Pesel);

        if (await clientHasTrip)
        {
            return BadRequest("Client with given Pesel has  already been signed to trip");
        }

        var isTripExist = _clientTripRepository.IsTripExist(addClient.IdTrip);
        if (!await clientHasTrip)
        {
            return BadRequest("Trip with given id doesn't exist");
        }

        var dateFromExist = _clientTripRepository.IsDateFromValid(addClient.IdTrip);
        if (!await dateFromExist)
        {
            return BadRequest("DateFrom in Trip is in the past");

        }
        
        
        using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var addedClientId = await _clientTripRepository.AddClient(addClient);

             await _clientTripRepository.AddClientToTrip(addedClientId, addClient);
            scope.Complete();
        }
        
        return Ok();
    }
    
}