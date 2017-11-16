<?php

/**
* @link https://pipiscrew.com
* @copyright Copyright (c) 2017 PipisCrew
* 
* revision 2 :
* -general.ProductPopularity class now works with preg_replace_callback 
*/

require_once 'general.php';
require_once 'interfaces.php';

class ProductController
{

    /**
     * @param string $id
     * @return string
     */
    public function detail($id)
    {
    	/* init the return variable */
    	$ret_str = array();
    	
    	/* init the if_cache_exists variable */
    	$use_cache = false;
    	
    	
    	
    	/* read config file */
    	$config = read_config_file();

		if (isset($config['use_cache']) && $config['use_cache']=='1')
		{
			$use_cache = true;
		}
		
		
		if ($use_cache){
			
			$cache = new Cache('cache.json');
			$product = $cache->findById($id);
			
			if ($product) {
				$ret_str = $product;
				$ret_str['product_name'] = '[byCACHE] ## '.$ret_str['product_name'];
			}
			
		}
    	
    	/* search Elastic */
//    	if(empty($ret_str)){
//	        $elastic = new ProductElastic();
//	        $ret_str = $elastic->findById($id);
//
//	        if ($use_cache && $cache) {
//		        //RULE : Whatever product id is passed the drivers always find a product. You do not have to deal with "Not found" exceptions.
//		        $cache->store($ret_str);
//			}
//		}
    		
    	/* search MYSQL */
    	if(empty($ret_str)){
	        $dbase = new ProductMySQL();
	        $ret_str = $dbase->findProduct($id);
	        
	        if ($use_cache && $cache) {
		        //RULE : Whatever product id is passed the drivers always find a product. You do not have to deal with "Not found" exceptions.
		        $cache->store($ret_str);
			}
		}
    	
    	/* increase the product number of requests */
    	//RULE : Whatever product id is passed the drivers always find a product. You do not have to deal with "Not found" exceptions.
		$product_requested = new ProductPopularity('product_popularity.txt');
		$product_requested->store($id);
    	
        return json_encode($ret_str);
    }
    

}




echo 'start, json output :';
$t = new ProductController();

//Elastic 	- 1001
//MySQL 	- 551
//To test MySQL, developer has to REMARK the lines 53-61, because we using static data.
echo "<pre>";
var_dump($t->detail(551));
echo "</pre>";

?>
