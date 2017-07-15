var server = require("./server");

server.start();


//firebase rules example : https://github.com/firebase/firefeed/blob/master/rules.json

logcat("started v0.3");

//////////////////////////////////////////////////////////// LIBRARIES
//request lib for random.org
var request = require('request');

//firebase lib
var Firebase = require('firebase');

//JQDeferred lib for when
var Deferred = require("JQDeferred");

//nodemailer lib
var nodemailer = require("nodemailer");

//filesystem
var fs = require('fs');
//////////////////////////////////////////////////////////// LIBRARIES

//////////////////////////////////////////////////////////// FILE LOGGER
function write2log(descr) {
	var log = fs.createWriteStream('log.txt', {
		'flags' : 'a'
	});
	// use {'flags': 'a'} to append and {'flags': 'w'} to erase and write a new file
	var datetime = new Date();
	log.write(datetime + " - " + descr);
	log.close();
}

//////////////////////////////////////////////////////////// FILE LOGGER

/////////////////////////////////////////////////////// NODEMAILER SETUP
//SENDER EMAIL CREDENTIALS
var smtpTransport = nodemailer.createTransport("SMTP", {
	service : "Gmail",
	auth : {
		user : "x@x.com",
		pass : "UKjA8Gmj123"
	}
});

function sendMail(toMail, subject, mailHTML) {

	if (isDebug) {
		if (toMail != "yg@x.com" && toMail != "1@1.net" && toMail.toLowerCase().indexOf("x2.com") == -1 && toMail.toLowerCase().indexOf("x3.gr") == -1)
			toMail = "yg@x.com";
	}

	var mailOptions = {
		from : "theAppName <x@x.com>", // sender address
		to : toMail, // list of receivers
		subject : subject + "to " + toMail, // Subject line
		text : mailHTML.replace(/<\/?[^>]+(>|$)/g, ""), // plaintext body
		html : mailHTML // html body
	};

	// send mail with defined transport object
	smtpTransport.sendMail(mailOptions, function(error, response) {
		if (error) {
			write2log("\r\nURGENT : Mail cannot be send\r\n\t" + "Mail recipient : " + toMail + "\r\n\tSubject : " + subject + "\r\n\tMailBody : " + mailHTML)

			logcat("sendMail - " + error);
			// console.log("sendMail - " + error);
		} else {
			logcat("sendMail - Message sent @ " + mailOptions.to + " : " + response.message);
			// console.log("sendMail - Message sent @ " + mailOptions.to + " : " + response.message);
			//todo - write also to FB
		}

	});
}

/////////////////////////////////////////////////////// NODEMAILER SETUP

function getSend_Mail2User_Winner(userID, competition_title, competition_nodeID) {
	var pushTitle = "You win «" + competition_title + "»";

	var db = new Firebase('https://' + baseURL + '/people/' + userID + "/mail");
	db.once('value', function(snapshot) {
		if (snapshot.val() === null) {
			//when is no user mail
			//send mail to c4c + push to ANDROID user
			//TODO : support for IPHONE?

			logcat("getSend_Mail2User_Winner - URGENT : winner drew but can find in people table");
			// console.log("getSend_Mail2User_Winner - URGENT : winner drew but can find in people table");

			//email admin
			sendMail("alert@x.com", "URGENT : winner drew but can find in people table", "<b>Competition Title :</b>" + competition_title + "<br><b>User ID : </b>" + userID);

			var pushDB = new Firebase('https://' + baseURL + '/people/' + userID + "/android/deviceID");
			pushDB.once('value', function(pushSnap) {
				if (pushSnap.val() === null) {
					sendMail("alert@x.com", "URGENT : winner drew but cant find user mail + no push", "<b>Competition Title :</b>" + competition_title + "<br><b>User ID : </b>" + userID);
				} else {
					sendPush2Androids(pushSnap.val(), pushTitle, userID, competition_nodeID);
				}
			});

		} else {
			logcat("getSend_Mail2User_Winner - SEND MAIL TO WINNER " + snapshot.val());
			// console.log("getSend_Mail2User_Winner - SEND MAIL TO WINNER " + snapshot.val());

			var msgbody = "You win the contest :<br><br> «" + competition_title + "»";

			sendMail(snapshot.val(), "Contest results", msgbody);

			//write to people/userID/compID/isWinner
			var record = {
				winner : '1'
			};

			logcat("getSend_Mail2User_Winner - UPDATE DB");
			// console.log("getSend_Mail2User_Winner - UPDATE DB");
			var upd2DB = new Firebase('https://' + baseURL + '/people/' + userID + "/competitions/" + competition_nodeID);
			Deferred.when(upd2DB.child('winner').set('1')).done(function(x) {
				logcat("getSend_Mail2User_Winner - people - winner set!");
				// console.log("getSend_Mail2User_Winner - people - winner set!");
			});

			//TODO : support for IPHONE?
			var pushDB = new Firebase('https://' + baseURL + '/people/' + userID + "/android/deviceID");
			pushDB.once('value', function(pushSnap) {
				if (pushSnap.val() === null) {
					logcat("getSend_Mail2User_Winner - winner drew, mail sent, but can there is no deviceID to push!");
					// console.log("getSend_Mail2User_Winner - winner drew, mail sent, but can there is no deviceID to push!");
				} else {
					sendPush2Androids(pushSnap.val(), pushTitle, userID, competition_nodeID);
				}
			});

		}
	});

}

function getSend_Mail2_Sponsor(companyID, competition_title, competition_nodeID) {

	var db = new Firebase('https://' + baseURL + '/companies/' + companyID + "/mail");
	db.once('value', function(snapshot) {
		if (snapshot.val() === null) {
			logcat("getSend_Mail2_Sponsor - URGENT : winner drew, cant mail *sponor* for the result he hasnt mail!");
			// console.log("getSend_Mail2_Sponsor - URGENT : winner drew, cant mail *sponor* for the result he hasnt mail!");
			//email admin
			sendMail("alert@x.com", "getSend_Mail2_Sponsor - URGENT : winner drew but can mail sponsor", "<b>Competition Title :</b>" + competition_title + "<br><b>User ID : </b>" + userID);

		} else {
			logcat("getSend_Mail2_Sponsor - SEND MAIL TO SPONSOR" + snapshot.val());
			// console.log("getSend_Mail2_Sponsor - SEND MAIL TO SPONSOR" + snapshot.val());

			var msgbody = "Αγαπητέ Χορηγέ<br><br>Σε ενημερώνουμε ότι ολοκληρώθηκε ο διαγωνισμός που υπέβαλες με τίτλο: <br><br>«" + competition_title;

			sendMail(snapshot.val(), "Contest results", msgbody);
		}
	});

}

function getSend_Mail2_SponsorCompetitionStart(companyID, competition_title, competition_nodeID) {

	var db = new Firebase('https://' + baseURL + '/companies/' + companyID + "/mail");
	db.once('value', function(snapshot) {
		if (snapshot.val() === null) {
			logcat("getSend_Mail2_SponsorCompetitionStart - URGENT : cant find Sponsor mail to inform him for the competition START!");
			// console.log("getSend_Mail2_SponsorCompetitionStart - URGENT : cant find Sponsor mail to inform him for the competition START!");
			//email admin
			sendMail("alert@x.com", "getSend_Mail2_SponsorCompetitionStart - URGENT : cant find Sponsor mail to inform him for the competition START!", "<b>Competition Title :</b>" + competition_title + "<br><b>User ID : </b>" + userID);

		} else {
			logcat("getSend_Mail2_SponsorCompetitionStart - SEND MAIL TO SPONSOR" + snapshot.val());
			// console.log("getSend_Mail2_SponsorCompetitionStart - SEND MAIL TO SPONSOR" + snapshot.val());

			var msgbody = "Αγαπητέ Χορηγέ<br><br>Σε ενημερώνουμε ότι ο διαγωνισμός που υπέβαλες με τίτλο: <br><br>«" + competition_title;

			sendMail(snapshot.val(), "Contest start competition", msgbody);
		}
	});

}

function getSend_Mail2User_Looser(userID, competition_title, competition_nodeID) {
	var pushTitle = "Δεν κέρδισες το διαγωνισμό «" + competition_title + "»";

	var db = new Firebase('https://' + baseURL + '/people/' + userID + "/mail");
	db.once('value', function(snapshot) {
		if (snapshot.val() === null) {
			//TODO : support for IPHONE?
			logcat("getSend_Mail2User_Looser - URGENT : winner drew but can find in people table");
			//console.log("getSend_Mail2User_Looser - URGENT : winner drew but can find in people table");
			//email admin
			sendMail("alert@x.com", "getSend_Mail2User_Looser - URGENT : winner drew but can find in people table", "<b>Competition Title :</b>" + competition_title + "<br><b>User ID : </b>" + userID);

			var pushDB = new Firebase('https://' + baseURL + '/peopleAndroid/' + userID + "/deviceID");
			pushDB.once('value', function(pushSnap) {
				if (pushSnap.val() === null) {
					sendMail("alert@x.com", "getSend_Mail2User_Looser - URGENT : winner drew but cant find user mail + no push", "<b>Competition Title :</b>" + competition_title + "<br><b>User ID : </b>" + userID);
				} else {
					sendPush2Androids(pushSnap.val(), pushTitle, userID, null);
				}
			});

		} else {
			logcat("getSend_Mail2User_Looser - SEND MAIL TO LOOSER" + snapshot.val());
			// console.log("getSend_Mail2User_Looser - SEND MAIL TO LOOSER" + snapshot.val());

			var msgbody = "Αγαπητέ/η Φίλε/η<br><br>Σε ενημερώνουμε ότι δυστυχώς δεν κέρδισες το διαγωνισμό με τίτλο: <br><br>«" + competition_title + "»<br>";

			sendMail(snapshot.val(), "Αποτελέσματα Διαγωνισμού", msgbody);

			//write to people/userID/compID/isWinner
			var record = {
				winner : '1'
			};

			//TODO : support for IPHONE?
			var pushDB = new Firebase('https://' + baseURL + '/peopleAndroid/' + userID + "/deviceID");
			pushDB.once('value', function(pushSnap) {
				if (pushSnap.val() === null) {
				} else {
					sendPush2Androids(pushSnap.val(), pushTitle, userID, null);
				}
			});

		}
	});

}

/////////////////////////////////////////////////////////
//universal function - insert node to a firebase URL
function addNode(url, itemOBJ, priority) {
	logcat("> addNode fired!");
	// console.log("> addNode fired!");
	var add2DB = new Firebase(url);

	if (priority)

		Deferred(function(def) {
			add2DB.setWithPriority(itemOBJ, priority, function(err) {
				if (err) {
					def.reject();
				} else {
					def.resolve();
				}
			})
		}).done(function() {
			/* run deferred code here */
		});

	// Deferred.when(add2DB.setWithPriority(itemOBJ, priority)).done(function(x) {
	// //console.log(new Date() + "added!");
	// });
	else
		Deferred.when(add2DB.set(itemOBJ)).done(function(x) {
			//console.log(new Date() + "added!");
		});

}

function updateNodeSET(url, itemOBJ, priority) {

	var upd2DB = new Firebase(url);

	for (var propertyName in itemOBJ) {
		Deferred.when(upd2DB.child(propertyName).set(itemOBJ[propertyName])).done(function(x) {
		});
	}

	if (priority) {
		Deferred.when(upd2DB.setPriority(priority)).done(function(x) {
		});
	}
}

function setPrority(url, priority) {

	var DB = new Firebase(url);

	Deferred.when(DB.setPriority(priority)).done(function(x) {
	});

}

function cloneNode(sourceURL, destURL, itemOBJ, priority) {
	logcat("> cloneNode fired!");
	// console.log(getDateTime() + "> cloneNode fired!");
	var clone2DBsource = new Firebase(sourceURL);
	var clone2DBdest = new Firebase(destURL);

	Deferred(function(def) {
		clone2DBdest.setWithPriority(itemOBJ, priority, function(err) {
			if (err) {
				def.reject();
			} else {
				def.resolve();
			}
		})
	}).done(function() {
		logcat("> cloned!");
		// console.log(getDateTime() + "> cloned!");
		Deferred(function(def) {
			clone2DBsource.remove(function(err) {
				if (err) {
					def.reject();
				} else {
					def.resolve();
				}
			})
		}).done(function() {
			logcat("> cloned!-source deleted");
			// console.log(getDateTime() + "> cloned!-source deleted");
		});

	});

}

/////////////////////////////////////////////////////////
//universal function to delete a node by firebase URL
function delNode(url) {
	logcat("> delNode fired!");
	// console.log("> delNode fired!");
	var del2DB = new Firebase(url);

	Deferred(function(def) {
		del2DB.remove(function(err) {
			if (err) {
				def.reject();
			} else {
				def.resolve();
			}
		})
	}).done(function() {
		/* run deferred code here */
	});

	// Deferred.when(del2DB.remove()).done(function(x) {
	//console.log(new Date() + "deleted!");
	// });

}

function logcat(str) {
	console.log(getDateTime() + ' - ' + str);
}

function draw() {
	console.log("draw()2");
	db = new Firebase('https://' + baseURL + '/countries/');

	db.once('value', function(snapshot) {

		//foreach country exists
		snapshot.forEach(function(country) {
			// console.log(country.val().title);
			queryCompetitions(country.val().title);
		});
	});
}

function queryCompetitions(countrySmallCode) {
	var db = new Firebase('https://' + baseURL + '/competitions/');

	//startAtfix = otherwise catch the GR0 when sponsor add a new competition!
	var startAtfix = countrySmallCode + 1388534400;
	//1388534400 = 1/1/2014

	logcat("queryCompetitions - startAT - " + startAtfix.toString() + " endAT - " + countrySmallCode + time4scan.toString());
	// console.log("queryCompetitions - startAT - " + startAtfix.toString() + " endAT - " + countrySmallCode + time4scan.toString())

	//via UTC now
	var dbQuery = db.startAt(startAtfix.toString()).endAt(countrySmallCode + time4scan.toString());

	dbQuery.once('value', function(snapshot) {

		//foreach competition exists (aka less today)
		snapshot.forEach(function(competition) {
			if (competition.val().is_offer != null) {
				if (competition.val().is_offer == 1) {
					logcat('offer found : ' + competition.val().title);
					moveOffer2Ended(competition);
				} else {
					logcat('competition found : ' + competition.val().title);
					generateNumber(competition);
				}
			}

		});
	});
}

function moveOffer2Ended(comp) {
	
	var record;
	if (comp.val().bids==null)
		record = {
			category : comp.val().category,
			category_id : comp.val().category_id,
			comment : comp.val().comment,
			logo : comp.val().logo,
			is_offer : 1,
			promos : comp.val().promos,
			title : comp.val().title,
			company_id : comp.val().company_id,
			company : comp.val().company,
			date_start : comp.val().date_start,
			date_end : comp.val().date_end,
			causes : comp.val().causes,
			causes_categories : comp.val().causes_categories,
			winners_number : comp.val().winners_number,
			price : comp.val().price,
			price_offer : comp.val().price_offer,
			redeem_code : comp.val().redeem_code,
			winner : "there are no bids"
		};
	else 
		record = {
			category : comp.val().category,
			category_id : comp.val().category_id,
			comment : comp.val().comment,
			logo : comp.val().logo,
			is_offer : 1,
			bids : comp.val().bids,
			promos : comp.val().promos,
			title : comp.val().title,
			company_id : comp.val().company_id,
			company : comp.val().company,
			date_start : comp.val().date_start,
			date_end : comp.val().date_end,
			causes : comp.val().causes,
			causes_categories : comp.val().causes_categories,
			price : comp.val().price,
			price_offer : comp.val().price_offer,
			redeem_code : comp.val().redeem_code,
			winners_number : comp.val().winners_number
		};

	//******************************* RESET CAUSECATEGORIES + CATEGORIES .PRIORITIES! (offer can ended before date_end via completion of 'no of winners')
	logcat("offer found :" + comp.val().title + " (" + comp.name() + ") - resetting cause + sponsor + category + causecats .priotities!");
	
	//**CATEGORY** (this is for mobile main activity)
	setPrority('https://' + baseURL + '/categories/' + comp.val().category_id + '/competitions/' + comp.name(), comp.getPriority());
	
	//**CAUSE CATEGORIES** (this is for mobile main activity)
	var causecategories = comp.child('causes_categories');
	causecategories.forEach(function(causecategory) {
		setPrority('https://' + baseURL + '/causecategories/' + causecategory.name() + '/competitions/' + comp.name(), comp.getPriority());
	});
	
	//**SPONSOR** (STATS IF ACTIVE @ DETAIL ACTIVITY)
	setPrority('https://' + baseURL + '/companies/' + comp.val().company_id + "/competitions/" + comp.name(), comp.getPriority());
	
	//**CAUSES** (STATS IF ACTIVE @ DETAIL ACTIVITY)
	var causes = comp.child('causes');
	causes.forEach(function(cause) {
		setPrority('https://' + baseURL + '/causes/' + cause.name() + '/competitions/' + comp.name(), comp.getPriority());
	});

	//**CAUSESCOMPANIES** (STATS IF ACTIVE @ DETAIL ACTIVITY)
	var causecompanies = comp.child('causes');
	causecompanies.forEach(function(causecompany) {
		setPrority('https://' + baseURL + '/causescompanies/' + causecompany.val().cause_company_id + '/competitions/' + comp.name(), comp.getPriority());
	});
	//******************************* RESET CAUSECATEGORIES + CATEGORIES .PRIORITIES! (offer can ended before date_end via completion of 'no of winners')
	
	logcat("offer found :" + comp.val().title + " (" + comp.name() + ") - moving to ENDED!");

	cloneNode('https://' + baseURL + '/competitions/' + comp.name(), 'https://' + baseURL + '/offers_ended/' + comp.name(), record, comp.getPriority());
}

function generateNumber(comp) {
	var bidsCount = comp.child('bids').numChildren();

	logcat('try to generateNumber - comp: ' + comp.val().title);
	// console.log('try to generateNumber - comp: ' + comp.val().title);

	if (!bidsCount)//when there is no bids!
	{
		//email admin
		sendMail("alert@x.com", "URGENT : CompetitionEnded without BID", "there is no winner for this competition," + " because no one bidded!<br><br><b>details:</b><br>Competition Title : " + comp.val().title + "<br>Competition ID : " + comp.name() + "<br><br>" + "https://x.firebaseio.com/competitions/" + comp.name());

		////////////////////////////////////// move to ENDED
		//add to competitionsENDED table

		//set competition details from snapshot to an object
		var record = {
			category : comp.val().category,
			category_id : comp.val().category_id,
			comment : comp.val().comment,
			logo : comp.val().logo,
			title : comp.val().title,
			company_id : comp.val().company_id,
			company : comp.val().company,
			date_start : comp.val().date_start,
			date_end : comp.val().date_end,
			causes : comp.val().causes,
			causes_categories : comp.val().causes_categories,
			winners_number : comp.val().winners_number,
			winner : "there are no bids"
		};

		logcat('generateNumber - comp: ' + comp.val().title + " - ran moveCompetition2competitionsENDED");
		// console.log('generateNumber - comp: ' + comp.val().title + " - ran moveCompetition2competitionsENDED");

		cloneNode('https://' + baseURL + '/competitions/' + comp.name(), 'https://' + baseURL + '/competitions_ended/' + comp.name(), record, comp.getPriority());

		return;
	}

	//grab from competition how many winners random.org will draw
	var competition_winnersNumber = comp.child('winners_number').val();

	//result from random.org
	var randomNums;

	//resp status from random.org
	var randomRESP_STATUSCODE;

	//resp error  from random.org
	var randomERROR;

	//when competition_winnersNumber > bids, fix otherwise random.org returns error!
	if (competition_winnersNumber > bidsCount) {
		competition_winnersNumber = bidsCount;

		//email admin and continue to draw
		sendMail("alert@x.com", "URGENT : competition winners is bigger than bidders", "<b>Competition Title :</b>" + comp.val().title + "<br>Competition ID : " + comp.name() + "<br>Winners :" + competition_winnersNumber + "<br>Bids :" + bidsCount);
	}
	// when competition_winnersNumber == bids is ok, but needed to be > 1, otherwise returns error!
	if (bidsCount == 1) {
		logcat("setting fake random.org response and run moveCompetition2ended");
		// console.log("setting fake random.org response and run moveCompetition2ended");

		//set the vars as valid random.org generation
		randomRESP_STATUSCODE = 200;

		randomERROR = null;

		randomNums = [1];

		moveCompetition2ended(comp, bidsCount, randomRESP_STATUSCODE, randomERROR, randomNums)

		return;
	}

	////////////////////////////////////////////////////// RANDOM.ORG [start]
	request({
		method : 'PUT',
		uri : 'https://api.random.org/json-rpc/1/invoke',
		headers : {
			'content-type' : 'application/json-rpc'
		},
		body : JSON.stringify({
			"jsonrpc" : "2.0",
			"method" : "generateSignedIntegers",
			"params" : {
				"apiKey" : "a2dad32f-ceba-4a19-a324-REPLACE with urs", //todo change with purchase
				"n" : competition_winnersNumber,
				"min" : 1,
				"max" : bidsCount,
				"replacement" : false, //by default true - the resulting numbers may contain duplicate values
				"base" : 10
			},
			"id" : 14215333 //A request identifier that allows the client to match responses to request. The service will return this unchanged in its response.
		})
	}, function(error, response, body) {

		var info;

		if (body != null)
			info = JSON.parse(body);

		randomRESP_STATUSCODE = response.statusCode || null;

		if (info) {
			//when contains error array means error!
			if (info.error) {
				randomNums = null;
				randomERROR = 'Error : ' + info.error.code + ' ' + info.error.message;
			} else {

				//check if response is valid
				if (response.statusCode != 200) {
					randomNums = null;
				} else {
					randomNums = info.result.random.data;
				}
			}
		} else {
			logcat("random.org - body is NULL");
			// console.log("random.org - body is NULL");
			randomNums = null;
		}

		moveCompetition2ended(comp, bidsCount, randomRESP_STATUSCODE, randomERROR, randomNums)
	})
	////////////////////////////////////////////////////// RANDOM.ORG [end]
}

function moveCompetition2ended(comp, bidsCount, randomRESP_STATUSCODE, randomERROR, randomNums) {

	//when there is no random.data OR ERROR array exists @ JSON - mail admin and exit!
	if (!randomRESP_STATUSCODE || !randomNums) {
		//email admin
		logcat("URGENT : No valid response from random.org - cant draw winner - email admin");
		// console.log("URGENT : No valid response from random.org - cant draw winner - email admin");

		if (randomERROR)
			sendMail("alert@x.com", "URGENT : No valid response from random.org - cant draw winner", "<b>Competition Title :</b>" + comp.val().title + "<br>Competition ID : " + comp.name() + "<br>Random.org Response Code :" + randomRESP_STATUSCODE + "<br>" + randomERROR);
		else
			sendMail("alert@x.com", "URGENT : No valid response from random.org - cant draw winner", "<b>Competition Title :</b>" + comp.val().title + "<br>Competition ID : " + comp.name() + "<br>Random.org Response Code :" + randomRESP_STATUSCODE);

		return;
	}

	// console.log('moveCompetition2ended - comp: ' + comp.val().Title + " - randoms generated! : " + randomNums);

	//used for draw only
	var biddersCounter = 0;

	//array - holds the drew USERS ID
	var winnerUSERS_ID = [];

	//array - holds the looser USERS ID
	var looserUSERS_ID = [];

	//get bids snapshot (used to find winners only)
	var bidders = comp.child('bids');

	///////////////////////////////////////////////////////////////////////////// draw
	//loop through bidders increase the counter until reach the needed
	//
	//1-loop through generated numbers
	for (var i = 0; i < randomNums.length; i++) {

		biddersCounter = 0;

		//2-loop through bidders
		bidders.forEach(function(bidder) {
			biddersCounter += 1;

			if (biddersCounter == randomNums[i]) {
				logcat(comp.val().title + " - winner - arrPOS:" + i + " person_DB_ID : " + bidder.name());
				// console.log(getDateTime() + " - " + comp.val().title + " - winner - arrPOS:" + i, "person_DB_ID : " + bidder.name());
				winnerUSERS_ID[i] = bidder.name();
				//store userID to array
			} else {
				if (looserUSERS_ID.indexOf(bidder.name()) == -1)
					looserUSERS_ID.push(bidder.name());
			}
		});
	}
	///////////////////////////////////////////////////////////////////////////// draw

	//when there is no elements in array!
	if (winnerUSERS_ID.length == 0) {
		//send email to admin
		sendMail("alert@x.com", "URGENT : Cannot draw the winner", "<b>details:</b><br>Competition Title : " + comp.val().title + "<br>Competition ID : " + comp.name() + "<br><br>this competition left under the tree Competitions<br><br>" + "https://x.firebaseio.com/competitions/" + comp.name() + "<br><br>random server returned : " + randomNums + "<br>bidders in total : " + bidsCount + "<br>there is no much with user!");
		return;
	} else {
		//send email to winners!
		for (var i = 0; i < winnerUSERS_ID.length; i++) {
			logcat("winner-" + looserUSERS_ID[i]);
			// console.log("winner-" + looserUSERS_ID[i])
			getSend_Mail2User_Winner(winnerUSERS_ID[i], comp.val().title, comp.name());

		}

		//send email to loosers!
		for (var i = 0; i < looserUSERS_ID.length; i++) {
			logcat("looser-" + looserUSERS_ID[i]);
			// console.log("looser-" + looserUSERS_ID[i])
			getSend_Mail2User_Looser(looserUSERS_ID[i], comp.val().title, comp.name());

		}

		//email sponsor
		getSend_Mail2_Sponsor(comp.val().company_id, comp.val().title, comp.name());
	}

	//set competition details from snapshot to an object
	var record = {
		category : comp.val().category,
		category_id : comp.val().category_id,
		comment : comp.val().comment,
		logo : comp.val().logo,
		title : comp.val().title,
		company_id : comp.val().company_id,
		company : comp.val().company,
		date_start : comp.val().date_start,
		date_end : comp.val().date_end,
		causes : comp.val().causes,
		causes_categories : comp.val().causes_categories,
		bids : comp.val().bids,
		winners_number : comp.val().winners_number,
		winner_randoms : randomNums,
		winner_users_ids : winnerUSERS_ID
	};

	cloneNode('https://' + baseURL + '/competitions/' + comp.name(), 'https://' + baseURL + '/competitions_ended/' + comp.name(), record, comp.getPriority());
}

/////////////////////////////////////////////////////////////////////////////////////////////
//*COMMON FUNCTIONS* FOR DRAW + CHECK4NEWCOMPETITIONS [START]
/////////////////////////////////////////////////////////////////////////////////////////////

function sendPush2Androids(users_ids, title, pplIDs, competitionID) {
	//user 1@1.net - pipiscrewDeviceID
	if (isDebug) {
		users_ids = "APA91bFiDbWa0Xo9XlyBMXPahF_PnwSsREPLACE with urs";
		logcat("sendPush2Androids - debug -" + pplIDs);
		// console.log("sendPush2Androids triggered for pplIDS " + pplIDs);
	}

	var pplArray = pplIDs.split(',');

	//even without comma enters, nice!
	if (pplArray) {
		//get UTC now
		var newDay = new Date();
		var t = new Date(newDay.toUTCString());
		var time1 = Math.round(t / 1000);
		//get UTC now

		var dbPPL = new Firebase('https://' + baseURL + '/people/');
		var destNode;

		for (var person = 0; person < pplArray.length; person++) {
			// logcat("valid-" + pplArray[person]);
			// console.log("valid-" + pplArray[person]);
			if (pplArray[person].length > 0 && pplArray[person] != ",") {
				// console.log(pplArray[person]);
				//store the push message @ firebase!
				destNode = dbPPL.child(pplArray[person] + "/messages/");

				if (competitionID)
					destNode.push({
						body : title,
						when : time1,
						is_red : "0",
						isWinner : competitionID,
						'.priority' : time1
					});
				else
					destNode.push({
						body : title,
						when : time1,
						is_red : "0",
						'.priority' : time1
					});
			}

		}
	}

	//the the push!
	request({
		method : 'POST',
		uri : androidPushURL,
		headers : {
			'content-type' : 'application/json'
		},
		form : {
			"message" : title,
			"registrationIDs" : users_ids + ","
		}

	}, function(error, response, body) {
		logcat("androidPUSH response : " + body);
		// console.log(body);
	});
}

function send_ios_push_post(categ) {
	request({
		method : 'POST',
		uri : iPhonePushURL,
		headers : {
			'content-type' : 'application/json'
		},
		form : {
			"categ" : categ
		}

	}, function(error, response, body) {
		logcat("iphonePUSH response : " + body)
		// console.log(body);
	});

}

function getDateTime() {
	var now = new Date();
	var year = now.getFullYear();
	var month = now.getMonth() + 1;
	var day = now.getDate();
	var hour = now.getHours();
	var minute = now.getMinutes();
	var second = now.getSeconds();
	if (month.toString().length == 1) {
		var month = '0' + month;
	}
	if (day.toString().length == 1) {
		var day = '0' + day;
	}
	if (hour.toString().length == 1) {
		var hour = '0' + hour;
	}
	if (minute.toString().length == 1) {
		var minute = '0' + minute;
	}
	if (second.toString().length == 1) {
		var second = '0' + second;
	}
	var dateTime = year + '/' + month + '/' + day + ' ' + hour + ':' + minute + ':' + second;
	return dateTime;
}

/////////////////////////////////////////////////////////////////////////////////////////////
//*COMMON FUNCTIONS* FOR DRAW + CHECK4NEWCOMPETITIONS [START]
/////////////////////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//										CHECK4NEWCOMPETITIONS [START]
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function check4newCompetitions() {
	var time2 = time4scan + 600;


	//ask for approved competitions
	var db = new Firebase('https://' + baseURL + '/competitions_approved');

	//1388534400 = 1/1/2014
	logcat("check4newCompetitions - startAT - 1388534400 = 1/1/2014 endAT - " + time2.toString());

	//via UTC now
	// var dbQuery = db.startAt(time4scan.toString()).endAt(time2.toString());
	var dbQuery = db.startAt("1388534400").endAt(time2.toString());

	dbQuery.once('value', function(snapshot) {

		snapshot.forEach(function(competition) {

			/////////////////start
			var record = {
				category : competition.val().category,
				comment : competition.val().comment,
				logo : competition.val().logo,
				title : competition.val().title,
				company_id : competition.val().company_id,
				company : competition.val().company,
				date_start : competition.val().date_start,
				date_end : competition.val().date_end,
				causes : competition.val().causes,
				is_offer : competition.val().is_offer,
				country : competition.val().country
			};

			var record4cause = {
				category : competition.val().category,
				comment : competition.val().comment,
				logo : competition.val().logo,
				title : competition.val().title,
				company_id : competition.val().company_id,
				company : competition.val().company,
				date_start : competition.val().date_start,
				date_end : competition.val().date_end,
				is_offer : competition.val().is_offer,
				country : competition.val().country
			};

			//**CATEGORIES**
			addNode('https://' + baseURL + '/categories/' + competition.val().category_id + '/competitions/' + competition.name(), record, competition.val().country + competition.val().date_end);

			//**CAUSES**
			var causes = competition.child('causes');
			causes.forEach(function(cause) {
				addNode('https://' + baseURL + '/causes/' + cause.name() + '/competitions/' + competition.name(), record4cause, competition.val().country + competition.val().date_end);
			});

			//**CAUSESCOMPANIES**
			var causecompanies = competition.child('causes');
			causecompanies.forEach(function(causecompany) {
				addNode('https://' + baseURL + '/causescompanies/' + causecompany.val().cause_company_id + '/competitions/' + competition.name(), record4cause, competition.val().country + competition.val().date_end);
			});

			//**COMPANIES**
			addNode('https://' + baseURL + '/companies/' + competition.val().company_id + "/competitions/" + competition.name(), {
				category : competition.val().category,
				causes : competition.val().causes,
				logo : competition.val().logo,
				title : competition.val().title,
				is_offer : competition.val().is_offer,
				date_end : competition.val().date_end
			}, competition.val().country + competition.val().date_end);

			//record for causecategories/id/companies
			var causecatCompREC = {
				logo : competition.val().company_logo,
				title : competition.val().company
			}

			//**CAUSE CATEGORIES**
			var causecategories = competition.child('causes_categories');
			causecategories.forEach(function(causecategory) {
				addNode('https://' + baseURL + '/causecategories/' + causecategory.name() + '/competitions/' + competition.name(), record, competition.val().country + competition.val().date_end);
			});

			//**CAUSE CATEGORIES/NodeKEY/causes/competitions **
			causes.forEach(function(cause) {
				var cause_categories = cause.val().cause_categories.toString().split('|');

				for (var c = 0; c < cause_categories.length - 1; c++) {

					addNode('https://' + baseURL + '/causecategories/' + cause_categories[c] + '/causes/' + cause.name() + '/competitions/' + competition.name(), {
						d : "",
						is_offer : competition.val().is_offer,
						company_id : competition.val().company_id
					}, competition.val().country + competition.val().date_end);
				}

				//todo if is only competitions
				// dbQuery.child(competition.name() + "/causes/" + cause.name() + "/" + cause_categories).set = "";
			});

			//**CATEGORIES** id/companies/userid**  -  LAST ADDITION BY DINA
			updateNodeSET('https://' + baseURL + '/categories/' + competition.val().category_id + '/companies/' + competition.val().company_id, causecatCompREC, competition.val().country);

			//**CATEGORIES**2nd the competition key under causecategories/id/companies/userid/comp/newid  -  LAST ADDITION BY DINA
			updateNodeSET('https://' + baseURL + '/categories/' + competition.val().category_id + '/companies/' + competition.val().company_id + '/competitions/' + competition.name(), {
				is_offer : competition.val().is_offer,
				d : ""
			}, competition.val().country + competition.val().date_end);

			//mark it as DONE on competitions aka change priority from 0 to real COUNTRY+DATE_END
			//+ store the who approved (comes from android admin)
			updateNodeSET('https://' + baseURL + '/competitions/' + competition.name(), {
				is_offer : competition.val().is_offer,
				approved_by_admin_id : competition.val().approved_by_admin_id
			}, competition.val().country + competition.val().date_end);

			// setPrority('https://' + baseURL + '/competitions/' + competition.name(), competition.val().country + competition.val().date_end);

			//mark it as DONE on competitions_approved, better delete it, will see!
			setPrority('https://' + baseURL + '/competitions_approved/' + competition.name(), 1);

			/////////////////end
			logcat('check4newCompetitions - competition moved! : ' + competition.val().title);
			// console.log('competition moved! : ' + competition.val().title);

			//TODO : UNREM!
			//push notification IOS + ANDROID
			send_ios_push_post(competition.val().category_id);

			// GRAB THE USERS BELONG TO CORRESPONING CATEGORY AND SEND PUSH FROM THERE
			fetchUsersIDs4AndroidPush(competition.val().category_id, competition.val().title, competition.val().country);

			//email sponsor
			getSend_Mail2_SponsorCompetitionStart(competition.val().company_id, competition.val().title, competition.name());

		});
		//forEach [END]

	});
	//dbQuery [END]
}

function fetchUsersIDs4AndroidPush(category_id, comp_title, country) {

	var dbQuery = new Firebase('https://' + baseURL + '/people');

	var users_ids = "";
	var ppl_ids = "";

	dbQuery.startAt(country).endAt(country).once('value', function(snapshot) {

		snapshot.forEach(function(person) {

			var hasAndroid = person.child('android/deviceID');

			if (hasAndroid.val() != null) {
				var category = person.child('categories/' + category_id);

				if (category.val() != null) {
					if (category.val() == "1") {

						ppl_ids += person.name() + ",";
						//used only for FB/messages node

						users_ids += hasAndroid.val().toString() + ",";
						//used for push is the user android device ID
					}
				}
			}
		});

		sendPush2Androids(users_ids, comp_title, ppl_ids, null);
	});

}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//										CHECK4NEWCOMPETITIONS [END]
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//global vars
var isDebug = true;
var baseURL = "";
var baseURLtoken = "";
var androidPushURL = "";
var iPhonePushURL = "";
var time4scan;

//get UTC now
var newDay;
var t;

/////////////////////////////////////////ENTRY POINT
newDay = new Date();
t = new Date(newDay.toUTCString());

time4scan = Math.round(t / 1000);
//get UTC now

var tmp = " > " + ( isDebug ? "**isDebug**" : "**isPRODUCTION**") + " starting scan for UTC " + time4scan.toString();
logcat(tmp);

// console.log(getDateTime() + " > " + ( isDebug ? "**isDebug**" : "**isPRODUCTION**") + " starting scan for UTC " + time4scan.toString());

if (isDebug) {
	// baseURL = "testarea.firebaseio.com/";
	// baseURLtoken = "L6pZod3BKGbBqMv7FgknDuiXbfnfs4f7hHce5FLi";

	baseURL = "xDEV.firebaseio.com";
	baseURLtoken = "8pr9bocyJ04p3qtBebREPLY with urs";
	androidPushURL = 'http://x.com/sponsorsDEV/pushANDROID.php';
	iPhonePushURL = 'http://x.com/sponsorsDEV/send_ios_push.php';
} else {
	baseURL = "x.firebaseio.com";
	baseURLtoken = "dHQkvzeRv5ZGRYWGogRQREPLY with urs";
	androidPushURL = 'http://x.com/sponsors/pushANDROID.php';
	iPhonePushURL = "http://x.com/sponsors/send_ios_push.php";
	//TODO iphoneURL^
}

//////////// LOGIN ONCE // ON VALID LOGIN RUN THE NEEDED FUNCTIONS
var FirebaseTokenGenerator = require("firebase-token-generator");
var tokenGenerator = new FirebaseTokenGenerator(baseURLtoken);

var token = tokenGenerator.createToken({
	"app_user_id" : 1234,
	"isModerator" : true
});

var db = new Firebase('https://' + baseURL);

db.auth(token, function(error) {
	if (error) {
		logcat("Login Failed! " + error);
	} else {
		logcat("Login Succeeded!");
		draw();
		check4newCompetitions();
	}
});

