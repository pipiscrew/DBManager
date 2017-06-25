<?php

//validations + skeleton for JS / CSS
require_once('template_top.php');

?>




		<div class="container" style="margin-top:50px">
            
            <div class="row">
                <div class="col-md-4"></div>
                <div class="col-md-4">
                      <a href="test_child_utc.php" class="btn btn-success btn-lg" style="display:block;margin:10px;">Convert UTC to browser time</a>
                      <a href="logout.php" class="btn btn-primary btn-lg" style="display:block;margin:10px;">Logout</a>
                      <a id="sweet1" class="btn btn-danger btn-lg" style="display:block;margin:10px;">sweetalert2 - cancel</a>
                      <a id="sweet2" class="btn btn-success btn-lg" style="display:block;margin:10px;">sweetalert2 - success</a>
                      <a href="test_child_summernote.php" class="btn btn-success btn-lg" style="display:block;margin:10px;">summernote</a>
                </div>
                <div class="col-md-4"></div>
            </div>
		</div>

<script>
				$('#sweet1').on('click', function(e) {
					e.preventDefault();
					
						swal(
						  'Oops...',
						  'Something went wrong!',
						  'error'
						);
						
				});
				
				$('#sweet2').on('click', function(e) {
					e.preventDefault();
					
						swal(
						  'Record saved',
						  'Comment',
						  'success'
						).then(function() {
							alert('when sweet closed');
						});
						
				});
				
</script>

<?php

//footer
require_once('footer.php');

?>