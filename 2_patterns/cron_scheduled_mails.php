/*
1-
	write the script on a php file

2-
	write on top of the script + run it :
		echo getcwd(); //returns the full path, we need it for cron execution path
		exit;

3-
	find the times http://wwp.greenwichmeantime.com/to/mst/index.htm
	
	
example :
php -q /xx/xx/public_html/cron_scheduled_mails.php

//the query field as :
`service_starts` date DEFAULT NULL

*/



////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//as a class by zinsou - https://www.phpclasses.org/package/10351-PHP-Execute-action-when-a-given-date-is-reached.html

<?php


//DB
require_once ('../config.php');
require_once ('../config_general.php');
 
$db = connect();

$rows = getSet($db, "select employee_mail, customer_mail, offer_company_name, DATE_FORMAT(service_starts,'%d/%m/%Y') as service_starts_formatted, DATE_FORMAT(service_ends,'%d/%m/%Y') as service_ends_formatted,reply_mail as user_mail from offers as tableA
where is_lead=0 and is_paid=1 and tableA.service_starts is not null and tableA.service_starts = CURDATE()",null);

//log to dbase the action
write_log($db,5, "Scheduled Task - ".date("Y-m-d H:i:s")."  - ServiceStart found : ".sizeof($rows));

if (sizeof($rows)>0)
{
	$report="";
	foreach($rows as $row) {
		$subject = "Test Inc. - {$row['offer_company_name']} Ad Service Starts Today";
		
		$body = "The contract starts ".$row["service_starts_formatted"]." ends " .$row["service_ends_formatted"]. " for the client ".$row['offer_company_name']."<br>";
		
		send_mail_to_user($row['employee_mail'], $row['customer_mail'] , $subject, $body);
	}

}
	
echo sizeof($rows);


function sendMail($employee_mail, $recipient_mail, $subject, $body)
{
	//$headers = "From: x@x.com <proposal@x.com>\r\nReply-to: {$employee_mail}\r\n";
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
