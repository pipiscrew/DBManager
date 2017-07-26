<?php
	require_once('general.php');
	
	date_default_timezone_set("UTC");

	$db = connect();
	
	$no_rows = getScalar($db,"select count(*) from feeds", null);

?>

<html>

<head>
<title>Most Wanted News! <?= $no_rows ?> </title>

<link rel="stylesheet" type="text/css" href='rssblack.css' />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="description"  content="PipisCrew Your feeds roll on your wall" />
<meta name="robots" content="noindex">

</head>

<body>

<div style='float:right; width: 70px;'>
 <div style='position: fixed'>
  <a style='font-size: x-small' href='http://pipiscrew.com' target="_blank">Homepage</a> 
 </div>
</div>

<?php




$template = "<a href='##url##' target='_blank'>##title##</a>
				<hr>
				<p class='dateaut'>##subtitle##</p>
				<br>";


if (isset($_GET['d'])){
	$days_back = intval($_GET['d']);

	if ($days_back > 6)
	{
		echo "Sorry max value is 6";
		exit;
	}
	
	$past_day = strtotime(date("Y-m-d")."UTC -{$days_back} days");
}
else
	$past_day = strtotime(date("Y-m-d")."UTC -1 days");

$feeds = getSet($db,"select feed_title, feed_url, (provider_headline || ' - ' || feed_date) as subtitle from feeds 
left join providers on providers.provider_id = feeds.feed_provider_id 
 where feed_timestamp::integer > {$past_day} 
 order by feed_timestamp desc", null);

	
   foreach ($feeds as $value){
		$a = str_replace('##url##', $value['feed_url'], $template);
		$a = str_replace('##title##', $value['feed_title'], $a);
		$a = str_replace('##subtitle##', $value['subtitle'], $a);

		echo $a;
    }


?>


</body>
</html>