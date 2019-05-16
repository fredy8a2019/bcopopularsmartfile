<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DevExpress.Web.Mvc.UI" %>
<%@ Import Namespace="DevExpress.Web.Mvc" %>
<%@ Import Namespace="GestorDocumental.Models" %>
<%
    Html.DevExpress().PivotGrid(settings => {
        
        settings.Name = "pivotGrid";         
        settings.Theme = "Glass";
        settings.CallbackRouteValues = new { 
            Controller = "Report", Action = "PivotGridPartial" };
       
        settings.Width = Unit.Percentage(100);
        settings.OptionsPager.RowsPerPage = 5000;            
        
        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "NegId";
            field.Caption = "Negocio";
            field.CollapseAll();
        });
        
        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "Etapa";
            field.Caption = "Etapa";
        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "Terminado";
            field.Visible = false;
            field.Caption = "Finalizado";
        });

        //settings.Fields.Add(field =>
        //{
        //    field.Area = PivotArea.FilterArea;     
        //    field.AreaIndex = 0;
        //    field.FieldName = "Etapa";
        //    field.Caption = "Etapa Dos";
        //});

        //settings.Fields.Add(field =>
        //{
        //    field.Area = PivotArea.FilterArea;
        //    field.AreaIndex = 0;
        //    field.FieldName = "Estado";
        //    field.Caption = "Estado Dos";
        //});
        
        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            //field.AllowedAreas = PivotGridAllowedAreas.FilterArea;
            field.AreaIndex = 0;            
            field.FieldName = "Estado";
            field.Caption = "Estado";
        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.Visible = false;    
            field.FieldName = "Ususario";
            field.Caption = "Usuario";
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
            field.FieldName = "Tiempo";
            field.Visible = false;    
            field.Caption = "Tiempo etapa";
            
        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "cliente";
            field.Caption = "Cliente";

        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "nitcliente";
            field.Visible = false;    
            field.Caption = "Nit Cliente";

        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "oficina";
            field.Caption = "Oficina";

        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "HoraTerminacion";
            field.Visible = false;    
            field.Caption = "Hora fin";
        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "producto";
            field.Caption = "Producto";
        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.FilterArea;
            field.AreaIndex = 0;
            field.FieldName = "HoraInicio";
            field.Visible = false;    
            field.Caption = "Hora inicio";
        });

        //settings.Fields.Add(field =>
        //{
        //    field.Area = PivotArea.FilterArea;
        //    field.AreaIndex = 0;
        //    field.FieldName = "Productividad";
        //    field.Caption = "Productividad";
        //    //field.UnboundType = DevExpress.Data.UnboundColumnType.String;
        //    field.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        //    ///field.CellFormat.FormatString = "p2";
        //});

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
            field.FieldName = "Year";
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
            field.FieldName = "LoteScaner";
            field.Visible = false;    
            field.Caption = "Numero Lote";
        });
        
        
        //settings.CustomCallback = (sender, e) =>
        //{            
        //    UpdatePivotGridLayout((MVCxPivotGrid)sender);
        //};
    })    
    .Bind(Model)
    .Render();
%>