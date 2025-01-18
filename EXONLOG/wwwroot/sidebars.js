///* global bootstrap: false */
(() => {
    'use strict'
    const tooltipTriggerList = Array.from(document.querySelectorAll('[data-bs-toggle="collapse"]'))
    tooltipTriggerList.forEach(tooltipTriggerEl => {
        new bootstrap.Tooltip(tooltipTriggerEl)
    })
})()
//document.addEventListener('DOMContentLoaded', function () {
//    // Select all links inside collapsible lists
//    const collapsibleLinks = document.querySelectorAll('.btn-toggle-nav a');

//    collapsibleLinks.forEach(function (link) {
//        link.addEventListener('click', function (e) {
//            // Prevent the collapse from closing when a link is clicked
//            e.stopPropagation();
//        });
//    });
//});

//document.addEventListener("DOMContentLoaded", () => {
//    const links = document.querySelectorAll(".link-dark");
//    links.forEach(link => {
//        link.addEventListener("click", (e) => {
//            e.stopPropagation(); // Prevent the event from bubbling up
//        });
//    });
//});


//// menu.js
//function toggleCollapse(event) {
//    event.stopPropagation(); // Prevent triggering parent collapse
//    const collapseElement = document.querySelector(event.currentTarget.dataset.bsTarget);
//    if (collapseElement) {
//        collapseElement.classList.toggle('show');
//    }
//}