<!-- #include File="general.asp" -->
 

<%
'encoding for this file must be UTF8-BOM 
Dim sql, countSQLwh, countSQL, rs, search, sortCol, order, startRecord, postalCode, sqlWhere, temp

' grid constants 
startRecord = request.form("start")
pageSize = request.form("length")
search = request.form("search[value]")
sortCol = request.form("order[0][column]")
order = request.form("order[0][dir]")

' extra parameter we passed by datatables ajax
postalCode = request.form("postalCode")

If Not IsNumeric(startRecord) Or Not IsNumeric(sortCol) Then
	response.write "general failure!"
	response.end
end if 

'default sort, always only on load comes with 0
if (sortCol=0) then sortCol = sortCol+1

'TODO RESTORE ON PROD
if postalCode="ADMIN" then
	sqlWhere ="1=1"
else	
	sqlWhere ="PostalCode='" & postalCode & "'"
end if	

Dim tableFields(4) 'the table fields is gonna appear on JSON
tableFields(0) = "CompanyName"
tableFields(1) = "ContactName"
tableFields(2) = "address"
tableFields(3) = "country"
tableFields(4) = "phone"

' generate by tableFields array like -> " and company_name like '%" & search & "%' or contact_name like '%" & search & "%' or address like '%" & search & "%' or country like '%" & search & "%' or phone like '%" & search & "%'"
temp = ""
If Not IsEmpty(search) And search <> "" then
	search=QuoteMod(search)

    For i = LBound(tableFields) To UBound(tableFields)
			temp = temp & tableFields(i) & " like '%" & search & "%'"
			if (i < UBound(tableFields)) then temp = temp & " OR "
    Next	
	
	sqlWhere = sqlWhere &  "AND (" & temp & ")"
end if 


' COUNT SQLs
countSQL = "SELECT COUNT(*) AS Total FROM customers"
countSQLwh = "SELECT COUNT(*) AS Total FROM customers where " & sqlWhere


Dim totalRecords, totalRecordsWh
totalRecords = ExecuteScalar(countSQL) 
totalRecordsWh = ExecuteScalar(countSQLwh) 

'MSSQL server variant 
sql = "SELECT * FROM customers WHERE " & sqlWhere & " ORDER BY " & sortCol & " " & order & " OFFSET " & startRecord & " ROWS FETCH NEXT " & pageSize & " ROWS ONLY"

Set rs = GetRecordset(sql)


' Output the data in JSON format
Response.ContentType = "application/json"
Dim jsonResponse
jsonResponse = "{""data"":["

Dim i
i=1

Do While Not rs.EOF
    If i > 1 Then jsonResponse = jsonResponse & ","  ' add comma between record array
	
    jsonResponse = jsonResponse & "[" & i & "," & RecordArray(tableFields) & "]"

    rs.MoveNext
	i = i+1
Loop


If Not rs Is Nothing Then
	rs.ActiveConnection.Close
	Set rs.ActiveConnection = Nothing
End If
	
jsonResponse = jsonResponse & "], ""recordsTotal"":" & totalRecords & ", ""recordsFiltered"":" & totalRecordsWh & ", ""sql"":" & Chr(34) & (sql) & Chr(34) & "}"
Response.Write jsonResponse

%>