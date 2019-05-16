var _temp = null;
(function ($) {

    _temp = window._temp = {

        grillaAlmacenar: function (temp, _trID, _valPosicion, _valLote_Negocio, _valTipo, _valCampo1) {
            temp = temp.replace(/{{trID}}/ig, _trID)
                       .replace(/{{valPosicion}}/ig, _valPosicion)
                       .replace(/{{valLote_Negocio}}/ig, _valLote_Negocio)
                       .replace(/{{valTipo}}/ig, _valTipo)
            //.replace(/{{valCampo1}}/ig, _valCampo1)
            ;

            return temp;
        },

        grillaDetalle: function (temp, _trID, _valNegocio, _valNo_Paginas, _valCodigo_Barras, _valCampo2) {
            temp = temp.replace(/{{trID}}/ig, _trID)
                        .replace(/{{valNegocio}}/ig, _valNegocio)
                        .replace(/{{valNo_Paginas}}/ig, _valNo_Paginas)
                        .replace(/{{valCodigo_Barras}}/ig, _valCodigo_Barras)
            //.replace(/{{valCampo2}}/ig, _valCampo2)
            ;

            return temp;
        }

    }
})(jQuery);