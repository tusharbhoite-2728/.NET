using Domain.Interfaces;
using Domain.VMModels;
using Infrastructure.Data;
using Infrastructure.Models;

namespace Infrastructure.Repository
{
    public class MasterRepository : IMasterRepository
    {
        private readonly AppDbContext _context;

        public MasterRepository(AppDbContext context)
        {
            this._context = context;
        } 

        public string Delete(int AirlineMasterID)
        {
            try
            {
                Master? master = _context.Master.FirstOrDefault(m => m.AirlineMasterID == AirlineMasterID);

                if (master == null)
                {
                    return $"Master with master {AirlineMasterID} not found";
                }

                _context.Master.Remove(master);
                _context.SaveChanges();

                return "success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString);
                return e.ToString();
            }
        }

        public List<VMMaster> Get()
        {
            List<VMMaster> result = new List<VMMaster>();

            result = _context.Master.Select(m => new VMMaster()
            {
                 AirlineMasterID = m.AirlineMasterID,
                 AirlineCode = m.AirlineCode,
                 AirlineName = m.AirlineName,
                 LPC = m.LPC,
                 IsOperational = m.IsOperational,
                 LastChangedAt = m.LastChangedAt
            }).ToList();

            return result;
        }

        public VMMaster? GetById(int id)
        {
           var master = _context.Master.FirstOrDefault(m =>m.AirlineMasterID == id);

            if (master == null) { 
                return null; 
            }

            return new VMMaster
            {
                AirlineMasterID = master.AirlineMasterID,
                AirlineCode = master.AirlineCode,
                AirlineName = master.AirlineName,
                LPC = master.LPC,
                IsOperational = master.IsOperational,
                LastChangedAt = master.LastChangedAt
            };
        }

        public string Post(VMMaster m)
        {
            try
            {
                Master master = new Master
                {
                    AirlineMasterID = m.AirlineMasterID,
                    AirlineCode = m.AirlineCode,
                    AirlineName = m.AirlineName,
                    LPC = m.LPC,
                    IsOperational = m.IsOperational,
                    LastChangedAt = m.LastChangedAt
                };

                bool ISExist = _context.Master.Any(m => m.AirlineCode == master.AirlineCode);
                if (ISExist)
                {
                    return "Airline already exists";
                }

                _context.Master.Add(master);
                _context.SaveChanges();
                return "success";

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return e.ToString();

            }
        }

        public string Put(VMMaster m)
        {
            try
            {
                Master? master = _context.Master.Where(x => x.AirlineMasterID == m.AirlineMasterID).FirstOrDefault();

                if (master == null)
                {
                    return $"Airline Master Id - {m.AirlineMasterID} is not exist";
                }

                master.AirlineMasterID = m.AirlineMasterID;
                master.AirlineCode = m.AirlineCode;
                master.AirlineName = m.AirlineName;
                master.LPC = m.LPC;
                master.IsOperational = m.IsOperational;
                master.LastChangedAt = m.LastChangedAt;

                bool ISExist = _context.Master.Any(m =>    m.AirlineMasterID != master.AirlineMasterID && m.AirlineCode == master.AirlineCode);
                if (ISExist)
                {
                    return "Airline already exists";
                }

                _context.Master.Update(master);
                _context.SaveChanges();

                return "success";
            } catch (Exception e) {

                Console.WriteLine(e.ToString());
                return e.ToString();
            }
        }
    }
}
