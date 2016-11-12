using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPT
{
    [XmlRpcUrl("http://localhost:4400/xmlrpc")]
    public interface PPTLib : IXmlRpcProxy
    {
        [XmlRpcMethod("PPTServer.executePQL")]
        System.Object executePQL(string query, int trials, int maxsec, int threads);
    }

    public static class Client
    {
        public static PPTLib proxy;

        public static void Init()
        {
            Client.proxy = XmlRpcProxyGen.Create<PPTLib>();
        }
    }
}
