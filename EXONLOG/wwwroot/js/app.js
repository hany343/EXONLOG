
// wwwroot/js/site.js
function setActiveClass(element) {
    // Remove 'active' class from all elements with 'nav-item'
    var elements = document.querySelectorAll('.link-dark');
    elements.forEach(function (item) {
        item.classList.remove('active');
    });

    // Add 'active' class to the clicked element
    element.classList.add('active');
}
let stream;

async function startCamera() {
    const video = document.getElementById('videoElement');
    try {
        stream = await navigator.mediaDevices.getUserMedia({ video: true });
        video.srcObject = stream;
        video.play();
    } catch (err) {
        console.error('Error accessing the camera: ', err);
    }
}

function capturePhoto() {
    const video = document.getElementById('videoElement');
    const canvas = document.getElementById('photoCanvas');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    const context = canvas.getContext('2d');
    context.drawImage(video, 0, 0, canvas.width, canvas.height);
    const imageData = canvas.toDataURL('image/png');
    // Example: Sending data to the Blazor model
    DotNet.invokeMethodAsync('EXONLOG', 'SetDriverImage', imageData);
    console.log('Captured image:', imageData);
}

function stopCamera() {
    if (stream) {
        stream.getTracks().forEach(track => track.stop());
    }
}