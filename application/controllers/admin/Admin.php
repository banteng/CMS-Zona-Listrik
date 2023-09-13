<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Admin extends CI_Controller {

    /*declare template*/
	private $_template = "admin/template_admin/template_admin";
    

    public function __construct()
	{
		parent::__construct();
		$this->load->model('admin/admin_model');
		$this->load->helper(['url_helper', 'form', 'download']);
		$this->load->library(['form_validation', 'session', 'upload',]);
        //error_reporting(0); 
	}
    
    /*session check */
    /*use $this->cek_sesi() for check session every new function */
    function cek_sesi(){
	    if($this->session->userdata('isLoginAdmin') == FALSE)
	    {
	      redirect('admin/login');
	    }
        else
	    {
	      $this->load->model('admin_login');
	      
	      $user = $this->session->userdata('username');
	      
	      $data['level'] = $this->session->userdata('level');        
	      $data['pengguna'] = $this->admin_login->dataPengguna($user);
		}
	}
    /* end check session*/
    
	public function index()
	{
        $this->cek_sesi();
        $data['title']  = "Dashboard - zonalistrik.com";
        $data['header'] = "Dashboard";

		$data['page']   = 'admin/view_index';
        
		$this->load->view($this->_template,$data);
	}
    
    public function poinView()
	{
        $this->cek_sesi();
        $data['title']  = "Gamma Poin - zonalistrik.com";
		$data['header'] = "Gamma Poin";

		$data['poin'] = $this->admin_model->getAllGammaPoin();

		$data['page']   = 'admin/view_poin';
		$this->load->view($this->_template,$data);
	}

	public function poinAdd($id)
	{
        $this->cek_sesi();
        
		$data['poin'] = $this->admin_model->getAllGammaPoin();

		$data['page']   = 'admin/view_poin';
		$this->load->view($this->_template,$data);
	}


	public function import()
	{
        $this->cek_sesi();
        $data['title']  = "Gamma Poin - zonalistrik.com";
		$data['header'] = "Gamma Poin";

		$data['page']   = 'admin/import';
		$this->load->view($this->_template,$data);
	}

	public function do_import()
    {
		$this->cek_sesi();
        // Load plugin PHPExcel nya
        include APPPATH.'third_party/PHPExcel/PHPExcel.php';

        $config['upload_path'] = './upload';
        $config['allowed_types'] = 'xlsx|xls|csv';
        $config['max_size'] = '10000';
        $config['encrypt_name'] = true;

        $this->load->library('upload', $config);
		$this->upload->initialize($config);
        if (!$this->upload->do_upload()) {

            //upload gagal
            $this->session->set_flashdata('notif', '<div class="alert alert-danger"><b>PROSES IMPORT GAGAL!</b> '.$this->upload->display_errors().'</div>');

            redirect('/admin/import/');
        } else {

            $data_upload = $this->upload->data();

            $excelreader     = new PHPExcel_Reader_Excel2007();
            $loadexcel         = $excelreader->load('upload/'.$data_upload['file_name']); // Load file yang telah diupload ke folder excel
            $sheet             = $loadexcel->getActiveSheet()->toArray(null, true, true ,true);

            $data = array();

            $numrow = 1;
            foreach($sheet as $row){
                            if($numrow > 1){
                                array_push($data, array(
									"KODE_POIN"  	 => $row['A'],
									"KODE_TOKO"      => $row['B'],
									"HASIL_POIN"     => $row['C']
                                ));
                    }
                $numrow++;
            }
            $this->db->insert_batch('poin', $data);
            //delete file from server
            unlink(realpath('upload/'.$data_upload['file_name']));

            //upload success
            $this->session->set_flashdata('notif', '<div class="alert alert-success"><b>PROSES IMPORT BERHASIL!</b> Data berhasil diimport!</div>');
            //redirect halaman
            redirect('/admin/import/');

        }
    }
	
    public function poinJson()
	{
        //$this->cek_sesi();
		$query = $this->admin_model->getAllGammaPoin();
		//print_r($query);die;
		$query['KODE_POIN'] = $query->KODE_POIN;
		$query['KODE_TOKO'] = $query->KODE_TOKO;
		echo json_encode($query);
	}

	public function downloadTemplate(){				
		force_download('upload/template/import.xlsx',NULL);
	}	
	
}
