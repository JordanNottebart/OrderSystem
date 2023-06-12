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

    $('#customerDropdown').select2({
    });
    $('#productDropdown').select2({
    });
    $('.product-dropdown').select2({
    });

    /*Home Index View*/
    $("#resupplyButton").click(function () {
        $.ajax({
            url: '/Home/Resupply',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                // Handle the success response here
                alert("Resupply successful!");
                location.reload();
            },
            error: function (xhr, status, error) {
                // Handle the error response here
                alert("Resupply failed: " + error);
            }
        });
    });

    /*Order Create View*/
    var selectedProducts = [];
    var customerDropdown = $('#customerDropdown');

    $('#addToCartButton').click(function () {
        var selectedProductID = $('#productDropdown option:selected').val();
        var selectedQuantity = $('input[name^="OrderDetails"]').map(function () {
            return $(this).val();
        }).get();

        if (selectedProductID !== "" && selectedQuantity.length > 0) {
            // Check if the selected product ID already exists
            var existingProduct = selectedProducts.find(function (product) {
                return product.productID === selectedProductID;
            });

            if (existingProduct) {
                // Product already exists, update the quantity
                existingProduct.quantities = existingProduct.quantities.map(function (quantity, index) {
                    var newQuantity = parseInt(quantity) + parseInt(selectedQuantity[index]);
                    return newQuantity.toString();
                });
            } else {
                // Product doesn't exist, add a new entry
                selectedProducts.push({
                    productID: selectedProductID,
                    quantities: selectedQuantity
                });
            }

            updateSelectedProducts();
            clearSelections();
            updateHiddenInputFields();

            if (selectedProducts.length > 0) {

                // Store the selected value in the hidden field
                var selectedCustomerID = customerDropdown.val();
                $('#customerIDHidden').val(selectedCustomerID);

            }
        }
    });

    function updateSelectedProducts() {
        $('#selectedProductList').empty();

        selectedProducts.forEach(function (product, index) {
            var productItem = $('<li>').text('Product ID: ' + product.productID + ', Quantity: ' + product.quantities.join(', '));
            var deleteButton = $('<button>').text('X').click(function () {
                removeProduct(index);
            });
            productItem.append(deleteButton);
            $('#selectedProductList').append(productItem);
        });
    }

    function clearSelections() {
        $('input[name^="OrderDetails"]').val('1');
    }

    function updateHiddenInputFields() {
        // Remove existing hidden input fields
        $('input[name^="SelectedProducts"]').remove();

        // Add hidden input fields for each selected product
        selectedProducts.forEach(function (product, index) {
            var productIDInput = $('<input>').attr('type', 'hidden').attr('name', 'SelectedProducts[' + index + '].ProductID').val(product.productID);
            var quantitiesInput = $('<input>').attr('type', 'hidden').attr('name', 'SelectedProducts[' + index + '].Quantity').val(product.quantities.join(','));

            $('#hiddenInputContainer').append(productIDInput).append(quantitiesInput);
        });
    }

    function removeProduct(index) {
        selectedProducts.splice(index, 1);
        updateSelectedProducts();
        updateHiddenInputFields();
    }
});


    /*Order Index View*/
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
};

