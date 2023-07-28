
function copyText(text){
    navigator.clipboard.writeText(text);
    $(".copied-indicator").removeClass("d-none");
}

function render(lobbyState){

    // Refresh player list
    $(".player-list").empty();
    $.each(lobbyState.players, function(i, e){
        var $li = $("<li></li>");
        $li.text(e);
        $(".player-list").append($li);
    });

    // Hide or show the start button
    if(lobbyState.players.length >= 5){
        $(".start-button").removeClass("d-none");
    } else {
        $(".start-button").addClass("d-none");
    }

    // If the game is started, reload the page and get redirected
    if(lobbyState.gameStarted){
        location.reload();
    }

}

function getLobbyState(){
    return $.ajax({
        method: "GET",
        url: baseUrl + "/LobbyState"
    });
}

function postStartGame(){
    return $.ajax({
        method: "POST",
        url: baseUrl + "/StartGame"
    });
}

function onRefreshLobbyState(){
    getLobbyState().then(render);
}

$(function(){

    // Connect to SignalR and announce arrival
    connection = new signalR.HubConnectionBuilder().withUrl(signalRUrl).build();
    connection.on("ReceiveUpdate", onRefreshLobbyState);
    connection
        .start()
        .then(function(){ connection.invoke("NotifyUpdate"); });
    
    // Link up the Start Game event
    $(".start-button").on("click", function(){
        postStartGame().then(function(){ connection.invoke("NotifyUpdate"); });
    })

});