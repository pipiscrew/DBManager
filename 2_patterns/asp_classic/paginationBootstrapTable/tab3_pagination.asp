<!-- #include File="general.asp" -->
 

<%
'encoding for this file must be UTF8-BOM 
Dim sql, countSQLwh, countSQL, rs, search, sortCol, order, startRecord, sqlWhere

' grid constants 
startRecord = Request.QueryString("offset")
pageSize = Request.QueryString("limit")
search = Request.QueryString("search")
sortCol = Request.QueryString("sort")
order = Request.QueryString("order")


'default sort, if is not defined on HTML is empty, set sort by default to column 1
if IsEmpty(sortCol) then sortCol = 1

sqlWhere ="1=1"

If Not IsEmpty(search) And search <> "" then
	search=QuoteMod(search)

	sqlWhere=sqlWhere & " and cust_name like '%" & search & "%' or date_rec like '%" & search & "%' or amount like '%" & search & "%'"
end if 


'//
'CONSTRUCT SQLs
countSQL = "SELECT COUNT(1) AS Total FROM customers"
countSQLwh = "SELECT COUNT(1) AS Total FROM customers where " & sqlWhere


Dim totalRecords

if len(sqlWhere) = 3 then 
	totalRecords = ExecuteScalar(countSQL) 
else 
	totalRecords = ExecuteScalar(countSQLwh) 
end if 

sql = "SELECT [id] ,[cust_name] ,[date_rec] ,[amount] from customers WHERE " & sqlWhere & " ORDER BY " & sortCol & " " & order & " OFFSET " & startRecord & " ROWS FETCH NEXT " & pageSize & " ROWS ONLY"

'//
	
Set rs = GetRecordset(sql)


' Output the data in JSON format
Response.ContentType = "application/json"
Dim jsonResponse
jsonResponse = "{""rows"":["

Dim i
i=1

Dim tableFields(3) 'the table fields is gonna appear on JSON
tableFields(0) = "id"
tableFields(1) = "cust_name"
tableFields(2) = "date_rec"
tableFields(3) = "amount"

Do While Not rs.EOF
    If i > 1 Then jsonResponse = jsonResponse & ","  ' add comma between record array
	
    jsonResponse = jsonResponse & "{" &  RecordArray(tableFields) & "}"

    rs.MoveNext
	i = i+1
Loop


If Not rs Is Nothing Then
	rs.ActiveConnection.Close
	Set rs.ActiveConnection = Nothing
End If


jsonResponse = jsonResponse & "], ""total"":" & totalRecords &  "}"
Response.Write jsonResponse



%>