(function ($) {
    //Transacciones Ajax

    var transact = window.transact = {

        ajaxGET: function (url, data, success, error, dataType) {

            if (!dataType)
                dataType = "json";

            $.ajax({
                "type": "GET",
                "url": url,
                "data": (data != null) ? data : {},
                "dataType": dataType,
                "success": success,
                "error": error
            });
        },

        ajaxPOST: function (url, data, success, error, dataType) {

            if (!dataType)
                dataType = "json";

            $.ajax({
                "type": "POST",
                "url": url,
                "data": (data != null) ? data : {},
                "success": success,
                "error": error
            });
        },


        ajaxPUT: function (url, data, success, error, dataType) {

            if (!dataType)
                dataType = "json";

            $.ajax({
                "type": "PUT",
                "url": url,
                "data": (data != null) ? data : {},
                "success": success,
                "error": error
            });
        },

        ajaxDELETE: function (url, data, success, error, dataType) {

            if (!dataType)
                dataType = "json";

            $.ajax({
                "type": "DELETE",
                "url": url,
                "data": (data != null) ? data : {},
                "dataType": dataType,
                "success": success,
                "error": error
            });
        }


    }


})(jQuery);