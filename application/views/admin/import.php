                            <?php echo $this->session->flashdata('notif') ?>
                            <div class="row">
							<div class="col-lg-12">
								<section class="panel">
									<header class="panel-heading">
										<div class="panel-actions">
											<a href="#" class="fa fa-caret-down"></a>
											<a href="#" class="fa fa-times"></a>
										</div>
						
										<h2 class="panel-title"><?php echo $header;?></h2>
									</header>
									<div class="panel-body">
										<form class="form-horizontal form-bordered" action="<?php echo base_url("admin/admin/do_import"); ?>" method="post" enctype="multipart/form-data">
											<div class="form-group">
												<label class="col-md-3 control-label">File Upload</label>
												<div class="col-md-6">
                                                    <input type="file" name="userfile" class="form-control">
                                                </div>

                                                <button type="submit" class="btn btn-success">UPLOAD</button>
                                        </form>
                                                </div>
                                                <a href="<?php echo base_url('download');?>"><button type="submit" class="btn btn-default">UNDUH TEMPLATE</button></a>
                                            
											</div>
                                        </form>   
									</div>
								</section>
						