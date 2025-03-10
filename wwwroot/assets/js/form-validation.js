(function () {
    'use strict';

    // Initialize Select2
    function initializeSelect2() {
        // Check if Select2 is available and elements with .select2 class exist
        if (typeof $ !== 'undefined' && $.fn.select2) {
            $('.select2').select2({
                minimumResultsForSearch: Infinity
            });
        } else {
            console.error('Select2 library or jQuery is not loaded.');
        }
    }

    // Apply Bootstrap validation to forms
    function initializeValidation() {
        // Fetch all forms with the class 'needs-validation'
        var forms = document.getElementsByClassName('needs-validation');

        // Loop over forms and prevent submission if invalid
        Array.prototype.forEach.call(forms, function (form) {
            form.addEventListener('submit', function (event) {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                form.classList.add('was-validated');
            }, false);
        });
    }

    // Run initialization functions when the DOM is fully loaded
    document.addEventListener('DOMContentLoaded', function () {
        initializeSelect2(); // Initialize Select2
        initializeValidation(); // Initialize Bootstrap validation
    });

    // Optional: Reinitialize for dynamically loaded content
    document.addEventListener('ajaxComplete', function () {
        initializeSelect2(); // Reinitialize Select2 for dynamically loaded content
        initializeValidation(); // Reinitialize validation for dynamically loaded forms
    });
})();