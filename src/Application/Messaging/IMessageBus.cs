using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bingo.src.Application.Messaging
{
    public interface IMessageBus
    {
        void Subscribe<TEvent>(Action<TEvent> handler);

        void Unsubscribe<TEvent>(Action<TEvent> handler);

        void Publish<TEvent>(TEvent eventMessage);
    }
}
