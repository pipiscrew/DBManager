<%

' ADO params - https://www.w3schools.com/asp/prop_comm_commandtype.asp + https://www.w3schools.com/asp/met_comm_createparameter.asp
Const adInteger = 3
Const adCmdText = 1
Const adVarWChar = 202
Const adParamInput = 1
Const adParamOutput = 2
Const adDBTimeStamp = 135

Public Function GetRecordset(sql)
        Dim objConn
        Dim rsServer
        
        Set objConn = Server.CreateObject("ADODB.Connection")
        objConn.ConnectionString = application("x")
        objConn.Open        

        set rsServer = Server.CreateObject("ADODB.Recordset")
		rsServer.CursorLocation = 3 'adUseClient
        rsServer.open sql,objconn, 3, 1 'adLockReadOnly
        
        Set GetRecordset = rsServer
End function

Public Function ExecuteScalar(sql)
    Dim objConn
    Dim rsServer
    Dim result

    Set objConn = Server.CreateObject("ADODB.Connection")
    objConn.ConnectionString = Application("x")
    objConn.Open        

    Set rsServer = Server.CreateObject("ADODB.Recordset")
    rsServer.CursorLocation = 3 'adUseClient
    rsServer.Open sql, objConn, 3, 1 'adLockReadOnly

    ' Check if the recordset has any records
	If Not rsServer.EOF Then
		result = rsServer.Fields(0).Value ' Get the value of the first column
	Else
		result = Null ' Return Null if no records found
	End If
	
    rsServer.Close
    Set rsServer = Nothing
    objConn.Close
    Set objConn = Nothing

    ExecuteScalar = result
End Function


Public Function RecordArray(arr)

	Dim result, i
	result = ""
	
    For i = LBound(arr) To UBound(arr)
        result = result & Chr(34) & arr(i) & Chr(34) & ":" & Chr(34) & HTMLEncode(rs(arr(i))) & Chr(34) 

		if (i < UBound(arr)) then result = result & ", "
    Next
    
    RecordArray = result
	
End Function

Public Function QuoteMod(Obj)
        if IsNull(Obj) or IsEmpty(Obj) then
           QuoteMod = ""
        else
           QuoteMod = Replace(Obj, "'", "''")
           QuoteMod = Replace(QuoteMod, """", "''")
        end if
End Function

Public Function HTMLEncode(str)
    Dim encodedStr
    encodedStr = str
    encodedStr = Replace(encodedStr, "&", "&amp;")
    encodedStr = Replace(encodedStr, "<", "&lt;")
    encodedStr = Replace(encodedStr, ">", "&gt;")
    encodedStr = Replace(encodedStr, """", "&quot;")
    encodedStr = Replace(encodedStr, "'", "&apos;")
    HTMLEncode = encodedStr
End Function

%>