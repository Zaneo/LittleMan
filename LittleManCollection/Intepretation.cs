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

namespace LittleMan {

    /// <summary>
    /// Handles the base interpretation of all instructions and memory acccess
    /// </summary>
    public class Interpreter {

        [BrowsableAttribute(false)]
        public ProgramType Type { get; private set; }

        /// <remarks>Short = 16 bits, 4 bits for intruction 12 bits for memory adddress</remarks> 
        const byte INSTRUCTION_BITS = 4;
        const byte MAX_INSTRUCTION_VALUE = (1 << INSTRUCTION_BITS) - 1;
        const byte ADDRESS_BITS = 12;
        const short ADDRESSMASK = (1 << ADDRESS_BITS) - 1;

        /// <summary>
        /// Instruction list
        /// </summary>
        public enum Instruction : ushort {
            END = 0,
            ADD,
            SUB,
            STA,
            LDA,
            BRA,
            BRP,
            BRZ,
            INP,
            OUT,
            DAT,
            STR = 15
        }

        /// <summary>
        /// Sets the program type
        /// </summary>
        /// <param name="type"> The type of program to set as. </param>
        public void SetProgramType(ProgramType type) {
            Type = type;
        }
        /// <summary>
        /// Converts the Instruction into a short and validates it
        /// </summary>
        /// <param name="instruction">Instruction to convert</param>
        /// <returns>Intstruction as a short</returns>
        public ushort SetInstruction(Instruction instruction) {
            return SetInstruction((ushort)instruction);
        }
        /// <summary>
        /// Validates the instruction
        /// </summary>
        /// <param name="value">Instruction to convert</param>
        /// <returns>Instruction as a short</returns>
        public ushort SetInstruction(ushort value) {
            if (value < 0)
                throw new IndexOutOfRangeException("Intruction must be a positive value");
            if (value > MAX_INSTRUCTION_VALUE)
                throw new IndexOutOfRangeException(string.Format("Intruction must be below or equal to MAX_INTRUCTION_VALUE: {0}", MAX_INSTRUCTION_VALUE));
            return (ushort)((value) << (ADDRESS_BITS));
        }
        /// <summary>
        /// Validates the memory address
        /// </summary>
        /// <param name="value">Validates unsafe address from short</param>
        /// <returns>Validated address</returns>
        public ushort SetAddress(short value) {
            if (value < 0)
                throw new IndexOutOfRangeException("Address must be a positive value");
            if (value > ADDRESSMASK)
                throw new IndexOutOfRangeException(string.Format("Address must be below or equal to MAX_ADDRESS: {0}", ADDRESSMASK));
            return (ushort)value;
        }
        /// <summary>
        /// Validates the memory address
        /// </summary>
        /// <param name="value">Validates unsafe address from ushort</param>
        /// <returns>Validated address</returns>
        public ushort SetAddress(ushort value) {
            if (value < 0)
                throw new IndexOutOfRangeException("Address must be a positive value");
            if (value > ADDRESSMASK)
                throw new IndexOutOfRangeException(string.Format("Address must be below or equal to MAX_ADDRESS: {0}", ADDRESSMASK));
            return value;
        }
        /// <summary>
        /// Unpacks instruction
        /// </summary>
        /// <param name="value">Value from which to get instruction</param>
        /// <returns>Instruction as a short</returns>
        public short GetInstruction(ushort value) {
            return (short)((value >> ADDRESS_BITS));
        }
        /// <summary>
        /// Unpacks instruction
        /// </summary>
        /// <param name="value">Value from which to get instruction</param>
        /// <returns>Instruction as a short</returns>
        public short GetInstruction(short value) {
            return (short)((value >> ADDRESS_BITS));
        }
        /// <summary>
        /// Unpacks address
        /// </summary>
        /// <param name="value">Value from which to get address</param>
        /// <returns>Address as a short</returns>
        public ushort GetAddress(ushort value) {
            return (ushort)(value & ADDRESSMASK);
        }
        /// <summary>
        /// Unpacks address
        /// </summary>
        /// <param name="value">Value from which to get address</param>
        /// <returns>Address as a short</returns>
        public short GetAddress(short value) {
            return (short)(value & ADDRESSMASK);
        }
        /// <summary>
        /// Gets the instruction from a short
        /// </summary>
        /// <param name="value">Short to convert</param>
        /// <returns>Instruction</returns>
        public Instruction ToInstruction(short value) {
            return (Instruction)((ushort)value);
        }
        /// <summary>
        /// Gets the instruction from a short
        /// </summary>
        /// <param name="value">Ushort to convert</param>
        /// <returns>Instruction</returns>
        public Instruction ToInstruction(ushort value) {
            return (Instruction)GetInstruction(value);
        }
    }
}
