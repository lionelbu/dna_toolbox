using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBoxCOM;

namespace Sandbox
{
    class Class1
    {
        static void Main()
        {
            // Open a compressed file on disk.
            // ... Then decompress it with the method below.
            Zip t = new Zip();
          //  t.ExtractZip(@"C:\Users\HP\Documents\Git\n.zip", @"C:\Users\HP\Documents\Git\");
            t.createZip(@"C:\Users\HP\Documents\Git\test", @"C:\Users\HP\Documents\Git\test\toto.zip");
        }
    }
}
