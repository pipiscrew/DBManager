<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<head>
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta http-equiv="Content-Type" content="text/html; charset=windows-1253">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Test Pagination</title>

	<link rel="stylesheet" href="assets/datatables/jquery.dataTables.min.css">
    <script language="javascript" type="text/javascript" src="assets/jquery-3.6.0.min.js"></script>
    <script language="javascript" type="text/javascript" src="assets/datatables/jquery.dataTables.min.js"></script>

</head>
<body>
<!-- #include File="general.asp" -->

<table id="grid1" class="display compact nowrap">
	<thead>
		<tr>
			<th>ID</th>
			<th>Company</th>
			<th>Contact</th>
			<th>Address</th>
			<th>Country</th>
			<th>Phone</th>
		</tr>
	</thead>
	<tbody>
		<%
			Response.Charset="UTF-8"
			
			Set rs = GetRecordset("select * from customers")
			Response.Write Schema1RecordsetToTableRows(rs)
			
			If Not rs Is Nothing Then
				rs.ActiveConnection.Close
				Set rs.ActiveConnection = Nothing
			End If
		%>
	</tbody>
</table>

<button onclick="GridSelected()" type="button" class="btn btn-sm btn-success" style="float:right">Selected Rows</button>

<script>
		////////////////// GRID1 INSTANTIATE
		var grid1 = $('#grid1').DataTable({
                paging: true,
        });
		
		grid1.on('click', 'tbody tr', function (e) {
			e.currentTarget.classList.toggle('selected');
		});
		
		function GridSelected(){
			var selectedRows = grid1.rows('.selected').data()
			
			if (selectedRows.length==0)
			{
				alert("Please select rows!")
				return;
			}
			else {
			
				let names = "Selected count : " + selectedRows.length + "\r\n";
				for (var i=0; i < selectedRows.length ;i++){
					names += selectedRows[i][2] + "\r\n"
				}
				
				alert(names);
				//alert(JSON.stringify(selectedRows));
			}
		}
</script>

</body>
</html>