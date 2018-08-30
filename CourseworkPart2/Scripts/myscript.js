
///MAIN BODY INDEX Jquery //


$(document).ready(function () {
    //for start cause form reduces the number of words remaining displayed below the input box.
    $(".causeInput").on("input", function () {
        var inputWord = $(".causeInput").val().length;
        $("#textBeneath").text(80 - inputWord + " characters remaining");
    });


  //on click see who signed here Link which will display a list of cause signees.
  $(".signees").on("click",function (event)
  {
    event.preventDefault();
    var seeWhoLink =$(this).closest(".cause").find(".seeWho");
    var fullSigs =$(this).closest(".cause").find(".petitionSigners");
    var seeLessOption = $(this).closest(".cause").find(".seeLess");
    //check to see if seeWho link is viewable, if so it hides it, and displays the signatures and the 'seeless' link instead.
    if(seeWhoLink.css("display") == "inline") {
      seeWhoLink.css("display", "none");
      seeLessOption.css("display", "inline");
      fullSigs.css("display", "inline");
    } 
  }); //end on click to view signatures


  //on click to minimize signatures. Hides the seeless link and signatures and displays the seeWho link again.
  $(".seeLess").on("click",function (event) {
    event.preventDefault();
    var seeWhoLink =$(this).closest(".cause").find(".seeWho");
    var fullSigs =$(this).closest(".cause").find(".petitionSigners");
    var seeLessOption = $(this).closest(".cause").find(".seeLess");
    fullSigs.css("display", "none");
    fullSigs.html();
    seeWhoLink.css("display", "inline");
    seeLessOption.css("display", "none");
  }); //end seeless button Click



  //searchPetition Page
  //on search button click
  $("#findSubmit").on("click",function (event) {
    var searchCriteria = $("#findInput").val();
    //ensure some search text has been added or display a message saying that nothing has been entered.
    if (searchCriteria.length == 0) {
        event.preventDefault();
        $("#emptyMessage").css("display", "inline");
    }
    else {
        $("#emptyMessage").css("display", "none");
    }
  });
}); //end doc READY


