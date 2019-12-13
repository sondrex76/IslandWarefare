// JavaScript source code used on playfab
handlers.helloWorld = function (args, context) {
    var message = "Hello " + currentPlayerId + "!";
    log.info(message);
    var inputValue = null;
    if (args && args.hasOwnProperty("inputValue"))
        inputValue = args.inputValue;
    log.debug("helloWorld:", { "input": inputValue });
    return { "messageValue": message };
}


handlers.calcAttackTime = function(args){
    
    
    //"ENUMS" for returns
    var INBATTLE = 0;
    var SUCCESS = 1;
    var ZEROSUPPORT = 2;
    
    //Get and parse the warID from player asking to attack
    var warIDData = server.GetUserData({"PlayFabId" : currentPlayerId, "Keys" : ["warID"]});
    var warID = warIDData.Data.warID.Value;
    
    //warID is always equal to zero when not attacking
    if(warID != 0){
        return{"result": INBATTLE};
    }

    //Get the islandID from arguments passed from client
    var islandDefenderID = null;
    if(args && args.hasOwnProperty("islandID")){
        islandDefenderID = args.islandID;
    }
    
    log.info(islandDefenderID);
    
    //Setting paramenters and sending POST request
    var url = 'https://api.mlab.com/api/1/databases/islandwarefare/collections/Island0?apiKey=Kkkn0RruUYR3Iw9lrhS37DcO5rBSA9v0';
	var method = 'get';
	var contentType = "application/json";
	var headers = {};
    var response = http.request(url, method)
    
    //Parse response from http request
    var obj = JSON.parse(response)
		  
    
    var defenderID = null;
    
    //Looping through all islands registerd to find the owner of the island client wants to attack
    for (var i = 0; i < obj.length; i++) {
        var user = obj[i]
        if(user.islandID == islandDefenderID){
            defenderID = user.userID;
            break;
        }
    }

    log.info(obj);

    //Get and parse the support power of the player attaking
    var playerDataAttacker = server.GetUserData({"PlayFabId" : currentPlayerId, "Keys" : ["SupportPower"]});
    var attackerSupplyPower = playerDataAttacker.Data.SupportPower.Value;

    //Get the current time
    var d = new Date(); 
    var timeNow = d.getTime();
    
    
    //Check if attacker can attack
    if(attackerSupplyPower > 0){
        
        //Calculate the actuall attack time in UNIX millisecods
        attackTime = timeNow + ((2000/attackerSupplyPower)^0.9) * 1000;
        
        //Setting up POST request to database
        var urlWar = 'https://api.mlab.com/api/1/databases/islandwarefare/collections/Wars?apiKey=Kkkn0RruUYR3Iw9lrhS37DcO5rBSA9v0';
        var headers = {
        };
        //Store the attackID, defenderID and time of attack in database
        var body = {
            attackerID: currentPlayerId,
            defenderID: defenderID,
            timeOfAttack: attackTime
        };
    
        var content = JSON.stringify(body);
        var httpMethod = "post";
        var contentType = "application/json";
    
        //POST info and get the ID of the attack
        var response = http.request(urlWar, httpMethod, content, contentType, headers);
        warObject = JSON.parse(response);
        
        
        log.info(warObject._id);
        
        //Store the ID of the attack in the players local database for easy access 
        var updateUserDataResult = server.UpdateUserData({
        PlayFabId: currentPlayerId,
        Data:
            {
            "warID": warObject._id.$oid
            }
    });
    
        //Store the ID of the attack in the players local database for easy access 
        var updateUserDataResult = server.UpdateUserData({
        PlayFabId: defenderID,
        Data:
            {
            "warID": warObject._id.$oid
            }
    });
        
        //Tell client it was a success
        return {"result": SUCCESS}

    }
    
    //Tell client it has zero SupportPower
    return {"result": ZEROSUPPORT}

}

//Returns time of battle in unix time
handlers.checkBattleTime = function(args){
    
    var warIDData = server.GetUserData({"PlayFabId" : currentPlayerId, "Keys" : ["warID"]});
    var warID = warIDData.Data.warID.Value;
    
    
       if(warID == 0){
        return{"timeOfAttack": "noBattle"}
    }
    
    
    var url = 'https://api.mlab.com/api/1/databases/islandwarefare/collections/Wars/' + warID + '?apiKey=Kkkn0RruUYR3Iw9lrhS37DcO5rBSA9v0';
		
	var method = 'get';
	
	var contentType = "application/json";
	
	var headers = {};

    var response = http.request(url, method);
    
    var obj = JSON.parse(response);
    
    log.info(obj.winner);
    
    
  
    
    if(obj.winner == "cancelled"){
        
        var defenderID = obj.defenderID;
        var attackerID = obj.attackerID;
        
        var updateUserDataResult = server.UpdateUserData({
        PlayFabId: attackerID,
        Data:
            {
            "warID": 0
            }
        });
    
        //Store the ID of the attack in the players local database for easy access 
        var updateUserDataResult = server.UpdateUserData({
        PlayFabId: defenderID,
        Data:
            {
            "warID": 0
            }
        });
        
        return {"timeOfAttack": "cancelled"}
    }
    
    var timeOfAttack = obj.timeOfAttack;
    
    var d = new Date(); 
    var timeNow = d.getTime();
    
    var diff = timeOfAttack - timeNow;
    
    
   
    
    
    return {"timeOfAttack": diff}
    
    
}

//Calculates the winner of a battle 
handlers.calcWinner = function(args){
    
    //Get the warID of client
    var warIDData = server.GetUserData({"PlayFabId" : currentPlayerId, "Keys" : ["warID"]});
    var warID = warIDData.Data.warID.Value;
    
    //Return empty if there is no war
    if(warID == 0){
        return{"result": ""}
    }
    
    //Get the document with the war stats
    var url = 'https://api.mlab.com/api/1/databases/islandwarefare/collections/Wars/' + warID + '?apiKey=Kkkn0RruUYR3Iw9lrhS37DcO5rBSA9v0';
	var method = 'get';
	var contentType = "application/json";
	var headers = {};
    var response = http.request(url, method)
    
    
    var obj = JSON.parse(response)
    
    log.info(response)
    
    var winner = null;
    
    //Get the winner of the battle
    if(obj.winner == null){
    
    
        //Get ID's of players
        var defenderID = obj.defenderID;
        var attackerID = obj.attackerID;
    
        //Get defence and attack power
        var playerDataAttacker = server.GetUserData({"PlayFabId" : attackerID, "Keys" : ["AttackPower"]});
        var playerDataDefender = server.GetUserData({"PlayFabId" : defenderID, "Keys" : ["DefencePower"]});
        var attackerPower = playerDataAttacker.Data.AttackPower.Value;
        var defencePower = playerDataDefender.Data.DefencePower.Value;
        
        //Set the winner ID
        if(attackerPower > defencePower){
            winner = attackerID;
        } else{
            winner = defenderID;
        }
        
        //Set the winner in the database
       var headers = {
        };
        
        var body = {
            $set : {winner: winner}
        };
        
        var content = JSON.stringify(body);
        var httpMethod = "put";
        var contentType = "application/json";
        var response = http.request(url, httpMethod, content, contentType, headers);
        warObject = JSON.parse(response);
        
        
        log.info(response);
        

    } else {
        winner = obj.winner
    }
    
    
    //Set the warID to zero
    var updateUserDataResult = server.UpdateUserData({
    PlayFabId: currentPlayerId,
    Data:
        {
        "warID": 0
        }
    });
    
    //Return the result to the client
    if(winner == currentPlayerId){
        return {"result": "You won the battle"}
    } else return{"result": "You lost the battle"}
    
    
}


handlers.cancleAttack = function(args){
    
    //Get the warID of client
    var warIDData = server.GetUserData({"PlayFabId" : currentPlayerId, "Keys" : ["warID"]});
    var warID = warIDData.Data.warID.Value;
    
    var url = 'https://api.mlab.com/api/1/databases/islandwarefare/collections/Wars/' + warID + '?apiKey=Kkkn0RruUYR3Iw9lrhS37DcO5rBSA9v0';
	var method = 'get';
	var contentType = "application/json";
	var headers = {};
    var response = http.request(url, method)
    
    var obj = JSON.parse(response)
    
    var defenderID = obj.defenderID;
    var attackerID = obj.attackerID;
    
    //Cancle the attack if the player asking is attacking
    if(currentPlayerId == attackerID){
        
               var headers = {
        };
        
        var body = {
            $set : {winner: "cancelled"}
        };
        
        var content = JSON.stringify(body);
        var httpMethod = "put";
        var contentType = "application/json";
        var response = http.request(url, httpMethod, content, contentType, headers);
        warObject = JSON.parse(response);
        
    }
    
}

handlers.addPlayerToDataBase = function(args){
    
    
    
    
    var url = 'https://F5079.playfabapi.com/Server/GetPlayersInSegment';
        var headers = {"X-SecretKey": "BFAUNKU49AITMY3KQCNNZ7YONC7BCS87NFOPP6Q9CQEYWCTGUX"
        };
        //Store the attackID, defenderID and time of attack in database
        var body = {
            "SegmentId": "2C3F0B4487C23F69",
            "SecondsToLive": "5",
            "MaxBatchSize": "500"
        };
    
        var content = JSON.stringify(body);
        var httpMethod = "post";
        var contentType = "application/json";
    
        //POST info and get the ID of the attack
        var response = http.request(url, httpMethod, content, contentType, headers);
        playerInSegmentData = JSON.parse(response);
    
    
    log.info(playerInSegmentData);
    
    
    
    
    
    var updateUserDataResult = server.UpdateUserData({
        PlayFabId: currentPlayerId,
        Data:
            {
            "IslandID": playerInSegmentData.data.ProfilesInSegment-1,
            "SupportPower": 5,
            "AttackPower": 5,
            "DefencePower": 5,
            "warID": 0
            }
    });
    
    
    
    var islandIDData = server.GetUserData({"PlayFabId" : currentPlayerId, "Keys" : ["IslandID"]});
    var islandID = islandIDData.Data.IslandID.Value;
    
    var url = 'https://api.mlab.com/api/1/databases/islandwarefare/collections/Island0?apiKey=Kkkn0RruUYR3Iw9lrhS37DcO5rBSA9v0';
        var headers = {
        };
        //Store the attackID, defenderID and time of attack in database
        var body = {
            userID: currentPlayerId,
            islandID: islandID,
        };
    
        var content = JSON.stringify(body);
        var httpMethod = "post";
        var contentType = "application/json";
    
        //POST info and get the ID of the attack
        var response = http.request(url, httpMethod, content, contentType, headers);
        warObject = JSON.parse(response);
    
    
}
