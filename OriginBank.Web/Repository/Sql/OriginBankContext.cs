using Microsoft.EntityFrameworkCore;
using OriginBank.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OriginBank.Repository.Sql
{
    public class OriginBankContext : DbContext
    {
        public OriginBankContext(DbContextOptions<OriginBankContext> options) : base(options)
        { }

        public DbSet<Card> Cards { get; set; }

        public DbSet<Operation> Operations { get; set; }

    }
}
