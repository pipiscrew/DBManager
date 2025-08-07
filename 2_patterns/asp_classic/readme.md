# UTF8 for html or MSSQL

If you have to involve UTF8 characters to a webpage, your **.asp files** must be **ANSI** and on the **codepage** you working on. Below example how Greek chars appears on  **UTF8 .asp file** :  

<img width="221" height="69" alt="Image" src="https://github.com/user-attachments/assets/31f542e7-2680-4b3d-bae7-b17718176f32" />  

meanwhile in VSCode modify at  
<img width="400" height="348" alt="Image" src="https://github.com/user-attachments/assets/1b51a20f-6cd7-48eb-86a4-4735417db6bc" />

---

The same occur when you instruct **ADO** to write UTF8 chars to database, even you use `Parameterized Queries` with adVarWChar failing.. Again here is a sample .asp, UTF8 vs ANSI database view :  

<img width="442" height="40" alt="Image" src="https://github.com/user-attachments/assets/56a75f99-92a8-4252-ae4a-5951ae5baca2" />

another method which tested&working with database (even the file is UTF8) :  
```
<%@ CodePage=65001 %>
```

if needed for html (no tested), for `ISO-8859-7` is :  
```
<%@ CodePage=28597 %>
```  

