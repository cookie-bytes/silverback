﻿// Copyright (c) 2018-2019 Sergio Aquilini
// This code is licensed under MIT license (see LICENSE file for details)

using System.Threading.Tasks;
using Silverback.Messaging.Subscribers;
using Silverback.Tests.Core.Messaging.Publishing;

namespace Silverback.Tests.Core.TestTypes.Subscribers
{
    public class ParallelSubscriberTestService : ISubscriber
    {
        public ParallelTestingUtil Parallel { get; } = new ParallelTestingUtil();

        [Subscribe(Parallel = true)]
        private void OnMessageReceived(object _) => Parallel.DoWork();

        [Subscribe(Parallel = true)]
        private Task OnMessageReceivedAsync(object _) => Parallel.DoWorkAsync();
    }
}