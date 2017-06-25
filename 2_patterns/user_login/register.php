<?php

require_once('general.php');

$db = new dbase();
$db->connect_mysql();


$message = null;

////////////////////////////REGISTER USER TO DBASE

if ($_SERVER['REQUEST_METHOD'] === 'POST') {

    if (!isset($_POST['user_name']) || !isset($_POST['user_password'])){
        $message = "Please write the password and username";
    }
    
    if (empty($_POST['user_name']) || empty($_POST['user_password'])) {
        $message = "Please write the password and username";
    } else {
    
        $pwd_md5 = md5($_POST['user_password']); //convert plain text to md5
        
        //////////////////////////////////
        //add user
        $sql = "INSERT INTO users (user_name, user_password, user_level, date_rec) VALUES (:user_name, :user_password, :user_level, :date_rec)";
        $stmt = $db->getConnection()->prepare($sql);

        $stmt->bindValue(':user_name' , $_POST['user_name']);
        $stmt->bindValue(':user_password' , $pwd_md5);
        $stmt->bindValue(':user_level' , 1);
        $stmt->bindValue(':date_rec' , date("Y-m-d H:i:s"));

        $stmt->execute();


        if ($stmt->errorCode()=="00000")
        {
            echo "<link href='css/bootstrap.min.css' rel='stylesheet'><div class='container'><div class='alert alert-success'>Registration success, you may proceed with login yourself, redirection in 5sec</div></div> <script> setTimeout(function(){ window.location='login.php'; }, 5000); </script>";
            return;
        }
        else {
            $message = "Error while adding the record to dbase.";
        }

        
    }
    
}

////////////////////////////REGISTER USER TO DBASE





?>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
		<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
		<link rel="shortcut icon" href="main.ico" type="image/png"/>
		<link rel="apple-touch-icon" href="main.ico" type="image/png"/>
		
		<title>Register new user</title>
		
		<link href="css/bootstrap.min.css" rel="stylesheet">
		<link href="css/signin.css" rel="stylesheet">
        
        <script type='text/javascript' src='js/jquery.min.js'></script>
		<script type='text/javascript' src='js/bootstrap.min.js'></script>
        <script type='text/javascript' src='js/bootstrap-selector.js'></script>

        <script>
            $(function() {
              
                
            }) //jQuery ends
     

        </script>
	</head>

	<body>

		<div class="container">
                <?php if ($message!=null) { ?>
                    <div class="alert alert-danger"><?php echo $message;?> </div>
                <?php }; ?>
            <div class="form-signin">
				<form id="formREG" role="form" method="post" onsubmit="return validate()">
						<div class='form-group'>
							<label>Username :</label>
							<input id='user_name' name='user_name' class='form-control' placeholder='Username' required>
						</div>
						
						<div class='form-group'>
							<label>Password :</label>
							<input id='user_password' name='user_password' class='form-control' placeholder='Password' required>
						</div>
 
                    
						<div class="modal-footer">
							<button id="bntCancel_formREG" type="button" onclick="location.href = 'login.php'" class="btn btn-default">
								cancel
							</button>
							<button id="bntSave_formREG" class="btn btn-primary" type="submit" name="submit">
								register
							</button>
						</div>
						
				</form>
			</div>
		</div>
        
<?php require_once('footer.php'); ?>
        
	</body>
</html>