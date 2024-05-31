using System.Runtime.InteropServices.JavaScript;

namespace webApp.DTOs;


public class GetTrip
{
    public String Name { get; set; }
    public String Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public List<Country> Countries { get; set; }
    public List<Country> Clients { get; set; }
}

public class Country
{
    public String Name { get; set; }
}
public class Client
{
    public String FirstName { get; set; }
    public String LastName { get; set; }
}