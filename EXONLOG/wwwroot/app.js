
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

function capturePhoto() {
    const videoElement = document.getElementById("videoElement");
    const canvasElement = document.getElementById("canvasElement");
    const capturedImage = document.getElementById("capturedImage");

    const context = canvasElement.getContext("2d");
    canvasElement.width = videoElement.videoWidth;
    canvasElement.height = videoElement.videoHeight;

    // Draw the video frame onto the canvas
    context.drawImage(videoElement, 0, 0, canvasElement.width, canvasElement.height);

    // Get the image data as a Base64 string
    const imageData = canvasElement.toDataURL("image/png");
    capturedImage.src = imageData;
    capturedImage.style.display = "block"; // Show the captured image
    videoElement.style.display = "none";   // Hide the video feed
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

function checkCameraSupport() {
    if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
        alert("Camera is not supported on your device.");
        return false;
    }
    return true;
}
function getCanvasImageData() {
    const canvas = document.getElementById("canvasElement");
    return canvas.toDataURL("image/png");
}
let videoStream = null;

function openCameraModal() {
    const videoElement = document.getElementById("videoElement");

    navigator.mediaDevices
        .getUserMedia({ video: true })
        .then((stream) => {
            videoStream = stream;
            videoElement.srcObject = stream;
        })
        .catch((err) => {
            console.error("Error accessing camera: ", err);
        });
}

function closeCameraModal() {
    if (videoStream) {
        const tracks = videoStream.getTracks();
        tracks.forEach((track) => track.stop()); // Stop all tracks
        videoStream = null; // Clear the stream reference
    }

    const videoElement = document.getElementById("videoElement");
    if (videoElement) {
        videoElement.srcObject = null; // Clear the video element's source
    }
}