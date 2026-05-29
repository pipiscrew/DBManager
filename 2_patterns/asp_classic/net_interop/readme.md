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
* For GUID use the Visual Studio option "Tools" > "Create GUID"
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

---

## use .net classes to vb6
* [Kellyethridge.VBCorLib](https://github.com/kellyethridge/VBCorLib)
* [WindowStations/VB6NameSpaces](https://github.com/WindowStations/VB6NameSpaces)
  * [VBForumsCommunity/VB6Porter](https://github.com/VBForumsCommunity/VB6Porter) [[2](https://github.com/VBForumsCommunity/VB6Porter/discussions/8)]
* [bclothier/vbDotNetLoader](https://gist.github.com/bclothier/1916699dad057b0bdf884c10f4cc8475)

more :  
  * [VB6 Dynamic Behaviors](https://github.com/vegitz/codes/blob/1de4798dd0dc6dc41e8982453f4bbb8ab51c4dce/0031%20VB6%20Dynamic%20Behaviors/App/Form1.frm#L142)
  * [VB6 Dictionary](https://github.com/vegitz/vb6/blob/master/dictionary/codes/Form1.frm)
  * [VB6 HttpWebServiceCall](https://github.com/vegitz/vb6/blob/master/HttpWebServiceCall/Codes/Form1.frm)
  * [VB6 + PHP - AES 256-bit encryption](https://gist.github.com/wqweto/42a6c1de16cc87e9bab2ac9f3c9d8510)
    * [2](https://www.vbforums.com/showthread.php?862103-VB6-Simple-AES-256-bit-password-protected-encryption&p=5547089#post5547089)
  * [VB6 - use .net objects via CreateObject](https://www.vbforums.com/showthread.php?900678-How-do-I-correctly-use-NET-objects-in-VB6-Is-a-TLB-available-for-it)
  * [Visual Basic v6.0 Resource Center](https://learn.microsoft.com/en-us/previous-versions/visualstudio/visual-basic-6/visual-basic-6.0-documentation)
  * [Microsoft InteropForms Toolkit](https://www.softpedia.com/get/Programming/Other-Programming-Files/InteropForms-Toolkit.shtml)
  * [Extending Visual Basic 6 ActiveX EXEs With Visual Basic 2005 and the Interop Forms Toolkit](https://learn.microsoft.com/en-us/previous-versions/bb397409(v=vs.80)?redirectedfrom=MSDN)
    * [2](https://msdn.microsoft.com/en-us/library/bb397409(VS.80).aspx)
  * [COM Interop (Visual Basic)](https://learn.microsoft.com/en-us/dotnet/visual-basic/programming-guide/com-interop/)
  * [VS2003_VB_en-us,pdf](https://www.microsoft.com/en-us/download/details.aspx?id=55979)
  * [Exposing ,NET components to COM](https://learn.microsoft.com/en-us/dotnet/framework/interop/exposing-dotnet-components-to-com)
  * [gerardo-lijs/CrystalReportsRunner - Runner to allow the use of Crystal Reports in .netCore using external process frm48 and named pipes for communication](https://github.com/gerardo-lijs/CrystalReportsRunner)
  * [NET to COM/ActiveX - params/ParamArray not supported for VB98/VB6 and earlier](https://github.com/dotnet/runtime/issues/109751)
