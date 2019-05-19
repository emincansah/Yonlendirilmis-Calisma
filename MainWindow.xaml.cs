using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection baglanti = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security = True");
        public MainWindow()
        {
            InitializeComponent();

            SqlCommand komut = new SqlCommand();
            komut.CommandText = "SELECT *FROM Parca";
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;

            SqlDataReader dr;
            baglanti.Open();
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                parcakytcmbbx.Items.Add(dr["ParcaAdi"]);
            }
            baglanti.Close();
        }
        private void girisbtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                baglanti.Open();
                string Sql = "Select * From [Login] where kullaniciAdi=@kullanıcıad   AND sifre=@sifresi";

                SqlParameter PRM1 = new SqlParameter("kullanıcıad", kullanıcıtxtx.Text.Trim());
                SqlParameter PRM2 = new SqlParameter("sifresi", sifretxtbx.Text.Trim());
                SqlCommand komut = new SqlCommand(Sql, baglanti);
                komut.Parameters.Add(PRM1);
                komut.Parameters.Add(PRM2);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(komut);
                da.Fill(dt);
                if (servisrdbtn.IsChecked == true && dt.Rows.Count > 0)
                {
                    KayıtGrd.Visibility = Visibility.Visible;
                    yedekparcaGrd.Visibility = Visibility.Collapsed;
                    loginGrd.Visibility = Visibility.Collapsed;

                }


                else if (yedekRdbtn.IsChecked == true && dt.Rows.Count > 0)
                {

                    KayıtGrd.Visibility = Visibility.Collapsed;
                    yedekparcaGrd.Visibility = Visibility.Visible;
                    loginGrd.Visibility = Visibility.Collapsed;

                    baglanti.Open();
                    komut.CommandText = "SELECT *FROM Arac";
                    komut.Connection = baglanti;
                    komut.CommandType = CommandType.Text;

                    SqlDataReader dr;
                    baglanti.Open();
                    dr = komut.ExecuteReader();
                    while (dr.Read())
                    {
                        dtgrdaracplaka.Items.Add(dr["Plaka"]);
                    }

                    string sorguCümle = "SELECT * FROM [Arac]";

                    komut = new SqlCommand(sorguCümle, baglanti);
                    SqlDataReader drVeriOku = komut.ExecuteReader();
                    DataTable dtDoldur = new DataTable();
                    dtDoldur.Load(drVeriOku);
                    dg1.ItemsSource = dtDoldur.DefaultView;
                    baglanti.Close();


                }
                else
                {
                    MessageBox.Show("Hatalı Giriş");
                }
                baglanti.Close();

            }
            catch (Exception)
            {


                MessageBox.Show("Hatalı Kod");
            }


        }

        private void yedekgeriBtn_Click(object sender, RoutedEventArgs e)
        {
            yedekparcaGrd.Visibility = Visibility.Collapsed;
            KayıtGrd.Visibility = Visibility.Collapsed;
            loginGrd.Visibility = Visibility.Visible;
            sifretxtbx.Text = "";
            kullanıcıtxtx.Text = "";
            baglanti.Close();
        }

        private void servisGeriBtn_Click(object sender, RoutedEventArgs e)
        {
            yedekparcaGrd.Visibility = Visibility.Collapsed;
            KayıtGrd.Visibility = Visibility.Collapsed;
            loginGrd.Visibility = Visibility.Visible;
            sifretxtbx.Text = "";
            kullanıcıtxtx.Text = "";
            baglanti.Close();
        }



        private void KayıtBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                string sorguCümle = "INSERT INTO [Arac](Plaka,SasiNo,Km,ParcaAdi,Tarih) VALUES(@Plaka,@SasiNo,@Km,@ParcaAdi,@Tarih)";
                SqlCommand komut = new SqlCommand(sorguCümle, baglanti);
                komut.Parameters.AddWithValue("@Plaka", plkkaytxt.Text);
                komut.Parameters.AddWithValue("@SasiNo", sasekytxt.Text);
                komut.Parameters.AddWithValue("@Km", kmkytxt.Text);
                komut.Parameters.AddWithValue("@ParcaAdi", parcakytcmbbx.Text);
                komut.Parameters.AddWithValue("@Tarih", trhytdatepicker.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();

                MessageBox.Show("Müşteri Kayıt İşlemi Gerçekleşti.");
            }
            catch (Exception)
            {
                MessageBox.Show("İşlem Sırasında Hata Oluştu.");
            }
            finally
            {


                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            yntloginGrd.Visibility = Visibility.Collapsed;
            kullanıcısilGrd.Visibility = Visibility.Collapsed;
            kullanıcıkytGrd.Visibility = Visibility.Collapsed;
            KayıtGrd.Visibility = Visibility.Collapsed;
            yedekparcaGrd.Visibility = Visibility.Collapsed;
            loginGrd.Visibility = Visibility.Visible;
            baglanti.Close();
            yntsifretxtbx.Text = "";
            yntkullanıcıtxtx.Text = "";

        }

        private void yntgirisbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                baglanti.Open();
                string Sql1 = "Select * From [Yonetici] where yoneticiadı=@kullanıcıadı   AND yoneticisifre=@sifresini";

                SqlParameter PRM3 = new SqlParameter("kullanıcıadı", yntkullanıcıtxtx.Text.Trim());
                SqlParameter PRM4 = new SqlParameter("sifresini", yntsifretxtbx.Text.Trim());
                SqlCommand komut = new SqlCommand(Sql1, baglanti);
                komut.Parameters.Add(PRM3);
                komut.Parameters.Add(PRM4);
                DataTable dq = new DataTable();
                SqlDataAdapter db = new SqlDataAdapter(komut);
                db.Fill(dq);
                baglanti.Close();
                if (yntservisrdbtn.IsChecked == true && dq.Rows.Count > 0)
                {
                    yntloginGrd.Visibility = Visibility.Collapsed;
                    kullanıcısilGrd.Visibility = Visibility.Collapsed;
                    kullanıcıkytGrd.Visibility = Visibility.Visible;
                    KayıtGrd.Visibility = Visibility.Collapsed;
                    yedekparcaGrd.Visibility = Visibility.Collapsed;
                    loginGrd.Visibility = Visibility.Collapsed;

                }

                else if (yntyedekRdbtn.IsChecked == true && dq.Rows.Count > 0)
                {
                    yntloginGrd.Visibility = Visibility.Collapsed;
                    kullanıcıkytGrd.Visibility = Visibility.Collapsed;
                    kullanıcısilGrd.Visibility = Visibility.Visible;
                    KayıtGrd.Visibility = Visibility.Collapsed;
                    yedekparcaGrd.Visibility = Visibility.Collapsed;
                    loginGrd.Visibility = Visibility.Collapsed;


                    baglanti.Open();

                    string sorguCümle = "SELECT * FROM [Login]";

                    komut = new SqlCommand(sorguCümle, baglanti);
                    SqlDataReader drVeriOku = komut.ExecuteReader();
                    DataTable dtDoldur = new DataTable();
                    dtDoldur.Load(drVeriOku);
                    dg1.ItemsSource = dtDoldur.DefaultView;
                    baglanti.Close();

                }
                else
                {
                    MessageBox.Show("Hatalı Giriş");
                }
                baglanti.Close();


            }
            catch (Exception)
            {


                MessageBox.Show("Hatalı Kod");
            }


        }

        private void yonetisayfabtn_Click(object sender, RoutedEventArgs e)
        {

            yntloginGrd.Visibility = Visibility.Visible;
            kullanıcıkytGrd.Visibility = Visibility.Collapsed;
            kullanıcısilGrd.Visibility = Visibility.Collapsed;
            KayıtGrd.Visibility = Visibility.Collapsed;
            yedekparcaGrd.Visibility = Visibility.Collapsed;
            loginGrd.Visibility = Visibility.Collapsed;
        }

        private void kullanıcıkayıtbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                string sorguCümle = "INSERT INTO [Login](kullaniciAdi,sifre) VALUES(@Kullaniciadii,@sifree)";
                SqlCommand komut = new SqlCommand(sorguCümle, baglanti);
                komut.Parameters.AddWithValue("@kullaniciadii", kullanıcıkyttxtx.Text);
                komut.Parameters.AddWithValue("@sifree", sifrekyttxtbx.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();

                MessageBox.Show("Müşteri Kayıt İşlemi Gerçekleşti.");
            }
            catch (Exception)
            {
                MessageBox.Show("İşlem Sırasında Hata Oluştu.");
            }
            finally
            {


                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();

            }

        }

        private void listviev_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            baglanti.Open();
            string secmeSorgusu = "SELECT * from [Login] where Id=@musterino";

            SqlCommand secmeKomutu = new SqlCommand(secmeSorgusu, baglanti);
            secmeKomutu.Parameters.AddWithValue("@musterino", txtbxklncsil.Text);

            SqlDataAdapter da = new SqlDataAdapter(secmeKomutu);
            SqlDataReader dr = secmeKomutu.ExecuteReader();

            if (dr.Read())
            {
                string isim = dr["kullaniciAdi"].ToString() + " " + dr["sifre"].ToString();
                dr.Close();


                string silmeSorgusu = "DELETE from [Login] where Id=@musterino";
                //musterino parametresine bağlı olarak müşteri kaydını silen sql sorgusu
                SqlCommand silKomutu = new SqlCommand(silmeSorgusu, baglanti);
                silKomutu.Parameters.AddWithValue("@musterino", txtbxklncsil.Text);
                silKomutu.ExecuteNonQuery();
                MessageBox.Show("Kayıt Silindi...");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
         //   foreach (DataGridRow slctrow in dtgrdaracliste.Rows)  //Seçili Satırları Silme
        ///{
        //int numara = Convert.ToInt32(slctrow);
          //  }
        }
    }
}
