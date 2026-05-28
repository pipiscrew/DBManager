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