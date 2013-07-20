using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
