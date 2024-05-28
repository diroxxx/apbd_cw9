using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApp.Data;
using webApp.DTOs;

namespace webApp.Controllers;
[ApiController]
[Route("api/")]
public class ClientController: ControllerBase
{
    private readonly ApbdContext _context;

    public ClientController(ApbdContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTrips()
    {
        var trips = await _context.Trips.Select(e => new
        {
            Name = e.Name,
            Countries = e.IdCountries.Select(c => new
            {
                Name = c.Name
            })
        }).ToListAsync();
        
        return Ok(trips);
    }
    [HttpPost]
    [Route("trips/clients{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(i => i.IdClient == idClient);
        
        if (client == null)
        {
            return NotFound("Client with given id doesn't exist");
        }
        
        var isClientHasTrip = _context.ClientTrips.AnyAsync()

        _context.Clients.Remove(client);
        
        return Ok("Client has been removed");
    }
    [HttpPost]
    [Route("trips/{addClient}/clients")]
    public async Task<IActionResult> AddClientToTrip(AddClient addClient)
    {
        
        
        
        
        return Ok();
    }
    
}