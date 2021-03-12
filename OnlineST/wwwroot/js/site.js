﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function ClickInputFile() {

    const spanFileName = document.getElementById("FileName");
    const inputFile = document.getElementById("InputFile");

    spanFileName.textContent = "";

    inputFile.click();
}

function UploadImage() {

    const spanFileName = document.getElementById("FileName");
    const inputFile = document.getElementById("InputFile");

    spanFileName.textContent = inputFile.files[0].name;
}