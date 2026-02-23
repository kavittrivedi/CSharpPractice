using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    //This principle suggests that clients should not be forced to depend on interfaces they do not use.It encourages the creation of specific,
    //smaller interfaces rather than large, monolithic ones.

    //Example in C#:
    //Suppose you have an interface called Worker that includes methods for both working and taking breaks.Instead of a single interface,
    //create two separate interfaces, IWorker and IBreakable, to segregate the responsibilities.


    // Before ISP
    interface Worker
    {
        void Work();
        void TakeBreak();
    }
}

namespace ConsoleApp.AfterISP 
{
    // After ISP
    interface IWorker
    {
        void Work();
    }

    interface IBreakable
    {
        void TakeBreak();
    }
}