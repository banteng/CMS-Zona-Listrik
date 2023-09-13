<?php if(!defined('BASEPATH')) exit('Hacking Attempt : Keluar dari sistem..!!');

class Admin_login extends CI_Model
{
  public function __construct()
  {
    parent::__construct();
  }
  
  public function ambilPengguna($username, $password)
  {
    $this->db->select('*');
    $this->db->from('admin');
    $this->db->where('USERNAME', $username);
    $this->db->where('PASSWORD', $password);
    $query = $this->db->get();
    
    return $query->row();
  }  
  
  public function dataPengguna($username)
  {
   $this->db->select('*');
   $this->db->where('USERNAME', $username);
   $query = $this->db->get('admin');
   
   return $query->row();
  } 
}
?>