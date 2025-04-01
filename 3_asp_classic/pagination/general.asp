
<%

Public Function GetRecordset(sql)
        Dim objConn
        Dim rsServer
        
        Set objConn = Server.CreateObject("ADODB.Connection")
		objConn.ConnectionString = "Provider=SQLOLEDB;Data Source=.\SQLEXPRESS;Initial Catalog=NORTHWND;User ID=;Password=;"
        objConn.Open        

        set rsServer = Server.CreateObject("ADODB.Recordset")
		rsServer.CursorLocation = 3 'adUseClient
        rsServer.open sql,objconn, 3, 1 'adLockReadOnly
        
        Set GetRecordset = rsServer
End Function

Public Function ExecuteScalar(sql)
        Dim objConn
        Dim rsServer
        
        Set objConn = Server.CreateObject("ADODB.Connection")
        objConn.ConnectionString = "Provider=SQLOLEDB;Data Source=.\SQLEXPRESS;Initial Catalog=NORTHWND;User ID=;Password=;"
        objConn.Open        

        set rsServer = Server.CreateObject("ADODB.Recordset")
        rsServer.open sql,objconn, 3, 1 'adLockReadOnly
        
		If Not rsServer.EOF Then
			result = rsServer.Fields(0).Value
		Else
			result = Null
		End If
		
        rsServer.Close
		objConn.Close
		
		ExecuteScalar = result
End function

Public Function Schema1RecordsetToTableRows(rs)
Dim i
i=0

if not rs.EOF then rs.movefirst

	while not rs.EOF
		i=i+1

		VBStr = VBStr& "<tr><td><span style='color:red'>" & i & "</span></td>"
		VBStr = VBStr & "<td>" & HTMLEncode(rs("CompanyName")) & "</td>"
		VBStr = VBStr & "<td>" & HTMLEncode(rs("ContactName"))	& "</td>"
		VBStr = VBStr & "<td>" & HTMLEncode(rs("address")) & "</td>"
		VBStr = VBStr & "<td>" & HTMLEncode(rs("country")) & "</td>"
		VBStr = VBStr & "<td>" & HTMLEncode(rs("phone")) & "</td></tr>"

		rs.MoveNext
	wend 

	Schema1RecordsetToTableRows = VBStr
	
End Function 


Public Function RecordArray(arr)

	Dim result, i
	result = ""
	
    For i = LBound(arr) To UBound(arr)
        result = result & Chr(34) & HTMLEncode(rs(arr(i))) & Chr(34) 
		
		if (i < UBound(arr)) then result = result & ", "
    Next
    
    RecordArray = result
	
End Function


Public Function HTMLEncode(str)
    Dim encodedStr
	
	if IsNull(str) or IsEmpty(str) then encodedStr = "" else encodedStr = trim(str)
		   
    'encodedStr = str
    encodedStr = Replace(encodedStr, "&", "&amp;")
    encodedStr = Replace(encodedStr, "<", "&lt;")
    encodedStr = Replace(encodedStr, ">", "&gt;")
    encodedStr = Replace(encodedStr, """", "&quot;")
    encodedStr = Replace(encodedStr, "'", "&apos;")
    HTMLEncode = encodedStr
End Function

Public Function QuoteMod(Obj)
        if IsNull(Obj) or IsEmpty(Obj) then
           QuoteMod = ""
        else
           QuoteMod = Replace(Obj, "'", "''")
           QuoteMod = Replace(QuoteMod, """", "''")
        end if
End Function



%>