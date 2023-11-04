function sleep(milliseconds) {
    return new Promise(resolve => setTimeout(resolve, milliseconds));
}
async function fun() {
    await sleep(1000);
    GetProducts();
}
$(function () {
    $("#txtsearchkey").keyup(function () {
        if ($(this).val().length < 3) {
            return;
        } else {
            $('.spinner').css('display', 'block');
            fun();
        }
    });
});
function GetProducts() {
    let formData = {
        SearchText: $("#txtsearchkey").val(),
        Criteria: $('#criteria option:selected').text(),
    }
    console.log(formData)
    $.ajax({
        type: "POST",
        url: "/Home/SearchProducts",
        dataType: 'json',
        data: formData,
        success: function (products) {
            var table = $("#tblProducts");
            console.log(products);
            table.find("tr:not(:first)").remove();
            $.each(products, function (i, products) {
                var table = $("#tblProducts");
                var row = table[0].insertRow(-1);
                $(row).append("<td />");
                $(row).find("td").eq(0).html(products.adi);
                $(row).append("<td />");
                $(row).find("td").eq(1).html(products.marka);
                $(row).append("<td />");
                $(row).find("td").eq(2).html(products.kodu);
                $(row).append("<td />");
                $(row).find("td").eq(3).html(products.barkod);
                $(row).append("<td />");
                $(row).find("td").eq(4).html(products.ulke);
                $('.spinner').css('display', 'none');
            });
        }
    });
}