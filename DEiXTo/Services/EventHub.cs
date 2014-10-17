using System;
using System.Collections;
using System.Collections.Generic;

namespace DEiXTo.Services
{
    public class EventHub : IEventHub
    {
        private static EventHub instance;
        // Type is the subject type, and the list is all the subscriber information for that subject.
        private readonly IDictionary<Type, IList> _subscribers = new Dictionary<Type, IList>();

        private EventHub() { }

        public static EventHub Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventHub();
                }

                return instance;
            }
        }

        public void Publish<T>(T subject)
        {
            foreach (SubscriberInfo<T> subscriberInfo in GetSubscriberInfos(subject))
            {
                subscriberInfo.Subscriber.Receive(subject);
            }
        }

        public void Subscribe<T>(ISubscriber<T> subscriber)
        {
            List<SubscriberInfo<T>> subscriberInfoList = GetSubscriberInfoList<T>();

            if (subscriberInfoList == null)
            {
                subscriberInfoList = new List<SubscriberInfo<T>>();
                _subscribers[typeof(T)] = subscriberInfoList;
            }

            subscriberInfoList.Add(new SubscriberInfo<T>(subscriber));
        }

        public void Unsubscribe<T>(ISubscriber<T> subscriber)
        {
            List<SubscriberInfo<T>> subscriberInfoList = GetSubscriberInfoList<T>();

            if (subscriberInfoList != null)
            {
                subscriberInfoList.RemoveAll((subscriberInfo) => ReferenceEquals(subscriberInfo.Subscriber, subscriber));
            }
        }

        private List<SubscriberInfo<T>> GetSubscriberInfoList<T>()
        {
            if (_subscribers.ContainsKey(typeof(T)))
            {
                return _subscribers[typeof(T)] as List<SubscriberInfo<T>>;
            }

            return null;
        }

        private IEnumerable<SubscriberInfo<T>> GetSubscriberInfos<T>(T subject)
        {
            List<SubscriberInfo<T>> subscriberInfos = GetSubscriberInfoList<T>();

            foreach (SubscriberInfo<T> subscriberInfo in subscriberInfos)
            {
                yield return subscriberInfo;
            }
        }

        private struct SubscriberInfo<T>
        {
            private readonly ISubscriber<T> _subscriber;

            public ISubscriber<T> Subscriber
            {
                get { return _subscriber; }
            }

            public SubscriberInfo(ISubscriber<T> subscriber)
            {
                _subscriber = subscriber;
            }
        }
    }
}
