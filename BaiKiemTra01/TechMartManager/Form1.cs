using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TechMartManager
{
    public partial class Form1 : Form
    {
        private readonly BindingList<Product> _masterList = new BindingList<Product>();
        private readonly BindingSource _bs = new BindingSource();

        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            // categories
            cboCategory.DisplayMember = "Text";
            cboCategory.ValueMember = "Value";
            cboCategory.Items.Add(new { Text = "Điện thoại", Value = "Điện thoại" });
            cboCategory.Items.Add(new { Text = "Laptop", Value = "Laptop" });
            cboCategory.Items.Add(new { Text = "Phụ kiện", Value = "Phụ kiện" });

            // data binding
            _bs.DataSource = _masterList;
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.DataSource = _bs;

            // configure DataGridView columns
            dgvProducts.Columns.Clear();
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "Mã SP", DataPropertyName = "ProductId", Width = 120 });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { Name = "colName", HeaderText = "Tên SP", DataPropertyName = "ProductName", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCategory", HeaderText = "Danh Mục", DataPropertyName = "Category", Width = 120 });
            var colPrice = new DataGridViewTextBoxColumn { Name = "colPrice", HeaderText = "Đơn Giá", DataPropertyName = "UnitPrice", Width = 140 };
            dgvProducts.Columns.Add(colPrice);
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { Name = "colQty", HeaderText = "Số Lượng", DataPropertyName = "Quantity", Width = 80 });

            dgvProducts.CellFormatting += DgvProducts_CellFormatting;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;

            // events
            dgvProducts.SelectionChanged += DgvProducts_SelectionChanged;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            btnChooseImage.Click += BtnChooseImage_Click;
            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            exportCSVToolStripMenuItem.Click += ExportCSVToolStripMenuItem_Click;
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;

            UpdateStatus();
        }

        private void DgvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvProducts.Columns[e.ColumnIndex].Name == "colPrice")
            {
                if (e.Value is decimal dec)
                {
                    e.Value = dec.ToString("N0") + " VNĐ";
                    e.FormattingApplied = true;
                }
            }
        }

        private void UpdateStatus()
        {
            toolStripStatusLabel1.Text = $"Tổng số sản phẩm: {_masterList.Count}";
        }

        private void BtnChooseImage_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtImagePath.Text = dlg.FileName;
                    try
                    {
                        using (var img = Image.FromFile(dlg.FileName))
                        {
                            picAvatar.Image = new Bitmap(img);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Không thể nạp ảnh.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private bool ValidateInputs()
        {
            errorProvider1.Clear();
            bool ok = true;
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                errorProvider1.SetError(txtProductName, "Tên SP không được để trống");
                ok = false;
            }
            if (!decimal.TryParse(txtUnitPrice.Text, out var price) || price <= 0)
            {
                errorProvider1.SetError(txtUnitPrice, "Đơn giá phải > 0");
                ok = false;
            }
            if (!int.TryParse(txtQuantity.Text, out var qty) || qty < 0)
            {
                errorProvider1.SetError(txtQuantity, "Số lượng phải >= 0");
                ok = false;
            }
            return ok;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;
            var p = new Product
            {
                ProductId = string.IsNullOrWhiteSpace(txtProductId.Text) ? Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper() : txtProductId.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                Category = cboCategory.SelectedItem != null ? cboCategory.GetItemText(cboCategory.SelectedItem) : string.Empty,
                UnitPrice = decimal.Parse(txtUnitPrice.Text),
                Quantity = int.Parse(txtQuantity.Text),
                ImagePath = txtImagePath.Text
            };
            _masterList.Add(p);
            _bs.ResetBindings(false);
            UpdateStatus();
            ClearInput();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;
            if (!ValidateInputs()) return;
            var p = dgvProducts.CurrentRow.DataBoundItem as Product;
            if (p == null) return;
            p.ProductId = txtProductId.Text.Trim();
            p.ProductName = txtProductName.Text.Trim();
            p.Category = cboCategory.SelectedItem != null ? cboCategory.GetItemText(cboCategory.SelectedItem) : string.Empty;
            p.UnitPrice = decimal.Parse(txtUnitPrice.Text);
            p.Quantity = int.Parse(txtQuantity.Text);
            p.ImagePath = txtImagePath.Text;
            _bs.ResetBindings(false);
            UpdateStatus();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;
            var p = dgvProducts.CurrentRow.DataBoundItem as Product;
            if (p == null) return;
            var res = MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                _masterList.Remove(p);
                _bs.ResetBindings(false);
                UpdateStatus();
            }
        }

        private void DgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
            {
                ClearInput();
                return;
            }
            var p = dgvProducts.CurrentRow.DataBoundItem as Product;
            if (p == null) return;
            txtProductId.Text = p.ProductId;
            txtProductName.Text = p.ProductName;
            txtUnitPrice.Text = p.UnitPrice.ToString();
            txtQuantity.Text = p.Quantity.ToString();
            // select category
            for (int i = 0; i < cboCategory.Items.Count; i++)
            {
                if (cboCategory.GetItemText(cboCategory.Items[i]) == p.Category)
                {
                    cboCategory.SelectedIndex = i; break;
                }
            }
            txtImagePath.Text = p.ImagePath;
            if (!string.IsNullOrWhiteSpace(p.ImagePath) && File.Exists(p.ImagePath))
            {
                try
                {
                    using (var img = Image.FromFile(p.ImagePath))
                    {
                        picAvatar.Image = new Bitmap(img);
                    }
                }
                catch { picAvatar.Image = null; }
            }
            else picAvatar.Image = null;
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            var kw = txtSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(kw))
            {
                _bs.DataSource = _masterList;
            }
            else
            {
                var filtered = _masterList.Where(p => p.ProductName.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                _bs.DataSource = new BindingList<Product>(filtered);
            }
            dgvProducts.DataSource = _bs;
        }

        private void ExportCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "CSV Files|*.csv";
                dlg.DefaultExt = "csv";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var lines = new List<string> { "ProductId,ProductName,Category,UnitPrice,Quantity,ImagePath" };
                        foreach (var p in _masterList)
                        {
                            var name = p.ProductName?.Replace('"', '\'') ?? string.Empty;
                            var line = $"\"{p.ProductId}\",\"{name}\",\"{p.Category}\",{p.UnitPrice},{p.Quantity},\"{p.ImagePath}\"";
                            lines.Add(line);
                        }
                        File.WriteAllLines(dlg.FileName, lines, Encoding.UTF8);
                        MessageBox.Show("Xuất CSV thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xuất CSV: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ClearInput()
        {
            txtProductId.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtUnitPrice.Text = string.Empty;
            txtQuantity.Text = "0";
            txtImagePath.Text = string.Empty;
            picAvatar.Image = null;
            cboCategory.SelectedIndex = -1;
            errorProvider1.Clear();
        }
    }
}
