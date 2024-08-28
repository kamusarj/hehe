using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QLBH.Models;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Diagnostics;
namespace QLBH
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        QlbhContext db = new QlbhContext();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            hienthicb();
            view();
        }
        private void hienthicb()
        {
            var query = from nv in db.NhanViens
                        select nv;
            cboTennv.ItemsSource = query.ToList();
            cboTennv.DisplayMemberPath = "TenNv";
            cboTennv.SelectedValuePath = "MaNv";
        }
        private void view()
        {
            var query = from banghi in db.HoaDons
                        orderby banghi.SoLuong * banghi.DonGia ascending
                        select new
                        {
                            SoHD = banghi.SoHd,
                            Tensp = banghi.TenSp,
                            Soluong = banghi.SoLuong,
                            Dongia = banghi.DonGia,
                            ThanhTien = banghi.SoLuong * banghi.DonGia,
                            banghi.MaNv
                        };
            dghoadon.ItemsSource = query.ToList();
        }
        private bool checkdl()
        {
            string mess = "";
            if (txtTensp.Text == "" || txtSoHD.Text == "" || txtSL.Text == "" || txtDongia.Text == "")
            {
                mess += "\nBan can nhap day du du lieu!";
            }
            if (!Regex.IsMatch(txtSL.Text, @"\d+"))
            {
                mess += "\nSo luong nhap vao phai la so nguyen!";
            }
            else
            {
                int sl = int.Parse(txtSL.Text);
                if (sl < 0)
                {
                    mess += "So luong nhap vao phai la so duong!";
                }
            }
            if (!Regex.IsMatch(txtDongia.Text, @"\d+"))
            {
                mess += "\nDon gia nhap vao phai la so nguyen!";
            }
            else
            {
                int dg = int.Parse(txtDongia.Text);
                if (dg < 0)
                {
                    mess += "Don gia nhap vao phai la so duong!";
                }
            }
            if (mess != "")
            {
                MessageBox.Show(mess, "Thong Bao", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private void dghoadon_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dghoadon.SelectedItem != null)
            {
                try
                {
                    Type t = dghoadon.SelectedItem.GetType();
                    PropertyInfo[] p = t.GetProperties();
                    txtSoHD.Text = p[0].GetValue(dghoadon.SelectedValue).ToString();
                    txtTensp.Text = p[1].GetValue(dghoadon.SelectedValue).ToString();
                    txtSL.Text = p[2].GetValue(dghoadon.SelectedValue).ToString();
                    txtDongia.Text = p[3].GetValue(dghoadon.SelectedValue).ToString();
                    cboTennv.SelectedValue = p[5].GetValue(dghoadon.SelectedValue).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Loi: " + ex.Message, "Thong Bao:", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (checkdl())
                {
                    var query = db.HoaDons.SingleOrDefault(x => x.SoHd.Equals(int.Parse(txtSoHD.Text)));
                    if (query != null)
                    {
                        MessageBox.Show("Da ton tai so hoa don tren! ", "Thong Bao", MessageBoxButton.OK);
                    }
                    else
                    {
                        HoaDon x = new();
                        x.SoHd = int.Parse(txtSoHD.Text);
                        x.TenSp = txtTensp.Text;
                        x.SoLuong = int.Parse(txtSL.Text);
                        x.DonGia = double.Parse(txtDongia.Text);
                        NhanVien a = (NhanVien)cboTennv.SelectedItem;
                        x.MaNv = a.MaNv;
                        db.HoaDons.Add(x);
                        db.SaveChanges();
                        MessageBox.Show("Da them!", "Thong bao", MessageBoxButton.OK);
                        view();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Thong Bao", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var query = db.HoaDons.SingleOrDefault(x => x.SoHd == int.Parse(txtSoHD.Text));
                if (query != null)
                {
                    if (checkdl())
                    {
                        query.TenSp = txtTensp.Text;
                        query.SoLuong = int.Parse(txtSL.Text);
                        query.DonGia = int.Parse(txtDongia.Text);
                        query.MaNv = ((NhanVien)cboTennv.SelectedItem).MaNv;
                        MessageBox.Show("Da sua!", "Thong bao", MessageBoxButton.OK);
                        db.SaveChanges();
                        view();
                    }
                }
                else
                {
                    MessageBox.Show("Khong tim thay san pham can sua!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi sửa!" + ex.Message);
            }
        }
        private void butthoat_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void buttim_Click(object sender, RoutedEventArgs e)
        {
            Window f = new Window1();
            f.ShowDialog();
        }
        /*private void btnxoa_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var query = db.HoaDons.SingleOrDefault(t => t.SoHd.Equals(int.Parse(txthoadon.Text)));
                if (query != null)
                {
                    MessageBoxResult rs = MessageBox.Show("Are you sure?", "Notification", MessageBoxButton.YesNoCancel);
                    if (rs == MessageBoxResult.Yes)
                    {
                        db.HoaDons.Remove(query);
                        db.SaveChanges();
                        view();
                    }
                }
                else
                {
                    MessageBox.Show("Chua ton tai sach can xoa ", "Thong bao");
                    view();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Co loi khi xoa:" + ex.Message, "Thong bao", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }*/
    }
}