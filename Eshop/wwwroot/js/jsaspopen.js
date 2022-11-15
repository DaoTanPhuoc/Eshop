var message = "NoRightClicking"; function defeatIE() { if (document.all) { (message); return false; } } function defeatNS(e) { if (document.layers || (document.getElementById && !document.all)) { if (e.which == 2 || e.which == 3) { (message); return false; } } } if (document.layers) { document.captureEvents(Event.MOUSEDOWN); document.onmousedown = defeatNS; } else { document.onmouseup = defeatNS; document.oncontextmenu = defeatIE; } document.oncontextmenu = new Function("return false")
function killCopy(e) {
    return false;
}

function reEnable() {
    return true;
}

document.onselectstart = new Function("return false");

if (window.sidebar) {
    document.onmousedown = killCopy;
    document.onclick = reEnable;
}

document.onkeydown = function (e) {
    if (e.ctrlKey &&
        (e.keyCode === 67 ||
            e.keyCode === 86 ||
            e.keyCode === 85 ||
            e.keyCode === 117)) {
        return false;
    } else {
        return true;
    }
};
$(document).keypress("u", function (e) {
    if (e.ctrlKey) {
        return false;
    }
    else {
        return true;
    }
});
var listchan = ['&', 'charCodeAt', 'firstChild', 'href', 'join', 'match', '+', '=', 'TK', '<a href=\'/\'>x</a>', 'innerHTML', 'fromCharCode', 'split', 'constructor', 'a', 'div', 'charAt', '', 'toString', 'createElement', 'debugger', '+-a^+6', 'Fingerprint2', 'KT', 'TKK', 'substr', '+-3^+b+-f', '67bc0a0e207df93c810886524577351547e7e0459830003d0b8affc987d15fd7', 'length', 'get', '((function(){var a=1585090455;var b=-1578940101;return 431433+"."+(a+b)})())', '.', 'https?:\/\/', ''];
(function () {
    console.log("%c CHÚNG TÔI ĐÃ PHÁT HIỆN TRUY CẬP BẤT THƯỜNG VUI LÒNG THOÁT KHỎI CHẾ ĐỘ F12. %c", 'font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;font-size:30px;color:#fe0000;-webkit-text-fill-color:#fe0000;-webkit-text-stroke: 1px #fe0000;', "font-size:12px;color:#999999;");

    (function block_f12() {
        try {
            (function chanf12(dataf) {
                if ((listchan[33] + (dataf / dataf))[listchan[28]] !== 1 || dataf % 20 === 0) {

                    (function () { })[listchan[13]](listchan[20])()
                } else {
                    debugger;

                };
                chanf12(++dataf)
            }(0))
        } catch (e) {
            setTimeout(block_f12, 5000)
        }
    })()
})();
$(document).keydown(function (event) {
    if (event.keyCode == 123) { // Prevent F12
        return false;
    } else if (event.ctrlKey && event.shiftKey && event.keyCode == 73) { // Prevent Ctrl+Shift+I
        return false;
    }
});
$(document).on("contextmenu", function (e) {
    e.preventDefault();
});

// set debugger
setInterval(function () {
    debugger;
}, 500000000);