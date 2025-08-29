using System;
using Bingo.src.Domain.Entities;

namespace Bingo.src.Domain.Abstractions
{
    public interface IWinRule
    {
        bool IsWinning(BingoCard card);
    }
}
