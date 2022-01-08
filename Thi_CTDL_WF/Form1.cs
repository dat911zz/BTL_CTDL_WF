using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thi_CTDL_WF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Load Form
        private void Form1_Load(object sender, EventArgs e)
        {
            TextBoxWriter writer = new TextBoxWriter(txtConsole);
            addPanel.Visible = false;
            deletePanel.Visible = false;
            Console.SetOut(writer);
        }
        //Khởi động lại Form
        public void restartForm()
        {
            dataGridView1.Refresh();
            addPanel.Visible = false;
            deletePanel.Visible = false;
            delPosPanel.Visible = false;
            addPosPanel.Visible = false;
            addBtn_2.Visible = false;
            txtConsole.Clear();
            Update();
        }
        //Hiện thị DSLK vào Console
        public void ShowLinkedList(LinkedList<HangHoa> list)
        {
            //Console.WriteLine();//<=== ERROR HERE
            txtConsole.AppendText(Environment.NewLine);//<=== FIX ERROR: STACK OVERFLOW OF Console.WriteLine();  
            Console.Write("Hàng hóa trong DSLK hiện có: ");
            foreach (var item in list)
            {
                Console.Write("[" + item.maHang + "] -> ");
            }
            Console.Write("[NULL]");
        }
        //====================================================//
        /*                Khai Báo Biến + DSLK                */
        HangHoa x = new HangHoa { };
        LinkedList<HangHoa> list1 = new LinkedList<HangHoa>();
        //====================================================//
        //Nút chạy
        private void RUN_Click(object sender, EventArgs e)
        {
            reloadConsole();
        }
        //Làm cho hiệu ứng scroll trong DGV mượt mà hơn :V
        void DataGridView1_MouseWheel(object sender, MouseEventArgs e)
        {
            int currentIndex = this.dataGridView1.FirstDisplayedScrollingRowIndex;
            int scrollLines = SystemInformation.MouseWheelScrollLines;

            if (e.Delta > 0)
            {
                this.dataGridView1.FirstDisplayedScrollingRowIndex
                    = Math.Max(0, currentIndex - scrollLines);
            }
            else if (e.Delta < 0)
            {
                this.dataGridView1.FirstDisplayedScrollingRowIndex
                    = currentIndex + scrollLines;
            }
        }
        //Update số thứ tự
        private void updateSTT()
        {
            clearNullRows();
            dataGridView1.Refresh();
            if (dataGridView1.Rows.Count >= 1)
            {
                for (int row = 0; row < dataGridView1.Rows.Count; row++)
                {
                    dataGridView1.Rows[row].Cells[0].Value = row + 1;
                }
            }           
            //reloadConsole();
            dataGridView1.Refresh();
        }

        //Điều khiển cập nhật Console
        private void reloadConsole()
        {
            //restartForm();
            txtConsole.Clear();
            ShowLinkedList(list1);
            txtConsole.Refresh();
            //Update();
        }
        //Hiện thị Data lên DataGridView
        private void ShowData(LinkedList<HangHoa> list)
        {
            int row = 0;            
            if (dataGridView1.Rows.Count >= 3)
            {
                return;
            }
            foreach (var item in list)
            {              
                dataGridView1.Rows.Add();
                for (int col = 0; col < dataGridView1.Rows[row].Cells.Count; col++)
                {
                    switch (col)
                    {
                        case 0:
                            dataGridView1.Rows[row].Cells[col].Value = row + 1;
                            break;
                        case 1:
                            dataGridView1.Rows[row].Cells[col].Value = item.maHang;
                            break;
                        case 2:
                            dataGridView1.Rows[row].Cells[col].Value = item.goi.tenKH;
                            break;
                        case 3:
                            dataGridView1.Rows[row].Cells[col].Value = " " + item.goi.diaChi.soNha + ", " + item.goi.diaChi.phuong + ", " + item.goi.diaChi.quan + ", " + item.goi.diaChi.tinhThanh;
                            break;
                        case 4:
                            dataGridView1.Rows[row].Cells[col].Value = item.goi.SDT;
                            break;
                        case 5:
                            dataGridView1.Rows[row].Cells[col].Value = item.nhan.tenKH;
                            break;
                        case 6:
                            dataGridView1.Rows[row].Cells[col].Value = item.nhan.diaChi.soNha + ", " + item.nhan.diaChi.phuong + ", " + item.nhan.diaChi.quan + ", " + item.nhan.diaChi.tinhThanh;
                            break;
                        case 7:
                            dataGridView1.Rows[row].Cells[col].Value = item.nhan.SDT;
                            break;
                        case 8:
                            dataGridView1.Rows[row].Cells[col].Value = item.phi;
                            break;
                        case 9:
                            dataGridView1.Rows[row].Cells[col].Value = item.trongLuong;
                            break;
                        case 10:
                            dataGridView1.Rows[row].Cells[col].Value = item.loai;
                            break;
                    }
                }               
                row++;
            }
        }
        //Tải lên dữ liệu từ file
        private void Upload_Click(object sender, EventArgs e)
        {
            restartForm();
            dataGridView1.AllowUserToAddRows = false;//Fix Uncommited
            if (list1.Count > 0)
            {
                return;
            }
            x.ReadFile(list1);
            //Upload data into GridView
            ShowData(list1);
            reloadConsole();
        }
        //===========================================
        //Điều khiển chức năng 
        private void addBtn_Click(object sender, EventArgs e)
        {
            //reloadConsole();
            restartForm();
            addPanel.Show();
        }
        //Add hàng hóa vào bảng + thêm vào DSLK
        private void addElementBtn_Click(object sender, EventArgs e)
        {

            HangHoa x = new HangHoa { };
            int row = list1.Count();
            addElement(row);
            
        }
        private void addElement(int row)
        {
            dataGridView1.AllowUserToAddRows = false;//Fix Uncommited
            HangHoa x = new HangHoa { };
            string[] tmp;
            if (dataGridView2.Rows[0].Cells[1].Value == null)//Xử lý lỗi: Người dùng chưa nhập vào bảng nhập
            {
                MessageBox.Show("Vui lòng nhập dữ liệu vào", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
            }
            else
            {
                //Add data vừa mới nhập vào DSLK
                dataGridView1.Rows.Add();
                for (int col = 0; col <= dataGridView2.Rows[0].Cells.Count; col++)
                {
                    switch (col)
                    {
                        case 0://STT
                            dataGridView1.Rows[row].Cells[col].Value = row + 1;
                            break;
                        case 1://Mã Hàng
                            dataGridView1.Rows[row].Cells[col].Value = dataGridView2.Rows[0].Cells[col - 1].Value;
                            x.maHang = dataGridView1.Rows[row].Cells[col].Value.ToString();
                            break;
                        case 2://Tên KH gởi
                            dataGridView1.Rows[row].Cells[col].Value = dataGridView2.Rows[0].Cells[col - 1].Value;
                            x.goi.tenKH = dataGridView1.Rows[row].Cells[col].Value.ToString();
                            break;
                        case 3://Địa chỉ
                            tmp = dataGridView2.Rows[0].Cells[col - 1].Value.ToString().Split(' ');
                            if (tmp.Length != 4)
                            {
                                MessageBox.Show("Bạn đã nhập thiếu, vui lòng nhập lại đầy đủ theo định dạng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
                                return;
                            }
                            dataGridView1.Rows[row].Cells[col].Value += x.goi.diaChi.soNha = tmp[0].Trim();
                            dataGridView1.Rows[row].Cells[col].Value += ", ";
                            dataGridView1.Rows[row].Cells[col].Value += x.goi.diaChi.phuong = tmp[1].Trim();
                            dataGridView1.Rows[row].Cells[col].Value += ", ";
                            dataGridView1.Rows[row].Cells[col].Value += x.goi.diaChi.quan = tmp[2].Trim();
                            dataGridView1.Rows[row].Cells[col].Value += ", ";
                            dataGridView1.Rows[row].Cells[col].Value += x.goi.diaChi.tinhThanh = tmp[3].Trim();
                            break;
                        case 4://SĐT
                            dataGridView1.Rows[row].Cells[col].Value = dataGridView2.Rows[0].Cells[col - 1].Value;
                            x.goi.SDT = dataGridView1.Rows[row].Cells[col].Value.ToString();
                            break;
                        case 5://Tên KH nhận
                            dataGridView1.Rows[row].Cells[col].Value = dataGridView2.Rows[0].Cells[col - 1].Value;
                            x.nhan.tenKH = dataGridView1.Rows[row].Cells[col].Value.ToString();
                            break;
                        case 6://Địa chỉ
                            tmp = dataGridView2.Rows[0].Cells[col - 1].Value.ToString().Split(' ');
                            if (tmp.Length != 4)
                            {
                                MessageBox.Show("Bạn đã nhập thiếu, vui lòng nhập lại đầy đủ theo định dạng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
                                return;
                            }
                            
                            dataGridView1.Rows[row].Cells[col].Value += x.nhan.diaChi.soNha = tmp[0].Trim();
                            dataGridView1.Rows[row].Cells[col].Value += ", ";
                            dataGridView1.Rows[row].Cells[col].Value += x.nhan.diaChi.phuong = tmp[1].Trim();
                            dataGridView1.Rows[row].Cells[col].Value += ", ";
                            dataGridView1.Rows[row].Cells[col].Value += x.nhan.diaChi.quan = tmp[2].Trim();
                            dataGridView1.Rows[row].Cells[col].Value += ", ";
                            dataGridView1.Rows[row].Cells[col].Value += x.nhan.diaChi.tinhThanh = tmp[3].Trim();
                            break;
                        case 7://SĐT
                            dataGridView1.Rows[row].Cells[col].Value = dataGridView2.Rows[0].Cells[col - 1].Value;
                            x.nhan.SDT = dataGridView1.Rows[row].Cells[col].Value.ToString();
                            break;
                        case 8://Phí
                            dataGridView1.Rows[row].Cells[col].Value = dataGridView2.Rows[0].Cells[col - 1].Value;
                            float.TryParse(dataGridView1.Rows[row].Cells[col].Value.ToString(), out x.phi);
                            break;
                        case 9://Trọng lượng
                            dataGridView1.Rows[row].Cells[col].Value = dataGridView2.Rows[0].Cells[col - 1].Value;
                            float.TryParse(dataGridView1.Rows[row].Cells[col].Value.ToString(), out x.trongLuong);
                            break;
                        case 10://Loại hàng
                            dataGridView1.Rows[row].Cells[col].Value = dataGridView2.Rows[0].Cells[col - 1].Value;
                            x.loai = dataGridView1.Rows[row].Cells[col].Value.ToString();
                            break;
                    }
                }
                //Thêm 1 hàng vào cuối bảng
                list1.AddLast(x);
                //Load lại UI
                reloadConsole();
            }
        }
        private void addBeforePosFunc_Click(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;//Fix Uncommited
            HangHoa x = new HangHoa { };
            int k = 0;
            if (add_Input.Text == "")
            {
                errorProvider1.SetError(add_Input, "Vui lòng nhập giá trị!");
            }
            else
            {
                int.TryParse(add_Input.Text, out k);
                if (k <= 0 || k > dataGridView1.Rows.Count)
                {
                    errorProvider1.SetError(add_Input, "Vui lòng nhập giá trị trong khoảng quy định [" + 1 + "," + dataGridView1.Rows.Count + "]");
                }
                else
                {                  
                    dataGridView1.Rows.Insert(k - 1);
                    getDataFromInputElement(ref x, k - 1);
                    //---------------------------------------
                    if (list1.Count == 0)
                    {
                        list1.AddLast(x);
                    }
                    else
                    {
                        list1.AddBefore(FindPosK(k - 1), x);//Thêm 1 Node trước vị trí được chỉ định
                    }                  
                    updateSTT();                   
                }
            }
            txtConsole.Clear();
            ShowLinkedList(list1);
        }

        private void addAfterFunc_Click(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;//Fix Uncommited
            HangHoa x = new HangHoa { };
            int k = 0;
            if (add_Input.Text == "")
            {
                errorProvider1.SetError(add_Input, "Vui lòng nhập giá trị");
            }
            else
            {
                int.TryParse(add_Input.Text, out k);
                if (k <= 0 || k >= dataGridView1.Rows.Count)
                {
                    errorProvider1.SetError(add_Input, "Vui lòng nhập giá trị trong khoảng quy định [" + 1 + "," + dataGridView1.Rows.Count + "]");
                }
                else
                {                   
                    dataGridView1.Rows.Insert(k);
                    getDataFromInputElement(ref x, k);
                    //---------------------------------------
                    if (list1.Count == 0)
                    {
                        list1.AddLast(x);
                    }
                    else
                    {
                        list1.AddAfter(FindPosK(k - 1), x);//Thêm 1 Node trước vị trí được chỉ định
                    }
                    updateSTT();
                }
            }
            txtConsole.Clear();
            ShowLinkedList(list1);
        }
        private void addPosFunc_Click(object sender, EventArgs e)
        {
            addPosPanel.Show();
        }

        //Load lại UI
        private void refreshBtn_Click(object sender, EventArgs e)
        {
            ShowData(list1);
            restartForm();
        }
        private void getDataFromInputElement(ref HangHoa x, int row)
        {
            //int row = list1.Count();
            string[] tmp;
            if (dataGridView2.Rows[0].Cells[1].Value == null)//Xử lý lỗi: Người dùng chưa nhập vào bảng nhập
            {
                MessageBox.Show("Vui lòng nhập dữ liệu vào", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
            }
            else
            {
                //Add data vừa mới nhập vào DSLK
                dataGridView1.Rows.Add();
                for (int col = 0; col <= dataGridView2.Rows[0].Cells.Count; col++)
                {
                    switch (col)
                    {
                        case 0://STT
                            dataGridView1.Rows[row].Cells[col].Value = row + 1;
                            break;
                        case 1://Mã Hàng
                            dataGridView1.Rows[row].Cells[col].Value = dataGridView2.Rows[0].Cells[col - 1].Value;
                            x.maHang = dataGridView1.Rows[row].Cells[col].Value.ToString();
                            break;
                        case 2://Tên KH gởi
                            dataGridView1.Rows[row].Cells[col].Value = dataGridView2.Rows[0].Cells[col - 1].Value;
                            x.goi.tenKH = dataGridView1.Rows[row].Cells[col].Value.ToString();
                            break;
                        case 3://Địa chỉ
                            tmp = dataGridView2.Rows[0].Cells[col - 1].Value.ToString().Split(' ');
                            if (tmp.Length != 4)
                            {
                                MessageBox.Show("Bạn đã nhập thiếu, vui lòng nhập lại đầy đủ theo định dạng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
                                return;
                            }
                            dataGridView1.Rows[row].Cells[col].Value += x.goi.diaChi.soNha = tmp[0];
                            dataGridView1.Rows[row].Cells[col].Value += ", ";
                            dataGridView1.Rows[row].Cells[col].Value += x.goi.diaChi.phuong = tmp[1];
                            dataGridView1.Rows[row].Cells[col].Value += ", ";
                            dataGridView1.Rows[row].Cells[col].Value += x.goi.diaChi.quan = tmp[2];
                            dataGridView1.Rows[row].Cells[col].Value += ", ";
                            dataGridView1.Rows[row].Cells[col].Value += x.goi.diaChi.tinhThanh = tmp[3];
                            break;
                        case 4://SĐT
                            dataGridView1.Rows[row].Cells[col].Value = dataGridView2.Rows[0].Cells[col - 1].Value;
                            x.goi.SDT = dataGridView1.Rows[row].Cells[col].Value.ToString();
                            break;
                        case 5://Tên KH nhận
                            dataGridView1.Rows[row].Cells[col].Value = dataGridView2.Rows[0].Cells[col - 1].Value;
                            x.nhan.tenKH = dataGridView1.Rows[row].Cells[col].Value.ToString();
                            break;
                        case 6://Địa chỉ
                            tmp = dataGridView2.Rows[0].Cells[col - 1].Value.ToString().Split(' ');
                            if (tmp.Length != 4)
                            {
                                MessageBox.Show("Bạn đã nhập thiếu, vui lòng nhập lại đầy đủ theo định dạng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
                                return;
                            }
                            dataGridView1.Rows[row].Cells[col].Value += x.nhan.diaChi.soNha = tmp[0];
                            dataGridView1.Rows[row].Cells[col].Value += ", ";
                            dataGridView1.Rows[row].Cells[col].Value += x.nhan.diaChi.phuong = tmp[1];
                            dataGridView1.Rows[row].Cells[col].Value += ", ";
                            dataGridView1.Rows[row].Cells[col].Value += x.nhan.diaChi.quan = tmp[2];
                            dataGridView1.Rows[row].Cells[col].Value += ", ";
                            dataGridView1.Rows[row].Cells[col].Value += x.nhan.diaChi.tinhThanh = tmp[3];
                            break;
                        case 7://SĐT
                            dataGridView1.Rows[row].Cells[col].Value = dataGridView2.Rows[0].Cells[col - 1].Value;
                            x.nhan.SDT = dataGridView1.Rows[row].Cells[col].Value.ToString();
                            break;
                        case 8://Phí
                            dataGridView1.Rows[row].Cells[col].Value = dataGridView2.Rows[0].Cells[col - 1].Value;
                            float.TryParse(dataGridView1.Rows[row].Cells[col].Value.ToString(), out x.phi);
                            break;
                        case 9://Trọng lượng
                            dataGridView1.Rows[row].Cells[col].Value = dataGridView2.Rows[0].Cells[col - 1].Value;
                            float.TryParse(dataGridView1.Rows[row].Cells[col].Value.ToString(), out x.trongLuong);
                            break;
                        case 10://Loại hàng
                            dataGridView1.Rows[row].Cells[col].Value = dataGridView2.Rows[0].Cells[col - 1].Value;
                            x.loai = dataGridView1.Rows[row].Cells[col].Value.ToString();
                            break;
                    }
                }
            }
        }
        //===========================================
        //Xóa phần tử cuối + xóa pt cuối khỏi DSLK
        private void removeBtn_Click(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;//Fix Uncommited
            if (dataGridView1.Rows.Count != 2 && dataGridView1.Rows.Count > 1)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                list1.RemoveLast();
                txtConsole.Refresh();
                reloadConsole();
            }
        }
        //Xóa toàn bộ bảng + DSLK
        private void removeAll_Click(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;//Fix Uncommited
            while (dataGridView1.Rows.Count != 0)
            {                
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            }
            dataGridView1.Refresh();
            dataGridView1.AllowUserToAddRows = true;
            list1.Clear();
            reloadConsole();
        }
        
        
        private void deleteFunctionBtn_Click(object sender, EventArgs e)
        {
            //reloadConsole();
            restartForm();
            deletePanel.Show();
        }


        private void delPos_Click(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            if (list1.Count != 0 && dataGridView1.Rows.Count != 0)
            {
                int k = 0;
                int.TryParse(InputPos.Text, out k);
                if (k > dataGridView1.Rows.Count || k <= 0)
                {
                    MessageBox.Show("Vị trí đã nhập ngoài khoảng cho phép!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
                }
                else
                {
                    dataGridView1.Rows.RemoveAt(k - 1);
                    var value = list1.ElementAt(k - 1);
                    
                    list1.Remove(value);
                    updateSTT();
                }
            }
            
        }
        private void removeHeadList_Click(object sender, EventArgs e)
        {
            if (list1.Count != 0 && dataGridView1.Rows[0].Cells[0].Value != null)
            {
                dataGridView1.Rows.RemoveAt(0);
                list1.RemoveFirst();
                reloadConsole();
                updateSTT();
            }          
        }

        private void delFuncPos_Click(object sender, EventArgs e)
        {
            InputPos.Enabled = true;
            delPosPanel.Show();
        }
        private void clearNullRows()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[1].Value == null)
                {
                    dataGridView1.Rows.RemoveAt(row.Index);
                }
            }
        }
        //===========================================
        private void showDSLKBtn_Click(object sender, EventArgs e)
        {
            ShowLinkedList(list1);
        }
        private void RunBtn_Click(object sender, EventArgs e)
        {
            txtConsole.Clear();
            clearNullRows();
            CacChucNangConLai();
        }
        private void headerFunc()
        {
            Console.Write("Chức năng: " + comboBox1.Text.ToString());
            txtConsole.AppendText(Environment.NewLine);
        }
        private void CacChucNangConLai()
        {
            int chon = comboBox1.SelectedIndex;
            txtConsole.AppendText(Environment.NewLine);
            switch (chon)
            {
                case 0:
                    SortData_TinhThanh_Loai();
                    headerFunc();
                    ShowLinkedList(list1);
                    txtConsole.AppendText(Environment.NewLine);
                    //XuatTinhThanh_Nhan(list1);
                    
                    break;
                case 1:
                    //addElement(dataGridView1.RowCount - 1);
                    addBtn_2.Show();
                    SortData_TinhThanh_Loai();
                    headerFunc();
                    txtConsole.AppendText(Environment.NewLine);
                    ShowLinkedList(list1);
                    break;
                case 2:
                    headerFunc();
                    demSoLuongHangCanGoiCuaTungTinhThanh();
                    break;
                case 3:
                    headerFunc();
                    timKHThanThiet();
                    break;
                case 4:
                    headerFunc();
                    thongTinLeAnh();
                    break;
                case 5:
                    headerFunc();
                    loaiHangGoiNhieuNhat();
                    break;
                case 6:
                    headerFunc();
                    timGoiHangCoPhiCaoNhat();
                    break;
            }
        }
        LinkedListNode<HangHoa> FindPosK(int k)
        {
            int count = 0;
            LinkedListNode<HangHoa> p = list1.First;
            while (p != null)
            {
                if (count == k)
                {
                    break;
                }
                count++;
                p = p.Next;
            }
            return p;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtConsole.Clear();
            //comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            txtConsole.AppendText(Environment.NewLine);
            if (comboBox1.SelectedIndex == 1)
            {
                txtConsole.Text = "Vui lòng nhập dữ liệu trước!";
                addPanel.Show();
                addBtn_2.Show();
            }
            else
            {
                addPanel.Visible = false;
                addBtn_2.Visible = false;
            }
        }
        
        private bool Ascending_HangHoa(HangHoa x, HangHoa y)
        {
            return x > y;
        }
        private bool Descending_HangHoa(HangHoa x, HangHoa y)
        {
            return x < y;
        }
        private bool Ascending_TinhThanh(HangHoa x, HangHoa y)
        {
            return string.Compare(x.nhan.diaChi.tinhThanh, y.nhan.diaChi.tinhThanh) > 0;
        }       
        private void SapXepTangDan_TinhThanh_Nhan()
        {
            list1.InterchangeSort(Ascending_TinhThanh);
            XuatTinhThanh_Nhan(list1);
        }
        private void XuatTinhThanh_Nhan(LinkedList<HangHoa> list)
        {
            txtConsole.AppendText(Environment.NewLine);
            Console.Write("DS Tỉnh thành hiện có (Dựa theo DSLK): ");
            foreach (var item in list)
            {
                Console.Write("[" + item.maHang + " | " + item.nhan.diaChi.tinhThanh + " | " + item.loai + "]-> ");
            }
            Console.Write("[NULL]");
        }
        private void XuatLoai(LinkedList<HangHoa> list)
        {
            txtConsole.AppendText(Environment.NewLine);
            Console.Write("DS Hang Hoa hien co: ");
            foreach (var item in list)
            {
                Console.Write("[" + item.loai + "] -> ");
            }
            Console.Write("[NULL]");
        }
        private void SortData_TinhThanh()
        {
            list1.InterchangeSort(Ascending_TinhThanh);
            for (int i = 0; i <= dataGridView1.Rows.Count; i++)
            {
                for (int j = i + 1; j <= dataGridView1.Rows.Count - 1; j++)
                {
                    string a = getTinhThanh_Nhan(i);
                    string b = getTinhThanh_Nhan(j);
                    if (string.Compare(a, b) > 0) 
                    {
                        var x = dataGridView1.Rows[i];
                        var y = dataGridView1.Rows[j];

                        dataGridView1.Rows.Remove(x);
                        dataGridView1.Rows.Remove(y);

                        dataGridView1.Rows.Insert(i, y);
                        dataGridView1.Rows.Insert(j, x);
                    }                   
                }
            }
        }
        private string getTinhThanh_Nhan(int pos)
        {
            string[] tmp = dataGridView1.Rows[pos].Cells[6].Value.ToString().Split(' ');
            return tmp[3];
        }
        private string getTinhThanh_Goi(int pos)
        {
            string[] tmp = dataGridView1.Rows[pos].Cells[3].Value.ToString().Split(' ');
            return tmp[3];
        }
        private bool dieuKienSort_TinhThanh_Loai(HangHoa x, HangHoa y)
        {
            return string.Compare(x.nhan.diaChi.tinhThanh, y.nhan.diaChi.tinhThanh) == 0 && string.Compare(x.loai, y.loai) > 0;
        }
        private void SortSList_TinhThanh_Loai()
        {
            //list1.InterchangeSort(Ascending_TinhThanh);
            //SapXepTangDan_TinhThanh_Nhan();
            //ShowLinkedList(list1);
            list1.InterchangeSort(dieuKienSort_TinhThanh_Loai);
            //ShowLinkedList(list1);
        }
        private void SortData_TinhThanh_Loai()
        {
            SortData_TinhThanh();
            SortSList_TinhThanh_Loai();
            for (int i = 0; i <= dataGridView1.Rows.Count; i++)
            {
                for (int j = i + 1; j <= dataGridView1.Rows.Count - 1; j++)
                {
                    string a = getTinhThanh_Nhan(i);
                    string b = getTinhThanh_Nhan(j);
                    var loai1 = dataGridView1.Rows[i].Cells[10].Value.ToString();
                    var loai2 = dataGridView1.Rows[j].Cells[10].Value.ToString();
                    if (string.Compare(a, b) == 0 && string.Compare(loai1, loai2) > 0) 
                    {
                        var x = dataGridView1.Rows[i];
                        var y = dataGridView1.Rows[j];

                        dataGridView1.Rows.Remove(x);
                        dataGridView1.Rows.Remove(y);

                        dataGridView1.Rows.Insert(i, y);
                        dataGridView1.Rows.Insert(j, x);
                    }
                }
            }
            updateSTT();
        }
        //Câu 5
        private void addBtn_2_Click(object sender, EventArgs e)
        {
            int row = list1.Count();
            addElement(row);
            SortData_TinhThanh_Loai();            
            reloadConsole();
        }
        //Câu 6
        private bool Ascending_TinhThanh_Goi(HangHoa x, HangHoa y)
        {
            return string.Compare(x.goi.diaChi.tinhThanh, y.goi.diaChi.tinhThanh) > 0;
        }
        private LinkedList<HangHoa> SortData_TinhThanh_Goi(LinkedList<HangHoa> list1)
        {
            LinkedList<HangHoa> list2 = list1;
            list2.InterchangeSort(Ascending_TinhThanh_Goi);
            //for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
            //{
            //    for (int j = i + 1; j <= dataGridView1.Rows.Count - 2; j++)
            //    {
            //        string a = getTinhThanh_Goi(i);
            //        string b = getTinhThanh_Goi(j);
            //        if (string.Compare(a, b) > 0)
            //        {
            //            var x = dataGridView1.Rows[i];
            //            var y = dataGridView1.Rows[j];

            //            dataGridView1.Rows.Remove(x);
            //            dataGridView1.Rows.Remove(y);

            //            dataGridView1.Rows.Insert(i, y);
            //            dataGridView1.Rows.Insert(j, x);
            //        }
            //    }
            //}
            return list2;
        }
        //Đếm số lượng gói hàng cần gởi theo từng tỉnh thành
        private void demSoLuongHangCanGoiCuaTungTinhThanh()
        {
            int i = 0;
            string[] tinhthanh = new string[100];
            int[] tong = new int[100];
            LinkedList<HangHoa> list2;
            if (list1.Count != 0)
            {
                list2 = SortData_TinhThanh_Goi(list1);
                //Trích tỉnh thành
                tinhthanh[0] = list1.First.Value.goi.diaChi.tinhThanh;
                tong[0]++;
                for (LinkedListNode<HangHoa> p = list1.First.Next; p != null; p = p.Next)
                {
                    if (string.Compare(tinhthanh[i], p.Value.goi.diaChi.tinhThanh) == 0)
                    {
                        tong[i]++;
                    }
                    else
                    {
                        i++;
                        tinhthanh[i] = p.Value.goi.diaChi.tinhThanh;
                        tong[i]++;
                    }
                }
                //Update số thứ tự 
                updateSTT();
                //Xuất tỉnh thành + số 
                showTongTinhThanh(tinhthanh, tong, ++i);
            }
        }
        private void showTongTinhThanh(string[] tinhthanh, int[] tong, int n)
        {
            //Show KQ
            txtConsole.AppendText(Environment.NewLine);
            Console.Write("-Số lượng gói hàng cần gửi của từng tỉnh thành-");
            for (int i = 0; i < n; i++)
            {
                txtConsole.AppendText(Environment.NewLine);
                Console.Write("\t" + tinhthanh[i] + "\t  |  Tổng : " + tong[i]);
            }
        }
        //Câu 7
        public static void Swap<T>(ref T first, ref T second)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            if (second == null)
                throw new ArgumentNullException("second");

            var tmp = first;
            first = second;
            second = tmp;
        }
        private bool Ascending_TenKH_Goi(HangHoa x, HangHoa y)
        {
            return string.Compare(x.goi.tenKH, y.goi.tenKH) > 0;
        }
        private LinkedList<HangHoa> SortData_TenKH_Goi(LinkedList<HangHoa> list1)
        {
            LinkedList<HangHoa> list2 = list1;
            list2.InterchangeSort(Ascending_TenKH_Goi);
            return list2;
        }       
        private void SortData_TongPhi(string[] tenKH, float[] tong, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (tong[i] < tong[j])
                    {
                        Swap(ref tenKH[i],ref tenKH[j]);
                        Swap(ref tong[i],ref tong[j]);
                    }
                }
            }
        }
        LinkedListNode<HangHoa> FindName(LinkedList<HangHoa> list, string name)
        {
            LinkedListNode<HangHoa> p = list.First;
            while (p != null)
            {
                if (string.Compare(name, p.Value.goi.tenKH) == 0)
                {
                    break;
                }
                p = p.Next;
            }
            return p;
        }
        private void showKHThanThiet(LinkedList<HangHoa> list2, string[] tenKH, float[] tong, int n)
        {
            LinkedListNode<HangHoa> tmp;
            Console.Write("\t\t-Top 5 khách hàng thân thiết-");
            txtConsole.AppendText(Environment.NewLine);
            Console.Write(string.Format("  {0,-5}\t", "Rank"));
            Console.Write(string.Format("{0,-50}\t", "Họ và tên KH"));
            Console.Write(string.Format("{0,10}", "Tổng phí (VND)"));
            txtConsole.AppendText(Environment.NewLine);
            for (int i = 0; i < 100; i++)
            {
                Console.Write("*");
            }
            txtConsole.AppendText(Environment.NewLine);
            for (int i = 0; i < 5; i++)
            {
                tmp = FindName(list2, tenKH[i]);
                Console.Write(string.Format("    #{0,-5}\t", i + 1));
                Console.Write(string.Format("{0,-50}\t", tmp.Value.goi.tenKH));
                Console.Write(string.Format("{0,10}", tong[i]));
                txtConsole.AppendText(Environment.NewLine);
            }
        }
        private void timKHThanThiet()
        {
            int i = 0;
            string[] tenKH = new string[100];
            float[] tong = new float[100];
            if (list1.Count != 0)
            {
                LinkedList<HangHoa> list2;
                list2 = SortData_TenKH_Goi(list1);

                //Trích xuất tên người gởi
                tenKH[0] = list2.First.Value.goi.tenKH;
                tong[0]++;
                for (LinkedListNode<HangHoa> p = list2.First.Next; p != null; p = p.Next)
                {
                    if (string.Compare(tenKH[i], p.Value.goi.tenKH) == 0)
                    {
                        tong[i] += p.Value.phi;
                    }
                    else
                    {
                        i++;
                        tenKH[i] = p.Value.goi.tenKH;
                        tong[i] += p.Value.phi;
                    }
                }
                //Sort DS
                SortData_TongPhi(tenKH, tong, ++i);
                //Xuất KH có tổng chi phí gởi hàng nhiều nhất
                showKHThanThiet(list2, tenKH, tong, i);
            }
        }
        //Câu 8
        private void thongTinLeAnh()
        {
            float tongGoi = 0, tongNhan = 0;
            //Trích xuất tên người gởi
            for (LinkedListNode<HangHoa> p = list1.First; p != null; p = p.Next)
            {
                if (string.Compare("LeAnh", p.Value.goi.tenKH) == 0)
                {
                    tongGoi++;
                }
                else
                {
                    if (string.Compare("LeAnh", p.Value.nhan.tenKH) == 0)
                    {
                        tongNhan++;
                    }
                }
            }
            //Show KQ
            txtConsole.AppendText(Environment.NewLine);
            Console.Write("Tổng gói hàng gởi đi: " + tongGoi);
            txtConsole.AppendText(Environment.NewLine);
            Console.Write("Tổng gói hàng nhận về: " + tongNhan);
        }

        //Câu 9
        private bool dieuKienSort_Loai(HangHoa x, HangHoa y)
        {
            return string.Compare(x.loai, y.loai) > 0;
        }
        private void SortData_Loai(string[] tenLoai, float[] tong, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (tong[i] < tong[j])
                    {
                        Swap(ref tenLoai[i], ref tenLoai[j]);
                        Swap(ref tong[i], ref tong[j]);
                    }
                }
            }
        }
        private void loaiHangGoiNhieuNhat()
        {
            int i = 0;
            string[] tenLoai = new string[100];
            float[] tong = new float[100];
            if (list1.Count != 0)
            {
                LinkedList<HangHoa> list2 = list1;
                //Sort tăng dần theo loại
                list2.InterchangeSort(dieuKienSort_Loai);
                //Trích loại
                tenLoai[0] = list2.First.Value.goi.tenKH;
                tong[0]++;
                for (LinkedListNode<HangHoa> p = list2.First.Next; p != null; p = p.Next)
                {
                    if (string.Compare(tenLoai[i], p.Value.loai) == 0)
                    {
                        tong[i]++;
                    }
                    else
                    {
                        i++;
                        tenLoai[i] = p.Value.loai;
                        tong[i]++;
                    }
                }
                //Tìm loại được gởi nhiều nhất
                SortData_Loai(tenLoai, tong, ++i);
                //Xuất loại được gởi nhiều nhất
                Console.Write("Loại được gởi nhiều nhất:\t");
                Console.Write(tenLoai[0]);
                txtConsole.AppendText(Environment.NewLine);
                Console.Write("Số lần gửi: " + tong[0]);
            }            
        }

        //Câu 10
        private bool dieuKienSort_Phi(HangHoa x, HangHoa y)
        {
            return x.phi < y.phi;
        }
        private void show1HH(HangHoa x)
        {
            Console.Write(string.Format("{0,-9} ", x.maHang));
            Console.Write(string.Format("{0,-10} ", x.goi.tenKH));
            Console.Write(string.Format(" {0,-4}, {1,-4}, {2,-4}, {3,-7} ", x.goi.diaChi.soNha, x.goi.diaChi.phuong, x.goi.diaChi.quan, x.goi.diaChi.tinhThanh ));
            Console.Write(string.Format("{0,-12} ", x.goi.SDT));
            Console.Write(string.Format("{0,-10} ", x.nhan.tenKH));
            Console.Write(string.Format(" {0,-4}, {1,-4}, {2,-4}, {3,-7} ", x.nhan.diaChi.soNha, x.nhan.diaChi.phuong, x.nhan.diaChi.quan, x.nhan.diaChi.tinhThanh));
            Console.Write(string.Format("{0,-12} ", x.nhan.SDT));
            Console.Write(string.Format("{0,-15} ", x.phi));
            Console.Write(string.Format("{0,-5} ", x.trongLuong));
            Console.Write(string.Format("{0,-1} ", x.loai));
        }
        private void headerHH()
        {
            Console.Write(string.Format(" {0,-5} ", "Rank"));
            Console.Write(string.Format("{0,-9} ", "Mã hàng"));
            Console.Write(string.Format("{0,-20}", "Tên KH gởi"));
            Console.Write(string.Format("{0,-60} ", "Địa chỉ gởi"));
            Console.Write(string.Format("{0,-12} ", "SDT"));
            Console.Write(string.Format("{0,-20} ", "Tên KH nhận"));
            Console.Write(string.Format("{0,-60} ", "Địa chỉ nhận"));
            Console.Write(string.Format("{0,-12} ", "SDT"));
            Console.Write(string.Format("{0,-15} ", "Phí"));
            Console.Write(string.Format("{0,-5} ", "Trọng lượng"));
            Console.Write(string.Format("{0,-1} ", "Loại"));
            txtConsole.AppendText(Environment.NewLine);
            for (int i = 0; i < 197; i++)
            {
                Console.Write("*");
            }
        }
        private void showTopPhi(LinkedList<HangHoa> list)
        {
            Console.Write("\t\t-Top 3 hàng hóa có phí vận chuyển cao nhất-");
            txtConsole.AppendText(Environment.NewLine);
            headerHH();
            txtConsole.AppendText(Environment.NewLine);
            int stt = 1;
            foreach (var item in list)
            {
                if (stt < 4)
                {
                    Console.Write(string.Format(" {0,-5} ", stt++));
                    show1HH(item);
                    txtConsole.AppendText(Environment.NewLine);
                }              
            }
        }
        private void timGoiHangCoPhiCaoNhat()
        {

            if (list1.Count != 0)
            {
                LinkedList<HangHoa> list2 = list1;
                list2.InterchangeSort(dieuKienSort_Phi);
                showTopPhi(list2);
            }
        }

        private void ExportDataBtn_Click(object sender, EventArgs e)
        {
            txtConsole.Clear();
            x.ExportFile(list1);
        }
    }
}
