<?php

set_error_handler("myErrorHandler");

ini_set('SMTP', 'x.x.com'); 
ini_set('smtp_port', 25); 

//set infinite for timeout
set_time_limit(0);

require_once "general.php";

//hold the start time
$time_start = microtime(true);

$oracle = new dbase();
$oracle->connect_oracle();

$mysql = new dbase();
$mysql->connect_mysql();


//construct today for Oracle
$today = date("d-M-Y");     

$three_months_back = date('d-M-Y', strtotime("-90 days"));



/* CLOSED */
$q ="select * from OWNER.TABLE where x in ('1a','2x') and ((close_date between TO_DATE('{$three_months_back}','dd-MON-yy') and TO_DATE('{$today}','dd-MON-yy')) or (update_date between TO_DATE('{$three_months_back}','dd-MON-yy') and TO_DATE('{$today}','dd-MON-yy') and status='completed'))";

mirror_table($q, "table1");




/* AGED */
$aged = "select * from OWNER.TABLE where update_date < SYSDATE-10 AND (x in ('1a','2x') and status not in ('completed','aborted'))";

mirror_table($aged, "table2");



// hold end time
$time_end = microtime(true);

//dividing with 60 will give the execution time in minutes other wise seconds
$execution_time = ($time_end - $time_start)/60;

//execution time of the script
$mail_body =  '<b>Total Execution Time:</b> '.$execution_time.' mins</br></br>';

//send mail
sendMail("x@x.com", "Oracle Report", $mail_body);



function mirror_table($src_query, $dest_table)
{
    global $oracle, $mysql;
        
    //ask for source rows
    $src_rows = $oracle->getSet($src_query, null);
    
    //delete the old rows from destination table
    $mysql->executeSQL("TRUNCATE {$dest_table}", null);
    
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
    if ($stmt = $mysql->getConnection()->prepare($insert_sql)){

        //for each source row
        foreach($src_rows as $row) {

            //for each field in the row
            foreach($src_cols as $fieldname)
                $stmt->bindValue(":{$fieldname}" , (string) $row["{$fieldname}"]);

            //execute the prepared statement
            $stmt->execute();	

            if($stmt->errorCode() != "00000"){
                echo $stmt->errorCode();
                exit;
            }
        }
    }
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

    sendMail("x@x.com", "Oracle Report Error", $mail_body);
    
    /* Execute PHP internal error handler */
    return false;
}
