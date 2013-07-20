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
using System.Data;
using System.Diagnostics;
using System.IO;
using LittleMan.IO;
using LittleMan.Util;

namespace LittleMan.Compilation {
    public class Compiler : Interpreter {
        Instruction _intr;
        IHumanInterface IOInterface;

        private InputHandler handler;

        public string OutputPath { get; private set; }

        readonly char[] splitCharacter = new char[] { ' ' };
        string[] _source;
        bool _sourceSet = false;
        ushort[] _memory;
        ushort _counter;
        ushort _extraVariables;

        public bool IsComplete { get; private set; }

        public bool SteppedMode { get; private set; }
        public bool RunAfterCompile { get; private set; }

        Dictionary<string, List<ushort>> unlinkedSymbols;
        Dictionary<string, List<ushort>> unlinkedVariables;
        Dictionary<string, List<ushort>> unlinkedExtraVariables;


        Dictionary<string, ushort> symbolLocation;
        Dictionary<string, ushort> variableLocation;
        Dictionary<string, ushort> extraVariablesLocation;

        public Compiler(IHumanInterface IOMethod) {
            IOInterface = IOMethod;
            SetProgramType(ProgramType.Compiler);
        }

        public void SetProperties(ref InputHandler handler) {
            SteppedMode = handler.StepThrough;
            RunAfterCompile = handler.RunAfterCompile;
            OutputPath = handler.outputName;

            string text;
            int endingLocation;

            using (StreamReader reader = new StreamReader(handler.InputMethod.GetInput(handler.InputTypePayload)))
            {
                text = reader.ReadToEnd();
            }
            endingLocation = text.IndexOf(MonoCompat.UnixLineEnding);
            if (endingLocation > text.Length - 2)
            {
                throw new InvalidDataException("Source was too short");
                //log and error
            }
            if (endingLocation == -1) {
                //log expected line ending found none
                SetSource(new string[]{text});
            }
            else if (text.Substring((endingLocation + 2), 2).CompareTo(MonoCompat.WindowsExtraEnding) == 1)
            {
                SetSource(text.Split(new string[] { MonoCompat.UnixLineEnding + MonoCompat.WindowsExtraEnding },
                                     StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                SetSource(text.Split(new string[] { MonoCompat.UnixLineEnding }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        /// <summary>
        /// Sets the source code to be compiled
        /// </summary>
        /// <param name="source">Source code array</param>
        public void SetSource(string[] source) {
            Reset();
            _source = source;
            _memory = new ushort[_source.Length + 1];
            _memory[0] = SetInstruction(Instruction.STR);
            _sourceSet = true;

        }

        public ushort[] GetSource() {
            if (!_sourceSet)
                throw new InvalidDataException("No source exists");
            return _memory;
        }
        public byte[] GetSourceAsRaw() {
            byte[] mem = new byte[_memory.Length * 2];
            for (int i = 0; i < _memory.Length; i+=2) {
                mem[i] = (byte)(_memory[i] >> 8);
                mem[i + 1] = (byte)(_memory[i] & ((ushort)0xff));
            }
            return mem;
        }

        void Reset() {
            _counter = 1;
            IsComplete = false;
            _sourceSet = false;
            _extraVariables = 0;
            _memory = null;

            unlinkedSymbols = new Dictionary<string, List<ushort>>();
            unlinkedVariables = new Dictionary<string, List<ushort>>();
            unlinkedExtraVariables = new Dictionary<string, List<ushort>>();

            symbolLocation = new Dictionary<string, ushort>();
            variableLocation = new Dictionary<string, ushort>();
            extraVariablesLocation = new Dictionary<string, ushort>();
        }
        /// <summary>
        /// Gets the compiled code
        /// </summary>
        /// <returns>Compiled code or null</returns>
        public ushort[] GetCompiledCode() {
            if (!IsComplete)
                return null;

            return _memory;
        }


        /// <summary>
        /// Compiles all the code at once
        /// </summary>
        public void CompileAll() {
            if (!_sourceSet)
                throw new InvalidOperationException("Unable to compile, no source set");

            for (_counter = 1; _counter < _memory.Length; _counter++) {

                // Read from 0 to N, but write from 1 to N
                Compile(ref _source[_counter - 1]);

            }
            LinkAllVariablesandSymbols();
            IsComplete = true;
            IOInterface.Output("Build Succeeded");
            if (RunAfterCompile)
            {
                // Link filename output or text to new locations
                Process.Start(Paths.ComputerFilePath, string.Format("-I File {0}", OutputPath));
            }
        }

        /// <summary>
        /// Compiles the code one step at a time
        /// </summary>
        public void CompileStep() {
            if (!_sourceSet)
                throw new InvalidOperationException("Unable to compile, no source set");
            if (_counter < _source.Length) {

                // Read from 0 to N, but write from 1 to N
                Compile(ref _source[_counter - 1]);
                _counter++;
            }
            else {
                LinkAllVariablesandSymbols();
                IsComplete = true;
            }
            IOInterface.Output("Build Succeeded");
            if (RunAfterCompile)
            {
                // Link filename output or text to new locations
                Process.Start(Paths.ComputerFilePath, string.Format("-I File {0}", OutputPath));
            }
        }

        /// <summary>
        /// Compiles the specific line of code
        /// </summary>
        /// <param name="input">Line of code to compile</param>
        void Compile(ref string input) {

            string[] inputValue = input.Split((splitCharacter));
            ushort val;

            if (EnumUtil.TryParse(inputValue[0], out _intr, true)) {
                ParseAddress(ref _intr, ref inputValue, 0);
            }
            else {
                // Checks to see if it is a variable declaration
                if (String.Compare(inputValue[1], "DAT", true) == 0) {
                    // Checks to see if the symbol is already defined
                    if (variableLocation.ContainsKey(inputValue[0]))
                        throw new InvalidOperationException(string.Format("{0} is already defined on line: {1}", inputValue[0], variableLocation[inputValue[0]]));
                    if (symbolLocation.ContainsKey(inputValue[0]))
                        throw new InvalidOperationException(string.Format("{0} is already defined on line: {1}", inputValue[0], symbolLocation[inputValue[0]]));

                    variableLocation.Add(inputValue[0], _counter);
                    if (inputValue.Length == 3) {
                        if (!ushort.TryParse(inputValue[2], out val))
                            throw new InvalidConstraintException("Unable to parse number");
                        _memory[_counter] = val;
                    }
                }
                else {
                    // Checks to see if the symbol is already defined
                    if (variableLocation.ContainsKey(inputValue[0]))
                        throw new InvalidOperationException(string.Format("{0} is already defined on line: {1}", inputValue[0], variableLocation[inputValue[0]]));
                    if (symbolLocation.ContainsKey(inputValue[0]))
                        throw new InvalidOperationException(string.Format("{0} is already defined on line: {1}", inputValue[0], symbolLocation[inputValue[0]]));

                    symbolLocation.Add(inputValue[0], _counter);
                    if (!EnumUtil.TryParse(inputValue[1], out _intr, true))
                        throw new InvalidConstraintException("Expected instruction after symbol");

                    ParseAddress(ref _intr, ref inputValue, 1);
                }
            }
            //using (System.IO.FileStream write = new System.IO.FileStream("log.txt", System.IO.FileMode.Append)) {
            //    using (System.IO.StreamWriter swrite = new System.IO.StreamWriter(write)) {
            //        if (inputValue.Length >= 2) {
            //            swrite.WriteLine(string.Format("{0}:{1} <-> {2}:{3}", inputValue[0], inputValue[1], GetInstruction(_memory[_counter]), GetAddress(_memory[_counter])));
            //        }
            //        else {
            //            swrite.WriteLine(string.Format("{0} <-> {1}:{2}", inputValue[0],GetInstruction(_memory[_counter]), GetAddress(_memory[_counter])));
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Fills in the unknown variable and symbol list with known locations
        /// </summary>
        void LinkAllVariablesandSymbols() {

            ushort index = (ushort)_memory.Length;
            if (extraVariablesLocation.Count == 0)
                return;

            Array.Resize(ref _memory, _memory.Length + _extraVariables);

            foreach (var item in unlinkedVariables) {
                Link(item.Key, item.Value, ref variableLocation);
            }
            foreach (var item in unlinkedSymbols) {
                Link(item.Key, item.Value, ref symbolLocation);
            }
            foreach (var item in unlinkedExtraVariables) {
                SetExtraVariable(item.Key, ref extraVariablesLocation, index);
                Link(item.Key, item.Value, ref extraVariablesLocation);
                index++;
            }

        }

        /// <summary>
        /// Matches the unlinked item to its location
        /// </summary>
        /// <param name="key">item name</param>
        /// <param name="values">locations of item in memory</param>
        /// <param name="locations">know location of item</param>
        void Link(string key, List<ushort> values, ref Dictionary<string, ushort> locations) {
            short val;
            try {
                val = (short)locations[key];
            }
            catch {
                throw new InvalidOperationException(string.Format("'{0}' is used without first being declared", key));
            }
            for (int i = 0; i < values.Count; i++) {
                _memory[values[i]] += SetAddress(val);
            }
        }

        /// <summary>
        /// Sets value at location, then sets list to contain location
        /// </summary>
        /// <param name="key">Which variable to link</param>
        /// <param name="variablePair">Variable name and value</param>
        /// <param name="location">location of where to store</param>
        void SetExtraVariable(string key, ref Dictionary<string, ushort> variablePair, ushort location) {
            _memory[location] = SetAddress(variablePair[key]);
            variablePair[key] = location;
        }


        /// <summary>
        /// Parse the values from the input code
        /// </summary>
        /// <param name="intr">Instruction to follow</param>
        /// <param name="input">String[] input to parse</param>
        /// <param name="offset">Offset in input array, not including instruction</param>
        void ParseAddress(ref Instruction intr, ref string[] input, byte offset) {
            switch (intr) {
                case Instruction.END:
                    _memory[_counter] = SetInstruction(Instruction.END);
                    break;
                case Instruction.INP:
                    _memory[_counter] = SetInstruction(Instruction.INP);
                    break;
                case Instruction.OUT:
                    _memory[_counter] = SetInstruction(Instruction.OUT);
                    break;
                case Instruction.STR:
                    _memory[_counter] = SetInstruction(Instruction.STR);
                    break;
                default:
                    ushort val;

                    // Branching treats it as a symbol or direct location not a variable
                    if (intr == Instruction.BRA || intr == Instruction.BRP || intr == Instruction.BRZ) {
                        switch (intr) {
                            case Instruction.BRA:
                                _memory[_counter] = SetInstruction(Instruction.BRA);
                                break;
                            case Instruction.BRP:
                                _memory[_counter] = SetInstruction(Instruction.BRP);
                                break;
                            case Instruction.BRZ:
                                _memory[_counter] = SetInstruction(Instruction.BRZ);
                                break;
                        }
                        if (input.Length > offset) {
                            if (ushort.TryParse(input[offset + 1], out val)) {
                                if (intr != Instruction.BRA && val == 0)
                                    throw new InvalidConstraintException("Only BRA may have an address of 0");
                                _memory[_counter] += SetAddress(val);
                            }
                            else
                                AddSymbol(input[offset + 1]);
                        }
                    }
                    else {
                        // Get the variable being added
                        if ((offset + 1) >= input.Length)
                            throw new InvalidConstraintException("Expected value");
                        if (ushort.TryParse(input[offset + 1], out val)) {
                            CreateVariable(input[offset + 1], val);
                            //break;
                        }
                        else {
                            AddVariable(input[offset + 1]);
                            //break;
                        }

                        // Set instruction in memory
                        switch (intr) {
                            case Instruction.ADD:
                                _memory[_counter] = SetInstruction(Instruction.ADD);
                                break;
                            case Instruction.SUB:
                                _memory[_counter] = SetInstruction(Instruction.SUB);
                                break;
                            case Instruction.STA:
                                _memory[_counter] = SetInstruction(Instruction.STA);
                                break;
                            case Instruction.LDA:
                                _memory[_counter] = SetInstruction(Instruction.LDA);
                                break;
                            default:
                                break;
                            //throw new InvalidOperationException(string.Format("Unknown symbol: {0}", input[offset + 1]));
                        }

                    }
                    break;
            }
        }

        /// <summary>
        /// Creates a variable with the specified name and value
        /// </summary>
        /// <param name="input">Name of variable</param>
        /// <param name="val">Value of variable</param>
        void CreateVariable(string input, ushort val) {


            if (!extraVariablesLocation.ContainsKey(input)) {
                extraVariablesLocation.Add(input, val);
                _extraVariables++;
            }

            if (!unlinkedExtraVariables.ContainsKey(input))
                unlinkedExtraVariables.Add(input, new List<ushort> { _counter });
            else
                unlinkedExtraVariables[input].Add(_counter);
        }
        /// <summary>
        /// Adds an unlinked variable
        /// </summary>
        /// <param name="input">Name of variable</param>
        void AddVariable(string input) {
            if (!unlinkedVariables.ContainsKey(input))
                unlinkedVariables.Add(input, new List<ushort> { _counter });
            else
                unlinkedVariables[input].Add(_counter);
        }
        /// <summary>
        /// Add an unlinked symbol
        /// </summary>
        /// <param name="input">Name of symbol</param>
        void AddSymbol(string input) {
            if (!unlinkedSymbols.ContainsKey(input))
                unlinkedSymbols.Add(input, new List<ushort> { _counter });
            else
                unlinkedSymbols[input].Add(_counter);
        }
    }
}
