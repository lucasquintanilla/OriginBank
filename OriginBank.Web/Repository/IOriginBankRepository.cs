using System;
using System.Collections.Generic;
using System.Text;

namespace OriginBank.Repository
{
    public interface IOriginBankRepository
    {
        ICardRepository Cards { get; }

        IOperationRepository Operations { get; }
    }
}
