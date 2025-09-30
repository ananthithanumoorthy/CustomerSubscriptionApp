/*$(document).ready(function () {*/
    // Globally restrict DataTable search inputs
    $.fn.dataTable.ext.errMode = 'throw'; // optional: for debugging

    // Hook after every DataTable initialization
    $(document).on('init.dt', function (e, settings) {
        // Get the search input
        const $searchInput = $(settings.nTableWrapper).find('input[type="search"]');

        // Apply Parsley validation only if not already applied
        if (!$searchInput.attr('data-parsley-pattern')) {
            $searchInput
                .attr('data-parsley-pattern', '^[a-zA-Z0-9 _-]*$')
                .attr('data-parsley-pattern-message', 'Only letters, numbers, space, dash (-), and underscore (_) are allowed.')
                .attr('data-parsley-trigger', 'keyup')
                .attr('autocomplete', 'off');

            // Initialize Parsley
            $searchInput.parsley();

            // Keypress restriction
            $searchInput.on('keypress', function (e) {
                const key = String.fromCharCode(e.which || e.keyCode);
                const regex = /^[a-zA-Z0-9 _-]$/;
                if (!regex.test(key)) {
                    e.preventDefault();
                }
            });

            // Optional: validate on input
            $searchInput.on('input', function () {
                $(this).parsley().validate();
            });
        }
    });
/*});*/
function ShowAlertMessage(message) {
    Swal.fire({
        text: message,
        icon: 'success',
        confirmButtonText: 'Ok',
        width: '380px'
    })
}
function ShowErrorAlertMessage(message) {
    Swal.fire({
        text: message,
        icon: 'error',
        confirmButtonText: 'Ok',
        width: '380px'
    })
}
// Function to format date in dd-mm-yyyy
function formatDate(dateString) {
    var date = new Date(dateString);
    var day = ("0" + date.getDate()).slice(-2);
    var month = ("0" + (date.getMonth() + 1)).slice(-2); // Month is 0-indexed
    var year = date.getFullYear();
    return day + '-' + month + '-' + year;
}
// Menu toggle functionality
// Function to bind menu events
function bindMenuEvents() {
    document.querySelectorAll(".menu-toggle").forEach(function (menuToggle) {
        menuToggle.addEventListener("click", function (e) {
            e.preventDefault(); // Prevent default behavior
            e.stopPropagation(); // Stop event from bubbling up

            let parentLi = this.closest("li");
            let subMenu = parentLi.querySelector(".menu-sub");

            if (!subMenu) return;

            let isOpen = parentLi.classList.contains("open");

            // Close all other open menus
            document.querySelectorAll(".menu-item.open").forEach(function (item) {
                if (item !== parentLi) {
                    item.classList.remove("open", "active");
                    let sub = item.querySelector(".menu-sub");
                    if (sub) sub.style.display = "none";
                }
            });

            // Toggle current menu
            if (isOpen) {
                parentLi.classList.remove("open", "active");
                subMenu.style.display = "none";
            } else {
                parentLi.classList.add("open", "active");
                subMenu.style.display = "block";
            }
        });
    });
}
function loaderEnable() {
    var loader = document.getElementById("loadingOverlay");
    if (loader) {
        loader.style.display = "flex"; // Show the loader
    }
}

function loaderDisable() {
    var loader = document.getElementById("loadingOverlay");
    if (loader) {
        loader.style.display = "none"; // Hide the loader
    }
}
