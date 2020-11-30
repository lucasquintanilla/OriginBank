using OriginBank.Models;
using OriginBank.Repository;
using OriginBank.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OriginBank.Web.Services
{
    public class ATMService
    {
        private ICardRepository _creditCardRepository;
        private IOperationRepository _operationRepository;

        public ATMService(ICardRepository creditCardRepository, IOperationRepository operationRepository)
        {
            _creditCardRepository = creditCardRepository;
            _operationRepository = operationRepository;
        }

        public async Task<int> GetCardIdByNumberAsync(string cardNumber)
        {
            var card = await _creditCardRepository.GetAsync(cardNumber);
            
            if (card == null)
            {
                throw new ArgumentException("Card with this number doesn't exist");
            }

            if (card.IsBlocked)
            {
                throw new InvalidOperationException("Card is invalid");
            }

            return card.Id;
        }

        public async Task<Card> BlockCard(int id)
        {
            var currentCard = await _creditCardRepository.GetAsync(id);
            currentCard.IsBlocked = true;

            var card = await _creditCardRepository.EditAsync(currentCard);

            return card;
        }

        public async Task<Operation> AddOperation(int cardId)
        {
            var operation = await _operationRepository.AddAsync(new Operation() { CardId = cardId, Timestamp = DateTime.Now});           
            return operation;
        }

        public async Task<bool> IsValidCardCombinationAsync(int id, int pin)
        {
            var card = await _creditCardRepository.GetAsync(id);

            if (card.Pin == pin)
            {
                return true;
            }

            return false;
        }

        public async Task<WithdrawalResultViewModel> WithdrawByIdAsync(int id, decimal amount)
        {
            var card = await _creditCardRepository.GetAsync(id);
            
            if (card.Balance < amount)
            {
                throw new InvalidOperationException("Insufficient funds");
            }
            if (amount <= 0)
            {
                throw new InvalidOperationException($"Invalid withdrawal amount: {amount}, must be above zero");
            }

            card.Balance -= amount;
            await _creditCardRepository.EditAsync(card);
            await _operationRepository.AddAsync(new Operation { CardId = card.Id, OperationCode = 1, Timestamp = DateTime.Now, WithdrawalAmount = amount });
            return new WithdrawalResultViewModel { Number = card.Number, RemainingBalance = card.Balance, WithdrawalAmount = amount };
        }
    }
}
