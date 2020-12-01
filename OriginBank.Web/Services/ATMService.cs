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
        private ICardRepository _cardRepository;
        private IOperationRepository _operationRepository;

        public ATMService(ICardRepository CardRepository, IOperationRepository operationRepository)
        {
            _cardRepository = CardRepository;
            _operationRepository = operationRepository;
        }

        public async Task<int> GetCardIdByNumberAsync(string cardNumber)
        {
            var card = await _cardRepository.GetAsync(cardNumber);
            
            if (card == null)
            {
                throw new InvalidOperationException("Card with this number doesn't exist");
            }

            if (card.IsBlocked)
            {
                throw new InvalidOperationException("Card Blocked");
            }

            return card.Id;
        }

        public async Task<Card> BlockCardAsync(int cardId)
        {
            var currentCard = await _cardRepository.GetAsync(cardId);
            currentCard.IsBlocked = true;

            var card = await _cardRepository.EditAsync(currentCard);

            return card;
        }

        public async Task<Operation> AddOperationAsync(int cardId)
        {
            var operation = await _operationRepository.AddAsync(new Operation() { CardId = cardId, Timestamp = DateTime.Now});           
            return operation;
        }

        public async Task<bool> IsValidCardCombinationAsync(int cardId, int pin)
        {
            var card = await _cardRepository.GetAsync(cardId);

            if (card.Pin == pin)
            {
                return true;
            }

            return false;
        }

        public async Task<WithdrawalResultViewModel> WithdrawByIdAsync(int cardId, decimal amount)
        {
            var card = await _cardRepository.GetAsync(cardId);
            
            if (card.Balance < amount)
            {
                throw new InvalidOperationException("Insufficient funds");
            }
            if (amount <= 0)
            {
                throw new InvalidOperationException($"Invalid withdrawal amount: {amount}, must be above zero");
            }

            card.Balance -= amount;
            await _cardRepository.EditAsync(card);
            await _operationRepository.AddAsync(new Operation { CardId = card.Id, OperationCode = 1, Timestamp = DateTime.Now, WithdrawalAmount = amount });
            return new WithdrawalResultViewModel { Number = card.Number, RemainingBalance = card.Balance, WithdrawalAmount = amount };
        }
    }
}
