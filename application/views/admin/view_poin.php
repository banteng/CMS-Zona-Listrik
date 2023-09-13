                            <section class="panel">
							<header class="panel-heading">
								<div class="panel-actions">
									<a href="#" class="fa fa-caret-down"></a>
									<a href="#" class="fa fa-times"></a>
								</div>
						
								<h2 class="panel-title">Gamma Poin</h2>
							</header>
							<div class="panel-body">
							<div class="col-md-12"><span class="show-grid-block"><a href="<?php echo base_url('admin/import');?>"><button type="submit" class="btn btn-default">Import</button></a></span></div>
                                        <br>  <br>   
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
                                        foreach($poin as $data): 
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
							</div>
						</section>