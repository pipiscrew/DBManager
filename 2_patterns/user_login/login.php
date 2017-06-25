<?php
    session_start();
    date_default_timezone_set("UTC");

    require_once('general.php');

    $db = new dbase();
    $db->connect_mysql();


    $message = null;

    ////////////////////////////LOGIN USER

    if ($_SERVER['REQUEST_METHOD'] === 'POST') {

        $message=null;
        
        if (!isset($_POST["u"]) && $_POST["p"] && $_POST["t"])
        {
            $message = "Please fill username and password.";
        }
        
        if (empty($_POST['u']) || empty($_POST['p']) || empty($_POST['t'])) {
            $message = "Please fill username and password.";
        }
        else {
            
            $user_timezone = intval($_POST["t"]);

            $pwd_md5 = md5($_POST["p"]); //convert plain text to md5

            //get the dbase password for this mail
            $r = $db->getRow("select user_id,user_level,user_name from users where user_name=? and user_password=?",array($_POST['u'], $pwd_md5));

            //^if record exists
            if ($r){
                    $_SESSION['id'] = $r["user_id"];
                    $_SESSION['level'] = (int) $r["user_level"]; //1=USER 2=ADMIN
                    $_SESSION['name'] = $r["user_name"];
                    $_SESSION['login_expiration'] = date("Y-m-d");
                    $_SESSION['timezone'] = (string) $user_timezone;

                    if ($_SESSION['level']==1)
                        header("Location: user.php");
                    else 
                        header("Location: index.php");

            } else {
                $message = "Couldnt find a user for the login combination provided.";
            }
        }

    }

?>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
		<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
		<link rel="shortcut icon" href="main.ico" type="image/png"/>
		<link rel="apple-touch-icon" href="main.ico" type="image/png"/>
		
		<title>User Login</title>
		
		<link href="css/bootstrap.min.css" rel="stylesheet">
		<link href="css/signin.css" rel="stylesheet">

	</head>

	<body>

		<div class="container">
            <?php if ($message!=null) { ?>
                <div class="alert alert-danger"><?php echo $message;?> </div>
            <?php }; ?>

            <button class="btn btn-sm btn-success" onclick="location.href = 'register.php'" style="float:right">Register new user</button>
            
			<div class="form-signin">
				<form class="form-signin" method="post">
					<h2 class="form-signin-heading">Login</h2>
					<input name="u" type="text" class="form-control" placeholder="username" autofocus required>
					<input name="p" type="password" class="form-control" placeholder="password" required>
                    <input name="t" id="t" hidden>
					
					<button class="btn btn-lg btn-primary btn-block" type="submit" name="submit" id="login">
						login
					</button>
				</form>
			</div>
		</div>
        
        <script>
            //set user timezone to input
            document.getElementById('t').value = new Date().getTimezoneOffset();
        </script>
        
<?php require_once('footer.php'); ?>
	</body>
</html>