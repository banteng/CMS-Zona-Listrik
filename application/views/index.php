<!doctype html>
<html lang="en">

  <head>
    <title>Zona Listrik</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    
    <link href="https://fonts.googleapis.com/css?family=Rubik:300,400,700|Oswald:400,700" rel="stylesheet">

    <link rel="stylesheet" href="<?php echo base_url();?>theme/frontend/v1/fonts/icomoon/style.css">

    <link rel="stylesheet" href="<?php echo base_url();?>theme/frontend/v1/css/bootstrap.min.css">
    <link rel="stylesheet" href="<?php echo base_url();?>theme/frontend/v1/css/jquery.fancybox.min.css">
    <link rel="stylesheet" href="<?php echo base_url();?>theme/frontend/v1/css/owl.carousel.min.css">
    <link rel="stylesheet" href="<?php echo base_url();?>theme/frontend/v1/css/owl.theme.default.min.css">
    <link rel="stylesheet" href="<?php echo base_url();?>theme/frontend/v1/fonts/flaticon/font/flaticon.css">
    <link rel="stylesheet" href="<?php echo base_url();?>theme/frontend/v1/css/aos.css">

    <!-- MAIN CSS -->
    <link rel="stylesheet" href="<?php echo base_url();?>theme/frontend/v1/css/style.css">

  </head>

  <body data-spy="scroll" data-target=".site-navbar-target" data-offset="300">

    <div id="overlayer"></div>
    <div class="loader">
      <div class="spinner-border text-primary" role="status">
        <span class="sr-only">Loading...</span>
      </div>
    </div>

    <div class="site-wrap" id="home-section">

      <div class="site-mobile-menu site-navbar-target">
        <div class="site-mobile-menu-header">
          <div class="site-mobile-menu-close mt-3">
            <span class="icon-close2 js-menu-toggle"></span>
          </div>
        </div>
        <div class="site-mobile-menu-body"></div>
      </div>

      <header class="site-navbar js-sticky-header site-navbar-target" role="banner">

        <div class="container">
          <div class="row align-items-center position-relative">


            <div class="site-logo">
              <a href="<?php echo base_url();?>" class="text-black"><span class="text-primary">zonalistrik</a>
            </div>

            <div class="col-12">
              <nav class="site-navigation text-right ml-auto " role="navigation">

                <ul class="site-menu main-menu js-clone-nav ml-auto d-none d-lg-block">
                  <li><a href="#home-section" class="nav-link">Home</a></li>
                  <li><a href="#services-section" class="nav-link">Cari</a></li>
                </ul>
              </nav>

            </div>

            <div class="toggle-button d-inline-block d-lg-none"><a href="#" class="site-menu-toggle py-5 js-menu-toggle text-black"><span class="icon-menu h3"></span></a></div>

          </div>
        </div>

      </header>

      <div class="ftco-blocks-cover-1">
        <div class="ftco-cover-1 overlay" style="background-image: url('https://source.unsplash.com/pSyfecRCBQA/1920x780')">
          <div class="container">
            <div class="row align-items-center">
              <div class="col-lg-6">
                <h1>Choose Your Quality Delivery of Your Cargo</h1>
                <p class="mb-5">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Est magni perferendis fugit modi similique, suscipit, deserunt a iure.</p>
                <form action="<?php echo base_url("welcome/search");?>" method="POST">
                  <div class="form-group d-flex">
                    <input type="text" class="form-control" placeholder="Masukan hasil poin/kode toko/kode poin" name="keyword">
                    <input type="submit" class="btn btn-primary text-white px-4" value="Cari!">
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>
        <!-- END .ftco-cover-1 -->
   
        <div id="view">  
            
        </div>
    </div>

    <script src="<?php echo base_url();?>theme/frontend/v1/js/jquery-3.3.1.min.js"></script>
    <script src="<?php echo base_url();?>theme/frontend/v1/js/popper.min.js"></script>
    <script src="<?php echo base_url();?>theme/frontend/v1/js/bootstrap.min.js"></script>
    <script src="<?php echo base_url();?>theme/frontend/v1/js/owl.carousel.min.js"></script>
    <script src="<?php echo base_url();?>theme/frontend/v1/js/jquery.sticky.js"></script>
    <script src="<?php echo base_url();?>theme/frontend/v1/js/jquery.waypoints.min.js"></script>
    <script src="<?php echo base_url();?>theme/frontend/v1/js/jquery.animateNumber.min.js"></script>
    <script src="<?php echo base_url();?>theme/frontend/v1/js/jquery.fancybox.min.js"></script>
    <script src="<?php echo base_url();?>theme/frontend/v1/js/jquery.easing.1.3.js"></script>
    <script src="<?php echo base_url();?>theme/frontend/v1/js/aos.js"></script>

    <script src="<?php echo base_url();?>theme/frontend/v1/js/main.js"></script>


  </body>

</html>
