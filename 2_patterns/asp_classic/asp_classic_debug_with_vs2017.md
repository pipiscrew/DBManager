## Debug ASP Classic with Visual Studio

* you running the website with `IIS Management Console` (not IIS Express), tested on windows10x64
* on EDGE, you using the option **Reload in Internet Explorer mode**
* Open the Classic ASP files in Visual Studio and set a breakpoint for test
* Debug Menu > Attach to Process (make sure you check `Show processes from all users`)
* On `Attach to` option select `Script code`
* Locate the IIS ASP worker process (`w3wp.exe` on IIS v6-v10, `dllhost.exe` on IIS5.1)
* Press `Attach` button

[source](https://stackoverflow.com/a/1765105)

 > tested with VS2017 most probably working on latest VS as well..