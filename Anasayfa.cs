using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KütüphaneOtomasyon
{
    public partial class Anasayfa : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Kutuphane kutuphane = new Kutuphane();
        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-67I32MNL;Initial Catalog=Kutuphane;Integrated Security=True");

        public Anasayfa()
        {
            InitializeComponent();
        }

        private void Anasayfa_Load(object sender, EventArgs e)
        {
            YenileListele();
           
         
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Kitap Numarası";
            dataGridView1.Columns[2].HeaderText = "Kitap Adı";
            dataGridView1.Columns[3].HeaderText = "Kitap Yazarı";
            dataGridView1.Columns[4].HeaderText = "Kitap Durumu";
            dataGridView1.Columns[5].HeaderText = "Giriş Tarihi";

            Toplam();
        }
        private void YenileListele()
        {
            string cümle = "select * from kitap";
            SqlDataAdapter adtr2 = new SqlDataAdapter();

            dataGridView1.DataSource = kutuphane.listele(adtr2, cümle);
        }
        private void Toplam()
        {
            int kayitsayisi;
            kayitsayisi = dataGridView1.RowCount;
            lblToplam.Text = kayitsayisi.ToString();
            YenileListele();
        }
        protected void Temizle()
        {
            txtKitapNumara.Text = "";
            txtKitapAd.Text = "";
            txtKitapYazar.Text = "";
            txtKitapDurum.Text = "";
            dtTarih.Text = "";        }



        bool durum;
        void mukerrer()
        {

            baglanti.Open();
            SqlCommand komut3 = new SqlCommand("Select * from kitap where KitapAd=@KitapAd", baglanti);
            komut3.Parameters.AddWithValue("@KitapAd", txtKitapAd.Text);
            SqlDataReader dr = komut3.ExecuteReader();

            if (dr.Read())
            {
                durum = false;

            }
            else { durum = true; }

            baglanti.Close();
        }
        void mukerrer2()
        {

            baglanti.Open();
            SqlCommand komut3 = new SqlCommand("Select * from kitap where KitapNo=@KitapNo", baglanti);
            komut3.Parameters.AddWithValue("@KitapNo", txtKitapNumara.Text);
            SqlDataReader dr = komut3.ExecuteReader();

            if (dr.Read())
            {
                durum = false;

            }
            else { durum = true; }

            baglanti.Close();
        }


        private void btnEkle_Click(object sender, EventArgs e)
        {
            mukerrer();
            mukerrer2();

            if (durum == true)
                {
                    baglanti.Open();
                    string cümle = "insert into kitap (KitapNo,KitapAd,KitapYazar,KitapDurum,KitapTarih) values (@KitapNo,@KitapAd,@KitapYazar,@KitapDurum,@KitapTarih)";
                    SqlCommand komut2 = new SqlCommand();
                    komut2.Parameters.AddWithValue("@KitapNo", txtKitapNumara.Text);
                    komut2.Parameters.AddWithValue("@KitapAd", txtKitapAd.Text);
                    komut2.Parameters.AddWithValue("@KitapYazar", txtKitapYazar.Text);
                    komut2.Parameters.AddWithValue("@KitapDurum", txtKitapDurum.Text);
                    komut2.Parameters.AddWithValue("@KitapTarih", dtTarih.Text);
                    kutuphane.ekle_sil_güncelle(komut2, cümle);
                    foreach (Control item in Controls) if (item is TextBox) item.Text = "";
                    
                    
                    
                    MessageBox.Show("Kitap Yüklendi", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Temizle();
                baglanti.Close();
                Toplam();
            }
                else
                {
                    MessageBox.Show("Bu Kitap Zaten Yüklü", "Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Temizle();
                }
            
            YenileListele();
            Toplam();
        }

        private void txtAra_EditValueChanged(object sender, EventArgs e)
        {
            string cümle = "select * from kitap where KitapAd like '%" + txtAra.Text + "%'";
            SqlDataAdapter adtr2 = new SqlDataAdapter();

            dataGridView1.DataSource = kutuphane.listele(adtr2, cümle);
        }
        private void txtKitapAd_EditValueChanged(object sender, EventArgs e)
        {
            txtKitapAd.Text = txtKitapAd.Text.ToUpper();

            txtKitapAd.SelectionStart = txtKitapAd.Text.Length;
        }

        private void txtKitapYazar_EditValueChanged(object sender, EventArgs e)
        {
            txtKitapYazar.Text = txtKitapYazar.Text.ToUpper();

            txtKitapYazar.SelectionStart = txtKitapYazar.Text.Length;
        }

        private void txtKitapDurum_EditValueChanged(object sender, EventArgs e)
        { txtKitapDurum.Text = txtKitapDurum.Text.ToUpper();

            txtKitapDurum.SelectionStart = txtKitapDurum.Text.Length;

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            
            DialogResult Secim = new DialogResult();
            Secim = MessageBox.Show("Silme İşlemini Onaylıyor Musunuz?", "Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Secim == DialogResult.Yes)
            {
                DataGridViewRow satır = dataGridView1.CurrentRow; 
                string cümle = "delete from kitap where ID='" + satır.Cells["ID"].Value.ToString() + "'";
                SqlCommand komut2 = new SqlCommand();

                kutuphane.ekle_sil_güncelle(komut2, cümle);
                foreach (Control item in Controls) if (item is TextBox) item.Text = "";
               

            }
             YenileListele();
            Toplam();           
            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
            txtKitapNumara.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtKitapAd.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtKitapYazar.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            txtKitapDurum.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            dtTarih.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string cümle = "update kitap set KitapNo=@KitapNo,KitapAd=@KitapAd,KitapYazar=@KitapYazar,KitapDurum=@KitapDurum,KitapTarih=@KitapTarih where KitapNo=@KitapNo";
            SqlCommand komut2 = new SqlCommand();
            komut2.Parameters.AddWithValue("@KitapNo", txtKitapNumara.Text);
            komut2.Parameters.AddWithValue("@KitapAd", txtKitapAd.Text);
            komut2.Parameters.AddWithValue("@KitapYazar", txtKitapYazar.Text);
            komut2.Parameters.AddWithValue("@KitapDurum", txtKitapDurum.Text);
            komut2.Parameters.AddWithValue("@KitapTarih", dtTarih.Text);
            kutuphane.ekle_sil_güncelle(komut2, cümle);
            foreach (Control item in Controls) if (item is TextBox) item.Text = "";
            YenileListele(); Temizle();
            MessageBox.Show("Kitap Yüklendi", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Toplam();



        }

        private void lblToplam_Click(object sender, EventArgs e)
        {
            
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}