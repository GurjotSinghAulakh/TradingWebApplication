﻿let BuyID = 1;

$(function () {
    //BuyID = window.location.search.substring(1);
    getAllStocks();
});


function getAllStocks() {
    $.get("/api/v1/Trading/GetStocks", function (stocks) {
        formaterStocks(stocks);
    })
        .fail(function (feil) {
            $("#feil").html("Feil på server - prøv igjen senere");
        });
}


function formaterStocks(stocks) {

    let ut = "<table class='table table-striped'>" +
        "<tr>" +
        "<th>Symbol</th><th>Name</th><th>Price</th><th>Volume</th><th></th><th></th>" +
        "</tr>";

    for (let stock of stocks) {
        ut += "<tr>" +
            "<td>" + stock.symbol + "</td>" +
            "<td>" + stock.name + "</td>" +
            "<td>" + stock.price + "</td>" +
            "<td>" + stock.volume + "</td>" +
            `<td> <a type='button' class='btn btn-primary' data-bs-toggle='modal' data-bs-target='#staticBackdrop' onclick='saveStockInfo("${stock.id}","${stock.symbol}", "${stock.name}", "${stock.price}")'>Buy</a></td>` +
            `<td> <a type='button' class='btn btn-primary' data-bs-toggle='modal' data-bs-target='#chart' onclick='showChart("${stock.name}")'>Show Chart</a></td>` +
            "</tr>";
    }

    ut += "</table>";
    $("#stocks").html(ut);
}

function saveStockInfo(stockID, stockSymbol, stockName, stockPrice) {
    $("#OrdreID").val(stockID);
    $("#SSymbol").val(stockSymbol);
    $("#SName").val(stockName);
    $("#SPrice").val(stockPrice);
    $("#UserID").val(BuyID);
}


function buyStock() {
    const url = "/api/v1/Trading/BuyStock";

    const newStock = {
        Id: $("#OrdreID").val(),
        Symbol: $("#SSymbol").val(),
        Name: $("#SName").val(),
        Price: $("#SPrice").val()
    }

    const newOrder = {
        Stock: newStock,
        Person: $("#UserID").val(),
        Quantity: $("#Quantity").val()
    }

    const stockID = window.location.search.substring(1);

    $.post(url, (stockID, newOrder), function (ok) {
        window.location.href = 'portfolio.html?Id=' + BuyID;
        console.log("OK")
        console.log(newOrder);
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
