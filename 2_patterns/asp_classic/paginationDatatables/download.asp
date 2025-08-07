<%
	' Get the file content from the form submission
	Dim fileContent, filename
	fileContent = Request.Form("fileContent")
	filename = Request.Form("fileName")

	' Set the response headers to prompt for download
	'Response.Charset="UTF-8"
	'Response.ContentType = "text/html; charset=UTF-8"
	'Response.CodePage = 65001
	Response.ContentType = "text/plain"
	Response.AddHeader "Content-Disposition", "attachment; filename=" & filename
	Response.Write fileContent
	Response.End
%>