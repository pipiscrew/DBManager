<?php
session_start();

if (!isset($_SESSION["u"])) {
	header("Location: index.html");
	exit ;
}

include ('general.php');

$conn = connect();

///////////////////READ SPEFIC RECORD
if (isset($_GET["id"])) {
	$query = 'SELECT * FROM x WHERE x_id = ' . $_GET["id"];

	$row_counter=0;
	if ($result = $conn->query($query)) {
		$row=array();

	    /* fetch associative array */
	    while ($rowItem = $result->fetch_assoc()) {
	        $row[$row_counter] = $rowItem;
	        $row_counter+=1;
	    }

	    /* free result set */
	    $result->free();
	}
}
///////////////////READ SPEFIC RECORD

$conn -> close();
?>

<script type="text/javascript">

	///////////////////////////////////////////////////////////// EDIT RECORD
	var jArray = <?php echo json_encode($row); ?>;
//console.log(jArray);
	if (jArray) {
		//WARNING THE FIELD NAMES IS CASE SENSITIVE ON ARRAY
		// console.log(jArray);
		//if checkbox - $('[name=visible_cat]').prop('checked', jArray["visible"]);
		$('[name=xsFORM_updateID]').val(jArray[0]["x_id"]);
		$('[name=x_show_val_min]').val(jArray[0]["x_show_val_min"]);
		$('[name=x_show_val_max]').val(jArray[0]["x_show_val_max"]);
		$('[name=x_val]').val(jArray[0]["x_val"]);

	}
	///////////////////////////////////////////////////////////// EDIT RECORD

	//	jquery
	$(function() {
		//	jquery

		$('#btn_xs_details_cancel').on('click', function(e) {
			$("#xs").show();
			$("#xs_details").hide();
		});

		$("#xs_FORM").submit(function(e) {
			e.preventDefault();
			//STOP default action

			$("#loading").height($('body').height());
			$("#loading").show();

			//$("[name=lastupdate]").val(date_now4mysql());

			var postData = $(this).serializeArray();
			var formURL = $(this).attr("action");
			$.ajax({
				url : formURL,
				type : "POST",
				data : postData,
				success : function(data, textStatus, jqXHR) {
					$("#loading").hide();

					if (data.indexOf("ok") == 0) {
						$("#xs").show();
						$("#xs_details").hide();
						load_xs_Records(last_xs_Page);
					} else
					{
							alert("ERROR - record not saved\r\n\r\nPlease try again!");
					}
				},
				error : function(jqXHR, textStatus, errorThrown) {
					$("#loading").hide();
					alert("ERROR");
				}
			});
		});
	});	//	jquery
</script>

<br>

<form id="xs_FORM" role="form" method="post" action="tab_xs_details_save.php">
	<button type='button' id="btn_xs_details_cancel" class="btn btn-default">
		<span class="glyphicon glyphicon-chevron-left"></span> πίσω
	</button>
	<button id="btn_xs_details_save"  class="btn btn-default btn-danger" type="submit" name="submit">
		<span class="glyphicon glyphicon-floppy-disk"></span> αποθήκευση
	</button>
	<br>
	<br>

	<form role="form">

		<div class="row">

			<div class="col-xs-6 col-md-4">
				<div class='form-group'>
					<label>x_show_val_min :</label>
					<input name='x_show_val_min' class='form-control' placeholder='x_show_val_min'>
				</div>
			</div>


			<div class="col-xs-6 col-md-4">
				<div class='form-group'>
					<label>x_show_val_max :</label>
					<input name='x_show_val_max' class='form-control' placeholder='x_show_val_max'>
				</div>
			</div>


			<div class="col-xs-6 col-md-4">
				<div class='form-group'>
					<label>x_val :</label>
					<input name='x_val' class='form-control' placeholder='x_val'>
				</div>
			</div>

		</div>



		<input name="xsFORM_updateID" class="form-control" style="display:none;">

	</form>
</form>
