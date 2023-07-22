
var ajax = (function(){

    function handleError(xhr, textStatus, errorThrown){
        console.log("An error has occurred: " + errorThrown);
    }

    return {
        getGameState: function(){
            return $.ajax({
                method: "GET",
                url: baseUrl + "/GameState",
                error: handleError
            });
        },
        passTheFloor: function(playerName){
            return $.ajax({
                method: "GET",
                url: baseUrl + "/PassTheFloor",
                data: { playerName },
                error: handleError
            });
        }
    };

})();

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
        triggerGive: trigger("give"),
        onGive: on("give")
    };

})();

var render = (function(){

    function renderPlayers(game){
        $(".player-list").empty();
        $.each(game.players, function(i, e){
            var $template = $("#player-row-template").html();
            var $playerRow = $($template);
            $(".player-list").append($playerRow);
            $playerRow.data("player-name", e);
            $playerRow.find(".player-name span").text(e);
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
    events.onGive(function(e){
        var $button = $(e.target);
        var $playerRow = $button.closest(".player-row");
        var playerName = $playerRow.data("player-name");
        ajax.passTheFloor(playerName).then(render);
    });
})();

// On start
$(function(){
    ajax.getGameState().then(render);
});
