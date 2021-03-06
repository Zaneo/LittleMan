// The MIT License (MIT)
//
// Copyright (c) 2013 Gareth Higgins
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using LittleMan.Util;

namespace LittleMan.IO {
    public static class ArgumentLoader {
        static List<IArgument> sharedArguments = new List<IArgument>() {
            new TypeArgument(),
            new OutputArgument(),
            new StepArgument(),
        };
        static List<IArgument> compilerArguments = new List<IArgument>(){
            new EmulationArgument()
        };
        static List<IArgument> computerArguments = new List<IArgument>() {
        };
        public static string[] SupportedArguments(ref ProgramType programType) {
            string[] supportedArgs;
            int offset = sharedArguments.Count;
            switch (programType) {
                case ProgramType.Compiler:
                    supportedArgs = new string[sharedArguments.Count + compilerArguments.Count];
                    for (int i = 0; i < sharedArguments.Count; i++) {
                        supportedArgs[i] = sharedArguments[i].FullName;
                    }
                    for (int i = 0; i < compilerArguments.Count; i++) {
                        supportedArgs[i + offset] = compilerArguments[i].FullName;
                    }
                    break;
                case ProgramType.Computer:
                    supportedArgs = new string[sharedArguments.Count + computerArguments.Count];
                    for (int i = 0; i < sharedArguments.Count; i++) {
                        supportedArgs[i] = sharedArguments[i].FullName;
                    }
                    for (int i = 0; i < compilerArguments.Count; i++) {
                        supportedArgs[i + offset] = computerArguments[i].FullName;
                    }
                    break;
                default:
                    // Unknown program type
                    supportedArgs = new string[] { "Error" };
                    break;                   
            }
            return supportedArgs;
            
        }
        public static void SelectArguments(ProgramType programType, InputHandler sourceHandler) {
            for (int i = 0; i < sharedArguments.Count; i++)
			{
			    sourceHandler.LoadArgument(sharedArguments[i]);
			}

            switch (programType) {
                case ProgramType.Compiler:
                    for (int i = 0; i < compilerArguments.Count; i++) {
                        sourceHandler.LoadArgument(compilerArguments[i]);
                    }
                    break;
                case ProgramType.Computer:
                    for (int i = 0; i <computerArguments.Count; i++) {
                        sourceHandler.LoadArgument(computerArguments[i]);
                    }
                    break;
                default:
                    throw new NotImplementedException("Unknown Program Type");
            }
        }
    }

    public interface IArgument {
        Char TypeIdentifier { get; }
        String FullName { get; }
        Byte NumberOfArguments { get; }
        void HandleArgument(ref String[] arguments, InputHandler handler);
    }
    class EmulationArgument : IArgument {
        public Char TypeIdentifier { get; private set; }
        public String FullName { get; private set; }
        public Byte NumberOfArguments { get; private set; }

        public EmulationArgument() {
            TypeIdentifier = 'E';
            FullName = "RunVMAfterCompilation";
            NumberOfArguments = 1;

        }
        public void HandleArgument(ref String[] arguments, InputHandler handler) {
            handler.RunAfterCompile = true;
        }
    }
    class StepArgument : IArgument {
        public Char TypeIdentifier { get; private set; }
        public String FullName { get; private set; }
        public Byte NumberOfArguments { get; private set; }

        public StepArgument() {
            TypeIdentifier = 'S';
            FullName = "StepThroughCode";
            NumberOfArguments = 1;
        }

        public void HandleArgument(ref String[] arguments, InputHandler handler) {
            handler.StepThrough = true;
        }
    }

    class OutputArgument : IArgument {
        public Char TypeIdentifier { get; private set; }
        public String FullName { get; private set; }
        public Byte NumberOfArguments { get; private set; }

        public OutputArgument() {
            TypeIdentifier = 'O';
            FullName = "OutputName";
            NumberOfArguments = 2;
        }

        public void HandleArgument(ref String[] arguments, InputHandler handler) {
            handler.outputName = Path.Combine(Paths.CompiledPath, arguments[1]);
        }
    }
    class DragDropArgument : IArgument {
        public Char TypeIdentifier { get; private set; }
        public String FullName { get; private set; }
        public Byte NumberOfArguments { get; private set; }

        public DragDropArgument() {
            TypeIdentifier = ' ';
            FullName = "DragDrop Input";
            NumberOfArguments = 2;
        }
        public void HandleArgument(ref string[] arguments, InputHandler handler) {
            handler.InputMethod = new InputFromDrag();
            handler.InputTypePayload = arguments[0];
        }
    }
    class TypeArgument : IArgument {
        public Char TypeIdentifier { get; private set; }
        public String FullName { get; private set; }
        public Byte NumberOfArguments { get; private set; }

        public TypeArgument() {
            TypeIdentifier = 'I';
            FullName = "InputType";
            NumberOfArguments = 3;
        }

       public void HandleArgument(ref string[] arguments, InputHandler handler) {
            EnumUtil.TryParse(arguments[1], out mInputType, true);

            switch (mInputType) {
                case InputType.Text:
                    handler.InputMethod = new InputFromText();
                    break;
                case InputType.File:
                    handler.InputMethod = new InputFromFile();
                    break;
                case InputType.Net:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException(string.Format("Invalid Input type: {0}", mInputType));
            }

            handler.InputTypePayload = arguments[2];
        }

        private InputType mInputType;
    }
}
