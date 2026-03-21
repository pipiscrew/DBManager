In **Visual Basic 6 (VB6)**, the compilation of applications can be done in two main modes: **P-Code** and **Native Compilation**.  

## P-Code Compilation
- **Definition**: P-Code (or pseudo-code) compilation generates an intermediate representation of the code.
- **Execution**: The compiled code is executed by the VB6 runtime, which interprets the P-Code.
- **File Size**: Typically results in `smaller executable` files because it only contains the P-Code.
- **Performance**: Generally slower execution compared to Native Code since each `P-Code instruction must be interpreted at runtime`.
- **Portability**: Offers better compatibility across different machines since it relies on the runtime environment.

## Native Compilation
- **Definition**: Native compilation generates machine code specific to the processor architecture.
- **Execution**: The compiled code runs directly on the processor without needing the VB6 runtime interpreter.
- **File Size**: Results in `larger executable` files due to the inclusion of actual machine code.
- **Performance**: Provides faster execution since no interpretation is required; the code is executed directly by the CPU.
- **Portability**: Less portable, as native code is specific to a particular processor type (e.g., x86 architecture).

### Summary Table

| Feature                 | P-Code Compilation                   | Native Compilation                       |
|-------------------------|-------------------------------------|-----------------------------------------|
| Code Type               | Intermediate representation          | Machine code                            |
| Execution Method        | Interpreted by VB6 runtime          | Directly executed by CPU                |
| File Size               | Generally smaller                   | Generally larger                        |
| Performance             | Slower                              | Faster                                  |
| Portability             | More portable                       | Less portable (architecture-dependent)  |

Choosing between P-Code and Native Compilation depends on your needs regarding performance and portability.
