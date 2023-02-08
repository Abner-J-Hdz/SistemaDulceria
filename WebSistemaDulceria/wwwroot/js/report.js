


function crearPDF() {

    var quotes = document.getElementById('factura');

    html2canvas(quotes).then(function (canvas) {
        var imgData = canvas.toDataURL('iamge/png')
        var imgWidth = 210;
        var pageHeight = 295;
        var imgHeight = canvas.height * imgWidth / canvas.width;
        var heightLeft = imgHeight;
        /*var doc = new jsPDF({
            orientation: "landscape",
            unit: "in",
            format: [4, 2]
        });*/

        var doc = new jsPDF('p', 'mm', 'a4')
        var position = 0;
        doc.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight);
        doc.save('FacturaDeVenta.pdf');
    });
}


function crearPDFReporteVentas() {

    var quotes = document.getElementById('factura');

    html2canvas(quotes).then(function (canvas) {
        var imgData = canvas.toDataURL('iamge/png')
        var imgWidth = 210;
        var pageHeight = 295;
        var imgHeight = canvas.height * imgWidth / canvas.width;
        var heightLeft = imgHeight;
        /*var doc = new jsPDF({
            orientation: "landscape",
            unit: "in",
            format: [4, 2]
        });*/

        var doc = new jsPDF('p', 'mm', 'a4')
        var position = 0;
        doc.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight);
        doc.save('ResumenDeVentaDelDia.pdf');
    });
}