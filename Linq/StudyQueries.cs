namespace Linq
{
    internal class StudyQueries
    {
        public string GetReqFlight(List<Flight> Flights)
        {
            List<int> fids = Flights.Where(f => f.Airline == "Delta").Select(f=>f.FlightId).ToList();
            return string.Join(", ", fids);
        }
    }
}
