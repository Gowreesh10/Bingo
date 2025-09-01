using System;

namespace Bingo.Shared.Models
{
    public class GameMessage
    {
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public object? Data { get; set; }

        public GameMessage() { }

        public GameMessage(string type, string content, object? data = null)
        {
            Type = type;
            Content = content;
            Data = data;
        }
    }

    public static class MessageTypes
    {
        public const string GameStarting = "GAME_STARTING";
        public const string NumberDrawn = "NUMBER_DRAWN";
        public const string PlayerReady = "PLAYER_READY";
        public const string MarkNumber = "MARK_NUMBER";
        public const string GameEnded = "GAME_ENDED";
        public const string PlayerWins = "PLAYER_WINS";
        public const string CardUpdate = "CARD_UPDATE";
        public const string Error = "ERROR";
        public const string PlayerNames = "PLAYER_NAMES";
        public const string StartGame = "START_GAME";
        public const string ContinueGame = "CONTINUE_GAME";
        public const string GameReady = "GAME_READY";
    }
}
