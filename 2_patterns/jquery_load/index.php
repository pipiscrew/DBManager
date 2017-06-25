<?php

session_start();

if (!isset($_SESSION["u"])) {
	header("Location: login.html");
	exit ;
}
?>

<!DOCTYPE html>
<html>
	<head>
		<title>Portal</title>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
		<meta name="viewport" content="width=device-width, initial-scale=1.0">

		<link href="css/bootstrap.min.css" rel="stylesheet">	
 
		<script type='text/javascript' src="js/jquery-1.10.2.min.js"></script>
		<script type='text/javascript' src="js/bootstrap.min.js"></script>
		<script type='text/javascript' src="js/jquery.validate.min.js"></script> 
		

<!--
	**COMMON** 
-->
		<style>
			/*bootstrap-table selected row*/
			.fixed-table-container tbody tr.selected td	{ background-color: #B0BED9; }
			
			/*progress*/
			.modal-backdrop { opacity: 0.7;	filter: alpha(opacity=70);	background: #fff; z-index: 2;}
			div.loading { position: fixed; margin: auto; top: 0; right: 0; bottom: 0; left: 0; width: 200px; height: 30px; z-index: 3; }
		</style>
		

	</head>
	<body>
	
	//indicator
	<div id="loading" style="display:none;background-color:rgba(0,0,0,0.5);position:absolute;top:0;left:0;height:100%;width:100%;z-index:999;vertical-align:middle; text-align:center"><img src='css/loading.gif' />
	</div>
	
<div id="box" style="padding: 10px">
		<ul class='nav nav-pills' id='tabContainer'>
			
			<li class="active">
				<a href="#categoriesTAB" data-toggle='tab'>Categories</a>
			</li>
			
			<li>
				<a href="#countriesTAB" data-toggle='tab'>Countries</a>
			</li>
			
			<li>
				<a href="#gggggTAB" data-toggle='tab'>ggggg</a>
			</li>
			
			<li>
				<a href="#ssdsdsdsTAB" data-toggle='tab'>ssdsdsds</a>
			</li>
			<li>
				<a href="#xTAB" data-toggle='tab'>x</a>
			</li>
			<li>
				<a href="#x_clicksTAB" data-toggle='tab'>X Clicks</a>
			</li>
			<li>
				<a href="#x_coolTAB" data-toggle='tab'>X Cool</a>
			</li>
			<li>
				<a href="#tcTAB" data-toggle='tab'>powered by PipisCrew</a>
			</li>

			<li class="nav navbar-nav navbar-right">
				<a href="logout.php" id="logout">Logout</a>
			</li>

		</ul>

		<!-- TABS Content [START] -->
		<div id="tabsContent" class="tab-content">


			<div class="tab-pane fade in active" id="categoriesTAB">
				<div id="categories">
					<?php
					include ('x_categories.php');
					?>
				</div>
				<div id="categories_details">
				</div>
			</div>
			
			<div class="tab-pane fade" id="countriesTAB">
				<div id="countries">
					<?php
					include ('x_countries.php');
					?>
				</div>
				<div id="countries_details">
				</div>
			</div>
			
			<div class="tab-pane fade" id="gggggTAB">
				<div id="tat">
					<?php
					include ('x_ggggg.php');
					?>
				</div>
				<div id="ggggg_details">
				</div>
			</div>
			
			<div class="tab-pane fade" id="ssdsdsdsTAB">
				<div id="impressions">
					<?php
					include ('x_ssdsdsds.php');
					?>
				</div>
				<div id="ssdsdsds_details">
				</div>
			</div>
			
			<div class="tab-pane fade" id="xTAB">
				<div id="ad_fees">
					<?php
					include ('x.php');
					?>
				</div>
				<div id="x_details">
				</div>
			</div>
			
			<div class="tab-pane fade" id="x_clicksTAB">
				<div id="web_clicks">
					<?php
					include ('x_clicks.php');
					?>
				</div>
				<div id="x_clicks_details">
				</div>
			</div>
			
			<div class="tab-pane fade" id="x_coolTAB">
				<div id="app_impressions">
					<?php
					include ('x_cool.php');
					?>
				</div>
				<div id="x_cool_details">
				</div>
			</div>
		</div>
		<!-- TABS Content [END] -->
</div>
	</body>
</html>