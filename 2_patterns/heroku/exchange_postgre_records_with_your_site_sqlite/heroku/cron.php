<?php

require_once('general.php');

// set infinite for timeout
set_time_limit(0);

// make sure to keep alive the script when a client disconnect.
ignore_user_abort(true);

// set default timezone
date_default_timezone_set('UTC');



$db = connect();

$no_rows = getScalar($db,"select count(*) from feeds", null);
$no_rows += getScalar($db,"select count(*) from provider_logs", null);


//if the total count of the tables exceed the 9000 cut them all till yesterday
if ( $no_rows > 9000 ) {
	
	if ( isset($_POST['del_recs']) && $_POST['del_recs'] == '1' ) {
		$past_day = strtotime(date("Y-m-d")."UTC -1 days");
		executeSQL($db, "TRUNCATE provider_logs", null);
		executeSQL($db, "delete from feeds where feed_timestamp::integer < {$past_day}", null);
	}
	else 
	{
		$dbopts           = parse_url(getenv('DATABASE_URL'));

		$arr = array(
		    'hostname' =>  $dbopts["host"],
		    'user' => $dbopts["user"],
		    'password' => $dbopts["pass"],
		    'database' => ltrim($dbopts["path"],'/'));
		    
		//https://stackoverflow.com/a/10189346/1320686
		$post_fields = "";
		foreach($arr as $key=>$field){
		    $post_fields .= $key . "=" . $field . "&";
		}
		rtrim($post_fields, '&');

		$ch = curl_init('https://x.com/import.php');
		curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
		curl_setopt($ch, CURLOPT_REFERER, 'http://x.heroku.com');
		curl_setopt($ch, CURLOPT_POST, count($arr));
		curl_setopt($ch, CURLOPT_POSTFIELDS, $post_fields);
		curl_setopt($ch, CURLOPT_FOLLOWLOCATION, true);
		//https://stackoverflow.com/a/11066378/1320686
		curl_setopt($ch, CURLOPT_CONNECTTIMEOUT ,0); // The number of seconds to wait while trying to connect. Use 0 to wait indefinitely.
		curl_setopt($ch, CURLOPT_TIMEOUT, 0); // The maximum number of seconds to allow cURL functions to execute.
		curl_setopt($ch, CURLOPT_USERAGENT, 'Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.6 (KHTML, like Gecko) Chrome/16.0.897.0 Safari/535.6');

		$data = curl_exec ($ch);
		//echo $data; //use it for debug
		//curl_close ($ch);
		exit;
	}
}

// send response to pipiscrew cron and shutdown the connection [START]
// 			src - https://stackoverflow.com/a/15273676/1320686
ob_start();
echo 'thanks.pipiscrew'; // send the response
header('Connection: close');
header('Content-Length: '.ob_get_length());
ob_end_flush();
ob_flush();
flush();
// send response to pipiscrew cron and shutdown the connection [END]





//-----------------------------------[CRON JOB]-----------------------------------
$providers = getSet($db, "select * from providers where provider_enabled=1", null);

foreach($providers as $provider) {
	//app parse feed logic here (omitted)
}
?>