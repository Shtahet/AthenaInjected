using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal
{
    public class SleepException : Exception
    {
        public SleepException(int ms)
            : base()
        {
            Time = TimeSpan.FromMilliseconds(ms);
        }

        public SleepException(TimeSpan time)
            : base()
        {
            Time = time;
        }

        public TimeSpan Time
        {
            get;
            private set;
        }
    }
}
