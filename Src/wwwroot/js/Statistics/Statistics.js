window.onload = function () {

    document.getElementById('total-money-2D').innerHTML=formatPricetoPrint(parseInt(document.getElementById('total-money-2D').innerHTML))+"₫";
    document.getElementById('total-money-3D').innerHTML=formatPricetoPrint(parseInt(document.getElementById('total-money-3D').innerHTML))+"₫";
    document.getElementById('total-money-4DX').innerHTML=formatPricetoPrint(parseInt(document.getElementById('total-money-4DX').innerHTML))+"₫";

    var Statistic2D = createDataPoint("2DMovieTitle","2DMovieTotalMoney");
    Statistic2D = Statistic2D.sort(compareDataPoint);

    var Statistic3D = createDataPoint("3DMovieTitle","3DMovieTotalMoney");
    Statistic3D = Statistic3D.sort(compareDataPoint);

    var Statistic4DX = createDataPoint("4DXMovieTitle","4DXMovieTotalMoney");
    Statistic4DX = Statistic4DX.sort(compareDataPoint);

    var chart1 = new CanvasJS.Chart("chart2DTicketContainer", {
        exportEnabled: true,
        animationEnabled: true,
        legend:{
            verticalAlign: "center",
			horizontalAlign: "right",
			cursor: "pointer",
			itemclick: explodePie
        },
        data: [{
            type: "pie",
            startAngle: 270,
            showInLegend: true,
            toolTipContent: "{name}: <strong>{y}₫</strong>",
            indexLabel: "{name} - {y}₫",
            dataPoints: Statistic2D
        }]
    });
    chart1.render();

    var chart2 = new CanvasJS.Chart("chart3DTicketContainer", {
        exportEnabled: true,
        animationEnabled: true,
        legend:{
            verticalAlign: "center",
			horizontalAlign: "right",
			cursor: "pointer",
			itemclick: explodePie
        },
        data: [{
            type: "pie",
            startAngle: 270,
            showInLegend: true,
            toolTipContent: "{name}: <strong>{y}₫</strong>",
            indexLabel: "{name} - {y}₫",
            dataPoints: Statistic3D
        }]
    });
    chart2.render();

    var chart3 = new CanvasJS.Chart("chart4DXTicketContainer", {
        exportEnabled: true,
        animationEnabled: true,
        legend:{
            verticalAlign: "center",
			horizontalAlign: "right",
			cursor: "pointer",
			itemclick: explodePie
        },
        data: [{
            type: "pie",
            startAngle: 270,
            showInLegend: true,
            toolTipContent: "{name}: <strong>{y}₫</strong>",
            indexLabel: "{name} - {y}₫",
            dataPoints: Statistic4DX
        }]
    });
    chart3.render();
}
    
function explodePie (e) {
    if(typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
        e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
    } else {
        e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
    }
    e.chart.render();

}
function createDataPoint( ClassNameOfTitlesElements, ClassNameOfTotalMoneyElements){
    var array = new Array();
    var title = document.getElementsByName(ClassNameOfTitlesElements);
    var money = document.getElementsByName(ClassNameOfTotalMoneyElements);
    for( var i = 0; i < title.length ; i++){
        array.push({'name': title[i].value, 'y': parseInt(money[i].value)});
    }
    return array;
}

function MyData(Title, Money){
	this.Title=Title;
	this.Money=parseInt(Money);
}
function formatPricetoPrint(a){
	a=a.toLocaleString()
	a=a.split(',').join('.');
	return a;
}
function compareDataPoint(dataPoint1, dataPoint2) {
    if (dataPoint1.y < dataPoint2.y){return -1}
    if ( dataPoint1.y > dataPoint2.y){return 1}
    return 0
}