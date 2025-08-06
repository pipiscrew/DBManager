<html>
<head>
    <meta http-equiv="Content-Language" content>
    <meta http-equiv="X-UA-Compatible" content="IE=11">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1253">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <title>Test Area</title>
    <link href="./assets/css/bootstrap.min.css" rel="stylesheet">
    <script src="./assets/jquery.min.js"></script>
    <script src="./assets/bootstrap.min.js"></script> <!-- bootstrap v3.3.6 for IE11 compatibility- https://bootstrapdocs.com/v3.3.6/docs/ -->
    <style>
        .row {
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <div class="container">
        <ul class="nav nav-tabs">
            <li role="presentation" class="active"><a href="#tab1" data-toggle="tab">tab1 title</a></li>
            <li role="presentation"><a href="#tab2" data-toggle="tab">tab2 title</a></li>
            <li role="presentation"><a href="#tab3" data-toggle="tab">tab3 title</a></li>
        </ul>
        <div class="tab-content">
            <div id="tab1" class="tab-pane fade in active">
                </br>
                <!-- #include file ="tab1.inc" -->
            </div>
            <div id="tab2" class="tab-pane fade">
                </br>
                <!-- #include file ="tab2.inc" -->
            </div>
            <div id="tab3" class="tab-pane fade">
                <h3>sample</h3>
                <p>..example content3..</p>
            </div>
        </div>
    </div>
</body>
</html>