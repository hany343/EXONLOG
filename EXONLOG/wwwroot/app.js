
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
