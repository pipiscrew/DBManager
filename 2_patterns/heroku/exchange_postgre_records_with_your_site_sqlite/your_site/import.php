<?php

require_once("general.php");

// set infinite for timeout
set_time_limit(0);

// make sure to keep alive the script when a client disconnect.
ignore_user_abort(true);

// set default timezone
date_default_timezone_set('UTC');

// define custom error handler
set_error_handler("myErrorHandler");

//used later for success mail
$log = '';

// check for proper post vars
if ($_SERVER["REQUEST_METHOD"] != "POST" || !isset($_POST['hostname']) || !isset($_POST['user']) || !isset($_POST['password']) || !isset($_POST['database'])){
	header($_SERVER['SERVER_PROTOCOL'] . ' 500 Internal Server Error', true, 500);
	exit;	
}

// validation against heroku curl referer
$ref = $_SERVER['HTTP_REFERER'];
$ref_details = parse_url($ref);


if (strtolower($ref_details['host']) != 'x.heroku.com'){
	header($_SERVER['SERVER_PROTOCOL'] . ' 500 Internal Server Error', true, 500);
	exit;	
}

// assign post vars to variables
$postgre_hostname = $_POST['hostname'];
$postgre_user = $_POST['user'];
$postgre_password = $_POST['password'];
$postgre_database = $_POST['database']; 
    
    
write_log("\r\n*** " . date('Y-m-d H:i:s') . " ***\r\n\thostname : $postgre_hostname\r\n" . "\tuser : $postgre_user\r\n" . "\tdatabase : $postgre_database\r\n" );



//hold the start time
$time_start = microtime(true);

try {
	$postgre = new dbase();
	$postgre->connect_postgre($postgre_hostname, $postgre_user, $postgre_password, $postgre_database);	
} catch(Exception $e){
	trigger_error("couldnt connect to Postgre " . $e->getMessage(), E_USER_ERROR);
}


write_log("\tPostgre - connected!\r\n");

try {
	$sqlite  = new dbase();
	$sqlite->connect_sqlite();
} catch(Exception $e){
	trigger_error("couldnt connect to sqlite " . $e->getMessage(), E_USER_ERROR);
}

write_log("\tsqlite - connected!\r\n");


// send response to heroku and shutdown the connection [START]
// 			src - https://stackoverflow.com/a/15273676/1320686
ob_start();
echo 'thanks.heroku'; // send the response
header('Connection: close');
header('Content-Length: '.ob_get_length());
ob_end_flush();
ob_flush();
flush();
// send response to heroku and shutdown the connection [END]

write_log("\theroku connection closed...\r\n");


//mirror 1st table
$q ='select provider_id, provider_url, provider_enabled, provider_once_per_day, provider_last_run, provider_headline from providers';
mirror_table($q, 'providers', true);
write_log("\tproviders - mirrored!");

//mirror 2nd table
write_log("\tsource feeds ".$postgre->getScalar('select count(*) from feeds', null) ."\r\n");
write_log("\tdestination feeds before ".$sqlite->getScalar('select count(*) from feeds', null) ."\r\n");
$q ='select feed_provider_id, feed_title, feed_url, feed_date, feed_timestamp from feeds';
mirror_table($q, 'feeds', false);
write_log("\tfeeds - mirrored!\r\n");
write_log("\tdestination feeds after ".$sqlite->getScalar('select count(*) from feeds', null) ."\r\n");



// hold end time
$time_end = microtime(true);

//dividing with 60 will give the execution time in minutes other wise seconds
$execution_time = ($time_end - $time_start)/60;

//execution time of the script
write_log("\t*Total Execution Time: ".$execution_time." mins");



//send success mail to admin
sendMail('x@x.com', "Heroku sync success!",$log);


//once all imported to site.com feeds.db dbase retrigger heroku cron
//with del_recs var to delete the recotds at postgre
call_heroku_cron();

function mirror_table($src_query, $dest_table, $truncate_table)
{
    global $postgre, $sqlite;
        
    //ask for source rows
    $src_rows = $postgre->getSet($src_query, null);
    

    //delete the old rows from destination table
    if ($truncate_table)
    	$sqlite->executeSQL("DELETE FROM {$dest_table}", null);
    
    //get source column names from first row
    $insert_cols="";
    $insert_vals="";
    
    $src_cols = array();
    
    foreach ($src_rows[0] AS $key => $value)
    {
        $insert_cols.="{$key}, ";
        $insert_vals.=":{$key}, ";
        $src_cols[] = $key;
    }
    
    //remove ", "
    $insert_cols = substr($insert_cols, 0, strlen($insert_cols)-2);
    $insert_vals = substr($insert_vals, 0, strlen($insert_vals)-2);
    
    //construct the SQL
    $insert_sql = "INSERT INTO {$dest_table} ({$insert_cols}) VALUES ({$insert_vals})";
    
    //prepare the SQL
    if ($stmt = $sqlite->getConnection()->prepare($insert_sql)){
    	
        //for each source row
        foreach($src_rows as $row) {
        	
            //for each field in the row
            foreach($src_cols as $fieldname)
                $stmt->bindValue(":{$fieldname}" , (string) $row["{$fieldname}"]);
                
            //execute the prepared statement
            $stmt->execute();	
            
            if($stmt->errorCode() != '00000'){
                echo $stmt->errorCode();
                exit;
            }
        }
    }
}


function write_log($txt){
	global $log;
	
	//used later for success mail
	$log.=$txt . '<br>';
	
	$myfile = fopen('log.txt', 'a');
	fwrite($myfile, "$txt\r\n");
	fclose($myfile);
}

function sendMail($recipient_mail, $subject, $body)
{
    $headers = "From: x@x.com\r\n";
    $headers .= "MIME-Version: 1.0\r\n";
    $headers .= "Content-Type: text/html; charset=utf-8\r\n";
     
    $message = '<html><body>';
    $message .= $body;
    $message .= '</body></html>';
 
    // line with trick - http://www.xpertdeveloper.com/2013/05/set-unicode-character-in-email-subject-php/
    $updated_subject = "=?UTF-8?B?" . base64_encode($subject) . "?=";
 
    if (mail($recipient_mail, $updated_subject, $message, $headers)) {
      return true;
    } else {
      return false;
    }
}


// A user-defined error handler function
function myErrorHandler($errno, $errstr, $errfile, $errline)
{
    if (!(error_reporting() & $errno)) {
        // This error code is not included in error_reporting, so let it fall
        // through to the standard PHP error handler
        return false;
    }
    $mail_body="";
    switch ($errno) {
    case E_USER_ERROR:
        $mail_body = "<b>My ERROR</b> [$errno] $errstr<br />\n";
        $mail_body .= "  Fatal error on line $errline in file $errfile";
        $mail_body .= ", PHP " . PHP_VERSION . " (" . PHP_OS . ")<br />\n";
        $mail_body .= "Aborting...<br />\n";
        //exit(1);
        break;
    case E_USER_WARNING:
        $mail_body = "<b>My WARNING</b> [$errno] $errstr<br />\n";
        break;
    case E_USER_NOTICE:
        $mail_body = "<b>My NOTICE</b> [$errno] $errstr<br />\n";
        break;
    default:
        $mail_body = "Unknown error type: [$errno] $errstr<br />\n";
        break;
    }
    sendMail("x@x.com", "Heroku sync Error", $mail_body);
    
    /* Execute PHP internal error handler */
    return false;
}

// retrigger the cron at heroku w del_recs post var
function call_heroku_cron()
{
	$arr = array('del_recs' => "1");
	//https://stackoverflow.com/a/10189346/1320686
	$post_fields = "";
	foreach($arr as $key=>$field){
	    $post_fields .= $key . "=" . $field . "&";
	}
	rtrim($post_fields, '&');
	 
	$ch = curl_init('https://x.herokuapp.com/cron.php');
	curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
	curl_setopt($ch, CURLOPT_REFERER, 'https://x.com');
	curl_setopt($ch, CURLOPT_POST, count($arr));
	curl_setopt($ch, CURLOPT_POSTFIELDS, $post_fields);
	curl_setopt($ch, CURLOPT_FOLLOWLOCATION, true);
	//https://stackoverflow.com/a/11066378/1320686
	curl_setopt($ch, CURLOPT_CONNECTTIMEOUT ,0); // The number of seconds to wait while trying to connect. Use 0 to wait indefinitely.
	curl_setopt($ch, CURLOPT_TIMEOUT, 0); // The maximum number of seconds to allow cURL functions to execute.
	curl_setopt($ch, CURLOPT_USERAGENT, 'Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.6 (KHTML, like Gecko) Chrome/16.0.897.0 Safari/535.6');

	curl_exec ($ch);	
}