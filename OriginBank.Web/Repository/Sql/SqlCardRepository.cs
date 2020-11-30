using Microsoft.EntityFrameworkCore;
using OriginBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginBank.Repository.Sql
{
    public class SqlCardRepository : ICardRepository
    {
        private readonly OriginBankContext _db;

        public SqlCardRepository(OriginBankContext db)
        {
            _db = db;
        }

        public async Task<Card> GetAsync(int id)
        {
            return await _db.Cards.FirstOrDefaultAsync(_card => _card.Id == id);
        }

        public async Task<Card> GetAsync(string cardNumber)
        {
            var card = await _db.Cards.FirstOrDefaultAsync(_card => _card.Number == cardNumber);

            return card;
        }

        public async Task<Card> EditAsync(Card card)
        {
            var current = await _db.Cards.FirstOrDefaultAsync(_card => _card.Id == card.Id);

            if (null == current)
            {
                throw new Exception("Card id Not found");
            }
            else
            {
                _db.Entry(current).CurrentValues.SetValues(card);
            }

            await _db.SaveChangesAsync();
            return card;
        }
    }
}
