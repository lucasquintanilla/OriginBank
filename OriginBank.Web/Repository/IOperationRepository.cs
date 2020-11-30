using OriginBank.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OriginBank.Repository
{
    public interface IOperationRepository
    {
        Task<IEnumerable<Operation>> GetAsync(int id);
        Task<Operation> AddAsync(Operation Operation);
    }
}
