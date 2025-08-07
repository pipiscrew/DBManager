<!-- #include File="general.asp" -->

<%
	Dim postalCode, pc, fileName, fileContent, rs, sql, sqlWhere

	postalCode=request.form("postalCode") 
	fileName = Request.Form("fileName")

	if postalCode="ADMIN" then
		sqlWhere ="1=1"
	else	
		sqlWhere ="PostalCode='" & postalCode & "'"
	end if	

	sql = "SELECT CompanyName,ContactName,address,country,phone FROM customers WHERE " & sqlWhere & " ORDER BY 1"

	set rs = GetRecordset(sql)
 
	fileContent = rs.GetString(,,vbTab,vbCrlf,"&nbsp;")  'http://www.w3schools.com/ASp/met_rs_getstring.asp

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