"use strict";
window.onload = Init();//толком не пащет , норм только потому что скрипт внизу хтмл страницы
let ID = 0;
function Init() {
    RegisterModal();
}


function RegisterModal() {
    var modal = document.getElementById("Modal");
    var span = document.getElementsByClassName("closeModal")[0];
    var btns = document.querySelectorAll('.LeftComment');
    //console.dir(btns);
    btns.forEach(function (t) {
        t.addEventListener('click', function () {
            modal.style.display = "block";
            //console.dir(this.parentElement.parentElement.lastElementChild.innerText);
             //ID = this.parentElement.parentElement.lastElementChild.innerText;
            ID = this.parentElement.parentElement;

        }, false);
    });

    span.onclick = function () {
        modal.style.display = "none"; ID = 0;
    };
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    };
}

function Publish() {
    //console.dir($("#email").val())
    //if (!ValidateEmail(mail)) {  return;    }
    //if (!ValidateNick($("#nick").val())) { return; }
    //if (!Validate($("#capcha").val())) { return; }
    ClientValidateSuccess();
}

function ValidateEmail(mail) {
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail)) {
        return (true);
    }
    alert("You have entered an invalid email address!");
    return (false);
}
function ValidateNick(t) {
    if (t.length<3) {
        alert("You NickName is too short");
        return (false);
    }
    if (/\d+/.test(t)) {
        alert("You NickName Cannot contain just numbers");
        return (false);
    }
    return (true);
}
function Validate(t) {
    if (t.length !=4) {
        alert("You picture code must have 4 numbers");
        return (false);
    }
    return (true);
}
function ClientValidateSuccess() {
    document.getElementById("Modal").style.display = "none";
    createJSONforModal();
    //window.location.href = '/';
}
function createJSONforModal() {
    var JSON_obj = [];
    var tmp = document.querySelectorAll('div.row');
    //console.dir($("#homepage").val());
    //console.dir(getStyle(ID, 'margin-left').substr(0, getStyle(ID, 'margin-left').length - 2) / convertRemToPixels(4));
    var item = {};
    item["Id"] = 0;
    item["DateComment"] = null;
    item["Title"] = $("#topic").val();
    item["Message"] = $("#message").val();
    item["LVL"] = getStyle(ID, 'margin-left').substr(0, getStyle(ID, 'margin-left').length - 2) / convertRemToPixels(4);
    item["UserName"] = tmp[0].children[1].value;
    item["Email"] = tmp[1].children[1].value;
    item["HomePage"] = tmp[2].children[1].value;
    item["Captcha"] = $("#capcha").val();
    item["IpAddres"] = "OnServer";
    if (ID.lastElementChild.innerText!="") {
        item["Parent_Id"] = ID.lastElementChild.innerText;
    }
    else {
        item["Parent_Id"] = null;
    }
    JSON_obj.push(item); 
    //console.dir(JSON_obj);

    jQuery.extend({
        postJSON: function (dt, callback) {
            return jQuery.ajax({
                type: "POST",
                url: "/Home/Publish",
                data: JSON.stringify({ 'data': dt}),
                success: callback,
                dataType: "json",
                contentType: "application/json",
                processData: false
            });
        }
    });
    $.postJSON(JSON_obj[0], Success());
}

function Success() {
    //console.dir("Success");
}

var getStyle = function (e, styleName) {
    var styleValue = "";
    if (document.defaultView && document.defaultView.getComputedStyle) {
        styleValue = document.defaultView.getComputedStyle(e, "").getPropertyValue(styleName);
    }
    else if (e.currentStyle) {
        styleName = styleName.replace(/\-(\w)/g, function (strMatch, p1) {
            return p1.toUpperCase();
        });
        styleValue = e.currentStyle[styleName];
    }
    return styleValue;
}

function convertRemToPixels(rem) {
    return rem * parseFloat(getComputedStyle(document.documentElement).fontSize);
}
