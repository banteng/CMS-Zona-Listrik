<?php if(!defined('BASEPATH')) exit('Hacking Attempt : Keluar dari sistem..!!');

class Login extends CI_Controller
{
  public function __construct()
  {
    parent::__construct();
    $this->load->model('admin/admin_login');
    $this->load->library('form_validation','session');
    $this->load->database();
    $this->load->helper('url', 'security');
    error_reporting(0); 
    
  }  
  
  public function index()
  {
      $session = $this->session->userdata('isLoginAdmin');
     
      if($session == FALSE)
      {
        redirect('admin/login');
      }
      else
      {
        redirect('admin/dashboard');
      }
  }
    
  public function do_login()
  {
    $this->form_validation->set_rules('username', 'Username', 'required|trim|xss_clean');
    $this->form_validation->set_rules('password', 'Password', 'required|xss_clean');
    $this->form_validation->set_error_delimiters('<span class="error">', '</span>');
    
      if($this->form_validation->run() == FALSE)
      {
        $this->load->view('admin/view_login');
      }else
      {
       $username = $this->input->post('username', TRUE);
       $password = $this->input->post('password', TRUE);
       $cek = $this->model_login->ambilPengguna($username, $password);
       //print_r($cek);die;
       if($cek > 0){
            $this->session->set_userdata('isLoginAdmin', TRUE);
            $this->session->set_userdata('id',$cek->ID);
            $this->session->set_userdata('username',$username);
            $this->session->set_userdata('nama',$cek->USERNAME);

            /*
            $log = array(
                        'LOG_USER' => $username,
                        'LOG_TIME' => $now,
                        'LOG_USER_AGENT' => $_SERVER['HTTP_USER_AGENT'],
                        'LOG_IP' => $this->getUserIpAddr(),
                        'LOG_SUCCESS' => 1
                        );
            $this->model_login->save_login('log_auth', $log);
            */
            redirect('admin/dashboard');
        }else{
            $this->session->set_flashdata('status', "<strong>User tidak terdaftar</strong>");
			      $this->session->set_flashdata('clr', 'danger');  
          
            redirect('admin/login');
        }
        
	 }	  
  }
  
  public function logout()
  {
   $this->session->sess_destroy();
   redirect('admin/login');
  }
}
?>