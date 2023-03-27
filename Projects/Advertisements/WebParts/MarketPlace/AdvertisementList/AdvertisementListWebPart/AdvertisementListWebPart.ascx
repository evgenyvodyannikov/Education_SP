<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdvertisementListWebPart.ascx.cs" Inherits="AdvertisementList.AdvertisementListWebPart.AdvertisementListWebPart" %>

<head>
	<meta charset="utf-8">
	<title>Marketplace</title>
	<meta name="description" content="">

	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
	
	<link rel="stylesheet" href="../_layouts/15/AdvertisementList/css/owl.carousel.min.css">
	<link rel="stylesheet" href="../_layouts/15/AdvertisementList/css/owl.theme.default.min.css">
    <link rel="icon" href="../_layouts/15/AdvertisementList/img/online-shopping.png">

	<link rel="stylesheet" href="../_layouts/15/AdvertisementList/libs/bootstrap/css/bootstrap-grid.min.css">
	<link href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700,900&amp;subset=cyrillic,cyrillic-ext" rel="stylesheet">
    <link href="../_layouts/15/AdvertisementList/css/main.css?ver=<%=DateTime.Now.Millisecond%>" rel="stylesheet" />
	<link rel="stylesheet" href="../_layouts/15/AdvertisementList/css/media.css">

</head>

<body>
<div id="wrapper">
	<div class="main bg-white">
		<div class="toping-box">
			<div class="container">
				<ul class="pagin">
					<li><a href="#">Главная</a></li>
					<li><a href="#">Объявления</a></li>
				</ul>
				<div class="title">Доска объявлений</div>
 				<div class="thanks-from default-form cat-form desk-form">
					<div class="inp btns-inp inactive">
						<p class="label">Объявления</p>
						<div class="btns">
							<a href="#active" class="btn active" id="first">Активные</a>
							<a href="#moderation" class="btn" id="second">На модерации</a>
						</div>
					</div>
					<div class="inp select-inp">
						<p class="label">Категории</p>
						 <select id="Categories">
							<option value="">Любая</option>
                            <option disabled value="">Загрузка...</option>
						 </select>
					</div>
					<div class="ch">
						<input type="checkbox" id="c1">
						<label for="c1">Мои объявления</label>
					</div>
					<input class="btn" type="button" value="+ Добавить объявление">
				</div>
			</div>
		</div>
		<div class="container">
			<div class="two-columns">
				<div class="row">
					<div class="col-md-9 col-sm-12">
						<div class="content-box">
							<div class="news-box section-padding">
								<div class="news-item pending">Загрузка...</div>
								<div class="pager">
									<ul>
									</ul>
								</div>
							</div>

						</div>
					</div>
					<div class="col-md-3 col-sm-12">
						<div class="aside aside-moved-top">
							<div class="aside-box fast-links">
								<div class="aside-title title">Быстрые ссылки</div>
								<ul class="list">
									<li><a href="#">Анонсы</a></li>
									<li><a href="#">Оргструктура</a></li>
									<li><a href="#">Онлайн библиотека</a></li>
									<li><a href="#">Объявления</a></li>
								</ul>
								<a href="#" class="like">Скажи спасибо</a>
							</div>
							<div class="aside-box birthday-box">
								<div class="aside-title title">Ближайшие дни рождения</div>
								<ul class="list">
									<li>
										<div class="img"><a href="#"><img src="../_layouts/15/AdvertisementList/img/face.png" alt=""></a></div>
										<div class="tt">
											<div class="date">26 марта</div>
											<div class="name"><a href="#">Анна Ефремова</a></div>
											<div class="fah">Дизайнер</div>
										</div>
									</li>
									<li>
										<div class="img"><a href="#"><img src="../_layouts/15/AdvertisementList/img//face.png" alt=""></a></div>
										<div class="tt">
											<div class="date">26 марта</div>
											<div class="name"><a href="#">Анна Ефремова</a></div>
											<div class="fah">Дизайнер</div>
										</div>
									</li>
								</ul>
								<a href="#" class="more-link">Показать больше</a>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<div class="footer footer-inner">
		<div class="container">
			<div class="row">
				<div class="col-sm-12">
					<div class="wrap">
						<div class="copy">© 2018 РН Банк. Корпоративный портал сотрудников банка.</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>


	<!--[if lt IE 9]>
	<script src="libs/html5shiv/es5-shim.min.js"></script>
	<script src="libs/html5shiv/html5shiv.min.js"></script>
	<script src="libs/html5shiv/html5shiv-printshiv.min.js"></script>
	<script src="libs/respond/respond.min.js"></script>
	<![endif]-->

	<script src="../_layouts/15/AdvertisementList/libs/jquery/jquery-1.11.2.min.js"></script>
    <script src="../_layouts/15/AdvertisementList/libs/plugins-scroll/plugins-scroll.js"></script>
	<script src="../_layouts/15/AdvertisementList/js/owl.carousel.js"></script>
	<script src="../_layouts/15/AdvertisementList/js/jcf.js"></script>
	<script src="../_layouts/15/AdvertisementList/js/jcf.checkbox.js"></script>
	<script src="../_layouts/15/AdvertisementList/js/jcf.radio.js"></script>
	<script src="../_layouts/15/AdvertisementList/js/common.js"></script>
    <script src="../_layouts/15/AdvertisementList/utils/helper.js"></script>
    <script src="../_layouts/15/AdvertisementList/js/script.js?ver=<%=DateTime.Now.Millisecond%>"></script>