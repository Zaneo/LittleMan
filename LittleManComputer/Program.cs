using System;
using System.Collections.Generic;
using System.Text;
using LittleMan.IO;

namespace LittleMan {
    class Program {
        static void Main(string[] args) {
            InputHandler test = new InputHandler(ProgramType.Computer);
            test.HandleArgs(args);
        }
    }
}
