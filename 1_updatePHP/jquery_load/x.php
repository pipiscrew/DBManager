<button id="btn_x_new" type="button" class="btn btn-primary">	New</button>
<button id="btn_x_edit" type="button" class="btn btn-primary">	Edit</button>


//check more for effects - https://www.w3schools.com/jquery/jquery_fade.asp

<script>
		//new record
		$('#btn_x_new').on('click', function(e) {
			$("#loading").height($('body').height());
			$("#loading").show();

			$("#x_details").load('x_details.php', function() {
				$("#loading").hide();
				$("#x").hide(); //the grid div
				
				$("#x_details").show(); //the table - add new elements
			});
		});
	
		//edit record
		$('#btn_x_edit').on('click', function(e) {
			var rowData = getSelected('records_xs_TBL');
			if (rowData == null) {
				alert("Please choose record!");
				return;
			}

			$("#loading").height($('body').height());
			$("#loading").show();
			
			
			//**the power**
			$("#xs_details").load('x_details.php?id=' + rowData[0], function() {
				$("#loading").hide();
				$("#x").hide(); //the grid div
				$("#x_details").show();//the table - edit elements
			});
		});
</script>
