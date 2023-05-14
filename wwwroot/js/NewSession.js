
$(function(){
    $(".add-button").on("click", onAddButtonClicked);
    $(".remove-button").on("click", onRemoveButtonClicked);
})

function onAddButtonClicked(e){

    var inputIndex = $(".minister-names .input-group").length;

    $label = $("<label></label>");
    $label.addClass("input-group");
    $label.addClass("mb-3");

    $input = $("<input></input>");
    $input.addClass("form-control");
    $input.attr("id", "MinisterNames_" + inputIndex + "_");
    $input.attr("name", "MinisterNames[" + inputIndex + "]");
    $input.attr("type", "text");

    $button = $("<button></button>");
    $button.addClass("btn");
    $button.addClass("btn-outline-secondary");
    $button.addClass("remove-button");
    $button.attr("type", "button");
    $button.text("Remove");

    $(".minister-names").append($label);
    $label.append($input);
    $label.append($button);

    $button.on("click", onRemoveButtonClicked);

}

function onRemoveButtonClicked(e){
    $inputGroup = $(e.target).closest(".input-group");
    $inputGroup.find("input").val("");
    $inputGroup.hide();
}
