﻿
function focusElementById(IdElement){
    let element = document.getElementById(IdElement);
    if (element) {
        element.focus();
    }
}

function showAlert(icon, title, text) {
    Swal.fire({
        position: 'center-center',
        icon: icon,
        title: title,
        text: text,
        showConfirmButton: false,
        timer: 1500
    })
}

function blockElement(elements) {
    for (let i = 0; i < elements.length; i++) {
        document.getElementById(elements[i]).disabled = true;
    }
}

function getUserEmail() {
    return document.getElementById('User-Email').innerHTML;
}

function verifyLogin() {
    let xd = document.getElementById('NoLogin')
    if (xd != null) {
        console.log('redirect');
        window.location.href = '/login'
    }
}

document.addEventListener("DOMContentLoaded", function () {
    verifyLogin();
});


/*
function enabledElement(elements) {
    for (let i = 0; i < elements.length; i++) {
        document.getElementById(elements[i]).disabled = false;
    }
}*/