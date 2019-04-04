<?php
class dbase{
	private $db;

	function connect_postgre() {
		$postgre_hostname = "localhost";
		$postgre_user = "root";
		$postgre_password = "password";
		$postgre_database = "x"; 
		 
		//Postgres, the default charset and collation is utf8_ci 
		$this->db = new PDO("pgsql:host=$postgre_hostname;dbname=$postgre_database", $postgre_user, $postgre_password, 
	  array(
		PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
		PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC));
	}
	
	function connect_mysql() {
		$mysql_hostname = "localhost";
		$mysql_user = "root";
		$mysql_password = "password";
		$mysql_database = "x"; 
		 
		$this->db = new PDO("mysql:host=$mysql_hostname;dbname=$mysql_database", $mysql_user, $mysql_password, 
	  array(
		PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
		PDO::MYSQL_ATTR_INIT_COMMAND => "SET NAMES utf8",
		PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC));
	}
    
	function connect_oracle() {
		$server         = "x";
		$db_username    = "x";
		$db_password    = "x#x";
		$sid            = "x";
		$port           = 1376;
		$dbtns          = "(DESCRIPTION=(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = {$server})(PORT = {$port})))(CONNECT_DATA=(SID={$sid})))";
		$this->db = new PDO("oci:dbname=" . $dbtns . ";charset=utf8", $db_username, $db_password, array(
			PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
			PDO::ATTR_EMULATE_PREPARES => false,
			PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC));
	}
    
	function connect_sqlite() {
		//if doesnt exist, will created.
		//$this->db = new PDO('sqlite:dbase.db');
		//$dbh->setAttribute( PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION ); //only for debug
		
		$this->db = new PDO('sqlite:dbase.db',null,null,array(
			PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
			PDO::ATTR_EMULATE_PREPARES => false,
			PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC));
		
		//check if table has records, if not create table
		$d = $this->getScalar("select count(*) from users",null);
		if ($d==0)
		{
			$this->executeSQL("CREATE TABLE [users] (user_id INTEGER PRIMARY KEY, user_mail TEXT, user_password TEXT, user_level INTEGER)", null);
			$this->executeSQL("your other tables here?",null);
			
			//read&write only server (user cant download the dbase)
			chmod("dbase.db", 0600);
		}
	}
	
	function connect_sqlserver() {
		//you need to download https://github.com/Microsoft/msphpsql/releases
		//depend on your php version, you have to download the corresponding Windows-7.?.zip
		//using phpinfo() check if needs 'Thread Safe' or not, for TS :
		//copy php_pdo_sqlsrv_73_ts.dll to php\ext
		//then add to php.ini
		//extension=pdo_sqlsrv_73_ts
		//restart the server.
		//
		//refs :
		//https://www.php.net/manual/en/intro.sqlsrv.php
		//http://www.synet.sk/php/en/230-php-drivers-for-microsoft-sql-server-mssql-sqlsrv-utf8
		//https://docs.microsoft.com/en-us/sql/connect/php/system-requirements-for-the-php-sql-driver?view=sql-server-2017
		
		
		$sql_servername = ".\sqlexpress";
		$sql_user = "sa";
		$sql_password = "123456";
		$sql_database = "testDB"; 
		 
		 //specify port via "sqlsrv:Server=server.dyndns.biz,1433;Database=DBNAME";
		 //https://stackoverflow.com/a/36212561
		$this->db = new PDO("sqlsrv:server=$sql_servername;database=$sql_database", $sql_user, $sql_password, array(
			PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
			PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC));
	}
    
    function getConnection(){
        return $this->db;
    }
    
	function getScalar($sql, $params) {
		if ($stmt = $this->db -> prepare($sql)) {
	 
			$stmt->execute($params);
	 
			return $stmt->fetchColumn();
		} else
			return 0;
	}
	 
	function getRow($sql, $params) {
		if ($stmt = $this->db -> prepare($sql)) {
	 
			$stmt->execute($params);
	 
			return $stmt->fetch();
		} else
			return 0;
	}
	 
	function getSet($sql, $params) {
		if ($stmt = $this->db -> prepare($sql)) {
	 
//            echo $sql;
//            exit;
			$stmt->execute($params);
	 
		  return $stmt->fetchAll();
		} else
			return 0;
	}
		
	function executeSQL($sql, $params) {
		if ($stmt = $this->db -> prepare($sql)) {
	 
			$stmt->execute($params);
	 
			return $stmt->rowCount();
		} else
			return false;
	}

    
	/* NEW FUNCTIONS */	
	
    
    function escape_str($value)
    {   //src - https://stackoverflow.com/a/1162502
        $search = array("\\",  "\x00", "\n",  "\r",  "'",  '"', "\x1a");
        $replace = array("\\\\","\\0","\\n", "\\r", "\'", '\"', "\\Z");

        return str_replace($search, $replace, $value);
    }

    function write_log($user_id, $ip, $log){
        
        $sql = "INSERT INTO `log` (user_id, ip, log_txt, date_rec) VALUES (:user_id, :ip, :log_txt, :date_rec)";
        $stmt = $this->db ->prepare($sql);
        
        $stmt->bindValue(':user_id' , $user_id);
        $stmt->bindValue(':ip' , $ip);
        $stmt->bindValue(':log_txt' , $log);
        $stmt->bindValue(':date_rec' , date("Y-m-d H:i:s"));

        $stmt->execute();

        $res = $stmt->rowCount();

        if($res != 1)
            die("error when inserting log");
        
    }
    
	function getSet_with_types($sql, $params) {
		if ($stmt = $this->db ->prepare($sql)) {
	 
			$stmt->execute($params);
	 
			$r = $stmt->fetchAll(); //FETCH_ASSOC must be enabled at connection or here.
		  return convertTypes($stmt, $r);
		} else
			return 0;
	}
	
	function row2class($row, $obj){
	   foreach ($row AS $key => $value){
			$obj->$key = $value;
	   }
		
	   return $obj;
	}
	
	function convertTypes(PDOStatement $statement, $assoc)
	{//src - http://stackoverflow.com/a/9952703 - extend for fetchAll
		
		//loop through all columns
		for ($i = 0; $columnMeta = $statement->getColumnMeta($i); $i++)
		{
			$type = $columnMeta['native_type'];
			
			switch($type)
			{
				case 'DECIMAL':
				case 'TINY':
				case 'SHORT':
				case 'LONG':
				case 'LONGLONG':
				case 'INT24':
					for($x= 0 ; $x < sizeof($assoc) ; $x++ ){ //for each row in rowset
						if ($assoc[$x][$columnMeta['name']]==null)
							continue;
						
						$assoc[$x][$columnMeta['name']] = (int) $assoc[$x][$columnMeta['name']];
					}
					break;
				case 'DATE':
				case 'DATETIME':
                case 'TIMESTAMP':
					for($x= 0 ; $x < sizeof($assoc) ; $x++ ){ //for each row in rowset
						$assoc[$x][$columnMeta['name']] = strtotime($assoc[$x][$columnMeta['name']]);
					}
					break;
					break;
				// default: keep as string
			}
		}
		
		return $assoc;
	}
	function str2date($src_val, $date_format = "Y-m-d H:i:s"){
		if ($src_val==null || startsWith($src_val, "0000")) //the date is null (aka SQL - date NULL) OR is empty (aka year is 0000)
		   return null;
		//
		$src_val = trim($src_val);
		
		if (strpos($src_val, ' ')==0){
			//occur when the date_format doesnt contain H:i:s - PHP automatically adds the current time!!
			$src_val .= " 00:00:00";
		}
		//
		
		$d = DateTime::createFromFormat($date_format, $src_val);
		if (!$d)
		   throw new Exception("string cant be converted to date >> ".$src_val);
		else
			return $d;
	}
	
	function startsWith($haystack, $needle)
	{
		 $length = strlen($needle);
		 return (substr($haystack, 0, $length) === $needle);
	}
}
