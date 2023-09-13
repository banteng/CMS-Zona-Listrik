<?php if(!defined('BASEPATH')) exit('Hacking Attempt : Keluar dari sistem..!!');

class Model_index extends CI_Model
{
  public function __construct()
  {
    parent::__construct();
  }

    public function search($keyword){
        $this->db->like('KODE_POIN', $keyword);
        $this->db->or_like('KODE_TOKO', $keyword);
        $this->db->or_like('HASIL_POIN', $keyword);
        
        $result = $this->db->get('poin')->result();
        
        return $result;
    }
}
?>
