using System.Runtime.InteropServices.JavaScript;
using webApp.Data;
using webApp.DTOs;

namespace webApp.Repositories;

public interface IClientTripRepository
{


    Task<PagedResult> GetTrip();
    
    Task<bool> DoesClientExist(int idClient);
    Task<bool> IsClientHasTrip(int idClient);
    Task removeClient(int idClient);
    Task<bool> IsClientExistWithGivenPesel(String pesel);
    Task<bool> IsClientExistWithGivenPeselInTrips(String pesel);
    Task<bool> IsTripExist(int idTrip);
    Task<bool> IsDateFromValid(int idTrip);
    Task<int> AddClient(AddClient addClient);
    Task AddClientToTrip(int idClient, AddClient addClient);

}