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
};

$(".btnAddLink").click(function () {
    //Reference the Name and Link TextBoxes.
    var txtLinkname = $("#txtLinkname");
    var txtLink = $("#txtLink");


    //Get the reference of the Table's TBODY element.
    var tBody = $("#tblLinks > TBODY")[0];

    //Add Row.
    var row = tBody.insertRow(-1);

    //Add Linkname cell.
    var cell = $(row.insertCell(-1));
    cell.html(txtLinkname.val());

    //Add URL cell.
    cell = $(row.insertCell(-1));
    cell.html(txtLink.val());

    //Add Button cell.
    cell = $(row.insertCell(-1));
    var btnRemove = $("<input class='btnless'/>");
    btnRemove.attr("type", "button");
    btnRemove.attr("onclick", "RemoveLink(this);");
    btnRemove.val("Ta bort");
    cell.append(btnRemove);

    //Clear the TextBoxes.
    txtLinkname.val("");
    txtLink.val("");
});

function RemoveLink(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    if (confirm("Do you want to delete: " + name)) {
        //Get the reference of the Table.
        var table = $("#tblLinks")[0];
        //Delete the Table row using it's Index.
        table.deleteRow(row[0].rowIndex);
    }
};

$(".btnAddWorkhistory").click(function () {
    //Reference the Name and Link TextBoxes.
    var txtEmployer = $("#txtEmployer");
    var txtPosition = $("#txtPosition");
    var txtDescription = $("#txtDescription");
    var txtDate = $("#txtDate");

    //Get the reference of the Table's TBODY element.
    var tBody = $("#tblWorkhistory > TBODY")[0];

    //Add Row.
    var row = tBody.insertRow(-1);

    //Add employer cell.
    var cell = $(row.insertCell(-1));
    cell.html(txtEmployer.val());

    //Add position cell.
    cell = $(row.insertCell(-1));
    cell.html(txtPosition.val());

    //Add description cell.
    cell = $(row.insertCell(-1));
    cell.html(txtDescription.val());

    //Add date cell.
    cell = $(row.insertCell(-1));
    cell.html(txtDate.val());

    //Add Button cell.
    cell = $(row.insertCell(-1));
    var btnRemove = $("<input class='btnless'/>");
    btnRemove.attr("type", "button");
    btnRemove.attr("onclick", "RemoveWorkhistory(this);");
    btnRemove.val("Ta bort");
    cell.append(btnRemove);

    //Clear the TextBoxes.
    txtEmployer.val("");
    txtPosition.val("");
    txtDescription.val("");
    txtDate.val("");
});



function RemoveWorkhistory(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    if (confirm("Do you want to delete: " + name)) {
        //Get the reference of the Table.
        var table = $("#tblWorkhistory")[0]; //TODO:Test för workhistory
        //Delete the Table row using it's Index.
        table.deleteRow(row[0].rowIndex);
    }
};



$("body").on("click", "#btnSave", function () {
    //Loop through the Table rows and build a JSON array.
    var links = new Array();
    var resume_id = $("#Resume_id").val();
    $("#tblLinks TBODY TR").each(function () {
        var row = $(this);
        var link = {};
        link.Name = row.find("TD").eq(0).html();
        link.Link = row.find("TD").eq(1).html();
        link.resume_id = resume_id;
        links.push(link);

    });

    var wh = new Array();
    var resume_id = $("#Resume_id").val();
    $("#tblWorkhistory TBODY TR").each(function () {
        var row = $(this);
        var workhistory = {};
        workhistory.employer = row.find("TD").eq(0).html().trim();
        workhistory.position = row.find("TD").eq(1).html().trim();
        workhistory.description = row.find("TD").eq(2).html().trim();
        workhistory.date = row.find("TD").eq(3).html().trim();

        workhistory.resume_id = resume_id;
        wh.push(workhistory);

    });

    //Send the JSON array to Controller using AJAX.
    $.ajax({
        type: "POST",
        url: "/Links/InsertLinks",
        data: JSON.stringify(links),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            //alert("link(s) updated.");
        }
    });


    $.ajax({
        type: "POST",
        url: "/Workhistories/InsertWorkhistory",
        data: JSON.stringify(wh),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            //alert("link(s) updated.");
        }
    });


});


