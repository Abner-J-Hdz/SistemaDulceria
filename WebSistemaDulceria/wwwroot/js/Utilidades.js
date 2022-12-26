
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