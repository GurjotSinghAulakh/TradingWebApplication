$(function () {
    getMyPortfolio();
});


function getMyPortfolio() {
/*let PId = window.location.search.substring(1);*/
    let PId = 1
    const url = "/api/v1/Trading/GetPortfolio?PId=" + PId;
    $.get(url, function (myStocks) {
        console.log(myStocks);
        formaterStocks(myStocks);
    })
        .fail(function (feil) {
            $("#feil").html("Feil på server - prøv igjen senere");
        });
}

function formaterStocks(myStocks) {
    let ut = "<table class='table table-striped'>" +
        "<tr>" +
        "<th>Symbol</th><th>Product Name</th><th>Price</th><th>Quantity</th><th></th><th></th><th></th>" +
        "</tr>";

    for (let stock of myStocks) {
        ut += "<tr>" +
            "<td>" + stock.stock_Symbol + "</td>" +
            "<td>" + stock.stock_Name + "</td>" +
            "<td>" + stock.stock_Price + "</td>" +
            "<td>" + stock.stock_Quantity + "</td>" +
            `<td> <a type='button' class='btn btn-primary' data-bs-toggle='modal' data-bs-target='#staticBackdrop' onclick='saveStockInfo("${stock.symbol}", "${stock.product_name}", "${stock.price}")'>Buy</a></td>` +
            `<td> <a type='button' class='btn btn-primary' data-bs-toggle='modal' data-bs-target='#chart' onclick='showChart("${stock.product_name}")'>Show Chart</a></td>` +
            `<td> <a type='button' class='btn btn-primary' data-bs-toggle='modal' data-bs-target='#sell' onclick='lagreSId("${stock.id}")'>Sell</a></td>` +
            "</tr>";
    }
    ut += "</table>";
    $("#myPortfolio").html(ut);
}

function lagreSId(bsId) {
    $("#bSId").val(bsId);
}


function selgeStock() {
    const url = "/api/v1/Trading/SellStock";
    const brukerStock = {
        antallstock: $("#antallStocks").val(),
        BSId: $("#bSId").val()
    }
    $.post(url, brukerStock, function (ok) {
        window.location.href = 'BrukerStocks.html?' + BId;
        console.log(ok)
    })

        .fail(function (feil) {
            if (feil.status == 401) {  // ikke logget inn, redirect til loggInn.html
                window.location.href = 'loggInn.html';
            }
            else {
                $("#feil").html("Feil på server - prøv igjen senere");
            }
        });

}


