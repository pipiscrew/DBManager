<%@ Language="VBScript" %>

<!-- #include File="general.asp" -->

<%
If Request.ServerVariables("REQUEST_METHOD") = "POST" Then

    Dim key, val
	Dim whereTeamsCodes
	whereTeamsCodes = "'0'"
	
	Dim mode
	
    Response.Write("<h2>Submitted Form Data:</h2>")
    Response.Write("<ul>")
    

    For Each key In Request.Form
		if Request.Form(key) = "on" then ' collect the checkboxes names (equals the dbase table #code# field)
			whereTeamsCodes = whereTeamsCodes & ",'" & key & "'"
		elseif key = "mode" then
			mode = Request.Form(key)
		else
			Response.Write("<li>" & key & ": " & Request.Form(key) & "</li>")
		end if 
    Next
    
    Response.Write("</ul>")
	Response.Write whereTeamsCodes & "<br>"
	Response.Write mode & "<br>"
	Response.Write CountString(whereTeamsCodes, ",") & "<br>"
	
	'--------------------------------------------------------------------------------
	'------------------------- VALIDATE WITH #Tournament PERIOD# EXISTS
	Dim validation4existingTournament 
	validation4existingTournament = ExecuteScalar("select id from Tournament where date_end is null")
	
	if (not isnull(validation4existingTournament)) then
		Response.Write "<span style='color:red'>Operation aborted! <br><br> Validation ERROR -- Tournament PERIOD ALREADY EXISTS TO DATABASE! -- <br><br> Operation aborted!</span>" & "<br>"
		Response.End
	end if
	

	'--------------------------------------------------------------------------------
	'------------------------- VALIDATE the posted TeamS if exist to dbase
	if (CountString(whereTeamsCodes, ",") = ExecuteScalar("select count(id) from Tournament_Teams where code in (" & whereTeamsCodes & ")")) then
		Response.Write "equals" & "<br>"
	else 
		Response.Write "<span style='color:red'>Operation aborted! <br><br> Validation ERROR the TeamS posted not equal with dbase records! <br><br> Operation aborted!</span>" & "<br>"
		Response.End
	end if
	
	
	'--------------------------------------------------------------------------------
	'------------------------- INSERT to Tournament
	Dim newTournamentRecordId 
	newTournamentRecordId = AddTeam("pipiscrew")

	if isnull(newTournamentRecordId) or isempty(newTournamentRecordId) then
		Response.Write "<span style='color:red'>Operation aborted! <br><br> newTournamentRecordId is empty! <br><br> Operation aborted!</span>" & "<br>"
		Response.End
	end if 
	
	
	'--------------------------------------------------------------------------------
	'------------------------- INSERT to Tournament_team_TIE
	Dim TournamentTeamTieCount
	set TournamentTeamTieCount = GetRecordset("insert into Tournament_team_tie (Tournament_id, Team_id) select " & newTournamentRecordId & ", id from Tournament_Teams where code in (" & whereTeamsCodes & ")")
	
	Response.Write ":OK:"
	

'useful	
'SELECT t.id,f.date_start, p.title from Tournament_team_tie t
'left join Tournament f on f.id= t.Tournament_id
'left join Tournament_Teams p on p.id= t.Team_id
'
'SELECT * from Tournament
'SELECT * from Tournament_team_tie

	
Else
    Response.Write("No data submitted.")
End If
%>
