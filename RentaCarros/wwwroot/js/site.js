$(document).ready(function () {
    $('#form').submit(function () {
        let valid = true;
        $('.form-control').each(function () {
            if (!$(this).hasClass('valid')) {
                if (!$(this).val()) {
                    $('#toast-body-normal').hide();
                    $('#toast-body-required-fields').show();
                } else {
                    $('#toast-body-required-fields').hide();
                    $('#toast-body-normal').show();
                }
                valid = false;
            }
        });
        if (!valid) {
            $('.toast').toast('show');
        }
    });
});