<?php
@session_start();

if (!isset($_SESSION["id"])) {
    echo json_encode(null);
    exit ;
}


// include DB
require_once ('general.php');

$table_columns = array(
    'question_id',
    'subcategory_id',
    'category_name',    
    'subcategory_name',
    'language_id',
    'title',
    'question',
    'misc1',
    'misc2',
    'misc3',
    'misc4',
    'date_rec',
);

if (!is_numeric($_GET["limit"]) || !is_numeric($_GET["offset"]))
{
	echo "error";
	exit;
}

$limit = $_GET["limit"];
$offset= $_GET["offset"];


    
//connect to database
$conn = new dbase();
$conn->connect_mysql();


//used when user search for something
$search = null;

if (isset($_GET["search"]))
	$search = $conn->escape_str($_GET["search"]);


$sql="select question_id, categories.category_name as category_name,  questions.subcategory_id as subcategory_id, subcategories.subcategory_name as subcategory_name, languages.language_name as language_id, title, question, misc1, misc2, misc3, misc4, (date_rec + INTERVAL -:browsertime MINUTE) as date_rec from questions 
 LEFT JOIN categories ON categories.category_id = questions.category_id
 LEFT JOIN subcategories ON subcategories.subcategory_id = questions.subcategory_id
 LEFT JOIN languages ON languages.language_id = questions.language_id
 where questions.category_id={$_GET["cat_id"]} ";
 
$count_query_sql = "select count(*) from questions";


//////////////////////////////////////WHEN SEARCH TEXT SPECIFIED
if ($search)
{
	$like_str = " or #field# like :searchTerm";
	$where = " 0=1 ";

	foreach($table_columns as $col)
	{
		//when we have joins, we have to exclude the fields on search. Workaround use table.fieldname
        if ($col=='category_name' || $col=='question_id' || $col=='category_id' || $col=='subcategory_id' || $col=='language_id' || $col=='subcategory_name')
            continue;
        
		$where.= str_replace("#field#", $col, $like_str);
	}

	//when the #where# defined on main query use 'and', otherwise 'where'
	$sql.= " and ". $where;
	$count_query_sql.= " where ". $where;
}

//////////////////////////////////////WHEN SORT COLUMN NAME SPECIFIED
if (isset($_GET["name"]) && isset($_GET["order"]))
{
	$ordercol= $conn->escape_str($_GET["name"]);
	$orderby= $conn->escape_str($_GET["order"]);
	
	if ($orderby=="asc" || $orderby=="desc"){
					
        //validation, if col provided exists
		$key=array_search($ordercol, $table_columns);
        
		$order=$table_columns[$key];

		$sql.= " order by {$order} {$orderby}";		
	}
}


//////////////////////////////////////PREPARE
$stmt = $conn->getConnection()->prepare($sql." limit :offset,:limit");


//////////////////////////////////////WHEN SEARCH TEXT SPECIFIED *BIND*
if ($search)    
	$stmt->bindValue(':searchTerm', '%'.$search.'%');



//////////////////////////////////////PAGINATION SETTINGS
$stmt->bindValue(':offset' , intval($offset), PDO::PARAM_INT);
$stmt->bindValue(':limit' , intval($limit), PDO::PARAM_INT);
$stmt->bindValue(':browsertime' , intval($_SESSION['timezone']), PDO::PARAM_INT);
	
//////////////////////////////////////FETCH ROWS
$stmt->execute();

$rows = $stmt->fetchAll();


//////////////////////////////////////COUNT TOTAL 
if (!empty($search))
	$count_recs = $conn->getScalar($count_query_sql, array(':searchTerm' => '%'.$search.'%'));
else
	$count_recs = $conn->getScalar($count_query_sql, null);


//////////////////////////////////////JSON ENCODE
$arr = array('total'=> $count_recs,'rows' => $rows);

header("Content-Type: application/json", true);

echo json_encode($arr);

?>
