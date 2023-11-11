function sleep(milliseconds) {
    return new Promise(resolve => setTimeout(resolve, milliseconds));
}
async function fun() {
    await sleep(500);
    GetProducts();
}
$(function () {
    $("#txtsearchkey").keyup(function () {
        $("#tblProducts").empty();
        $('.spinner').css('display', 'block');
        fun();
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
            table.append("<tr><th>Marka</th><th>Kod</th><th>Barkod</th><th>Ulke</th><th>Sil</th><th>Guncelle</th></tr>");
            table.find("tr:not(:first)").remove();
            $.each(products, function (i, products) {
                var table = $("#tblProducts");
                var row = table[0].insertRow(-1);
                //$(row).append("<td />");
                //$(row).find("td").eq(0).html(products.adi);
                $(row).append("<td />");
                $(row).find("td").eq(0).html(products.marka);
                $(row).append("<td />");
                $(row).find("td").eq(1).html(products.kodu);
                $(row).append("<td />");
                $(row).find("td").eq(2).html(products.barkod);
                $(row).append("<td />");
                $(row).find("td").eq(3).html(products.ulke);
                $(row).append("<td />");
                $(row).find("td").eq(4).html("<img src='/images/icons/delete.png' alt='sil' width='20' height='20' class='rounded-circle' value='Sil' onclick='DeleteOdemeSeti(" + products.id + ")'>");
                $(row).append("<td />");
                $(row).find("td").eq(5).html("<a href='/Admin/CreateOrUpdate?id=" + products.id + "'>Guncelle</a>");
                $('.spinner').css('display', 'none');
            });
        }
    });
}
//product delete func
function DeleteOdemeSeti(id) {
    let result = confirm("Silmek istediginizden emin misiniz ?");
    let formData = {
        id: id,
    }
    console.log(formData)
    if (result) {
        $.ajax({
            type: "POST",
            url: "/Admin/Delete",
            dataType: 'json',
            data: formData,
            success: function (response) {
                alert("islem Basarili.");
               // location.reload(true);
                GetProducts();
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                location.reload();
            }
        });
    } else {
        alert("islem iptal Edildi !");
    }
}