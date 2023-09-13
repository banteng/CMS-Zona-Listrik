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

    <!-- Data Tables CSS -->
		<link rel="stylesheet" href="<?php echo base_url();?>theme/admin/assets/vendor/select2/select2.css" />
		<link rel="stylesheet" href="<?php echo base_url();?>theme/admin/assets/vendor/jquery-datatables-bs3/assets/css/datatables.css" />

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

          
    <div class="site-section bg-light" id="services-section">
        <div class="container">
          <div class="row mb-5 justify-content-center">
            <div class="col-md-7 text-center">
              <div class="block-heading-1">
                <h2>Data pencarian</h2>
                </div>
            </div>
          </div>
          <table class="table table-bordered table-striped mb-none" id="datatable-default">
									<thead>
										<tr>
											<th width="20%">No. </th>
											<th width="25%">Kode Poin</th>
											<th width="25%">Kode Toko</th>
											<th width="15%">Hasil Poin</th>
										</tr>
									</thead>
									<tbody>
                      <?php 
                         $no=1; 
                         foreach($results as $data): 
                      ?>
                            <tr>
                              <th><?php echo $no++;?></th>
                              <th><?php echo $data->KODE_POIN;?></th>
                              <th><?php echo $data->KODE_TOKO;?></th>
                              <th><?php echo $data->HASIL_POIN;?></th>
                            </tr>
                        <?php endforeach; ?>
									</tbody>
                </table>
        <!-- END .ftco-cover-1 -->
   
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

    <!-- Data Tables -->
		<script src="<?php echo base_url();?>theme/admin/assets/vendor/select2/select2.js"></script>
		<script src="<?php echo base_url();?>theme/admin/assets/vendor/jquery-datatables/media/js/jquery.dataTables.js"></script>
		<script src="<?php echo base_url();?>theme/admin/assets/vendor/jquery-datatables/extras/TableTools/js/dataTables.tableTools.min.js"></script>
		<script src="<?php echo base_url();?>theme/admin/assets/vendor/jquery-datatables-bs3/assets/js/datatables.js"></script>

		<script src="<?php echo base_url();?>theme/admin/assets/javascripts/tables/examples.datatables.default.js"></script>
		<script src="<?php echo base_url();?>theme/admin/assets/javascripts/tables/examples.datatables.row.with.details.js"></script>
		<script src="<?php echo base_url();?>theme/admin/assets/javascripts/tables/examples.datatables.tabletools.js"></script>


  </body>

</html>
