using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using LittleMan.Emulation;
using LittleMan.IO;

namespace LittleMan.IDE {
    class ThreadedVirtualMachineContainer {
        VirtualMachine vm;

        void SetUp(IHumanInterface interfaceDevice) {
            vm = new VirtualMachine(interfaceDevice);
        }
        void DoSpin() {
            for (; ;) {
                try {
                }
                catch (ThreadAbortException e) {
                    vm.IOInterface.Output<string>(e.Message);
                }
                catch (ThreadInterruptedException e) {
                    vm.IOInterface.Output<string>(e.Message);
                }
            }
        }

        void ExecuteStep() {
            vm.ExecuteStep();
        }

        void ExecuteAll(bool stopOnError) {
            vm.ExecuteAll(stopOnError);
        }
    }
}
