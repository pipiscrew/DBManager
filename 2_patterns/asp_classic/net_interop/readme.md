# net implementation

```csharp
// src - https://github.com/gerardo-lijs/VB6-NetStandard-Interop
// modded

[Guid("45f838c9-d8cd-4198-b2b3-f4e8c2a5b588")]
[InterfaceType(ComInterfaceType.InterfaceIsDual)]
[ComVisible(true)]
public interface IMathUtils
{
    double CalculateComplexMethod();
    void CommandWithParameters(int number1, int number2);
}

[Guid("293bcd3a-e771-45d5-8a53-5413997c2de8")]
[ClassInterface(ClassInterfaceType.None)]
[ComVisible(true)]
[ProgId("NetStandardLibrary.MathUtils")]
public class MathUtils : IMathUtils
{
    public MathUtils() {  }

    public double CalculateComplexMethod() => 4;

    public void CommandWithParameters(int number1, int number2)
    {
        MessageBox.Show("!!");
    }
}
```  


### InterfaceIsDual `used by default`
* Early binding (v-table)
* Late binding (IDispatch)
* Works with VB6 typed variables
* Works with CreateObject
* Best choice for VB6 interop

### InterfaceIsIUnknown
* Early binding only (v-table)
* Fastest calls
* No late binding
* CreateObject not usable
* Requires VB6 reference + strong typing
* Use for strict / controlled scenarios

### InterfaceIsIDispatch
* Late binding only
* Works with CreateObject
* No early binding
* No IntelliSense in VB6
* Slower (Invoke calls)
* Rarely used  

Early binding example :
```vb
' Requires VB6 reference
Dim o As IMathUtils
Set o = New NetStandardLibrary.MathUtils
```

Late binding example :
```vb
' Without VB6 reference -- you cannot use the interface in late binding
Dim o As Object
Set o = CreateObject("NetStandardLibrary.MathUtils")
```
---

### ClassInterfaceType.None
* No **automatic COM interface** generated -- yeah will use ours ;)
* Must use explicit interface (recommended)
* Stable versioning (safe for VB6)
* Best practice
### ClassInterfaceType.AutoDual `used by default`
* Automatically creates interface from class
* Supports early + late binding
* Method **order** can change ? breaks VB6 (if not recompile the VB6 project)
* Versioning is unsafe
* Strongly discouraged
### ClassInterfaceType.AutoDispatch
* Auto-generated interface
* Late binding only (IDispatch)
* No early binding / IntelliSense
* Limited control
* Rarely useful

---

## development
* For GUID use the Visual Studio option "Tools" > "Create GUID
* Compile the .net library, to generate tlb from assembly and register tlb to the system. Use/run Developer Command Prompt for Visual Studio as Administrator.
> regasm.exe /tlb /codebase NetStandardLibrary.dll  
> using **/codebase** - instruct to load this DLL from its current file path, not from the GAC.

## deployment
* Add the `dll` file of the .net library to your installer
* **No need** to deploy `tlb` file
* For the `dll` will need to use regasm.exe. Use 32 or 64bit version depending on your needs.
* Register with `/codebase` argument
* Unregister with `/unregister` argument
* To locate regasm.exe in client computer you can find the related registry keys in `HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework`.
* x86 - `C:\Windows\Microsoft.NET\Framework\v4.0.30319`
* x64 - `C:\Windows\Microsoft.NET\Framework64\v4.0.30319`