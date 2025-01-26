
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


    function setupVideo(videoElement, stream) {
        videoElement.srcObject = stream;
    }

    function capturePhoto(canvasElement, videoElement) {
        const context = canvasElement.getContext('2d');
    canvasElement.width = videoElement.videoWidth;
    canvasElement.height = videoElement.videoHeight;
    context.drawImage(videoElement, 0, 0, canvasElement.width, canvasElement.height);
    }

    function getCanvasImageData() {
        const canvasElement = document.getElementById('canvasElement');
    return canvasElement.toDataURL();
    }

window.getCameraStream = async function () {
    try {
        const stream = await navigator.mediaDevices.getUserMedia({ video: true });
        return stream;
    } catch (err) {
        console.error('Error accessing camera: ', err);
        return null;
    }
};
