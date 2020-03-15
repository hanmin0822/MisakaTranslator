using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsHelper
{
    public class SettingsNotFoundEventArgs : EventArgs
    {
        public readonly string EventMessage = String.Empty;

        public SettingsNotFoundEventArgs(string message)
        {
            EventMessage = message;
        }
    }
}
