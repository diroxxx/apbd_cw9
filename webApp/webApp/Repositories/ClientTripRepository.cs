using Microsoft.EntityFrameworkCore;
using webApp.Data;
using webApp.DTOs;
using webApp.Models;
using Client = webApp.Models.Client;

namespace webApp.Repositories;

public class ClientTripRepository: IClientTripRepository
{
    
    private readonly ApbdContext _context;

    public ClientTripRepository(ApbdContext context)
    {
        _context = context;
        
       
    }


    public async Task<PagedResult> GetTrip()
    {

     // IEnumerable<PagedResult>  pagedResult = new PagedResult();
        
        // var trips = _context.
        // var trips = await (from trip in _context.Trips
        //     join countryTrip in _context.Cou on trip.IdTrip equals countryTrip.IdTrip
        //     join country in _context.Countries on countryTrip.IdCountry equals country.IdCountry
        //     join clientTrip in _context.ClientTrips on trip.IdTrip equals clientTrip.IdTrip
        //     join client in _context.Clients on clientTrip.IdClient equals client.IdClient
        //     group new { trip, country, client } by new
        //     {
        //         trip.IdTrip,
        //         trip.Name,
        //         trip.Description,
        //         trip.DateFrom,
        //         trip.DateTo,
        //         trip.MaxPeople
        //     } into tripGroup
        //     select new
        //     {
        //         tripGroup.Key.Name,
        //         tripGroup.Key.Description,
        //         tripGroup.Key.DateFrom,
        //         tripGroup.Key.DateTo,
        //         tripGroup.Key.MaxPeople,
        //         Countries = tripGroup.Select(g => g.country.Name).Distinct(),
        //         Clients = tripGroup.Select(g => new
        //         {
        //             g.client.FirstName,
        //             g.client.LastName
        //         }).Distinct()
        //     }).ToListAsync();
        //
        // return new Task<IEnumerable<PagedResult>>();

        return new PagedResult();
    }


    public async Task<bool> DoesClientExist(int idClient)
    {
       var client = await _context.Clients.AnyAsync(i => i.IdClient == idClient);
       return client;

    }

    public async Task<bool> IsClientHasTrip(int idClient)
    {
        var isClientHasTrip = await _context.ClientTrips.AnyAsync(i => i.IdClient == idClient);
        return isClientHasTrip;
    }

    public async Task removeClient(int idClient)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(i => i.IdClient == idClient);

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

    }

    public async Task<bool> IsClientExistWithGivenPesel(string pesel)
    {
        var client =await  _context.Clients.AnyAsync(i => i.Pesel == pesel);
        return client;

    }

    public async Task<bool> IsClientExistWithGivenPeselInTrips(string pesel)
    {
        var client = _context.Clients.FirstOrDefaultAsync(i => i.Pesel == pesel);

        var clientInTrips = await _context.ClientTrips.AnyAsync(i => i.IdClient == client.Id);
        return clientInTrips;
    }


    public async Task<bool> IsTripExist(int idTrip)
    {
        return await _context.Trips.AnyAsync(t => t.IdTrip == idTrip);
        
    }
    
    public async Task<bool> IsDateFromValid(int idTrip)
    {
        var trip = await _context.Trips.FirstOrDefaultAsync(i => i.IdTrip == idTrip);
        if (trip == null)
        {
            return false; 
        }

        return trip.DateFrom >= DateTime.Now;
    }

    public async Task<int> AddClient(AddClient addClient)
    {
        var client = new Client()
        {
            FirstName = addClient.FirstName,
            LastName = addClient.LastName,
            Email = addClient.Email,
            Telephone = addClient.Telephone,
            Pesel = addClient.Pesel
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
    
        return client.IdClient;
    }


    public async Task AddClientToTrip(int idClient, AddClient addClient)
    {

        _context.ClientTrips.Add(new ClientTrip()
        {
            IdClient = idClient,
            IdTrip = addClient.IdTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = addClient.PaymentDate
        });
        await _context.SaveChangesAsync();
    }
}