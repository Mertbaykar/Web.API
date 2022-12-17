var jsHelper = function () {

    this.postData = function (url, data, success, fail) {

        $.ajax(url, {
            type: "POST",
            data: data,
            dataType: "json",
            cache: false,
            success: success,
            error: fail
        });
    };

    this.submitForm = function (formSelector, url, success, fail) {

        var $form = $(formSelector);
        $form.validate();

        var isValid = $form.valid()
        if (isValid) {
            var data = $form.serialize();
            this.postData(url, data, success, fail);
        }

        return false;
    };

    this.success = function (message, closeCallback) {
        debugger;
        var successModalDOM = document.getElementById('successModal');
        var successModal = new bootstrap.Modal(successModalDOM);
        $('#successModal .modal-body').html(message);

        successModalDOM.addEventListener('hidden.bs.modal', function (event) {
            // modal tamamen kapanmışken..
            closeCallback();
        });

        successModal.show();
    };

    this.error = function (message, closeCallback) {
        debugger;
        var errorModalDOM = document.getElementById('errorModal');
        var errorModal = new bootstrap.Modal(errorModalDOM);
        $('#errorModal .modal-body').html(message);

        errorModalDOM.addEventListener('hidden.bs.modal', function (event) {
            // modal tamamen kapanmışken..
            closeCallback();
        });

        errorModal.show();
    };
}

var mert = new jsHelper();