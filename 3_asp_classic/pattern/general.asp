<%

Public Function GetRecordset(sql)
        Dim objConn
        Dim rsServer
        
        Set objConn = Server.CreateObject("ADODB.Connection")
        objConn.ConnectionString = application("connection_string")
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
    objConn.ConnectionString = Application("connection_string")
    objConn.Open        

    Set rsServer = Server.CreateObject("ADODB.Recordset")
    rsServer.CursorLocation = 3 'adUseClient
    rsServer.Open sql, objConn, 3, 1 'adLockReadOnly

    ' Check if the recordset has any records
	If Not rsServer.EOF Then
		result = rsServer.Fields(0).Value
	Else
		result = Null
	End If
	
    rsServer.Close
    Set rsServer = Nothing
    objConn.Close
    Set objConn = Nothing

    ExecuteScalar = result
End Function

Public Function GetTeams(rs)
Dim i, result, template

template = "	<label><input type='checkbox' name='{code}'> {title}</label><br>"

if not rs.EOF then rs.movefirst

	while not rs.EOF
		
		result = result & replace(template, "{code}", rs("code"))
		result = replace(result, "{title}", rs("title")) & vbcrlf

		rs.MoveNext
		
	wend 

	GetTeams = result
	
End Function 

Function CountString(mainString, subString)
    Dim count, position
    count = 0
    position = InStr(mainString, subString)

    ' Loop until InStr returns 0 (no more occurrences)
    While position > 0
        count = count + 1
        position = InStr(position + 1, mainString, subString)
    Wend

    CountString = count
End Function

Function AddTeam(user)
	Dim rs, sql, cmd
	dim adCmdText, adInteger, adVarWChar, adDBTimeStamp, adParamInput, adParamOutput
	' https://www.w3schools.com/asp/prop_comm_commandtype.asp + https://www.w3schools.com/asp/met_comm_createparameter.asp
	adInteger = 3
	adCmdText = 1
	adVarWChar = 202
	adParamInput = 1
	adParamOutput = 2
	adDBTimeStamp = 135

	'dummy sql to get the ActiveConnection
	set rs = GetRecordset("select top 1 id from Tournament")

	sql = "INSERT INTO [Tournament]([user_op],[date_start],[date_end])VALUES(?, ?, ?); SET ?= SCOPE_IDENTITY();"

	'instantiate COMMAND and pass the RS connection
	Set cmd = Server.CreateObject("ADODB.Command")
	
	with cmd 

		.ActiveConnection = rs.ActiveConnection 
		.CommandText = sql
		.CommandType = adCmdText
		
		.Parameters.Append(.CreateParameter("@user_op", adVarWChar, adParamInput, 50))
		.Parameters("@user_op").Value = user
		
		.Parameters.Append(.CreateParameter("@date_start", adDBTimeStamp, adParamInput))
		.Parameters("@date_start").Value = Now()
		
		.Parameters.Append(.CreateParameter("@date_end", adDBTimeStamp, adParamInput))
		.Parameters("@date_end").Value = Null

		.Parameters.Append(.CreateParameter("@newID", adInteger, adParamOutput))

		.Execute
	   
		Dim newID
		newID = .Parameters("@newID").Value

	End With

	If Not rs Is Nothing Then
		rs.ActiveConnection.Close
		Set rs.ActiveConnection = Nothing
	End If
							
	AddTeam = newID
	
	'ref
	'https://www.vbforums.com/showthread.php?835777-ADO-Command-Object-with-INSERT-INTO-SQL-Statement-Return-new-ID-generated-by-DB&p=5092001&viewfull=1#post5092001
	'https://stackoverflow.com/q/16478821
	'https://stackoverflow.com/q/28682604
	'https://stackoverflow.com/a/22037613
	'https://stackoverflow.com/a/30325853
	
End Function


Function UnTournament(TournamentID)
	Dim rs, sql, cmd
	dim adCmdText, adInteger, adVarWChar, adDBTimeStamp, adParamInput, adParamOutput
	adInteger = 3
	adCmdText = 1
	adVarWChar = 202
	adParamInput = 1
	adParamOutput = 2
	adDBTimeStamp = 135

	'dummy sql to get the ActiveConnection
	set rs = GetRecordset("select top 1 id from Tournament")

	sql = "update Tournament set date_end = ? where id = ?"

	'instantiate COMMAND and pass the RS connection
	Set cmd = Server.CreateObject("ADODB.Command")
	
	with cmd 

		.ActiveConnection = rs.ActiveConnection 
		.CommandText = sql
		.CommandType = adCmdText
		
		.Parameters.Append(.CreateParameter("@date_end", adDBTimeStamp, adParamInput))
		.Parameters("@date_end").Value = Now()
		
		.Parameters.Append(.CreateParameter("@id", adInteger, adParamInput))
		.Parameters("@id").Value = TournamentID

		.Execute
	   
	End With

	If Not rs Is Nothing Then
		rs.ActiveConnection.Close
		Set rs.ActiveConnection = Nothing
	End If
							
	UnTournament = true

End Function

%>