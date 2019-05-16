<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DevExpress.Web.Mvc.UI" %>
<%@ Import Namespace="DevExpress.Web.Mvc" %>
<%@ Import Namespace="GestorDocumental.Models" %>
<%
    Html.DevExpress().PivotGrid(settings => {
        
        settings.Name = "pivotGrid";
        settings.Theme = "Glass";
        settings.CallbackRouteValues = new {
            Controller = "Report",
            Action = "PivotGridPartialFacturacion"
        };
                
        settings.CustomCallback = (sender, e) =>
        {
            UpdatePivotGridLayout((MVCxPivotGrid)sender);
            
        };
       
        settings.Width = Unit.Percentage(100);
        settings.OptionsPager.RowsPerPage = 5000;
        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "Cliente";
            field.Caption = "Cliente";
            field.CollapseAll();
        });
        
        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "Oficina";
            field.Caption = "Oficina";
        });
        
        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "Producto";
            field.Caption = "Producto";
        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "Sociedad";
            field.Caption = "Sociedad";
        });
        
        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "CodigoBarras";
            field.Visible = false;
            field.Caption = "CodigoBarras";
        });

              
        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            //field.AllowedAreas = PivotGridAllowedAreas.FilterArea;
            field.AreaIndex = 0;            
            field.FieldName = "Etapa";
            field.Caption = "Etapa";
        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "Cantidad";
            field.Caption = "Cantidad";
        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "FechaRadicacion";
            field.Caption = "Fecha Radicacion";
        });
               

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "Mes";
            field.Visible = false;    
            field.Caption = "Mes";
            field.CellFormat.FormatType = FormatType.DateTime;
        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "Año";
            field.Visible = false;    
            field.Caption = "Año";
        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "Dia";
            field.Visible = false;    
            field.Caption = "Dia";
        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "Dia";
            field.Visible = false;
            field.Caption = "Dia";
        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;            
            field.FieldName = "Precio";
            field.Visible = true;
            field.Caption = "Total  ";
            field.CellFormat.FormatType = FormatType.Custom;
            field.CellFormat.FormatString =  "{0:$#,##0.00}";
        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "Precio";
            field.Visible = true;
            field.Caption = "Precio unitario";
            //field.ValueTemplate
            field.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Average;
            
            field.CellFormat.FormatType = FormatType.Custom;
            field.CellFormat.FormatString = "{0:$#,##0.00}";
        });        

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "Rango";
            field.Visible = true;
            field.Caption = "Rango";
        });

 
    })    
    .Bind(Model)
    .Render();
    
    
%>

<script runat="server">
    public void UpdatePivotGridLayout(MVCxPivotGrid pivotGrid){
        //EL SWITCH ME DETERMINA SI VOY A SALVAR O VOY A RESTAURAR
        
        string nombre = "PivotGrid.xlsx";
        Session["nombre_archivo"] = nombre;
        new MVCxPivotGridExporter(pivotGrid).ExportToXlsx(Server.MapPath("~/Content/Tmp/"+nombre));   
                
    }

    private void pivotGridControl1_CustomSummary(object sender,
  PivotGridCustomSummaryEventArgs e)
    {
        //if (e.DataField != fieldExtendedPrice) return;
        //// A variable which counts the number of orders whose sum exceeds $500.
        //int order500Count = 0;
        //// Get the record set corresponding to the current cell.
        //PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();
        //// Iterate through the records and count the orders.
        //for (int i = 0; i < ds.RowCount; i++)
        //{
        //    PivotDrillDownDataRow row = ds[i];
        //    // Get the order's total sum.
        //    decimal orderSum = (decimal)row[fieldExtendedPrice];
        //    if (orderSum >= minSum) order500Count++;
        //}
        //// Calculate the percentage.
        //if (ds.RowCount > 0)
        //{
        //    e.CustomValue = (decimal)order500Count / ds.RowCount;
        //}

        e.CustomValue = 9;
    }
</script>

