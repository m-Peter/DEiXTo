using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public interface IEventHub
    {
        /// <summary>
        /// Publishes an event with the given subject to all subscribers.
        /// </summary>
        /// <typeparam name="T">The type of the subject.</typeparam>
        /// <param name="subject">The subject.</param>
        void Publish<T>(T subject);

        /// <summary>
        /// Adds a subscriber for a given subject type.
        /// </summary>
        /// <typeparam name="T">The type of the subject.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        void Subscribe<T>(ISubscriber<T> subscriber);

        /// <summary>
        /// Unsubscribe an event subscriber.
        /// </summary>
        /// <typeparam name="T">The type of the subject.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        void Unsubscribe<T>(ISubscriber<T> subscriber);
    }
}
