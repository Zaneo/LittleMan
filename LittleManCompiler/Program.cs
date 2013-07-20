using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LittleMan.IO;
using LittleMan.Compilation;

namespace LittleMan.Compilation {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            InputHandler argumentHandler = new InputHandler(ProgramType.Compiler);
            argumentHandler.HandleArgs(args);
            Compiler mainC = new Compiler(new ConsoleInterface());
            mainC.SetProperties(ref argumentHandler);
        }
    }
}
