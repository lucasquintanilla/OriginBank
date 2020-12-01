using Microsoft.EntityFrameworkCore;
using OriginBank.Repository;
using OriginBank.Repository.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OriginBank.Repository.Sql
{
    public class SqlOriginBankRepository : IOriginBankRepository
    {
        private readonly DbContextOptions<OriginBankContext> _dbOptions;

        public SqlOriginBankRepository(DbContextOptionsBuilder<OriginBankContext>
            dbOptionsBuilder)
        {
            _dbOptions = dbOptionsBuilder.Options;
            using (var db = new OriginBankContext(_dbOptions))
            {
                db.Database.EnsureCreated();
            }
        }

        public ICardRepository Cards => new SqlCardRepository(
            new OriginBankContext(_dbOptions));

        public IOperationRepository Operations => new SqlOperationRepository(
            new OriginBankContext(_dbOptions));

    }
}
