function expandSidebar() {
    document.getElementById("mySidebar").style.width = "250px";

}

function collapseSidebar() {
    document.getElementById("mySidebar").style.width = "85px";
}

$(document).ready(function () {
    // Attach click event listeners to the "Home", "Order", "Product" and "Customer" links
    $('a[href="/"], a[href="/Order"], a[href="/Product"], a[href="/Customer"]').on('click', function (event) {
        // Prevent the default click behavior
        event.preventDefault(); 

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

    // Convert the customerTable, productTable and orderTable based on ID to a DataTable
    $('#customerDatatable').DataTable();
    $('#productDatatable').DataTable();
    $('#orderDatatable').DataTable();

    // Convert the dropdowns with ID customerDropdown and productDropdown to a Select2 dropdown
    $('#customerDropdown').select2({
    });
    $('#productDropdown').select2({
    });
    // Convert the dropdown with the class as product-dropdown to a Select2 dropdown
    $('.product-dropdown').select2({
    });

    /*Home Index View*/
    $("#resupplyButton").click(function () {
        // Post an AJAX request to the method "Resupply" in the HomeController
        $.ajax({
            url: '/Home/Resupply',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                // Show an alert that the resupply was successful
                alert("Resupply successful!");

                // Manually reload the page
                location.reload();
            },
            error: function (xhr, status, error) {
                // If there was an error, show it as an alert
                alert("Resupply failed: " + error);
            }
        });
    });

    /*Order Create View*/
    var selectedProducts = [];
    var customerDropdown = $('#customerDropdown');

    $('#addToCartButton').click(function () {
        var selectedProductID = $('#productDropdown option:selected').val();
        var selectedProductName = $('#productDropdown option:selected').text();
        var selectedQuantity = $('input[name^="OrderDetails"]').map(function () {
            return $(this).val();
        }).get();

        // Check if there was a product selected and the quantity was higher than 0
        if (selectedProductID !== "" && selectedQuantity.length > 0) {
            // Check if the selected product ID already exists
            var existingProduct = selectedProducts.find(function (product) {
                return product.productID === selectedProductID;
            });

            if (existingProduct) {
                // Product already exists in the cart, so only update the quantity
                existingProduct.quantities = existingProduct.quantities.map(function (quantity, index) {
                    var newQuantity = parseInt(quantity) + parseInt(selectedQuantity[index]);
                    return newQuantity.toString();
                });
            } else {
                // Product doesn't exist, add a new entry
                selectedProducts.push({
                    productID: selectedProductID,
                    quantities: selectedQuantity,
                    productName: selectedProductName
                });
            }

            updateSelectedProducts();
            clearSelections();
            updateHiddenInputFields();

            // If there are products in the cart
            if (selectedProducts.length > 0) {

                // Store the selected value in the hidden field
                var selectedCustomerID = customerDropdown.val();
                $('#customerIDHidden').val(selectedCustomerID);

            }
        }
    });

    function updateSelectedProducts() {
        // Clear the existing list of selected products
        $('#selectedProductList').empty();

        selectedProducts.forEach(function (product, index) {
            // Create a <li> element for each selected product
            var productItem = $('<li>');

            // Display the product name
            var productName = $('<span>').text('Product: ' + product.productName);

             // Attach a click event handler to the delete button to remove the product
            var deleteButton = $('<button>').text('X').click(function () {
                removeProduct(index);
            });

            // Append the product name to the product item
            productItem.append(productName);

            // Append the delete button to the product item
            productItem.append(deleteButton);

            // Create a nested <ul> for quantities
            var quantityList = $('<ul>');
            
            product.quantities.forEach(function (quantity) {
                // Display each quantity
                var quantityItem = $('<li>').text('Quantity: ' + quantity);

                // Append each quantity as a <li> to the nested <ul>
                quantityList.append(quantityItem); 
            });

            // Append the nested <ul> to the product item
            productItem.append(quantityList);

            // Append the product item to the selected product list
            $('#selectedProductList').append(productItem);
        });
    }

    function clearSelections() {
        // Set the value for the Quantity Input back to 1
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
        // Remove the product at the specified index from the selectedProducts array
        selectedProducts.splice(index, 1);

        // Update the display of the selected products list
        updateSelectedProducts();

        // Update the hidden input fields of the selected products
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
            // If the response was succes
            if (response.success) {
                // If the updateUnits bool were set to true
                if (updateUnits) {
                    // Send the AJAX POST request to update the units in stock
                    updateUnitsInStock(orderId);
                } else {
                    // Reload the page to show the updated status
                    location.reload();
                }
            } else {
                // Display an error message if the update failed
                alert(`The quantity you have chosen: ${response.quantityProduct}\nis higher than the available units in stock: ${response.unitsInStockProduct}\nfor the product: ${response.productName}!`);
            }
        },
        error: function (xhr, status, error) {
            // Show the error in the console if it failed
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
            // If the response was failed
            if (response.failed) {
                // Display an error message if the update failed
                alert(`The quantity you have chosen: ${response.quantityProduct}\nis higher than the available units in stock: ${response.unitsInStockProduct}\nfor the product: ${response.productName}!`);
            }
            else {
                // Manually reload the page to show the updated units in stock
                location.reload();
            }
        },
        error: function (xhr, status, error) {
            // Show the error in the console if it failed
            console.error(error);
        }
    });
};

