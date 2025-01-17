/* global bootstrap: false */
(() => {
    'use strict'
    const tooltipTriggerList = Array.from(document.querySelectorAll('[data-bs-toggle="collapse"]'))
    tooltipTriggerList.forEach(tooltipTriggerEl => {
        new bootstrap.Tooltip(tooltipTriggerEl)
    })
})()
document.addEventListener('DOMContentLoaded', function () {
    // Select all links inside collapsible lists
    const collapsibleLinks = document.querySelectorAll('.btn-toggle-nav a');

    collapsibleLinks.forEach(function (link) {
        link.addEventListener('click', function (e) {
            // Prevent the collapse from closing when a link is clicked
            e.stopPropagation();
        });
    });
});

