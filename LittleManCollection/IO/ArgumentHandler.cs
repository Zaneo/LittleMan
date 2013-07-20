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

namespace LittleMan.IO {
    public enum InputType {
        Text,
        File,
        Drag,
        Net,
    }
    public interface IInput {
        InputType Type { get; }
        Stream GetInput(string param);
    }
    public class InputFromDrag : IInput {
        public InputType Type { get; private set; }
        public InputFromDrag() {
            Type = InputType.Drag;
        }
        public Stream GetInput( string param){
            return File.OpenRead(param);
        }
    }
    public class InputFromFile : IInput {
        public InputType Type { get; private set; }
        public InputFromFile() {
            Type = InputType.File;
        }
        public Stream GetInput(string param) {                   
            if (!Paths.TestFile("CompiledFile", param, false, true, false)) {
                return null;
            }
            return File.OpenRead(param);
        }

    }

    public class InputFromText : IInput {

        public InputType Type { get; private set; }
        public InputFromText() {
            Type = InputType.Text;
        }

        public Stream GetInput(string param) {
            MemoryStream baseStream = new MemoryStream();
            StreamWriter writeStream = new StreamWriter(baseStream);

            writeStream.Write(param);
            return baseStream;
        }
    }
    public class InputFromNet {
        public InputType Type { get; private set; }
        public InputFromNet() {
            Type = InputType.Net;
        }
        public string[] GetInput(string param) {
            throw new NotImplementedException();
        }
    }

    public class InputHandler {
        public ProgramType ParentProgram { get; private set; }
        public IInput InputMethod { get; set; }
        public String InputTypePayload { get; set; }
        public Boolean StepThrough { get; set; }
        public Boolean RunAfterCompile { get; set; }

        public string outputName = Paths.DefaultCompiledPath;

        private Dictionary<Char, IArgument> SupportedArgument = new Dictionary<Char, IArgument>();

        public InputHandler(ProgramType type) {
            ParentProgram = type;
            ArgumentLoader.SelectArguments(ParentProgram, this);
        }

        public void LoadArgument(IArgument argument) {
            SupportedArgument.Add(argument.TypeIdentifier, argument);
        }

        public void HandleArgs(string[] arguments) {
            int index = 0;

            while (index < arguments.Length) {
                IArgument argument;
                if (arguments.Length == 1 && !SupportedArgument.TryGetValue(arguments[index][1], out argument)) {
                    argument = new DragDropArgument();
                }
                else if (!SupportedArgument.TryGetValue(arguments[index][1], out argument)) {
                    // Error, also wtf arguments[0][1]?
                    throw new ArgumentNullException("Input argument");
                }
                if (argument == null) {
                    throw new ArgumentException("Null arguement");
                }
                argument.HandleArgument(ref arguments, this);
                index += argument.NumberOfArguments;
            }
        }
    }    
}
