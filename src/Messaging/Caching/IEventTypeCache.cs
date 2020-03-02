using CodeMonkeys.Core.Messaging;

using System;
using System.Collections.Generic;

namespace CodeMonkeys.Messaging.Caching
{
    internal interface IEventTypeCache
    {
        IList<Type> GetOrAddEventTypesOf(ISubscriber subscriber);
    }
}
