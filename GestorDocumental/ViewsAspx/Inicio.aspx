<%@ Page Language="C#" MasterPageFile="Site1.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs"
    Inherits="GestorDocumental.ViewsAspx.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .img
        {
            color: #666;
            background: #333;
            height: 300px !important;
            padding-top: 90px;
            display: block;
            font-size: 52px;
            text-align: center;
            font-family: "trebuchet ms" , sans-serif;
        }
        
        .carousel
        {
            margin: 20px 0;
            width: 90%;
            margin-top:3%;
        }
        
        .bs-example
        {
            margin: 20px;
        }
    </style>
    <script>
        $(document).ready(function () {
            $('.carousel').carousel({
                interval: 2300
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="myCarousel" class="carousel slide">
        <ol class="carousel-indicators">
            <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
            <li data-target="#myCarousel" data-slide-to="1"></li>
            <li data-target="#myCarousel" data-slide-to="2"></li>
        </ol>
        <!-- Carousel items -->
        <div class="carousel-inner">
            <div class="active item">
                <img src="../../Images/Slides/Slide1.png" alt="banner1" />
                <div class="carousel-caption">
                    <h1>
                    </h1>
                </div>
            </div>
            <div class="item">
                <img src="../../Images/Slides/Slide2.png" alt="banner2" />
                <div class="carousel-caption">
                    <h1>
                    </h1>
                </div>
            </div>
            <div class="item">
                <img src="../../Images/Slides/Slide3.png" alt="banner3" />
                <div class="carousel-caption">
                    <h1>
                    </h1>
                </div>
            </div>
        </div>
        <!-- Carousel nav -->
        <a class="carousel-control left" href="#myCarousel" data-slide="prev"><span class="glyphicon glyphicon-chevron-left">
        </span></a><a class="carousel-control right" href="#myCarousel" data-slide="next"><span
            class="glyphicon glyphicon-chevron-right"></span></a>
    </div>
</asp:Content>
