<?php
date_default_timezone_set("UTC");

// include your code to connect to DB.
require_once ('general.php');


$table_columns = array(
'feed_title',
'feed_url',
'feed_date'

);


$conn     = new dbase();

$conn->connect_sqlite();


if (!is_numeric($_GET['limit']) || !is_numeric($_GET['offset']))
{
	echo 'error';
	exit;
}

$limit = $_GET['limit'];
$offset= $_GET['offset'];

// http://sqlite.1065341.n5.nabble.com/SELECT-query-first-run-is-VERY-slow-td33100i20.html
//$sql='select feed_provider_id, feed_title, feed_url, feed_date from feeds ';
$sql='select feed_title, feed_url, feed_date from feeds ';
$count_query_sql = 'select count(*) from feeds ';


//////////////////////////////////////WHEN SEARCH TEXT SPECIFIED
if (isset($_GET['search']) && !empty($_GET['search']))
{
	$search_arr = explode(',', $_GET['search']);
	
	if (sizeof($search_arr)==1){
		$sql.= ' where feed_title like :searchTerm or feed_url like :searchTerm';
		$count_query_sql.= ' where feed_title like :searchTerm or feed_url like :searchTerm';
	} else {
		$sql.= ' where (feed_title like :searchTerm and feed_title like :searchTerm2 and feed_title like :searchTerm3) or (feed_url like :searchTerm and feed_url like :searchTerm2 and feed_url like :searchTerm3)';
		$count_query_sql.= ' where (feed_title like :searchTerm and feed_title like :searchTerm2 and feed_title like :searchTerm3) or (feed_url like :searchTerm and feed_url like :searchTerm2 and feed_url like :searchTerm3)';
	}

}

//////////////////////////////////////WHEN SORT COLUMN NAME SPECIFIED
if (isset($_GET['name']) && isset($_GET['order']))
{
	$name= $_GET['name'];
	$order= $_GET['order'];

//bug on sqlite, when trying to use bind on orderby
	if (strpos($name, ' ')==0 && strpos($name, "'")==0) {
		if ($name=='feed_date')
			$name = 'feed_timestamp';
			
		$sql.= " order by $name $order ";
	}
		
}


//////////////////////////////////////PREPARE
$stmt = $conn->getConnection()->prepare($sql." LIMIT :limit OFFSET :offset");

//////////////////////////////////////WHEN SEARCH TEXT SPECIFIED *BIND*
if (isset($_GET['search']) && !empty($_GET['search'])) {
	if (sizeof($search_arr)==1){
		$stmt->bindValue(':searchTerm', '%'.$search_arr[0].'%');
	} else {
		$stmt->bindValue(':searchTerm', '%'.$search_arr[0].'%');
		$stmt->bindValue(':searchTerm2', '%'.$search_arr[1].'%');
		$stmt->bindValue(':searchTerm3', '%'.$search_arr[2].'%');
	}
}


//////////////////////////////////////PAGINATION SETTINGS
$stmt->bindValue(':offset' , intval($offset), PDO::PARAM_INT);
$stmt->bindValue(':limit' , intval($limit), PDO::PARAM_INT);

	
//////////////////////////////////////FETCH ROWS
write_log('execute');
$stmt->execute();
write_log('executed');

$rows = $stmt->fetchAll(PDO::FETCH_ASSOC);


//////////////////////////////////////COUNT TOTAL 
if (isset($_GET['search']) && !empty($_GET['search'])){
	if (sizeof($search_arr)==1){
		$count_recs = $conn->getScalar($count_query_sql, array(':searchTerm' => '%'.$search_arr[0].'%'));
	} else {
		$count_recs = $conn->getScalar($count_query_sql, array(':searchTerm' => '%'.$search_arr[0].'%', ':searchTerm2' => '%'.$search_arr[1].'%', ':searchTerm3' => '%'.$search_arr[2].'%'));
	}
}
else
{	
	$count_recs = $conn->getScalar($count_query_sql, null);
}

//////////////////////////////////////JSON ENCODE
$arr = array('total'=> $count_recs,'rows' => $rows);

header('Content-Type: application/json', true);

echo json_encode($arr);


function write_log($val){
	$f = fopen('log_visitors.txt', 'a');

	fwrite($f, $_SERVER['REMOTE_ADDR'].' - '.date('Y-m-d H:i:s').' - '.$val."\r\n");
	fclose($f);
}
?>