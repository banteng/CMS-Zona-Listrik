<?php if(!defined('BASEPATH')) exit('Hacking Attempt!!!');

class Admin_model extends CI_Model
{
  public function __construct()
  {
    parent::__construct();
  }

 public function getAllGammaPoin()
  {
        return $this->db->get('poin')->result();
  }
}
