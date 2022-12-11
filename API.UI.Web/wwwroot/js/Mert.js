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
}

var mert = new jsHelper();