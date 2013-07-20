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
using System.ComponentModel;
using System.IO;
using System.Threading;
using LittleMan.IO;

namespace LittleMan.Emulation {
    public enum InputFlag {
        Signed,
        Unsigned
    }
    public enum InputType {
        Char,
        Byte,
        Short
    }
   public class VirtualMachine:Interpreter {

        #region Setup
        Memory _memory;
        ushort[]_stockMemory;
        bool _memorySet;
        bool _done;
        bool SteppedMode;
        
        short[] _enviroment = new short[7];

        [Browsable(false)]
        public readonly object syncRoot;
        [Browsable(false)]
        public Memory Memory { get { return _memory; } private set { _memory = value; } }

        [ReadOnly(false),
        Category("IO"),
        Description("The input for the program")]
        public short Input { get { return _enviroment[0]; } set { _enviroment[0] = value; } }
        
       [ReadOnly(true),
        Category("IO"),
        Description("The output for the program")] 
        public short Output { get { return _enviroment[1]; } private set { _enviroment[1] = value; } }
        
       [ReadOnly(true),
        Category("Enviroment"),
        Description("The current position within the memory")]
        public ushort ProgramCounter { get { return (ushort)_enviroment[2]; } private set { _enviroment[2] = (short)value; } }
        
       [ReadOnly(true),
        Category("Enviroment"),
        Description("Value currently in the accumulator")]
        public short Accumulator { get { return _enviroment[3]; } private set { _enviroment[3] = value; } }
        
       [ReadOnly(false),
        Category("Enviroment"),
        Description("Currently loaded memory address")]
        public ushort MemoryAddress { get { return (ushort)_enviroment[4]; } set { _enviroment[4] = (short)value; MemoryValue = RequestMemVal(value); } }
        
       [ReadOnly(false),
        Category("Enviroment"),
        Description("Currently loaded memory value")]
        public short MemoryValue { get { return _enviroment[5]; } set { _enviroment[4] = value; SetMemory(MemoryAddress, (ushort)value); } }
        
       [ReadOnly(false),
        Category("Exception"),
        Description("If an overlow operation occured")]
        public bool Overflow { get { return _enviroment[6] > 0 ? true : false; } private set { _enviroment[6] = (short)(value == true ? 1 : 0); } }


       [ReadOnly(false),
        Category("Exception"),
        Description("If an overlow operation occured")]       
        public bool Completed { get { return _done; } }

        [Browsable(false)]
        public IHumanInterface IOInterface { get; private set; }

        [Browsable(false)]
        private ManualResetEvent waitOn;

        Exception innerException;

        public string OutputPath;

        /// <summary>
        /// Creates a new Virtual Machine
        /// </summary>
        /// <param name="IOMethod"> Uses specified human interface</param>
        public VirtualMachine(IHumanInterface IOMethod) {
            IOInterface = IOMethod;
            waitOn = new ManualResetEvent(true);
            IOMethod.SetManualResetEvent(ref waitOn);
            //IOInterface.InputReady += _handler;
            SetProgramType(ProgramType.Computer);

            Input = 0;
            Output = 0;
            Accumulator = 0;
            ProgramCounter = 1;
            MemoryAddress = 0;
            MemoryValue = 0;
            Overflow = false;
            _memorySet = false;
            _done = false;
            _memory = new Memory();

            syncRoot = new object();
        }

        public void SetProperties(ref InputHandler handler) {
            OutputPath = handler.outputName;
            Stream stream = handler.InputMethod.GetInput(handler.InputTypePayload);
            InitialiseMemory((short)(stream.Length / 2));
            for (int i = 0; i < _memory.Length; i++) {
                _memory[i] = (ushort)(stream.ReadByte() << 8);
                _memory[i] += (ushort)stream.ReadByte();
            }
            _stockMemory = _memory.Get();
            _memorySet = true;

            SteppedMode = handler.StepThrough;
        }
       
        /// <summary>
        /// Initialise VM's memory
        /// </summary>
        /// <param name="size">Size of memory array</param>
        public void InitialiseMemory(short size) {
            _memory.Set(new ushort[size]);
            _stockMemory = _memory.Get();
            _memorySet = true;
        }

        /// <summary>
        /// Initialise and sets VM's memory
        /// </summary>
        /// <param name="memory">Remory array being copied</param>
        public void SetProgram(ushort[] memory){
            _memory.Set(memory);
            _stockMemory = memory;
            _memorySet = true;
        }

        public void SubscribeListChanged(ListChangedEventHandler handler) {
            _memory.ListChanged += handler;
        }

        /// <summary>
        /// Sets the the memory at a specified location
        /// </summary>
        /// <param name="index">Index of location</param>
        /// <param name="value">Value to be set</param>
        public void SetMemory(ushort index, ushort value) {
            if (!_memorySet) {
                innerException = new InvalidOperationException("Memory must be initialised, before it can be set.");
                return;
            }
            _memory[index] = value;
            _stockMemory[index] = value;
        }
        #endregion

        #region Instructions
        /// <summary>
        /// Loads address into accumulator
        /// </summary>
        /// <param name="address">Address to load</param>
        void LDA(ushort address) {
            Accumulator = RequestMemVal(address);
        }
        /// <summary>
        /// Stores value at specified location
        /// </summary>
        /// <param name="address">Address</param>
        void STA(ushort address) {
            RequestSetMem(address, Accumulator);
        }
        /// <summary>
        /// Branches always
        /// </summary>
        /// <param name="address">Address</param>
        void BRA(ushort address) {
            if (address == 0)
                address = (ushort)Accumulator;

            RequestJMP(address);
        }
        /// <summary>
        /// Branches if positive or zero
        /// </summary>
        /// <param name="address">Address</param>
        void BRP(ushort address) {
            if (Accumulator >= 0) {
                if (address == 0)
                    address = (ushort)Accumulator;

                RequestJMP(address);
            }            
        }
        /// <summary>
        /// Branches if zero
        /// </summary>
        /// <param name="address">address</param>
        void BRZ(ushort address) {
            if (Accumulator == 0) {
                if (address == 0)
                    address = (ushort)Accumulator;

                RequestJMP(address);
            }            
        }
        /// <summary>
        /// Gets input from interface
        /// </summary>
        void INP() {
            Input = RequestINP();
            Accumulator = Input;
        }
        /// <summary>
        /// Writes output to interface
        /// </summary>
        void OUT() {
            Output = Accumulator;
            RequestOUT(Output);
        }
        /// <summary>
        /// Adds value in memory to accumulator
        /// </summary>
        /// <param name="address">address</param>
        void ADD(ushort address)
        {
            Overflow = false;
            try {
                Accumulator = checked((short)(Accumulator + RequestMemVal(address)));
            }
            catch (OverflowException) {

                Overflow = true;
            }
        }
        /// <summary>
        /// Subtracts accumulator by value in memory
        /// </summary>
        /// <param name="address"></param>
        void SUB(ushort address) {
            Overflow = false;

            try {
                Accumulator = checked((short)(Accumulator - RequestMemVal(address)));
            }
            catch (OverflowException) {

                Overflow = true;
            }
        }    
        #endregion

        #region Operation
        /// <summary>
        /// Executes code one step at a time
        /// </summary>
        public void ExecuteStep() {
            if (_done) {
                innerException = new InvalidProgramException("Program has already completed running");
                return;
            }
            ushort value = _memory[ProgramCounter];
            Instruction intr = ToInstruction(value);
            ushort address = GetAddress(value);
            if (_done)
                return;

            switch (intr) {
                case Instruction.END:
                    _done = true;
                    break;
                case Instruction.ADD:
                    ADD(address);
                    break;
                case Instruction.SUB:
                    SUB(address);
                    break;
                case Instruction.STA:
                    STA(address);
                    break;
                case Instruction.LDA:
                    LDA(address);
                    break;
                case Instruction.BRA:
                    BRA(address);
                    break;
                case Instruction.BRP:
                    BRP(address);
                    break;
                case Instruction.BRZ:
                    BRZ(address);
                    break;
                case Instruction.INP:
                    INP();
                    break;
                case Instruction.OUT:
                    OUT();
                    break;
                default:
                    innerException = new InvalidOperationException("Unknown instruction encountered");
                    break;
                    
            }
            ProgramCounter++;
        }

        /// <summary>
        /// Executes code until completion
        /// </summary>
        /// <remarks>Warning this can cause an infinite loop, make sure code is terminating</remarks>
        public void ExecuteAll(bool stopOnError) {
            lock (syncRoot) {
                while (!_done) {
                    ExecuteStep();
                    if (innerException != null && stopOnError) {
                        this.IOInterface.Output<string>(innerException.Message);
                        _done = true;
                    }
                    else if (SteppedMode) {
                        waitOn.Reset();
                        while (!waitOn.WaitOne(1000 * 60)) {
                            innerException = new TimeoutException("Suspected that advance has hung");
                        }
                    }
                }
            }
        }
        #endregion

        #region Requests
        /// <summary>
        /// The VM makes a request to a human interface for user input
        /// </summary>
        /// <returns>Input from user</returns>
        short RequestINP() {
            IOInterface.PreFetch();
            while (!waitOn.WaitOne(1000 * 60)) {
                innerException = new TimeoutException("Suspected that input has hung");
            }
            return IOInterface.Input();
        }
       /// <summary>
       /// Gets the entire contents of the enviroment memory.
       /// </summary>
       /// <returns> The contents of the enviroment memory, accumulator, program counter, etc.</returns>
        short[] RequestEnviroment() {
            return _enviroment;
        }

        /// <summary>
        /// Outputs Value to IOInterface
        /// </summary>
        /// <param name="value">value to be output</param>
        void RequestOUT(short value) {
            IOInterface.Output(value);
        }
        public void RequestOUT(string text) {
            IOInterface.Output(text);
        }
        

        /// <summary>
        /// Request jump to specific line of code
        /// </summary>
        /// <param name="location">Location to jump to</param>
        /// <remarks>Requesting an invalid location is undefined behavior</remarks>
        public void RequestJMP(ushort location){            
            ushort buffer = location;
            if (location > _memory.Length){
                 buffer = (ushort)(location % _memory.Length);
                 innerException = new OverflowException(string.Format("Attempted jump to out of bounds location: {0} , rolled over to: {1}", location, buffer));
            }
            ProgramCounter = buffer;         
        }

        /// <summary>
        /// Gets the entire content of memory and converts it to a short[]
        /// </summary>
        /// <returns>Virtual Machine memory as a short[]</returns>
        public short[] RequestAllMemory() {
            return _memory.GetShort();
        }
        /// <summary>
        /// Requests that the VM returns the value stored in memory at the specified location
        /// </summary>
        /// <param name="location">Location in memory to view</param>
        /// <returns>Value at provided location</returns>
        /// <remarks>Requesting an invalid location is undefined behavior</remarks>
        public short RequestMemVal(ushort location) {            
            if (BoundCheck(location))
                return (short)_memory[location];
            else {
                innerException = new IndexOutOfRangeException(string.Format("RequestMemVal: {0}", location));
                return 0;
            }
        }

        public void RequestSetMem(ushort location, short value) {
            ushort buffer = location;
            if (location > _memory.Length) {
                buffer = (ushort)(location % _memory.Length);
                innerException = new OverflowException(string.Format("Attempted set memory to out of bounds location: {0} , rolled over to: {1}", location, buffer));
            }
            _memory[buffer] = (ushort)value;
        }

        /// <summary>
        /// Resets the VM
        /// </summary>
        public void RequestReset() {
            Input = 0;
            Output = 0;
            Accumulator = 0;
            ProgramCounter = 1;
            MemoryAddress = 0;
            MemoryValue = 0;

            _memory.Set(_stockMemory);
            _done = false;
        }

        /// <summary>
        /// Clears the VM of all code and resets the variables
        /// </summary>
        public void RequestClear() {
            _memorySet = false;
            _stockMemory = null;
            RequestReset();
        }
        #endregion

        /// <summary>
        /// Determines if the values is within range of the memory
        /// </summary>
        /// <param name="value">location to check</param>
        /// <returns>True if the values is within bounds</returns>
        bool BoundCheck(ushort value) {
            if (_memory == null) {
                return false;
            }
            if (value > _memory.Length)
                return false;
            return true;
        }

        /// <summary>
        /// Returns the last encountered exception
        /// </summary>
        /// <returns>Last exception, or null if no exception</returns>
        /// <remarks>The error is not thrown, only logged. Additionally after GetError is called the exception is cleared</remarks>
        public Exception GetError() {
            Exception buffer = innerException;
            innerException = null;
            return buffer;
        }
    }
}

