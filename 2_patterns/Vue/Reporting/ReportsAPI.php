<?php
/*
@session_start();

if (!isset($_SESSION['id']) || empty($_POST['id'])) {
	echo json_encode(null);
	exit ;
}
*/

require_once 'general.php';

if (!isset($_POST['action'])){
	ReportJS(1, 'No valid parameter!');
	exit;
}

////////////////ACTIONS
$action = $_POST['action'];

switch ($action) {
	case 'FillRoutines' :
		FillRoutines();
		break;
	case 'ExecuteView':
		ExecuteView();
		break;	
	case 'GetRoutineParameters':
		GetRoutineParameters();
		break;
	case 'ExecuteProcedure':
		ExecuteProcedure();
		break;

	default :
		ReportJS(2, 'No action defined!');
		exit;
}

function ExecuteProcedure(){
	if (!isset($_POST['rname']) || !isset($_POST['rparams'])){
		ReportJS(2, 'Not found required fields!');
		return;
	}
    
    $routine_name = $_POST['rname'];
    $routine_name = substr($routine_name, 4); //cut the '[p] ' prefix

    $pre_routine_params = $_POST['rparams'];
	$routine_params = json_decode($pre_routine_params);

	$params = array();
    foreach ($routine_params as $value)
		$params[] = $value->val;

	$sql = "CALL `$routine_name`(".implode(",", $params).')';
   
    $db = new dbase();
    $db->connect_mysql();

    $recordSet = $db->getSet($sql, null);

    echo json_encode( array('total'=> sizeof($recordSet), 'data' => $recordSet) );
}

function GetRoutineParameters(){
	if (!isset($_POST['rname'])){
		ReportJS(2, 'Not found required fields!');
		return;
	}
    
    $routine_name = $_POST['rname'];
    $routine_name = substr($routine_name, 4); //cut the '[p] ' prefix

    $db = new dbase();
    $db->connect_mysql();

    $sql = "select p.parameter_name, p.data_type
            from information_schema.routines r
            left join information_schema.parameters p on p.specific_schema = r.routine_schema and p.specific_name = r.specific_name
            where r.specific_name ='$routine_name' and p.parameter_mode = 'IN' and r.routine_schema = (SELECT DATABASE())
            order by p.ordinal_position;";

    $recordSet = $db->getSet($sql, null);

    echo json_encode( $recordSet );
}

//execute VIEW or PROCEDURE w/o params
function ExecuteView(){
	if (!isset($_POST['rname'])){
		ReportJS(2, 'Not found required fields!');
		return;
	}
    
    $routine_name = $_POST['rname'];
    $rType = substr($routine_name, 0, 3);
    $routine_name = substr($routine_name, 4);
    
    $sql=null;
    switch($rType) {
        case '[p]':  //PROCEDURE WITHOUT PARAMETER
            $sql = "CALL `$routine_name`";
            break;
        case '[v]':  //VIEW
            $sql = "SELECT * FROM `$routine_name`";
            break;
    }

    $db = new dbase();
    $db->connect_mysql();

    
    $recordSet = $db->getSet($sql, null);

    echo json_encode( array('total'=> sizeof($recordSet), 'data' => $recordSet) );

}

function FillRoutines(){
   
    $sql= "select concat('[v]', ' ', table_name) as routine, table_name as n FROM information_schema.views where TABLE_SCHEMA in (SELECT DATABASE())
    UNION ALL
    select concat('[p]', ' ', routine_name) as routine, routine_name as n from information_schema.routines where ROUTINE_SCHEMA in (SELECT DATABASE())
    order by 2;";

    $db = new dbase();
    $db->connect_mysql();

    echo json_encode( $db->getSet($sql, null));
}







?>