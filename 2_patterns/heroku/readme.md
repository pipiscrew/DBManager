Hi all, 

Looking a way to store data in heroku, at first I used sqlite but nah is not a good solution as the heroku randomly restores the last commit and overwrites the dbase file. 

The solution is to create a heroku Postgre dbase, the data never get deleted but you still have some limits on free plan as the following :

 - expected uptime 99.5% (< 4 hr downtime per month)
 - rows limit 10,000
 - concurrent connections limit 20

Step 1 - Create the Postgre dbase by going to [https://data.heroku.com/](https://data.heroku.com/)
![snap332](https://user-images.githubusercontent.com/3852762/28497991-81eb5b22-6f94-11e7-8219-cf453a5780df.png)

Step2 - Navigate to heroku app dashboard under **Resources** tab, the dbase is there, click it.
![snap328](https://user-images.githubusercontent.com/3852762/28498004-ceebd4b0-6f94-11e7-9d1c-3c1e9d9b0473.png)

Drives you to dbase cpanel, click **View Credentials**
![snap329](https://user-images.githubusercontent.com/3852762/28498014-fc9bf1ce-6f94-11e7-9880-745c6902f60f.png)

there you can find all the needed information to get connected at first to setup the tables 
![snap331](https://user-images.githubusercontent.com/3852762/28498037-703a6c5a-6f95-11e7-9f5e-6967d3bb4ab6.png)

how to setup ?
any desktop/web application doing the job (except PremiumSoft.Navicat for PostgreSQL I got an error). The following freeware applications tested&working : 

 - Desktop application https://www.pgadmin.org/
 - Single PHP file - https://www.adminer.org/ (upload it to repository :0)

As we read before these credentials are temporary, our application has to get the connection info every time launched by asking heroku.

The following PHP snippet doing the job :
```php
//src - https://devcenter.heroku.com/articles/heroku-postgresql#connecting-in-php
function connect()
{
	$dbopts           = parse_url(getenv('DATABASE_URL'));

	$postgre_hostname = $dbopts["host"];
	$postgre_user     = $dbopts["user"];
	$postgre_password = $dbopts["pass"];
	$postgre_database = ltrim($dbopts["path"],'/');

	//Postgres, the default charset and collation is utf8_ci
	$dbh              = new PDO("pgsql:host=$postgre_hostname;dbname=$postgre_database", $postgre_user, $postgre_password,
		array(
			PDO::ATTR_ERRMODE           => PDO::ERRMODE_EXCEPTION,
			PDO::ATTR_DEFAULT_FETCH_MODE=> PDO::FETCH_ASSOC)
			);

	return $dbh;
}
```



----------


tip1 :
the table before :
```sql
CREATE TABLE foo (
id INTEGER PRIMARY KEY AUTOINCREMENT,
bar varchar);
```
after (Postgre flavor)
```sql
CREATE TABLE foo (
id SERIAL,
bar varchar);
```
⋅⋅

tip2 :
lets say you want to trigger a PHP file (x.php) every x minutes, there is no possibility to do it at heroku freeplan, use your own server cron :) by executing :

    wget https://xnews.herokuapp.com/x.php
⋅⋅
⋅⋅
more info about heroku [http://www.pipiscrew.com/2017/04/deploy-your-angular2-app-to-heroku/](http://www.pipiscrew.com/2017/04/deploy-your-angular2-app-to-heroku/)
