using Business.Abstract;
using Business.Concrete;
using Northwind.DataAccess.Concrete.EntityFramework;
using Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Northwind.WebFormsUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _categoryService = new CategoryManager(new EfCategoryDal());
            _productService = new ProductManager(new EfProductDal());
        }
        private ICategoryService _categoryService;
        private IProductService _productService;
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadCategories();
        }

        private void LoadProducts()
        {
            dgwProduct.DataSource = _productService.GetAll();
        }
        private void LoadCategories()
        {
            cbxCategory.DataSource = _categoryService.GetAll();
            cbxCategory.DisplayMember = "CategoryName";
            cbxCategory.ValueMember = "CategoryId";

            cbxKategori.DataSource = _categoryService.GetAll();
            cbxKategori.DisplayMember = "CategoryName";
            cbxKategori.ValueMember = "CategoryId";

            cbxCategoryUpdate.DataSource = _categoryService.GetAll();
            cbxCategoryUpdate.DisplayMember = "CategoryName";
            cbxCategoryUpdate.ValueMember = "CategoryId";
        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgwProduct.DataSource = _productService.GetProductsByCategory(Convert.ToInt32(cbxCategory.SelectedValue));
            }
            catch
            {              
            }
        }

        private void tbxProductName_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbxProductName.Text))
            {
                dgwProduct.DataSource = _productService.GetProductsByProductName(tbxProductName.Text);
            }
            else
            {
                LoadProducts();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _productService.Add(new Product
            {
                CategoryId = Convert.ToInt32(cbxKategori.SelectedValue),
                ProductName = tbxProductName2.Text,
                QuantityPerUnit = tbxQuantityPerUnit.Text,
                UnitPrice = Convert.ToDecimal(tbxUnitPrice.Text),
                UnitsInStock = Convert.ToInt16(tbxStock.Text)
            });
            MessageBox.Show("Ürün Eklendi.");
            LoadProducts();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            _productService.Update(new Product 
            {
             ProductId=Convert.ToInt32(dgwProduct.CurrentRow.Cells[0].Value),
             ProductName=tbxProductNameUpdate.Text,
             CategoryId=Convert.ToInt32(cbxCategoryUpdate.SelectedValue),
             QuantityPerUnit=tbxQuantityPerUnitUpdate.Text,
             UnitsInStock=Convert.ToInt16(tbxUnitsInStock.Text),
             UnitPrice=Convert.ToDecimal(tbxUnitPriceUpdate.Text)         
            });
            MessageBox.Show("Ürün Güncellendi.");
            LoadProducts();
        }

        private void dgwProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dgwProduct.CurrentRow;
            tbxProductNameUpdate.Text = row.Cells[1].Value.ToString();
            cbxCategoryUpdate.SelectedValue = row.Cells[2].Value;
            tbxUnitPriceUpdate.Text = row.Cells[3].Value.ToString();
            tbxQuantityPerUnitUpdate.Text = row.Cells[4].Value.ToString();
            tbxUnitsInStock.Text = row.Cells[5].Value.ToString();

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgwProduct.CurrentRow != null)
                {
                    _productService.Delete(new Product
                    {
                        ProductId = Convert.ToInt32(dgwProduct.CurrentRow.Cells[0].Value)
                    });
                }

                MessageBox.Show("Ürün Silindi.");
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
          
        }
    }
}
