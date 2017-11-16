<?php 

/**
* @link https://pipiscrew.com
* @copyright Copyright (c) 2017 PipisCrew
*/

class ProductMySQL implements IMySQLDriver {
	
    function __construct(){}
    
    function findProduct($id){
        return array('product_id' => 551, 'product_name' => 'MYSQL-Marila Crema Intensa zrnková káva 1 kg');
    }
}

class ProductElastic implements IElasticSearchDriver {
	
    function __construct(){}
    
    function findById($id){
        return array('product_id' => 1001, 'product_name' => 'ELASTIC-Lavazza Top Class zrnková káva 1 kg');
    }
}


interface IElasticSearchDriver
{
	/**
	 * @param string $id
	 * @return array
	 */
	public function findById($id);
}

interface IMySQLDriver
{
	/**
	 * @param string $id
	 * @return array
	 */
	public function findProduct($id);
}
