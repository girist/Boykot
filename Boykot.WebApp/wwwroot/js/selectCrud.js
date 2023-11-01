    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.cdnjs.com/ajax/libs/json2/20110223/json2.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#txtsearchkey").keyup(function () {
                if ($(this).val().length < 3)
                    return;

                GetProducts();
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
                    });
                }
            });
        }
    </script>