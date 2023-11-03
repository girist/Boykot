$(function () {
    $("#txtsearchkey").keyup(function () {
        if ($(this).val().length < 3) {
            return;
        } else {
            $('.spinner').css('display', 'block');
            GetProducts();
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
                $(row).append("<td />");
                $(row).find("td").eq(5).html("<img src='/images/icons/delete.png' alt='sil' width='20' height='20' class='rounded-circle' value='Sil' onclick='DeleteOdemeSeti(" + products.id + ")'>");
                $(row).append("<td />");
                $(row).find("td").eq(6).html("<a href='Admin/CreateOrUpdate?id=" + products.id + "'>Güncelle</a>");
                $('.spinner').css('display', 'none');
            });
        }
    });
}
//product delete func
function DeleteOdemeSeti(id) {
    let result = confirm("Silmek istediðinizden Emin Misiniz?");
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
                alert("Ýslem Basarili");
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                location.reload();
            }
        });
    } else {
        alert("Ýþlemde Ýptal Edildi");
    }
}