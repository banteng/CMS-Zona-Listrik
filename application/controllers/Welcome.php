<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Welcome extends CI_Controller {

	/**
	 * Index Page for this controller.
	 *
	 * Maps to the following URL
	 * 		http://example.com/index.php/welcome
	 *	- or -
	 * 		http://example.com/index.php/welcome/index
	 *	- or -
	 * Since this controller is set as the default controller in
	 * config/routes.php, it's displayed at http://example.com/
	 *
	 * So any other public methods not prefixed with an underscore will
	 * map to /index.php/welcome/<method_name>
	 * @see https://codeigniter.com/user_guide/general/urls.html
	 */

	public function __construct()
	{
		parent::__construct();
		$this->load->model('model_index');
		$this->load->helper(['url_helper', 'form', 'download']);
		$this->load->library(['form_validation', 'session', 'upload',]);
        //error_reporting(0); 
	}
    
	public function index()
	{
		$this->load->view('index');
	}

	function search()
    {
        $keyword    =   $this->input->post('keyword');
        $data['results']    =   $this->model_index->search($keyword);
        $this->load->view('hasil',$data);
	}

}
