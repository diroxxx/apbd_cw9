using System.ComponentModel.DataAnnotations;

namespace webApp.DTOs;

public class AddClient
{
    public String FirstName { get; set; }
    public String LastName { get; set; }
    [EmailAddress]
    public String Email { get; set; }
    public String Telephone { get; set; }
    public String Pesel { get; set; }
    public int IdTrip { get; set; }
    public String TripName { get; set; }
    public DateTime? PaymentDate { get; set; }
}