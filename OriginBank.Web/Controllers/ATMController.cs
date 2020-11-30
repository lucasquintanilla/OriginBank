﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OriginBank.Web.Filters;
using OriginBank.Web.Utilities;
using Microsoft.AspNetCore.Mvc;
using OriginBank.Repository;
using OriginBank.Web.Models;
using OriginBank.Web.Services;
using Microsoft.AspNetCore.Diagnostics;

namespace OriginBank.Web.Controllers
{
    public class ATMController : Controller
    {
        private ICardRepository _repository;
        private ITerminalSessionManager _sessionManager;
        private ATMService _terminalService;

        public ATMController(ICardRepository repository, ITerminalSessionManager sessionManager, ATMService terminalService)
        {
            _repository = repository;
            _sessionManager = sessionManager;
            _terminalService = terminalService;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Card));
        }

        public IActionResult Card()
        {
            _sessionManager.ClearSession(HttpContext);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Card(string number)
        {
            var card = await _repository.GetAsync(number);            

            if (card == null)
            {
                throw new InvalidOperationException("Card not found");
            }

            if (card.IsBlocked)
            {
                throw new InvalidOperationException("Card is blocked");
            }

            _sessionManager.SetSessionCardId(HttpContext, card.Id);

            return RedirectToAction(nameof(Pin));
        }

        public IActionResult Pin()
        {
            _sessionManager.Unauthorize(HttpContext);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [TerminalIdFilter]
        public async Task<IActionResult> Pin(int pin)
        {
            var pinAttempts =_sessionManager.GetPinAttempts(HttpContext);
            _sessionManager.SetPinAttempts(HttpContext, ++pinAttempts);

            int cardId = _sessionManager.GetSessionCardId(HttpContext).Value;

            int maxAttempts = 4;

            if (pinAttempts >= maxAttempts)
            {
                //Bloquear tarjeta
                var card = await _terminalService.BlockCard(cardId);
                throw new InvalidOperationException("Card blocked");
            }  

            bool isValid = await _terminalService.IsValidCardCombinationAsync(cardId, pin);

            if (isValid)
            {
                _sessionManager.Authorize(HttpContext);
                return RedirectToAction(nameof(Menu));
            }

            _sessionManager.SetPinAttempts(HttpContext, ++pinAttempts);

            return RedirectToAction(nameof(Pin));
        }

        public IActionResult Menu()
        {
            return View();
        }

        public async Task<IActionResult> Balance()
        {
            int cardId = _sessionManager.GetSessionCardId(HttpContext).Value;
            var result = await _terminalService.AddOperation(cardId);
            return View(result);
        }
        public IActionResult Withdraw()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(decimal withdrawalAmount)
        {
            int cardId = _sessionManager.GetSessionCardId(HttpContext).Value;
            var result = await _terminalService.WithdrawByIdAsync(cardId, withdrawalAmount);
            return RedirectToAction(nameof(WithdrawalResult), result);
        }

        public IActionResult WithdrawalResult(WithdrawalResultViewModel viewModel)
        {
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorView = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            var exceptionHandlerPathFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            errorView.ErrorMessage = exceptionHandlerPathFeature?.Error.Message;

            Debug.WriteLine("Path " + exceptionHandlerPathFeature.Path);

            return View(errorView);
        }
    }
}
