
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
        }
    };

})();

var render = (function(){

    function renderPlayers(game){
        $(".player-list").empty();
        $.each(game.players, function(i, e){
            var $template = $("#player-row-template").html();
            var $playerRow = $($template);
            $(".player-list").append($playerRow);
            $playerRow.find(".player-row").data("player-name", e);
            $playerRow.find(".player-name span").text(e);
        });
    }

    function renderDeck(game){
        $(".draw-pile-count").text(game.drawPileSize);
    }

    function renderDiscardPile(game){
        $(".discard-pile-count").text(game.discardPileSize);
    }

    return function(game){
        renderPlayers(game);
        renderDeck(game);
        renderDiscardPile(game);
    };

})();

$(function(){
    ajax.getGameState().then(function(game){
        setTimeout(function(){ render(game); }, 3000);
    });
});
