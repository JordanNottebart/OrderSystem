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

        // Check if the target URL path is the same as the current URL path
        if (url === window.location.pathname) {
            return; // Exit the event handler, nothing happens
        }

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

    $('#customerDatatable').DataTable();
    $('#productDatatable').DataTable();
    $('#orderDatatable').DataTable();

});

$('.confirm-button').click(function (e) {
    e.preventDefault();

    // Get the order ID from the data attribute
    var orderId = $(this).data('order-id');

    // Determine the status
    var status = 'Pending';

    // Send the AJAX POST request to update the order status
    updateOrderStatus(orderId, status, false);
});

$('.process-button').click(function (e) {
    e.preventDefault();

    // Get the order ID from the data attribute
    var orderId = $(this).data('order-id');

    // Determine the status
    var status = 'Shipped';

    // Send the AJAX POST request to update the order status and units in stock
    updateOrderStatus(orderId, status, true);
});

function updateOrderStatus(orderId, status, updateUnits) {
    $.ajax({
        url: '/Order/UpdateStatus',
        type: 'POST',
        data: { orderId: orderId, status: status },
        success: function (response) {
            // Handle the success response
            if (response.success) {
                if (updateUnits) {
                    // Send the AJAX POST request to update the units in stock
                    updateUnitsInStock(orderId);
                } else {
                    // Reload the page to show the updated status
                    location.reload();
                }
            } else {
                // Display an error message if the update failed
                console.error(response.message);
            }
        },
        error: function (xhr, status, error) {
            // Handle the error response
            console.error(error);
        }
    });
}

function updateUnitsInStock(orderId) {
    $.ajax({
        url: '/Order/UpdateUnitsInStock',
        type: 'POST',
        data: { orderId: orderId },
        success: function (response) {
            // Manually reload the page to show the updated units in stock
            location.reload();
        },
        error: function (xhr, status, error) {
            // Handle the error response
            console.error(error);
        }
    });
}

$(document).ready(function () {
    // Store the original dropdown options
    var originalOptions = $("#customerDropdown").html();

    // Handle the keyup event on the filter input
    $("#customerIdFilter").keyup(function () {
        var filterValue = $(this).val().trim().toLowerCase();
        if (filterValue !== "") {
            // Filter the dropdown options based on the input value
            var filteredOptions = $(originalOptions).filter(function () {
                var optionValue = $(this).val().toLowerCase();
                var optionText = $(this).text().toLowerCase();
                return optionValue.startsWith(filterValue) || optionText.includes(filterValue);
            });

            // Update the dropdown with the filtered options
            $("#customerDropdown").html(filteredOptions);
        } else {
            // If no filter value, restore the original options
            $("#customerDropdown").html(originalOptions);
        }
    });
});

