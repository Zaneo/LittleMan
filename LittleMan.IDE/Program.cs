﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LittleMan.IDE {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        [MTAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}