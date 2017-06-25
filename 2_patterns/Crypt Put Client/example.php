<?php
require_once('class.cryptput.php');

$a = new CryptPutClient();
$id = $a->put('this is my message');
print_r($id);
$data = $a->get($id);
echo $data, "\n";
$data = $a->get($id, true);
echo $data, "\n";
