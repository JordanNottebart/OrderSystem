function expandSidebar() {
    document.getElementById("mySidebar").style.width = "250px";

}

function collapseSidebar() {
    document.getElementById("mySidebar").style.width = "85px";
}

$(document).ready(function () {
    // Attach click event listeners to the "Home", "Orders", "Products" and "Customers" links
    $('a[href="/"], a[href="/Order"], a[href="/Product"], a[href="/Customer"]').on('click', function (event) {
        event.preventDefault(); // Prevent the default click behavior

        // Get the target URL from the link's "href" attribute
        var url = $(this).attr('href');

        // Make an AJAX request to load the content from the target URL
        $.get(url, function (response) {
            // Create a temporary container to hold the loaded content
            var $tempContainer = $('<div>').html(response);

            // Find the main content area within the loaded content
            var $mainContent = $tempContainer.find('main');

            // Replace the content of the container with the main content
            $('#container').html($mainContent.html());

            // Update the URL without triggering a page refresh
            history.pushState(null, '', url);
        });
    });

    // Handle browser back/forward navigation
    $(window).on('popstate', function () {
        // Get the current URL
        var url = window.location.href;

        // Make an AJAX request to load the content from the target URL
        $.get(url, function (response) {
            // Create a temporary container to hold the loaded content
            var $tempContainer = $('<div>').html(response);

            // Find the main content area within the loaded content
            var $mainContent = $tempContainer.find('main');

            // Replace the content of the container with the main content
            $('#container').html($mainContent.html());
        });
    });

    $(".detailsLink").click(function (e) {
        e.preventDefault(); // Prevent the default behavior of the link

        var url = $(this).data("url");
        $('#exampleModal .modal-body').load(url); // Load the details content into the modal's body
        $('#exampleModal').modal('show'); // Show the modal
    });

    $('#productDatatable').DataTable();
    $('#customerDatatable').DataTable();
});