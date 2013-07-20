LittleMan
=========

A naive assembler, VM and IDE.

Inspired by use of this java applet, http://www.yorku.ca/sychen/research/LMC/index.html

Project
=======

Currently exists in four parts.

LittleManCore - The library from which everything is run.

LittleManCompiler - Shell code for a commandline based assembler/compiler using LittleManCore.

LittleManComputer - Shell code for a commandline based virtual machine using LittleManCore.

LittleManIDE - Begining of a simple IDE with code debugging.

Addressing
==========
Address are 12 bits, the zero memory space is a reserved location, nothing may jump to 0*.

Instruction Set
===============

A 4 bit instructions is used, with at 12 bit addressing scheme, packed into 16 bit register.

Terms with an x after them take an address.

0:  END  - Stop executing the current program.

1:  ADDx - Add the value at x to accumulator.

2:  SUBx - Subtract the accumulator by x.

3:  STAx - Store the acummulator at x.

4:  LDAx - Store the value at x, in the accumulator.

5:  BRAx - If x is zero, branch to the value in the accumulator, otherwise branch to x.

6:  BRPx - If accumulator is Positive or Zero, branch to x.

7:  BRZx - If accumulator is Zero, branch to x.

8:  INP  - Read input and store in accumulator, input is a ushort.

9:  OUT  - Write accumulator to output.

10: DAT  - Specifies variable declaration.

15: STR  - Signifies program start, always appears in the zero memory location.
