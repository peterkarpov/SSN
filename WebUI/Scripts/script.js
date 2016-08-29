//dropdown-submenu
$(document).ready(function () {
    $('.dropdown-submenu a.test').on("click", function(e){
        $(this).next('ul').toggle();
        e.stopPropagation();
        e.preventDefault();
    });
});

// Comment auto update
function AjaxAddCommentSend(TargetId) {

    $('#' + TargetId).load('/Comments/CommentsOnTarget?TargetId=' + TargetId, {
        asd: Math.random() // что б IE не кешировал
    });

}

// Like scripts
function SetLike(TargetId) {

    //console.log(TargetId);

    $.ajax({
        type: "GET",
        url: "/Like/AjaxSetLike",
        data: { jsonData: TargetId },
        //beforeSend: function () {
        //    /* что-то сделать перед */
        //},
        //success: function () {
        //    /* обработать результат */
        //},
        //error: function () {
        //    /* обработать ошибку */
        //}
    });

    $.ajax({
        type: "GET",
        url: "/Like/AjaxGetLike",
        data: { jsonData: TargetId },
        success: function (data) {
            //console.log(data);
            $('#GetLike' + TargetId).text(data);
        },
    });
}

// Dislike scripts
function SetDislike(TargetId) {

    $.ajax({
        type: "GET",
        url: "/Like/AjaxSetDislike",
        data: { jsonData: TargetId },
    });

    $.ajax({
        type: "GET",
        url: "/Like/AjaxGetDislike",
        data: { jsonData: TargetId },
        success: function (data) {
            $('#GetDislike' + TargetId).text(data);
        },
    });
}


// changeVid
function changeVid() {
    $("#itemVisibleSmall").toggleClass("hidden");
    $("#itemVisibleBig").toggleClass("hidden");
    $("#itemVisibleGallery").toggleClass("hidden");
}

function ViewBig() {
    $("#itemVisibleBig").removeClass("hidden");

    $("#itemVisibleSmall").addClass("hidden");
    $("#itemVisibleGallery").addClass("hidden");
}

function ViewSmall() {
    $("#itemVisibleSmall").removeClass("hidden");

    $("#itemVisibleBig").addClass("hidden");
    $("#itemVisibleGallery").addClass("hidden");
}

function ViewGallery() {
    $("#itemVisibleGallery").removeClass("hidden");

    $("#itemVisibleSmall").addClass("hidden");
    $("#itemVisibleBig").addClass("hidden");
}

// show\hide
function anichange(objName) {
    if ($(objName).css('display') == 'none') {
        $(objName).animate({ height: 'show' }, 400);
    } else {
        $(objName).animate({ height: 'hide' }, 200);
    }
}