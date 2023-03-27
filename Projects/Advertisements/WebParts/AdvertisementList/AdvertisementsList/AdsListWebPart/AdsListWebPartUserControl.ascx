<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdsListWebPartUserControl.ascx.cs" Inherits="AdvertisementsList.AdsListWebPart.AdsListWebPartUserControl" %>

<head>
	<meta charset="utf-8">
	<title>Marketplace</title>
	<meta name="description" content="">

	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
	
	<link rel="stylesheet" href="http://sharepoint:2013/SiteAssets/marketplace/css/owl.carousel.min.css">
	<link rel="stylesheet" href="http://sharepoint:2013/SiteAssets/marketplace/css/owl.theme.default.min.css">

	<link rel="stylesheet" href="http://sharepoint:2013/SiteAssets/marketplace/libs/bootstrap/css/bootstrap-grid.min.css">
	<link href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700,900&amp;subset=cyrillic,cyrillic-ext" rel="stylesheet">
	<link rel="stylesheet" href="http://sharepoint:2013/SiteAssets/marketplace/css/main.css">
	<link rel="stylesheet" href="http://sharepoint:2013/SiteAssets/marketplace/css/media.css">

</head>

<body>
<div id="wrapper">
	<div class="main bg-white">
		<div class="toping-box">
			<div class="container">
				<ul class="pagin">
					<li><a href="">Главная</a></li>
					<li><a href="">Кредитный департамент</a></li>
					<li><a hre	f="">Отдел прогназирования спроса</a></li>
				</ul>
				<div class="title">Доска объявлений</div>
                    <div class="first-row">
                        <div class="choices-inp">
						    <p class="label">Объявления</p>
						    <div class="choices">
							    <a href="#active" class="choice active" onclick="selectStatus('Active', this)">Активные</a>
							    <a href="#moderation" class="choice" onclick="selectStatus('Moderation', this)">На модерации</a>
						    </div>
					    </div>
					    <div class="inp">
						    <p class="label">Категории</p>
						    <select class="form-control" id="Categories">
							    <option value="">Any</option>
                                <option disabled value="">Pending...</option>
						    </select>
					    </div>
					    <input class="btn" type="submit" value="+ Create Advertisement">
				    </div>
                    <div class="second-row">
                         <div class="ch">
						    <input type="checkbox" id="c1">
						    <label for="c1">Мои объявления</label>
					    </div>
                    </div>
                </div>
			</div>
		</div>
		<div class="container">
			<div class="two-columns">
				<div class="row">
					<div class="col-md-9 col-sm-12">
						<div class="content-box">
							<div class="news-box section-padding">
								<div class="news-item pending">Pending...</div>
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
									<li><a href="">Анонсы</a></li>
									<li><a href="">Оргструктура</a></li>
									<li><a href="">Онлайн библиотека</a></li>
									<li><a href="">Объявления</a></li>
								</ul>
								<a href="" class="like">Скажи спасибо</a>
							</div>
							<div class="aside-box birthday-box">
								<div class="aside-title title">Ближайшие дни рождения</div>
								<ul class="list">
									<li>
										<div class="img"><a href=""><img src="http://sharepoint:2013/SiteAssets/marketplace/images/face.png" alt=""></a></div>
										<div class="tt">
											<div class="date">26 марта</div>
											<div class="name"><a href="">Анна Ефремова</a></div>
											<div class="fah">Дизайнер</div>
										</div>
									</li>
									<li>
										<div class="img"><a href=""><img src="http://sharepoint:2013/SiteAssets/marketplace/images/face.png" alt=""></a></div>
										<div class="tt">
											<div class="date">26 марта</div>
											<div class="name"><a href="">Анна Ефремова</a></div>
											<div class="fah">Дизайнер</div>
										</div>
									</li>
								</ul>
								<a href="" class="more-link">Показать больше</a>
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

	<script src="http://sharepoint:2013/SiteAssets/marketplace/libs/jquery/jquery-1.11.2.min.js"></script>
    <script src="http://sharepoint:2013/SiteAssets/marketplace/utils/helper.js"></script>
    <script src="http://sharepoint:2013/SiteAssets/marketplace/js/script.js"></script>
	<script src="http://sharepoint:2013/SiteAssets/marketplace/libs/plugins-scroll/plugins-scroll.js"></script>
	<script src="http://sharepoint:2013/SiteAssets/marketplace/js/owl.carousel.js"></script>

	<script src="http://sharepoint:2013/SiteAssets/marketplace/js/common.js"></script>
</body>