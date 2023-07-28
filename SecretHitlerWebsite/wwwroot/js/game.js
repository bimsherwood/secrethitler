
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
        triggerMarkPlayer: trigger("give"),
        onMarkPlayer: on("give"),
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
        markPlayer: function(playerName){
            return $.ajax({
                method: "POST",
                url: baseUrl + "/MarkPlayer",
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
            if(game.playerMarked == e.name){
                $playerRow.find(".player-marker").addClass("active");
            }
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
            $playerRow.find(".player-marker").click(events.triggerMarkPlayer);
            $playerRow.find(".player-give-button button").click(events.triggerGive);
        });
    }

    function renderDeck(game){
        $(".draw-pile-count").text(game.drawPileSize);
        if(game.currentPlayer == game.hasTheFloor){
            $(".deck-draw-three-button").removeClass("d-none");
            $(".deck-draw-button").removeClass("d-none");
            $(".discard-pile-undiscard-button").removeClass("d-none");
            $(".undo-policy-button").removeClass("d-none");
        } else {
            $(".deck-draw-three-button").addClass("d-none");
            $(".deck-draw-button").addClass("d-none");
            $(".discard-pile-undiscard-button").addClass("d-none");
            $(".undo-policy-button").addClass("d-none");
        }
    }

    function renderDiscardPile(game){
        $(".discard-pile-count").text(game.discardPileSize);
    }

    function renderPolicies(game){
        $(".policy").prop("src", cardBackUrl);
        $(".policy-liberal").slice(0, game.liberalPolicyPassed).prop("src", cardBackLiberalUrl);
        $(".policy-fascist").slice(0, game.fascistPolicyPassed).prop("src", cardBackFascistUrl);
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

var toast = (function(){

    function addToast(message){
        var $template = $("#toast-template").html();
        var $toast = $($template);
        $toast.find(".toast-body").text(message);
        $(".toast-container").append($toast);
        $toast.toast('show');
    }

    function preventSpam(){
        var $allToasts = $(".toast.show");
        var toastCount = $allToasts.length;
        if(toastCount > 8){
            $allToasts.slice(0, toastCount - 8).toast("hide");
        }
    }

    return function(message){
        addToast(message);
        preventSpam();
    }
})();

// Bindings
(function(){

    function signalRNotifyForMessage(message){
        return function(){
            events.triggerSignalRNotify(message);
        }
    }

    events.onAjaxError(function(xhr, textStatus, errorThrown){
        console.log("An error has occurred: " + errorThrown);
        $(".connection-error").removeClass("d-none");
    });
    events.onGive(function(e){
        var $button = $(e.target);
        var $playerRow = $button.closest(".player-row");
        var playerName = $playerRow.data("player-name");
        ajax.passTheFloor(playerName).then(signalRNotifyForMessage("Gave to " + playerName));
    });
    events.onMarkPlayer(function(e){
        var $marker = $(e.target);
        var $playerRow = $marker.closest(".player-row");
        var playerName = $playerRow.data("player-name");
        ajax.markPlayer(playerName).then(signalRNotifyForMessage("Marked " + playerName));
    });
    events.onVote(function(e){
        var $button = $(e.target);
        var vote = "Undecided";
        var message = "Cleared votes";
        if ($button.hasClass("vote-button-yes")){
            vote = "Yes";
            message = "Voted";
        } else if($button.hasClass("vote-button-no")){
            vote = "No";
            message = "Voted";
        }
        ajax.castVote(vote).then(signalRNotifyForMessage(message));
    });
    events.onDrawFromDeck(function(e){
        ajax.drawFromDeck().then(signalRNotifyForMessage("Drew"));
    });
    events.onDrawThree(function(e){
        ajax.drawThree().then(signalRNotifyForMessage("Drew 3"));
    });
    events.onReplaceOnDeck(function(e){
        var $button = $(e.target);
        var index = $button.data("hand-index");
        ajax.replaceOnDeck(index).then(signalRNotifyForMessage("Put back"));
    });
    events.onDiscard(function(e){
        var $button = $(e.target);
        var index = $button.data("hand-index");
        ajax.discard(index).then(signalRNotifyForMessage("Discarded"));
    });
    events.onUnDiscard(function(e){
        ajax.unDiscard().then(signalRNotifyForMessage("Un-discarded"));
    });
    events.onPassPolicy(function(e){
        ajax.passPolicy().then(signalRNotifyForMessage("Passed policy"));
    });
    events.onUndoPolicy(function(e){
        var $button = $(e.target);
        var policy = $button.data("policy");
        ajax.undoPolicyToHand(policy).then(signalRNotifyForMessage("Repealed policy"));
    });
    events.onPassTopPolicy(function(e){
        ajax.passTopPolicy().then(signalRNotifyForMessage("Passed top policy"));
    });

    events.onSignalRFailure(function(ex){
        $(".connection-error").removeClass("d-none");
    });
    events.onSignalRSuccess(function(){
        console.log("SignalR Connected");
    });
    events.onSignalRReceived(function(notification){
        console.log("Notification", notification);
        toast(notification);
        ajax.getGameState().then(render);
    });

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

    events.onSignalRNotify(function(message){
        connection.invoke("NotifyUpdate", message);
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
