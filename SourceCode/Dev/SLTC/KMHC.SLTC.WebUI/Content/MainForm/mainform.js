function swapScreen(id) {
    jQuery('.visible').removeClass('visible animated fadeInUp');
    jQuery('#' + id).addClass('visible animated fadeInUp');
}

var code;
function createCode() {
    code = "";
    var codeLength = 4; //��֤��ĳ���
    var checkCode = document.getElementById("checkCode");
    var codeChars = new Array(0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
    'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'); //���к�ѡ�����֤����ַ�����ȻҲ���������ĵ�
    for (var i = 0; i < codeLength; i++) {
        var charNum = Math.floor(Math.random() * 52);
        code += codeChars[charNum];
    }
    if (checkCode) {
        checkCode.className = "code";
        checkCode.innerHTML = code;
    }
}

function refreshValidateCode() {
    $("#imgCheckNo").attr('src',  $("#imgCheckNo").attr('src')+'?time=' + (new Date()).getTime());
}

//$(function () {
//    $("#imgCheckNo").click(function () {
//        $(this).attr('src', this.attr('src') + '?time=' + (new Date()).getTime());
//    });
//});

function validateCode() {
    var inputCode = document.getElementById("inputCode").value;
    if (inputCode.length <= 0) {
        alert("��������֤�룡");
    }
    else if (inputCode.toUpperCase() != code.toUpperCase()) {
        alert("��֤����������");
        createCode();
    }
    else {
        alert("��֤����ȷ��");
    }
}






