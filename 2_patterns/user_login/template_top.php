<?php
//on all pages include this file

	session_start();
    date_default_timezone_set("UTC");

	if (!isset($_SESSION["id"])) {
        session_destroy(); //destroy the session
		header("Location: login.php");
		exit ;
	}
	else {
		
		//user everyday has to login
		if ($_SESSION["login_expiration"] != date("Y-m-d"))
		{	
            session_destroy(); //destroy the session
            header("Location: login.php");
			exit ;
		}
        
        if (!isset($_SESSION['timezone']))
		{	
            session_destroy(); //destroy the session
            header("Location: login.php");
			exit ;
		}  
        
        if ($_SESSION['level'] == 1)
		{	
            session_destroy(); //destroy the session
            header("Location: login.php?q=Invalid User Level for Admin Panel");
			exit ;
		} 
        
	}
?>


<!DOCTYPE html>
<html>
	<head>
	<!--http://glyphicons.bootstrapcheatsheets.com/-->
		<meta charset="UTF-8">
		<title>Portal</title>
		<meta content='width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no' name='viewport'>
        
		<!-- bootstrap -->
		<link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />

        <!-- bootstrap-table -->
        <link rel="stylesheet" href="css/bootstrap-table.min.css">
        
        <!-- sweetalert2 -->
        <link rel="stylesheet" href="css/sweetalert2.min.css">

		<!-- jQuery 2.0.2 -->
		<script src="js/jquery.min.js" type="text/javascript"></script>
		
		<!-- bootstrap http://getbootstrap.com -->
		<script src="js/bootstrap.min.js" type="text/javascript"></script>
        
        <!-- bootstrap-table https://github.com/wenzhixin/bootstrap-table -->
        <script src="js/bootstrap-table.min.js"></script>
        
        <!-- sweetalert2 https://limonte.github.io/sweetalert2 -->
        <script src="js/sweetalert2.min.js"></script>
        
		<style>
			/*bootstrap-table selected row*/
			.fixed-table-container tbody tr.selected td	{ background-color: #B0BED9; }
			
			/*indicator*/
			.modal-backdrop { opacity: 0.7;	filter: alpha(opacity=70);	background: #fff; z-index: 2;}
			div.loading { position: fixed; margin: auto; top: 0; right: 0; bottom: 0; left: 0; width: 200px; height: 30px; z-index: 3; }
		</style>
        
        <script>
			//indicator 
			var loading = $('<div class="modal-backdrop"></div><div class="progress progress-striped active loading"><div class="progress-bar" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%">');
        </script>
        
	</head>
	
	<body>
	
