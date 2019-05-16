using System;
using System.Collections.Generic;

namespace GestorDocumental.WebService
{
    public class DatosEntrada
    {
        public decimal neg_id { get; set; }
        public decimal id_neg_cabecera { get; set; }
        public string accion { get; set; }
        public string origen { get; set; }
        public string land1 { get; set; }
        public string facturaPDFBase64 { get; set; }
        public decimal? estado { get; set; }
        public List<Factura_cabecera> FacturaCabecera { get; set; }
        public List<Factura_posiciones> FacturaPosiciones { get; set; }
        public List<Factura_impuestos> FacturaImpuestos { get; set; }
    }

    public class Factura_cabecera
    {
        public decimal neg_id { get; set; }
        public string fac_id { get; set; }
        public string doc_type { get; set; }
        public string doc_number { get; set; }
        public DateTime? doc_date { get; set; }
        public string vendor_rfc { get; set; }
        public string company_rfc { get; set; }
        public decimal? total_amount { get; set; }
        public string currency_code { get; set; }
        public decimal? employee_number { get; set; }
        public string purchase_order { get; set; }
        public decimal? discount_amount { get; set; }
        public decimal? withholding_tax { get; set; }
        public string advance_flag { get; set; }
        public string fiscal_doc_number { get; set; }
    }

    public class Factura_posiciones
    {
        public string delivery_note { get; set; }
        public DateTime? deliveryDate { get; set; }
        public string item_text { get; set; }
        public decimal? item_quantity { get; set; }
        public decimal? item_price { get; set; }
        public decimal? item_amount { get; set; }
        public decimal? item_percentage { get; set; }
    }

    public class Factura_impuestos
    {
        public decimal vat_base_amount { get; set; }
        public decimal vat_percentage { get; set; }
        public decimal vat_tax_amount { get; set; }
        public string vat_tax_id { get; set; }
    }

    public class Resultado
    {
        public string rcode { get; set; }
        public List<ResultadoDetalle> IT_Resultado { get; set; }
    }

    public class ResultadoDetalle
    {
        public string ok_code { get; set; }
        public string descripcion { get; set; }
    }

    public class IndicesGrillas
    {
        public int grillaPosiciones { get; set; }
        public int grillaImpuestos { get; set; }
    }
}