<?php
    session_start(); //to ensure you are using same session

    //LOGOUT
	session_destroy(); //destroy the session
	header("location: login.php"); //to redirect back to login
	exit();

?>