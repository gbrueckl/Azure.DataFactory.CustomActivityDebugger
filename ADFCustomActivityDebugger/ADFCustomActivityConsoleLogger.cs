using Microsoft.Azure.Management.DataFactories.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.DataFactory
{
    public class ADFCustomActivityConsoleLogger : IActivityLogger
    {
        // custom Logger to write the output to the console
        public void Write(string format, params object[] args)
        {
            Console.Write(format, args);
        }
    }
}
