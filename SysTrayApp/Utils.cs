﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace YeelightTray
{
    class Utils
    {
        /// <summary>
        /// Function to get a sub part of a string, exemple : startexempleend, by using "start" as begin param and "end" as end param, you receive "exemple"
        /// Return false if no match
        /// </summary>
        /// <param name="str"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="ret"></param>
        /// <returns></returns>
        public static bool GetSubString(string str, string begin, string end, ref string ret)
        {
            int beginId = -1;
            int endId = -1;

            beginId = str.IndexOf(begin);
            if (beginId != -1)
            {
                ret = str.Substring(beginId + begin.Length);
                endId = ret.IndexOf(end);
                ret = ret.Substring(0, endId);
                return true;
            }

            return false;
        }

        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
