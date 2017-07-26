<?php  
/**
* @link https://pipiscrew.com
* @copyright Copyright (c) 2016 PipisCrew
*/

	
function connect() {
	$dbopts = parse_url(getenv('DATABASE_URL'));
	
	$postgre_hostname = $dbopts["host"];
	$postgre_user = $dbopts["user"];
	$postgre_password = $dbopts["pass"];
	$postgre_database = ltrim($dbopts["path"],'/');
	 
	$dbh = new PDO("pgsql:host=$postgre_hostname;dbname=$postgre_database", $postgre_user, $postgre_password, 
  array(
	PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
	PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC));
	
	 return $dbh;
}
	
function connect_mysql() {
    $mysql_hostname = "localhost";
    $mysql_user = "";
    $mysql_password = "";
    $mysql_database = "test"; 
     
    $dbh = new PDO("mysql:host=$mysql_hostname;dbname=$mysql_database", $mysql_user, $mysql_password, 
  array(
    PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
    PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
    PDO::MYSQL_ATTR_INIT_COMMAND => "SET NAMES utf8"
  ));
 
  return $dbh;
}
function connect_oracle() {
	//enable ext - php_pdo_oci.dll
	//src - http://stackoverflow.com/a/36639484 -- https://www.devside.net/wamp-server/connect-wamp-server-to-oracle-with-php-php_oci8_11g-dll
	$server         = "127.0.0.1";
	$db_username    = "SYSTEM";
	$db_password    = "Oracle_1";
	$sid            = "ORCL";
	$port           = 1521;
	$dbtns          = "(DESCRIPTION=(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = {$server})(PORT = {$port})))(CONNECT_DATA=(SID={$sid})))";
	$dbh = new PDO("oci:dbname=" . $dbtns . ";charset=utf8", $db_username, $db_password, array(
		PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
		PDO::ATTR_EMULATE_PREPARES => false,
		PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC));
		
	 return $dbh;
}

function connect_sqlite() {
    $dbh = new PDO('sqlite:feeds.db', '', '', array(
 PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
));
   

    return $dbh;
}

function getScalar($db, $sql, $params) {
    if ($stmt = $db -> prepare($sql)) {
 
        $stmt->execute($params);
 
        return $stmt->fetchColumn();
    } else
        return 0;
}
 
function getRow($db, $sql, $params) {
    if ($stmt = $db -> prepare($sql)) {
 
        $stmt->execute($params);
 
        return $stmt->fetch();
    } else
        return 0;
}
 
function getSet($db, $sql, $params) {
    if ($stmt = $db -> prepare($sql)) {
 
        $stmt->execute($params);
 
//        return $stmt->fetchAll(PDO::FETCH_ASSOC);
        return $stmt->fetchAll();
    } else
        return 0;
}
 
function executeSQL($db, $sql, $params) {
    if ($stmt = $db -> prepare($sql)) {
 
        $stmt->execute($params);
 
        return $stmt->rowCount();
    } else
        return false;
}
?>