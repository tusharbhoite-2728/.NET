using Linq;


var flights = new List<Flight>
{
    new Flight { FlightId = 1, Airline = "Delta", Origin = "JFK", Price = 320 },
    new Flight { FlightId = 2, Airline = "Delta", Origin = "LAX", Price = 280 },
    new Flight { FlightId = 3, Airline = "Delta", Origin = "SEA", Price = 210 },

    new Flight { FlightId = 4, Airline = "United", Origin = "JFK", Price = 300 },
    new Flight { FlightId = 5, Airline = "United", Origin = "ORD", Price = 260 },
    new Flight { FlightId = 6, Airline = "United", Origin = "LAX", Price = 340 },

    new Flight { FlightId = 7, Airline = "American", Origin = "JFK", Price = 290 },
    new Flight { FlightId = 8, Airline = "American", Origin = "DFW", Price = 230 },
    new Flight { FlightId = 9, Airline = "American", Origin = "LAX", Price = 310 },

    new Flight { FlightId = 10, Airline = "Emirates", Origin = "DXB", Price = 900 },
    new Flight { FlightId = 11, Airline = "Emirates", Origin = "DXB", Price = 880 },
    new Flight { FlightId = 12, Airline = "Emirates", Origin = "DXB", Price = 950 },

    new Flight { FlightId = 13, Airline = "Lufthansa", Origin = "FRA", Price = 720 },
    new Flight { FlightId = 14, Airline = "Lufthansa", Origin = "FRA", Price = 690 },
    new Flight { FlightId = 15, Airline = "Lufthansa", Origin = "MUC", Price = 650 },

    new Flight { FlightId = 16, Airline = "Qatar", Origin = "DOH", Price = 860 },
    new Flight { FlightId = 17, Airline = "Qatar", Origin = "DOH", Price = 830 },

    new Flight { FlightId = 18, Airline = "IndiGo", Origin = "DEL", Price = 120 },
    new Flight { FlightId = 19, Airline = "IndiGo", Origin = "BOM", Price = 140 },
    new Flight { FlightId = 20, Airline = "IndiGo", Origin = "BLR", Price = 130 }
};



Console.WriteLine("Flights Under 300");
Console.WriteLine();

var m1 = flights.Where(f => f.Price <= 300);

foreach (var item in m1)
{
    Console.WriteLine($"{item.FlightId} {item.Airline}  {item.Origin}  {item.Price}");
}


Console.WriteLine("====================================================================");
Console.WriteLine("Flights Above 300");
Console.WriteLine();
var q1 = from f in flights
         where f.Price > 300
         select f;

foreach(var item in q1)
{
    Console.WriteLine($"{item.FlightId} {item.Airline}  {item.Origin}  {item.Price}");
}


Console.WriteLine("====================================================================");
Console.WriteLine("All Airline with Price");
Console.WriteLine();


var m2 = flights.Select(f => new { f.Airline, f.Price });

foreach(var item in m2)
{
    Console.WriteLine($" {item.Airline}  {item.Price}");
}


Console.WriteLine("====================================================================");
Console.WriteLine("Flight ID and Origin of Indigo Airline");
Console.WriteLine();


var q2 = from f in flights
         where f.Airline == "IndiGo"
         select new { f.FlightId, f.Origin };

foreach (var item in q2)
{
    Console.WriteLine($"{item.FlightId}   {item.Origin} ");
}



Console.WriteLine("====================================================================");
Console.WriteLine("Sorted Flights By Price");
Console.WriteLine();

var m3 = flights.OrderBy(f => f.Price);

foreach (var item in m3)
{
    Console.WriteLine($"{item.FlightId} {item.Airline}  {item.Origin}  {item.Price}");
}

Console.WriteLine("====================================================================");
Console.WriteLine("Sorted Flights By Price Descending");
Console.WriteLine();


var q3 = from f in flights
         orderby f.Price descending
         select f;

foreach (var item in q3)
{
    Console.WriteLine($"{item.FlightId} {item.Airline}  {item.Origin}  {item.Price}");
}


Console.WriteLine("====================================================================");
Console.WriteLine("Flights group By Airline");
Console.WriteLine();


var m4 = flights.GroupBy(f => f.Airline);


foreach (var group in m4)
{
    Console.WriteLine();

    Console.WriteLine($"Airline: {group.Key}");

    foreach (var flight in group)
    {
        Console.WriteLine($"{flight.FlightId}   {flight.Origin}  {flight.Price}");
        Console.WriteLine();
    }
}




Console.WriteLine("====================================================================");
Console.WriteLine("Distinct Airline");
Console.WriteLine();


var m5 = flights.ToList().Select(f => f.Airline).Distinct();

foreach (var item in m5)
{
    Console.WriteLine(item);
}


