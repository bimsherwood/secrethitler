
function copyText(text){
    navigator.clipboard.writeText(text);
    $(".copied-indicator").removeClass("d-none");
}