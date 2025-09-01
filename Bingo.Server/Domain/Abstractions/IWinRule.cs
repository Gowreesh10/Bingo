using System;
using Bingo.Server.Domain.Entities;

namespace Bingo.Server.Domain.Abstractions
{
    public interface IWinRule
    {
        bool IsWinning(BingoCard card);
    }
}
