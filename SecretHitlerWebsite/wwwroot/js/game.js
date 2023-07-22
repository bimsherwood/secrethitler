
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
        onVote: on("vote")
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
                method: "GET",
                url: baseUrl + "/PassTheFloor",
                data: { playerName },
                error: events.triggerAjaxError
            });
        },
        castVote: function(vote){
            return $.ajax({
                method: "GET",
                url: baseUrl + "/CastVote",
                data: { vote },
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
            $(".hand-title").text("Hand (you have the floor)");
            $.each(game.hand, function(i, e){
                if(e == "Liberal"){
                    $(".hand").eq(i).prop("src", cardBackLiberalUrl);
                } else {
                    $(".hand").eq(i).prop("src", cardBackFascistUrl);
                }
                $(".hand").eq(i).removeClass("d-none");
                $(".hand-label").eq(i).removeClass("d-none");
            });
        } else {
            $(".hand-title").text("(" + game.hasTheFloor + " has the floor)");
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
    });
    events.onGive(function(e){
        var $button = $(e.target);
        var $playerRow = $button.closest(".player-row");
        var playerName = $playerRow.data("player-name");
        ajax.passTheFloor(playerName).then(render);
    });
    events.onVote(function(e){
        var $button = $(e.target);
        var vote = "Undecided";
        if ($button.hasClass("vote-button-yes")){
            vote = "Yes";
        } else if($button.hasClass("vote-button-no")){
            vote = "No";
        }
        ajax.castVote(vote).then(render);
    })
})();

// On start
$(function(){
    $(".vote-button-pane button").click(events.triggerVote);
    ajax.getGameState().then(render);
});
