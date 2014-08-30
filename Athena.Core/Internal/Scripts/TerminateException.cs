using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.Scripts
{
    public class TerminateException : Exception
    {
        public TerminateException()
            : base()
        { }

        public TerminateException(string reason)
            : base()
        { }

        public string Reason
        {
            get;
            private set;
        }
    }
}
