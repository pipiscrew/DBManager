<?php

/**
* @link https://pipiscrew.com
* @copyright Copyright (c) 2017 PipisCrew
*/

function read_config_file(){
	
	try{ //prevent any file system error

		if ( file_exists('config.ini') )
			return parse_ini_file('config.ini');

	} 
	catch (Exception $e) {
		
	}

}


class Cache {
	
	private $cache_file = '';
	
	function __construct($cache_file){
		$this->cache_file = $cache_file;
	}
	
	function findById($product_id) {
		
		$cache_content = $this->load();
		
		/* when product_id exists to cache file */
		if (isset($cache_content[$product_id])) 
			return $cache_content[$product_id];
		
	}

	function store($product_array) {

		/* load cache file */
    	$cache_content = $this->load();

		if ($cache_content) {
			/*
				when file exists
				add item to array OR if item exists update it (maybe changed at dbase, senario when customer disable cache then enable it)
			*/
				$cache_content[$product_array['product_id']] = $product_array; 
		}
		else {
			/*
				when file doesnt exist, create a new array
			*/
			
			$cache_content = array( $product_array['product_id'] => $product_array );
		}

		
		$cache_save = json_encode($cache_content);
		
		file_put_contents($this->cache_file, $cache_save);

	}

	private function load() {
		
		if ( file_exists($this->cache_file) ) {
			
		  $file = file_get_contents($this->cache_file);
		  return json_decode($file, true);
		  
		} 

	}
	
}


class ProductPopularity {
	
	private $file = '';
	
	function __construct($file){
		$this->file = $file;
	}
	
	function store($product_id) {

		/* load popularity file */
		$content = "";
		
		if ( file_exists($this->file) ) {
			
		  $content = file_get_contents($this->file);		
		
		} 
		
		// indicator if product_id matched
		$product_found = false;


		/* Line 111
		* #use# - In anonymous function definition to pass variables inside the function -- ref - http://php.net/manual/en/functions.anonymous.php
		* passing variable #product_found# ByRef -- ref - http://php.net/manual/en/language.references.pass.php
		*
		* the regEX - matches the \nproductid_ at $content, and increase the popularity by one!!
		*/
		$content =  preg_replace_callback("/[\n]{$product_id}_(\d+)/", function($matches) use ($product_id, &$product_found)
					{
						// mark the product found (prevent adding double records)
						$product_found = true;
						
						
					    return "\n".$product_id . '_' . (1 + $matches[1]);
					    
					}
					, $content);

		if (!$product_found)
		{// when the product_id doesnt exist landed here
			$content.= "\r\n".$product_id.'_1';			
		}

		// the txt file has the structure of PRODUCTCODE_POPULARITY

		//write to disk
		file_put_contents($this->file, $content);
	}
	
}
