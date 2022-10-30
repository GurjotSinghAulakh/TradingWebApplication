let BuyID = 1;

/* This is a jQuery function that calls the function getAllStocks() when the page is loaded. */
$(function () {
    getAllStocks();
});


/**
 * The function getAllStocks() is a jQuery function that gets the stocks from the server and then calls
 * the function formaterStocks(stocks) to format the stocks
 */
function getAllStocks() {
    $.get("/api/v1/Trading/GetStocks", function (stocks) {
        formaterStocks(stocks);
    })
        .fail(function (feil) {
            $("#feil").html("Feil på server - prøv igjen senere");
        });
}


/**
 * It takes a list of stocks and formats them into a table
 * @param stocks - the array of stocks to be displayed
 */

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

/**
 * It takes the stockID, stockSymbol, stockName, and stockPrice and puts them into the hidden fields in
 * the form.
 * @param stockID - The ID of the stock
 * @param stockSymbol - The stock symbol
 * @param stockName - The name of the stock
 * @param stockPrice - The current price of the stock
 */

function saveStockInfo(stockID, stockSymbol, stockName, stockPrice) {
    $("#OrdreID").val(stockID);
    $("#SSymbol").val(stockSymbol);
    $("#SName").val(stockName);
    $("#SPrice").val(stockPrice);
    $("#UserID").val(BuyID);
}


/**
 * It takes the values from the input fields and sends them to the API
 */
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

/**
 * It creates a new chart object, and then it sets the type of chart to line, and then it sets the data
 * for the chart, and then it sets the options for the chart
 * @param stockName - The name of the stock you want to display
 */
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
