﻿using Newtonsoft.Json.Linq;
using System.Text;

namespace EricUtility.Networking.Commands
{
    public class DisconnectJsonCommand : AbstractJsonCommand
    {
        public static string COMMAND_NAME = "DISCONNECT";

        public DisconnectJsonCommand()
        {
        }
    }
}