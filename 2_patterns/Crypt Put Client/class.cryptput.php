<?php
define("API_URL", 'http://crypto-put.zpmfhq6y4c.us-west-2.elasticbeanstalk.com/');
define("API_USERNAME", '');
define("API_PASSWORD", '');

class JsonApiBase {
  private $_url;
  private $_token;

  public function __construct($url, $username, $password) {
    $this->_url = $url;
    $this->_token = $this->pullToken($username, $password);
  }

  public function getUrl($add = '') {
    return $this->_url . "$add";
  }

  private function getToken() {
    return $this->_token;
  }

  private function pullToken($username, $password) {
    $url = $this->getUrl('api-auth/');

    $data = json_encode(array(
      'username' => $username,
      'password' => $password
    ));

    $headers = array(
      "Content-type: application/json\r\n"
    );

    $response = json_decode($this->rawRequest($data, $headers, $url));
    try {
      $ret = $response->token;
    }
    catch(Exception $e) {
      $ret = null;
      error_log("Could not assign token from $url");
    }
    return $ret;
  }

  private function rawRequest($data, $headers, $url, $method='POST') {
    $options = array(
        'http' => array(
            'header' => $headers,
            'method' => $method,
            'content' => $data,
        ),
    );
    $context = stream_context_create($options);
    $result = file_get_contents($url, false, $context);
    return $result;
  }

  public function apiRequest($data, $url, $method='POST') {
    if( null === $this->getToken() ) {
      return null;
    }

    $headers = array(
      "Content-type: application/json",
      "Authorization: JWT {$this->getToken()}"
    );
    return $this->rawRequest($data, $headers, $url, $method);
  }
}

class CryptPutClient extends JsonApiBase {
  public function __construct() {
    parent::__construct(API_URL, API_USERNAME, API_PASSWORD);
  }

  public function put($data_string) {
    $data = json_encode(array(
      'd' => $data_string
    ));
    if( $response = $this->apiRequest($data, $this->getUrl('put/')) ) {
      $r = json_decode($response);
      return $r->id;
    }
    else {
      throw new Exception("Could not put data, bad token?");
    }
  }

  public function get($id, $decrypt=false) {
    $data = array('i' => $id);
    if( true === $decrypt ) {
      $data['d'] = '1';
    }
    $post_data = json_encode($data);
    if( $response = $this->apiRequest($post_data, $this->getUrl('get/')) ) {
      $r = json_decode($response);
      return $r->data;
    }
    else {
      throw new Exception("Could not get data, bad id?");
    }
  }
}
