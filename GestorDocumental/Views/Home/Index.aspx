<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Inicio Smart File
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .img {
            color: #666;
            background: #333;
            height: 300px !important;
            padding-top: 90px;
            display: block;
            font-size: 52px;
            text-align: center;
            font-family: "trebuchet ms", sans-serif;
        }

        .carousel {
            margin: 20px 0;
            /*width: 90%; JFP*/
            width: 100%;
            margin-top: 3%;
        }

        .bs-example {
            margin: 20px;
        }

        .carousel.carousel-fade .item {
            -webkit-transition: opacity 0.5s ease-in-out;
            -moz-transition: opacity 0.5s ease-in-out;
            -ms-transition: opacity 0.5s ease-in-out;
            -o-transition: opacity 0.5s ease-in-out;
            transition: opacity 0.5s ease-in-out;
            opacity: 0;
        }

        .carousel.carousel-fade .active.item {
            opacity: 1;
        }

        .carousel.carousel-fade .active.left,
        .carousel.carousel-fade .active.right {
            left: 0;
            z-index: 2;
            opacity: 0;
            filter: alpha(opacity=0);
        }

        .carousel.carousel-fade .next,
        .carousel.carousel-fade .prev {
            left: 0;
            z-index: 1;
        }

        .carousel.carousel-fade .carousel-control {
            z-index: 3;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
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
                    <h1></h1>
                </div>
            </div>
            <div class="item">
                <img src="../../Images/Slides/Slide2.png" alt="banner2" />
                <div class="carousel-caption">
                    <h1></h1>
                </div>
            </div>
            <div class="item">
                <img src="../../Images/Slides/Slide3.png" alt="banner3" />
                <div class="carousel-caption">
                    <h1></h1>
                </div>
            </div>
        </div>
        <!-- Carousel nav -->
        <a class="carousel-control left" href="#myCarousel" data-slide="prev">
            <span class="glyphicon glyphicon-chevron-left"></span>
        </a>
        <a class="carousel-control right" href="#myCarousel" data-slide="next">
            <span class="glyphicon glyphicon-chevron-right"></span>
        </a>
    </div>

    <script>
        $(document).ready(function () {
            $('.carousel').carousel({
                interval: 2000
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
