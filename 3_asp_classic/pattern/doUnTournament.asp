<%@ Language="VBScript" %>

<!-- #include File="general.asp" -->

<%
If Request.ServerVariables("REQUEST_METHOD") = "POST" Then

	Dim postID 
	postID = Request.Form("id")

	if postID = "" then
		Response.Write "<span style='color:red'>Operation aborted! <br><br> Validation ERROR -- ID is empty! -- <br><br> Operation aborted!</span>" & "<br>"
		Response.End
	end if
	
	'--------------------------------------------------------------------------------
	'------------------------- VALIDATE #ID# against DATABASE 
	Dim dbaseUnTournamentID
	dbaseUnTournamentID = ExecuteScalar("SELECT id from Tournament where date_end is null")
	
	if (not isnull(dbaseUnTournamentID)) then
		postID = CStr(postID)
		dbaseUnTournamentID = CStr(dbaseUnTournamentID)
		if postID<>dbaseUnTournamentID then
			Response.Write "<span style='color:red'>Operation aborted! <br><br> Validation ERROR -- FormID (" & postID & ") vs Database (" & dbaseUnTournamentID & ") is not equal! -- <br><br> Operation aborted!</span>" & "<br>"
			Response.End
		end if
	else 
		Response.Write "<span style='color:red'>Operation aborted! <br><br> Validation ERROR -- There is no record to UnTournament! -- <br><br> Operation aborted!</span>" & "<br>"
		Response.End
	end if
	
	'--------------------------------------------------------------------------------
	'------------------------- UPDATE DATABASE
	
	'GetRecordset("update Tournament set date_end = ? where id = ?" )
	if UnTournament(dbaseUnTournamentID) then
		Response.Write ":OK:"
	else 
		Response.Write "<span style='color:red'>Operation aborted! <br><br> Error while update the unTournament record -- <br><br> Operation aborted!</span>" & "<br>"
	end if 
	
	
End If
%>