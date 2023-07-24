
var events = (function(){

    var handlers = { };

    function on(eventName){
        return function(f){
            handlers[eventName] = f;
        }
    }

    function trigger(eventName){
        return function(...args){
            handlers[eventName](...args);
        }
    }

    return {
        triggerAjaxError: trigger("ajaxError"),
        onAjaxError: on("ajaxError"),
        triggerGive: trigger("give"),
        onGive: on("give"),
        triggerVote: trigger("vote"),
        onVote: on("vote"),
        triggerDrawFromDeck: trigger("drawFromDeck"),
        onDrawFromDeck: on("drawFromDeck"),
        triggerDrawThree: trigger("drawThree"),
        onDrawThree: on("drawThree"),
        triggerReplaceOnDeck: trigger("replaceOnDeck"),
        onReplaceOnDeck: on("replaceOnDeck"),
        triggerDiscard: trigger("discard"),
        onDiscard: on("discard"),
        triggerUnDiscard: trigger("unDiscard"),
        onUnDiscard: on("unDiscard"),
        triggerPassPolicy: trigger("passPolicy"),
        onPassPolicy: on("passPolicy"),
        triggerUndoPolicy: trigger("undoPolicy"),
        onUndoPolicy: on("undoPolicy"),
        triggerPassTopPolicy: trigger("passTopPolicy"),
        onPassTopPolicy: on("passTopPolicy"),
        triggerSignalRSuccess: trigger("signalRSuccess"),
        onSignalRSuccess: on("signalRSuccess"),
        triggerSignalRFailure: trigger("signalRFailure"),
        onSignalRFailure: on("signalRFailure"),
        triggerSignalRReceived: trigger("signalRReceived"),
        onSignalRReceived: on("signalRReceived"),
        triggerSignalRNotify: trigger("signalRNotify"),
        onSignalRNotify: on("signalRNotify")
    };

})();

var ajax = (function(){

    return {
        getGameState: function(){
            return $.ajax({
                method: "GET",
                url: baseUrl + "/GameState",
                error: events.triggerAjaxError
            });
        },
        passTheFloor: function(playerName){
            return $.ajax({
                method: "POST",
                url: baseUrl + "/PassTheFloor",
                data: { playerName },
                error: events.triggerAjaxError
            });
        },
        castVote: function(vote){
            return $.ajax({
                method: "POST",
                url: baseUrl + "/CastVote",
                data: { vote },
                error: events.triggerAjaxError
            });
        },
        drawFromDeck: function(){
            return $.ajax({
                method: "POST",
                url: baseUrl + "/DrawFromDeck",
                error: events.triggerAjaxError
            });
        },
        drawThree: function(){
            return $.ajax({
                method: "POST",
                url: baseUrl + "/DrawThree",
                error: events.triggerAjaxError
            });
        },
        replaceOnDeck: function(index){
            return $.ajax({
                method: "POST",
                url: baseUrl + "/ReplaceOnDeck",
                data: { index },
                error: events.triggerAjaxError
            });
        },
        discard: function(index){
            return $.ajax({
                method: "POST",
                url: baseUrl + "/Discard",
                data: { index },
                error: events.triggerAjaxError
            });
        },
        unDiscard: function(){
            return $.ajax({
                method: "POST",
                url: baseUrl + "/UnDiscard",
                error: events.triggerAjaxError
            });
        },
        passPolicy: function(){
            return $.ajax({
                method: "POST",
                url: baseUrl + "/PassPolicy",
                error: events.triggerAjaxError
            });
        },
        undoPolicyToHand: function (policy){
            return $.ajax({
                method: "POST",
                url: baseUrl + "/UndoPolicyToHand",
                data: { policy },
                error: events.triggerAjaxError
            });
        },
        passTopPolicy: function (){
            return $.ajax({
                method: "POST",
                url: baseUrl + "/PassTopPolicy",
                error: events.triggerAjaxError
            });
        },
    };

})();

var render = (function(){

    function renderPlayers(game){
        $(".player-list").empty();
        $.each(game.players, function(i, e){
            var $template = $("#player-row-template").html();
            var $playerRow = $($template);
            $(".player-list").append($playerRow);
            $playerRow.data("player-name", e.name);
            $playerRow.find(".player-name span").text(e.name);
            if(e.vote == "Yes"){
                $playerRow.find(".player-vote-indicator span").addClass("vote-yes");
                $playerRow.find(".player-vote-indicator span").text("Yes");
            } else if(e.vote == "No"){
                $playerRow.find(".player-vote-indicator span").addClass("vote-no");
                $playerRow.find(".player-vote-indicator span").text("No");
            }
            if (e.alliance != null){
                $playerRow.find(".player-alliance-indicator span").text(e.alliance);
                $playerRow.find(".player-alliance-indicator").removeClass("d-none");
            } else {
                $playerRow.find(".player-alliance-indicator").addClass("d-none");
            }
            $playerRow.find(".player-give-button button").click(events.triggerGive);
        });
    }

    function renderDeck(game){
        $(".draw-pile-count").text(game.drawPileSize);
    }

    function renderDiscardPile(game){
        $(".discard-pile-count").text(game.discardPileSize);
    }

    function renderPolicies(game){
        $(".policy").prop("src", cardBackUrl);
        $(".policy-liberal").slice(0,game.liberalPolicyPassed).prop("src", cardBackLiberalUrl);
        $(".policy-fascist").slice(0,game.fascistPolicyPassed).prop("src", cardBackFascistUrl);
    }

    function renderHand(game){
        $(".hand").addClass("d-none");
        $(".hand-label").addClass("d-none");
        if(game.currentPlayer == game.hasTheFloor){
            $(".hand-title").text("You have the floor");
            $.each(game.hand, function(i, e){
                if(e == "Liberal"){
                    $(".hand").eq(i).prop("src", cardBackLiberalUrl);
                } else {
                    $(".hand").eq(i).prop("src", cardBackFascistUrl);
                }
                $(".hand").eq(i).removeClass("d-none");
                $(".hand-label").eq(i).removeClass("d-none");
            });
            if (game.hand.length == 1){
                $(".pass-policy-button").removeClass("d-none");
            } else {
                $(".pass-policy-button").addClass("d-none");
            }
        } else {
            $(".hand-title").text(game.hasTheFloor + " has the floor");
        }
    }

    function renderRole(game){
        if(game.currentPlayerRole == "Liberal"){
            $(".role-card").prop("src", cardBackLiberalUrl);
        } else {
            $(".role-card").prop("src", cardBackFascistUrl);
        }
        $(".role-annotation").text(game.currentPlayerRole);
    }

    return function(game){
        renderPlayers(game);
        renderDeck(game);
        renderDiscardPile(game);
        renderPolicies(game);
        renderHand(game);
        renderRole(game);
    };

})();

// Bindings
(function(){
    events.onAjaxError(function(xhr, textStatus, errorThrown){
        console.log("An error has occurred: " + errorThrown);
        $(".connection-error").removeClass("d-none");
    });
    events.onGive(function(e){
        var $button = $(e.target);
        var $playerRow = $button.closest(".player-row");
        var playerName = $playerRow.data("player-name");
        ajax.passTheFloor(playerName).then(events.triggerSignalRNotify);
    });
    events.onVote(function(e){
        var $button = $(e.target);
        var vote = "Undecided";
        if ($button.hasClass("vote-button-yes")){
            vote = "Yes";
        } else if($button.hasClass("vote-button-no")){
            vote = "No";
        }
        ajax.castVote(vote).then(events.triggerSignalRNotify);
    });
    events.onDrawFromDeck(function(e){
        ajax.drawFromDeck().then(events.triggerSignalRNotify);
    });
    events.onDrawThree(function(e){
        ajax.drawThree().then(events.triggerSignalRNotify);
    });
    events.onReplaceOnDeck(function(e){
        var $button = $(e.target);
        var index = $button.data("hand-index");
        ajax.replaceOnDeck(index).then(events.triggerSignalRNotify);
    });
    events.onDiscard(function(e){
        var $button = $(e.target);
        var index = $button.data("hand-index");
        ajax.discard(index).then(events.triggerSignalRNotify);
    });
    events.onUnDiscard(function(e){
        ajax.unDiscard().then(events.triggerSignalRNotify);
    });
    events.onPassPolicy(function(e){
        ajax.passPolicy().then(events.triggerSignalRNotify);
    });
    events.onUndoPolicy(function(e){
        var $button = $(e.target);
        var policy = $button.data("policy");
        ajax.undoPolicyToHand(policy).then(events.triggerSignalRNotify);
    });
    events.onPassTopPolicy(function(e){
        ajax.passTopPolicy().then(events.triggerSignalRNotify);
    });

    events.onSignalRFailure(function(ex){
        $(".connection-error").removeClass("d-none");
    });
    events.onSignalRSuccess(function(){
        console.log("SignalR Connected");
    });
    events.onSignalRReceived(render);

})();

// SignalR
var gameHub = (function(){

    var connection = null;

    function connect(){
        connection = new signalR.HubConnectionBuilder().withUrl(signalRUrl).build();
        connection.on("ReceiveUpdate", events.triggerSignalRReceived);
        connection.onclose(events.triggerSignalRFailure);
        connection
            .start()
            .then(events.triggerSignalRSuccess)
            .catch(events.triggerSignalRFailure);
    }

    events.onSignalRNotify(function(){
        connection.invoke("NotifyUpdate");
    });

    return {
        connect
    };

})();

// On start
$(function(){
    $(".vote-button-pane button").click(events.triggerVote);
    $(".deck-draw-button").click(events.triggerDrawFromDeck);
    $(".deck-draw-three-button").click(events.triggerDrawThree);
    $(".discard-pile-undiscard-button").click(events.triggerUnDiscard);
    $(".discard-button").click(events.triggerDiscard);
    $(".replace-button").click(events.triggerReplaceOnDeck);
    $(".pass-policy-button").click(events.triggerPassPolicy);
    $(".undo-policy-button").click(events.triggerUndoPolicy);
    $(".pass-top-policy-button").click(events.triggerPassTopPolicy);
    gameHub.connect();
    ajax.getGameState().then(render);
});
