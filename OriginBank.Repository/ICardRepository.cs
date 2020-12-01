using OriginBank.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OriginBank.Repository
{
    public interface ICardRepository
    {
        Task<Card> GetAsync(string value);
        Task<Card> GetAsync(int id);
        Task<Card> EditAsync(Card card);
    }
}
