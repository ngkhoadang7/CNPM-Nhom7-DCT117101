function formatPricetoPrint(a){
	a=a.toLocaleString()
	a=a.split(',').join('.');
	return a;
}
function UpdateSeatAndPrice(){
    var seat = document.getElementsByName('selectedSeats');
    var seatSelected = 0;
    var string ="";
    for( var i = 0; i < seat.length; i++){
        if(( seat[i].checked==true) && (seat[i].disabled!=true)){
            seatSelected++;
            if(string == ""){
                string += seat[i].value;
            } else {
                string +=", " + seat[i].value;
            }
        } 
    }
    var format = document.getElementById('theater-format').value;
    
    var money = 95000;
    if (format == "3D")
        money = 125000;
    else if (format == "4DX")
        money = 150000;
    
    document.getElementById('seatsSelected').innerHTML = string;
    document.getElementById('totalPrice').innerHTML=formatPricetoPrint(seatSelected * money)+"₫";
    
}