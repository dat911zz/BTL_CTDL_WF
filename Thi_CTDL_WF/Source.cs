using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thi_CTDL_WF
{
	//========================================================================
	//Struct and constant
	public struct DiaChi
	{
		public string soNha;
		public string phuong;
		public string quan;
		public string tinhThanh;

        public static bool operator !=(DiaChi x, DiaChi y)
        {
            return string.Compare(x.tinhThanh, y.tinhThanh) != 0;
        }
        public static bool operator ==(DiaChi x, DiaChi y)
        {
            return string.Compare(x.tinhThanh, y.tinhThanh) == 0;
        }
        public static bool operator >(DiaChi x, DiaChi y)
        {
            return string.Compare(x.tinhThanh, y.tinhThanh) > 0;
        }
        public static bool operator <(DiaChi x, DiaChi y)
        {
            return string.Compare(x.tinhThanh, y.tinhThanh) < 0;
        }
    };
	public struct KhachHang
	{
		public string tenKH;
		public DiaChi diaChi;
		public string SDT;
	};
    public class HangHoa
    {
		public string maHang;
		public KhachHang goi;
		public KhachHang nhan;
		public float phi, trongLuong;
		public string loai;
        //========================================================================
        //Functions
        public void ReadFile(LinkedList<HangHoa> list)
        {
            string[] line;
            //Đọc giá trị từ file
            try
            {
                line = File.ReadAllLines("../Input.txt");
            }
            catch (IOException e)
            {
                Console.Write(e.Message);
                return;
            }
            try
            {
                
                for (int i = 0; i < line.Length; ++i)
                {
                    HangHoa x = new HangHoa { };
                    string[] data = line[i].Split(' ');
                    ////Xuất ra dữ liệu trong file
                    //foreach (var item in data)
                    //{
                    //    Console.Write($"<{item}>");
                    //}                    
                    //-------------INPUT DATA----------------
                    //Ma Hang
                    x.maHang = data[0];
                    //KH gởi
                    x.goi.tenKH = data[1];
                    x.goi.diaChi.soNha = data[2];
                    x.goi.diaChi.phuong = data[3];
                    x.goi.diaChi.quan = data[4];
                    x.goi.diaChi.tinhThanh = data[5];
                    x.goi.SDT = data[6];
                    //KH Nhận
                    x.nhan.tenKH = data[7];
                    x.nhan.diaChi.soNha = data[8];
                    x.nhan.diaChi.phuong = data[9];
                    x.nhan.diaChi.quan = data[10];
                    x.nhan.diaChi.tinhThanh = data[11];
                    x.nhan.SDT = data[12];
                    //Phi van chuyen
                    float.TryParse(data[13],out x.phi);
                    //Trong luong
                    float.TryParse(data[14], out x.trongLuong);
                    //Loai hang
                    x.loai = data[15];
                    //---------------------------------------
                    //Thêm phần tử vào cuối DSLK
                    list.AddLast(x);
                }

            }
            catch (IOException e)
            {
                Console.Write(e.Message);
                return;
            }
        }
        public void ExportFile(LinkedList<HangHoa> list)
        {
            Console.Write("Xuất file thành công! Địa chỉ file: {0}", new FileInfo("../Output.txt").Directory.FullName);
            string[] dataHangHoa = new string[999999];
            int i = 0;
            foreach (var item in list)
            {
                dataHangHoa[i] += item.maHang + " ";

                dataHangHoa[i] += item.goi.tenKH + " ";
                dataHangHoa[i] += item.goi.diaChi.soNha + " ";
                dataHangHoa[i] += item.goi.diaChi.phuong + " ";
                dataHangHoa[i] += item.goi.diaChi.quan + " ";
                dataHangHoa[i] += item.goi.diaChi.tinhThanh + " ";
                dataHangHoa[i] += item.goi.SDT + " ";

                dataHangHoa[i] += item.nhan.tenKH + " ";
                dataHangHoa[i] += item.nhan.diaChi.soNha + " ";
                dataHangHoa[i] += item.nhan.diaChi.phuong + " ";
                dataHangHoa[i] += item.nhan.diaChi.quan + " ";
                dataHangHoa[i] += item.nhan.diaChi.tinhThanh + " ";
                dataHangHoa[i] += item.nhan.SDT + " ";

                dataHangHoa[i] += item.phi + " ";
                dataHangHoa[i] += item.trongLuong + " ";
                dataHangHoa[i] += item.loai + " ";
                i++;
            }


            using (StreamWriter sw = new StreamWriter("../Output.txt"))
            {

                foreach (string s in dataHangHoa)
                {
                    sw.WriteLine(s);
                }
            }

            //// doc va hien thi du lieu trong textfile.txt
            //string line = "";
            //using (StreamReader sr = new StreamReader("textfile.txt"))
            //{
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        Console.WriteLine(line);
            //    }
            //}
            //Console.ReadKey();
        }
        //=========================
        //Nạp chồng toán tử
        //---------------
        //So sánh mã hàng
        public static bool operator !=(HangHoa x, HangHoa y)
        {
            return string.Compare(x.maHang, y.maHang) != 0;
        }
        public static bool operator ==(HangHoa x, HangHoa y)
        {
            return string.Compare(x.maHang, y.maHang) == 0;
        }
        public static bool operator >(HangHoa x, HangHoa y)
        {
            return string.Compare(x.maHang, y.maHang) > 0;
        }
        public static bool operator <(HangHoa x, HangHoa y)
        {
            return string.Compare(x.maHang, y.maHang) < 0;
        }
        //=========================
    };
    public static class LinkedListExtensions
    {
        public static LinkedList<T> SwapPairwise<T>(this LinkedList<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var current = source.First;

            if (current == null)
                return source;

            while (current.Next != null)
            {
                current.SwapWith(current.Next);
                current = current.Next;

                if (current != null)
                    current = current.Next;
            }

            return source;
        }

        public static void SwapWith<T>(this LinkedListNode<T> first, LinkedListNode<T> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            if (second == null)
                throw new ArgumentNullException("second");

            var tmp = first.Value;
            first.Value = second.Value;
            second.Value = tmp;
        }

        public static LinkedList<T> InterchangeSort<T>(this LinkedList<T> list, Func<T, T, bool> Compare)
        {
            for (LinkedListNode<T> p = list.First; p != null; p = p.Next)
            {
                for (LinkedListNode<T> q = p.Next; q != null; q = q.Next)
                {
                    if (Compare(p.Value, q.Value) == true) 
                    {
                        Swap(p, q);                       
                    }
                }
            }
            return list;
        }       

        public static void Swap<T>(LinkedListNode<T> first, LinkedListNode<T> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            if (second == null)
                throw new ArgumentNullException("second");

            var tmp = first.Value;
            first.Value = second.Value;
            second.Value = tmp;
        }
    }
}
