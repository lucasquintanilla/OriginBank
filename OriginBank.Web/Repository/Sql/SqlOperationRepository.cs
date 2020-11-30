using Microsoft.EntityFrameworkCore;
using OriginBank.Models;
using OriginBank.Repository;
using OriginBank.Repository.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OriginBank.Web.Repository.Sql
{
    public class SqlOperationRepository : IOperationRepository
    {
        private readonly OriginBankContext _db;

        public SqlOperationRepository(OriginBankContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Operation>> GetAsync(int cardId)
        {
            return await _db.Operations.Where(_operation => _operation.CardId == cardId).ToListAsync();
        }

        public async Task<Operation> AddAsync(Operation operation)
        {
            _db.Operations.Add(operation);
            await _db.SaveChangesAsync();
            return operation;
        }
    }
}
