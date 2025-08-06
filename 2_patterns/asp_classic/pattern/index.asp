<html>
<head>
<meta http-equiv="Content-Language" content>
<meta http-equiv="X-UA-Compatible" content="IE=11">
<meta http-equiv="Content-Type" content="text/html; charset=windows-1253">
<title>Tournament Period</title>

<style>
    body {
      font-family: Arial, sans-serif;
      padding: 40px;
      background-color: #f4f4f4;
    }
    h1 {
      color: #333;
    }
    .section {
      margin-bottom: 20px;
    }
    button {
      margin-right: 10px;
      padding: 10px 20px;
      font-size: 16px;
    }
</style>
<!-- #include File="general.asp" -->

<%

Dim rsCheck, lastDate, lastDateId

'lastDate = ExecuteScalar("select date_start from Tournament where date_end is null")
set rsCheck = GetRecordset("select * from Tournament where date_end is null")

'if IsNull(rsCheck("date_start")) then
If rsCheck.EOF Then
	response.write "IS NULL"
else
	lastDateId = rsCheck("id")
	lastDate = rsCheck("date_start")
	response.write "Tournament άρχισε : " & lastDate & " with ID : " & lastDateId
end if


%>

</head>
<body>
<h1>Tournament Period</h1>

<form id="daForm" method="post"  <% If not IsEmpty(lastDateId) then Response.Write "action='doUnTournament.asp'" else Response.Write "action='doTournament.asp'" end if %> >
	<div class="section">
		<h3>Επιλογές:</h3>
	<%
		Response.Write GetTeams(GetRecordset("select * from Tournament_Teams order by ordinal"))
	%>
	</div>


	<div class="section">
		<h3>Επιλογή Λειτουργίας:</h3>
		<label><input type="radio" name="mode" value="auto"> Αυτόματο</label><br>
		<label><input type="radio" name="mode" value="manual"> Χειροκίνητο</label>
	</div>
	 
	<div class="section">
		<%
		if IsEmpty(lastDateId) then
		%>
			<button onclick="startTournament(event)" style="background-color: green; color: white;">Έναρξη Tournament</button>
		<% else %>
			<button onclick="endTournament(event)" style="background-color: red; color: black;">Διακοπή Tournament</button>
		<% end if %>
	</div>
	
	<input type="hidden" name="id" value="<%=lastDateId%>">
</form>
<script>
    function startTournament(event) {
		event.preventDefault();
		
		if (!ValidateCheckBoxes()){
			alert("Παρακαλώ επιλέξτε Tournament");
			return;
		}
		else if (!ValidateOptionButtons()){
			alert("Παρακαλώ επιλέξτε λειτουργία");
			return;
		}

		document.getElementById("daForm").submit();
    }
 
    function endTournament() {
      event.preventDefault();
	  
	  if (confirm("Είστε σίγουροι για την διακοπή Tournament ;"))
	  {
		document.getElementById("daForm").submit();
	  }
	  else 
		alert("N");
    }
	
	function ValidateCheckBoxes(){
		var checkboxes = document.querySelectorAll('input[type="checkbox"]');
		var isCheckboxSelected = false;

		for (var i = 0; i < checkboxes.length; i++) {
			if (checkboxes[i].checked) {
				isCheckboxSelected = true;
				break;
			}
		}

		return isCheckboxSelected;
	}
	
	function ValidateOptionButtons(){
		var radios = document.getElementsByName('mode');
		
		for (var i = 0; i < radios.length; i++) {
			if (radios[i].checked) {
				return true;
			}
		}
		return false;
	}

</script>
</body>
</html>