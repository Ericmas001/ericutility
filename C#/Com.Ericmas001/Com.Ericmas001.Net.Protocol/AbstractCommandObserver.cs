﻿using System;
using System.Reflection;

namespace Com.Ericmas001.Net.Protocol
{
    public abstract class AbstractCommandObserver
    {
        public event EventHandler<StringEventArgs> CommandReceived = delegate { };

        protected abstract void receiveSomething(string line);

        public virtual void messageReceived(string line)
        {
            if (line == null)
            {
                return;
            }
            CommandReceived(this, new StringEventArgs(line));
            receiveSomething(line);
        }
    }
}