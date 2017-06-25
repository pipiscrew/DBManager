<?php

//validations
require_once('template_top.php');

?>

<div class="container" style="margin-top:50px">
	<code>
	The user timezone everytime user logins stored to *SESSION* variable (login.php).<br><br>

	On all tables we have a datetime field, called *date_rec* and store the UTC datetime<br>
	The template_top.php, set by default the timezone to UTC. <br><br>

	example of save <br>
	$stmt->bindValue(':date_rec' , date("Y-m-d H:i:s"));<br><br>

	--<br><br>

	Later on using MySQL function to convert to user timezone via :<br><br>
	**1**<br>
	$find_sql = "SELECT plaintext_id, categories.category_name as category, language_name as language_id, plaintext_title<br>
	, date_rec + INTERVAL -{$_SESSION['timezone']} MINUTE as date_rec FROM `plaintexts`";<br><br>
	  
	 ///////OR<br><br>
	  
	**2**<br>
	select question_id, categories.category_name as category_name,(date_rec + INTERVAL -:browsertime MINUTE) as date_rec from questions limit :offset,:limit<br><br>
	 
	//////////////////////////////////////BIND PARAMS<br>
	$stmt->bindValue(':offset' , intval($offset), PDO::PARAM_INT);<br>
	$stmt->bindValue(':limit' , intval($limit), PDO::PARAM_INT);<br>
	$stmt->bindValue(':browsertime' , intval($_SESSION['timezone']), PDO::PARAM_INT);<br><br>
	     
	//////////////////////////////////////FETCH ROWS<br>
	$stmt->execute();<br><br>
	 
	$rows = $stmt->fetchAll();<br><br>
	  
	 ///////OR<br><br>
	 
	**3**<br>
	$find_sql = "SELECT plaintext_id, (date_rec + INTERVAL -? MINUTE) as date_rec FROM `plaintexts` where plaintext_id = ?";<br><br>
	 
	$row = $db->getSet($find_sql, array($_SESSION['timezone'], $_GET["id"]));

	</code>
</div>

<?php

//validations
require_once('footer.php');

?>