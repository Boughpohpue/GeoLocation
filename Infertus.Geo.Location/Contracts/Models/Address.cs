namespace Infertus.Geo.Location.Contracts.Models;

public class Address
{
    // Address
    public string? Road { get; set; }
    public string? HouseNumber { get; set; }

    // Local areas
    public string? Neighbourhood { get; set; }
    public string? Suburb { get; set; }
    public string? Borough { get; set; }

    // City-level
    public string? City { get; set; }
    public string? Village { get; set; }
    public string? Municipality { get; set; }

    // Regional
    public string? County { get; set; }
    public string? State { get; set; }

    // Postal / country
    public string? Postcode { get; set; }
    public string? Country { get; set; }


    public override string ToString()
    {
        var parts = new List<string>();

        if (HouseNumber != null) parts.Add(HouseNumber);
        if (Road != null) parts.Add(Road);
        if (Neighbourhood != null) parts.Add(Neighbourhood);
        if (Suburb != null) parts.Add(Suburb);
        if (Borough != null) parts.Add(Borough);
        if (City != null) parts.Add(City);
        if (Village != null) parts.Add(Village);
        if (Municipality != null) parts.Add(Municipality);
        if (County != null) parts.Add(County);
        if (State != null) parts.Add(State);
        if (Postcode != null) parts.Add(Postcode);
        if (Country != null) parts.Add(Country);

        return string.Join(", ", parts);
    }
}
