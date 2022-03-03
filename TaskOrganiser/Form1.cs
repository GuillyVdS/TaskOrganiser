using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskOrganiser.Model;

namespace TaskOrganiser
{
    public partial class Form1 : Form
    {
        private taskorgDBContext taskorgContext;
        public Form1()
        {
            InitializeComponent();

            taskorgContext = new taskorgDBContext();

            var statuses = taskorgContext.Statuses.ToList();

            foreach (Status s in statuses)
            {
                cboStatus.Items.Add(s);
            }

            refreshData();
        }

        private void refreshData()
        { 
            BindingSource bi = new BindingSource();

            var query = from t in taskorgContext.Tasks
                        orderby t.DueDate
                        select new { t.Id, t.Name, StatusName = t.Status.Name, t.DueDate };
            bi.DataSource = query.ToList();

            dataGridView1.DataSource = bi;
            dataGridView1.Refresh();
        }

        private void clearFields()
        {
            cmdUpdateTask.Text = "Update";
            txtTask.Text = String.Empty;
            dateTimePicker1.Value = DateTime.Now;
            cboStatus.Text = "Please Select...";
        }

        private void cmdCreateTask_Click(object sender, EventArgs e)
        {
            if (cboStatus.SelectedItem != null && txtTask.Text != String.Empty)
            {
                var newTask = new Model.Task
                {
                    Name = txtTask.Text,
                    StatusId = (cboStatus.SelectedItem as Model.Status).Id,
                    DueDate = dateTimePicker1.Value
                };

                taskorgContext.Tasks.Add(newTask);
                taskorgContext.SaveChanges();
                refreshData();
            }
            else
            {
                MessageBox.Show("Please make sure all data has been entered.");
            }
        }

        private void cmdDeleteTask_Click(object sender, EventArgs e)
        {
            var t = taskorgContext.Tasks.Find((int)dataGridView1.SelectedCells[0].Value);

            taskorgContext.Tasks.Remove(t);
            taskorgContext.SaveChanges();
            refreshData();
        }

        private void cmdUpdateTask_Click(object sender, EventArgs e)
        {
            if (cmdUpdateTask.Text == "Update")
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    txtTask.Text = dataGridView1.SelectedCells[1].Value.ToString();
                    dateTimePicker1.Value = (DateTime)dataGridView1.SelectedCells[3].Value;
                    foreach (Status s in cboStatus.Items)
                    {
                        if (s.Name == dataGridView1.SelectedCells[2].Value.ToString())
                        {
                            cboStatus.SelectedItem = s;
                        }
                    }
                    cmdUpdateTask.Text = "Save";
                }
                else 
                {
                    MessageBox.Show("Please select data you wish to update.");
                }
                
            }
            else if (cmdUpdateTask.Text == "Save")
            {
                var t = taskorgContext.Tasks.Find((int)dataGridView1.SelectedCells[0].Value);
                t.Name = txtTask.Text;
                t.StatusId = (cboStatus.SelectedItem as Status).Id;
                t.DueDate = dateTimePicker1.Value;
                taskorgContext.SaveChanges();
                refreshData();
                clearFields();
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            clearFields();
        }
    }
}
