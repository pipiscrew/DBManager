## IDE Properties

<img width="398" height="315" alt="Image" src="https://github.com/user-attachments/assets/fea72d04-f526-45a2-b462-47f76cc8e3e5" />  

<img width="398" height="316" alt="Image" src="https://github.com/user-attachments/assets/e6f980db-2fee-462f-86be-028e3e12b57d" />  

<img width="399" height="315" alt="Image" src="https://github.com/user-attachments/assets/56d9abb8-5d41-47f5-aff1-10705305f8f4" />  

## Compile On Demand
`Compile On Demand` = ON (Checked): Allows large projects to start running faster because only the called parts of the code are compiled just before execution.

`Compile On Demand` = OFF (Unchecked): Recommended by most experienced developers. It forces VBA to catch syntax and compile errors across your entire codebase before any code runs. This helps catch hidden bugs immediately rather than crashing mid-execution [src](https://colinlegg.wordpress.com/2014/02/17/a-guided-tour-through-the-vba-ides-options/)

## Use of DLL static-global-functions on Main EXE

In case the developer split the code from main exe to `ActiveX DLL`, use of `Modules` on a **DLL** is not accessible by 3rd party file (aka main EXE) by default. The workaround is to drop the methods on a new `Class` and set `Instancing` property to `GlobalMultiUse`.

* Create a new class in vb6 dll/ocx project and change it's **Instancing** property to **GlobalMultiUse**
* Add **User Defined Types** to that class and declare them **public**
* Any project referencing those can now create those **User Defined Types** and pass them around

[source](https://www.vbforums.com/showthread.php?783383-RESOLVED-How-to-access-Module-items-in-a-DLL&p=4805795&viewfull=1#post4805795)

---

## DLL Properties

<img width="340" height="247" alt="Image" src="https://github.com/user-attachments/assets/43071c08-ad32-41f7-8c15-0ce8d6ef0be0" />  
  
### Version Compatibility 
* No Compatibility
    * This option indicates that the current changes in the DLL are not designed to maintain backward compatibility.
    * It means any modifications can break existing code that relied on previous versions of the DLL.
* Project Compatibility
    * This option allows for some changes as long as the public interface of the DLL remains consistent.
	* You can add new methods or properties without breaking existing calls, but changes to existing method signatures or removed methods will lead to compatibility issues.
* Binary Compatibility
    * This option is the most stringent and ensures that existing programs can use the updated DLL without recompilation.
	* If a method name, parameters, or return types change, the DLL needs to maintain the same GUID (Globally Unique Identifier) and type library.
	* This is typically used for component-based development.

when having .vbg (visual basic group) that has a `activexDLL` & a `Standar EXE` projects... Once you done a change to `activexDLL` and compile it, `throws an error` that the `DLL is used by another application`. Possible solution is the `Standar EXE` has a reference to dll by **other directory** and not by direct `activexDLL` folder (where build). [reference](https://stackoverflow.com/a/8181512)  

use of [kinook.VisBuildPro](https://www.kinook.com/VisBuildPro/Manual/index.htm?makevb6.htm)  


### Remote Server

* Remote Server Files 
    * This option specifies that the DLL is intended to function in a distributed environment (can be utilized by client applications running on different computers), typically using DCOM (Distributed Component Object Model) which enables remote procedure calls (RPC). It allows the DLL to be used across different machines or network locations.
