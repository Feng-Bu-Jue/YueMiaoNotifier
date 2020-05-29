using Hangfire;
using Hangfire.MemoryStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace YueMiaoNotifier
{
    public class ConsoleMemoryStorage : MemoryStorage
    {
        public ConsoleMemoryStorage()
        {
            Current = this;
        }
    }
}
