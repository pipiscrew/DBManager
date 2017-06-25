<?php

//validations + skeleton for JS / CSS
require_once('template_top.php');

?>

<!-- include summernote css/js https://github.com/summernote/summernote -->
<link href="css/summernote.css" rel="stylesheet">
<script src="js/summernote.min.js"></script>

<script>

    $(function() {

        $('#summernote').summernote({height: 300});
        
        //get HTML from dbase (PHP code ommited)
        //$('#summernote').summernote('code', jArray[0]["the_text"]);
	});
	
	
    function validate(){
	    //pass the summernote html code to, hidden textarea
	    $('#the_text').html($('#summernote').summernote('code'));
    }
</script>


<form id="plaintexts_FORM" role="form" method="post" action="x_save.php" onsubmit="return validate()">
        <div id='summernote'></div>
        <textarea id='the_text' name='the_text' hidden></textarea>
</form>


<?php

//footer
require_once('footer.php');

?>