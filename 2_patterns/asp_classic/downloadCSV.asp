<!-- #include File="general.asp" -->

<%
If Request.ServerVariables("REQUEST_METHOD") <> "POST" Then
	Response.Write("Report this error code to developer 0x81474.")
	Response.End
End If

fileName = Request.Form("fileName") 'test.xls, coming from the html form. User can open it on excel direct

sql = "select * from test where status in (0,1,2)"
set rs = GetRecordset(sql)


header = "CustomerName	Telephone	City	Area	Amount"

'magic line, convert all recordset, columns to TAB, records between with ENTER
fileContent = header & vbCrlf & rs.GetString(,,vbTab,vbCrlf,"NULL")

If Not rs Is Nothing Then
	rs.ActiveConnection.Close
	Set rs.ActiveConnection = Nothing
End If

' Prompt for download
Response.ContentType = "text/plain"
Response.AddHeader "Content-Disposition", "attachment; filename=" & filename
Response.Write fileContent
Response.End
%>