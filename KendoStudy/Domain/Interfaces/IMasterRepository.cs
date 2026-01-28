using Domain.VMModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IMasterRepository
    {
        public List<VMMaster> Get();

        VMMaster? GetById(int id);

        public string Post(VMMaster m);

        public string Put(VMMaster m);

        public string Delete(int AirlineMasterID);
    }
}
