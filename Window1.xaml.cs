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
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using QLBH.Models;

namespace QLBH
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        
        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            QlbhContext db = new QlbhContext();
            var query = from index in db.HoaDons
                        join k in db.NhanViens
                        on index.MaNv equals k.MaNv
                        where index.MaNv == 1
                        select new
                        {
                            SoHD = index.SoHd,
                            Tensp = index.TenSp,
                            Soluong = index.SoLuong,
                            Dongia = index.DonGia,
                            ThanhTien = index.SoLuong * index.DonGia
                        };
            dginfo.ItemsSource = query.ToList();
        }
    }
}
