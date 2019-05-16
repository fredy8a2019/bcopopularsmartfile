(function ($) {

    var _fx = window._fx = {

        sumTwoValue: function (val1, val2) {
            return val1 + val2;
        },

        validNumerical: function (_idElement, mensajeValid, mensajeInValid) {


        },

        validRequested: function (_idElement, mensaje) {

            if (_idElement.val() == "" || _idElement.val() == -1) {
                _idElement.after("<span id='mensajeReq'>" + mensaje + "</span>");
                $("#mensajeReq").css("color", "red");
            } else if (_idElement.val() != "" || _idElement.val() != "-1") {
                $("#mensajeReq").removeAttr("style");
            }

        },
        validEmail: function (_idElement) {

        },
        validLengthNum: function (_idElement, min, max) {

        },

        clearCampos: function (elemento) {
            $(elemento).val("");
        },

        clearContent: function (elemento) {
            var hijos = $(elemento).children();
            if (hijos.length > 0) {
                $(elemento).children().remove();
            }
        },

        formatoDDMMAAAA: function (fecha) {
            var fechaFormato = fecha.split('/')[2] + "/" + fecha.split('/')[1] + "/" + fecha.split('/')[0];
            return fechaFormato;
        },

        validaFechaDDMMAAAA: function (fecha) {
            var dtCh = "/";
            var minYear = 1900;
            var maxYear = new Date().getFullYear();
            function isInteger(s) {
                var i;
                for (i = 0; i < s.length; i++) {
                    var c = s.charAt(i);
                    if (((c < "0") || (c > "9"))) return false;
                }
                return true;
            }
            function stripCharsInBag(s, bag) {
                var i;
                var returnString = "";
                for (i = 0; i < s.length; i++) {
                    var c = s.charAt(i);
                    if (bag.indexOf(c) == -1) returnString += c;
                }
                return returnString;
            }
            function daysInFebruary(year) {
                return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
            }
            function DaysArray(n) {
                for (var i = 1; i <= n; i++) {
                    this[i] = 31
                    if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
                    if (i == 2) { this[i] = 29 }
                }
                return this
            }
            function isDate(dtStr) {
                var daysInMonth = DaysArray(12)
                var pos1 = dtStr.indexOf(dtCh)
                var pos2 = dtStr.indexOf(dtCh, pos1 + 1)
                var strDay = dtStr.substring(0, pos1)
                var strMonth = dtStr.substring(pos1 + 1, pos2)
                var MonthStringInt = dtStr.substring(pos1 + 1, pos2)
                var strYear = dtStr.substring(pos2 + 1)
                var YearStringInt = dtStr.substring(pos2 + 1)
                strYr = strYear
                if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1)
                if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1)
                for (var i = 1; i <= 3; i++) {
                    if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
                }
                month = strMonth;
                day = strDay;
                year = strYr;
                if (pos1 == -1 || pos2 == -1) {
                    return false
                }
                if (strMonth.length < 1 || month < 1 || month > 12 || MonthStringInt.length > 2) {
                    return false
                }
                if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month]) {
                    return false
                }
                if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
                    return false
                }
                if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
                    return false
                }
                return true
            }
            if (isDate(fecha)) {
                return true;
            } else {
                return false;
            }
        }


    }

})(jQuery);