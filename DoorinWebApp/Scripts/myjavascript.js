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

//Ändra text på inskickat objekt
function changeText(obj, text) {
    obj.innerText = text;
};


//Disablar så att filtreringsformuläret skickas iväg när man klickar enter
$("form").keypress(function (e) {
    if (e.which == 13) {
        return false;
    }
});


// Script rörande Competens-DropDownListan
var selectedCompetence;
$(document).ready(function () {
    $("#Competences").change(function () {

        var selectedCompetenceId = document.getElementById("Competences");
        var competenceId = selectedCompetenceId.options[selectedCompetenceId.selectedIndex].value;
        selectedCompetence = competenceId;

    });
});

function AddCompetence() {
    var resume_id = $("#Resume_id").val();

    var compObj = {
        competence_id: selectedCompetence, resume_id: resume_id, name: null
    };

    $.ajax({
        type: "POST",
        url: "/Resumes/AddMyCompetences",
        data: JSON.stringify(compObj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            document.location.reload(true)
        }
    });
}


// Script lägga till teknologi
var selectedRemoveTechnologyId; // ligger här så jag når dem då jag ska lägga till en teknologi
var selectedRank;
var selectedCoreTech;
$(document).ready(function () {
    $("#Technologies").change(function () {

        var selectedTechnology = document.getElementById("Technologies");
        var technologyId = selectedTechnology.options[selectedTechnology.selectedIndex].value;

        selectedRemoveTechnologyId = technologyId;
    });
});

$(document).ready(function () {
    $("#rank").change(function () {

        var selectedRankId = document.getElementById("rank");
        var rankValue = selectedRankId.options[selectedRankId.selectedIndex].value;

        selectedRank = rankValue;

    });
});


$(document).ready(function () {
    $("#core_technology").change(function () {

        var selectedCore = document.getElementById("core_technology");
        var coreValue = selectedCore.options[selectedCore.selectedIndex].value;

        selectedCoreTech = coreValue;

    });
});
function AddTechnology() {

    var resume_id = $("#Resume_id").val();

    var techObj = {
        technology_id: selectedRemoveTechnologyId, resume_id: resume_id, core_technology: selectedCoreTech,
        rank: selectedRank, resume: null, technology: null
    };

    $.ajax({
        type: "POST",
        url: "/Resumes/AddMyTecgnologies",
        data: JSON.stringify(techObj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            document.location.reload(true)
        }
    });
}

// Script ta bort teknologier
var selectedTechnologyId; // ligger här så jag når dem då jag ska ta bort de man valt i dropdwonlistan
$(document).ready(function () {
    $("#MyTechnologies").change(function () {

        var selectedTechnology = document.getElementById("MyTechnologies");
        var technologyId = selectedTechnology.options[selectedTechnology.selectedIndex].value;

        selectedTechnologyId = technologyId;

    });

});

function RemoveTechnology(button) {

    var row = $(button).closest("TR");
    var tech = $("TD", row).eq(2).html();
    var resume_id = $("#Resume_id").val();

    var techObj = {
        technology_id: tech, resume_id: resume_id, core_technology: null,
        rank: 0, resume: null, technology: null
    };

    $.ajax({
        type: "POST",
        url: "/Resumes/RemoveMyTecgnologies",
        data: JSON.stringify(techObj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            document.location.reload(true)
        }
    });

}


// Script ta bort kompetens
var selectedCompetenceId; // ligger här så jag når dem då jag ska ta bort de man valt i dropdwonlistan
$(document).ready(function () {
    $("#MyCompetences").change(function () {

        var selectedCompetence = document.getElementById("MyCompetences");
        var competenceId = selectedCompetence.options[selectedCompetence.selectedIndex].value;
        selectedCompetenceId = competenceId;

    });
});

function RemoveCompetence(button) {

    var row = $(button).closest("TR");
    var comp = $("TD", row).eq(2).html();
    var resume_id = $("#Resume_id").val();

    var compObj = {
        competence_id: comp, resume_id: resume_id, name: null
    };

    $.ajax({
        type: "POST",
        url: "/Resumes/RemoveMyCompetences",
        data: JSON.stringify(compObj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            document.location.reload(true)
        }
    });
}


