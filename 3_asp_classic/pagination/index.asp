<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<head>  
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta http-equiv="Content-Type" content="text/html; charset=windows-1253">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Test Inline</title>

	<link rel="stylesheet" href="assets/datatables/jquery.dataTables.min.css">
    <script language="javascript" type="text/javascript" src="assets/jquery-3.6.0.min.js"></script>
    <script language="javascript" type="text/javascript" src="assets/datatables/jquery.dataTables.min.js"></script>

</head>
<body>
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
	<tbody></tbody>
</table>
<button onclick="GridSelected()" type="button" class="btn btn-sm btn-success" style="float:right">Selected Rows</button>
<button onclick="GridOneExport()" type="button" class="btn btn-sm btn-success" style="float:left">Download Page Rows</button>
<button onclick="GridOneExportAll()" type="button" class="btn btn-sm btn-success" style="float:left">Download All Rows</button>
<script>
	////////////////// GRID1 INSTANTIATE
	var grid1 = $('#grid1').DataTable({
			processing: true,
			serverSide: true,
			pageLength: 10,
			ajax: {
			url: 'pagination.asp',
			type: 'POST', // Specify the request type as POST
			data: function (d) {
				// You can modify the data sent to the server here if needed 'd' contains the default parameters sent by DataTables, you can add additional parameters if needed
				d.postalCode = 'ADMIN'; //'1010';
			}}
	});
	
	//////////////////////////////////////////////// SELECTED ROWS
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
	//////////////////////////////////////////////// SELECTED ROWS
	
	//////////////////////////////////////////////// DOWNLOAD
	function GridOneExport(){
		DownloadGridRows(grid1, "1");
	}
		
	function DownloadGridRows(grid, gridNo){
		var txt = "";
		
		grid.rows().every(function(rowIdx, tableLoop, rowLoop) {
			var data = this.data();

			for (var i = 1; i < data.length; i++) {
				txt = txt + data[i] + "\t";
			}
			
			txt = txt + "\n"
		});
		
		downloadTextFile(getTodayFormatted()+"-grid" + gridNo + ".xls", txt);
	}
	
	function downloadTextFile(filename, content) {
		var form = document.createElement('form');
		form.method = 'POST';
		form.acceptCharset = 'UTF-8';  //req for utf8
		form.action = 'download.asp';

		var input = document.createElement('input');
		input.type = 'hidden';
		input.name = 'fileContent';
		input.value = content;
		console.log(content);
		
		var input2 = document.createElement('input');
		input2.type = 'hidden';
		input2.name = 'fileName';
		input2.value = filename;

		form.appendChild(input);
		form.appendChild(input2);

		document.body.appendChild(form);

		form.submit();

		document.body.removeChild(form);
	}
	
	function getTodayFormatted() {
		var today = new Date();
		var year = today.getFullYear();
		var month = today.getMonth() + 1; // Months are zero-based
		var day = today.getDate();

		// Ensure month and day are two digits
		month = (month < 10 ? '0' : '') + month;
		day = (day < 10 ? '0' : '') + day;

		return year + month + day;
	}
	
	function GridOneExportAll(){
		var form = document.createElement('form');
		form.method = 'POST';
		form.action = 'downloadAll.asp';

		
		var input = document.createElement('input');
		input.type = 'hidden';
		input.name = 'fileName';
		input.value = getTodayFormatted()+"-AllRows.xls";

		var input2 = document.createElement('input');
		input2.type = 'hidden';
		input2.name = 'postalCode';
		input2.value = "<%="ADMIN"%>";

		form.appendChild(input);
		form.appendChild(input2);
		

		document.body.appendChild(form);

		form.submit();

		document.body.removeChild(form);
	}
	//////////////////////////////////////////////// DOWNLOAD
</script>

</body>
</html>