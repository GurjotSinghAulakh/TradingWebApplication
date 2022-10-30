﻿$(function () {
    getMyPortfolio();
});

let sellId = 1;


function getMyPortfolio() {
    /*let PId = window.location.search.substring(1);*/
    const url = "/api/v1/Trading/GetPortfolio";
    $.get(url, function (myStocks) {
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

    for (let i = 0; i < myStocks.length; i++) {
        ut += "<tr>" +
            "<td>" + myStocks[i].symbol + "</td>" +
            "<td>" + myStocks[i].name + "</td>" +
            "<td>" + myStocks[i].price + "</td>" +
            "<td>" + myStocks[i].quantity + "</td>" +
            `<td> <a type='button' class='btn btn-primary' data-bs-toggle='modal' data-bs-target='#staticBackdrop' onclick='saveStockInfo("${myStocks[i].id}","${myStocks[i].symbol}", "${myStocks[i].name}", "${myStocks[i].price}", "${myStocks[i].quantity}")'>Update Stock</a></td>` +
            `<td> <a type='button' class='btn btn-primary' data-bs-toggle='modal' data-bs-target='#chart' onclick='showChart("${myStocks[i].name}")'>Show Chart</a></td>` +
            `<td> <a type='button' class='btn btn-primary' data-bs-toggle='modal' onclick='sellStock("${myStocks[i].id}")'>Sell</a></td>` +
            "</tr>"
    }
    ut += "</table>";
    $("#myPortfolio").html(ut);
}

function saveStockInfo(orderID, stockSymbol, stockName, stockPrice, maxQuantity) {
    $("#OrdreID").val(orderID);

    const url = "/api/v1/Trading/GetOneOrder?orderID=" + orderID;
    $.get(url, function (myOrder) {
        console.log(myOrder);
        $("#SSymbol").val(myOrder.symbol);
        $("#SName").val(myOrder.name);
        $("#SPrice").val(myOrder.price);
        $("#MaxQuantity").html(myOrder.quantity);
        $("#UserID").val(1)
    })
        .fail(function (feil) {
            $("#feil").html("Feil på server - prøv igjen senere");
        });
}

function updateBuyStock() {
    const url = "/api/v1/Trading/UpdateBuyStock";

    const newOrder = {
        Id: $("#OrdreID").val(),
        Person: $("#UserID").val(),
        Quantity: $("#Quantity").val()
    }

    const stockID = window.location.search.substring(1);

    $.post(url, newOrder, function (ok) {
        window.location.href = 'portfolio.html';
        console.log("OK")
    })
        .fail(function (feil) {
            $("#feil").html("Feil på server - prøv igjen senere");
        });
}


function updateSellStock() {
    const url = "/api/v1/Trading/UpdateSellStock";

    const newOrder = {
        Id: $("#OrdreID").val(),
        Person: $("#UserID").val(),
        Quantity: $("#Quantity").val()
    }

    const stockID = window.location.search.substring(1);

    $.post(url, newOrder, function (ok) {
        window.location.href = 'portfolio.html';
        console.log("OK")
    })
        .fail(function (feil) {
            $("#feil").html("Feil på server - prøv igjen senere");
        });
}

function sellStock(sellID) {
    console.log("ID er : " + sellID)
    const url = "/api/v1/Trading/SellStock";
    const newOrder = sellID;

    $.post(url, { sellID: newOrder }, function (ok) {
        window.location.href = 'portfolio.html?Id=' + newOrder;
        console.log(ok)
    })

        .fail(function (feil) {
            $("#feil").html("Feil på server - prøv igjen senere");

        });

}

function showChart(stockName) {
    new Chart("myChart", {
        type: "line",
        data: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Des'],
            datasets: [{
                label: stockName,
                data: [
                    Math.random() * 250,
                    Math.random() * 250,
                    Math.random() * 250,
                    Math.random() * 250,
                    Math.random() * 250,
                    Math.random() * 250,
                    Math.random() * 250,
                    Math.random() * 250,
                    Math.random() * 250,
                    Math.random() * 250,
                    Math.random() * 250,
                    Math.random() * 250
                ],

                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)',
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}


