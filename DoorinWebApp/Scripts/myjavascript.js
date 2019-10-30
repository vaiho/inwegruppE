function myFunction() {
    var x = document.getElementById("filters");
    if (x.style.display === "none") {
        x.style.display = "block";
        document.getElementById("filterbtn").classList.add('filterbtnactive');
    }
    else {
        x.style.display = "none";
        document.getElementById("filterbtn").classList.remove('filterbtnactive');
    }
}