
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;


namespace JobSearcher_16._11
{
    public partial class Form1 : Form
    {
        public MainLogic ml { get; set; }
        System.Windows.Forms.DataGridView dgv { get; set; }
        public Button curbutton { get; set; }

        public Form1()
        {
            InitializeComponent();
            ml = new MainLogic();

        }

        private void btn_JobMaster_Click(object sender, EventArgs e)
        {
           
            jobButtonWasClicked();
            ml.JobMasterWasChosen();
            
            l_pageID.Text = ml.NumPagesLabel;
            l_status.Text = ml.StatusLabel;
            if (ml.checkIfToCreateTable() == false)
            {
                btn_JobMaster.PerformClick();
            }
            else
            {
                BuildTable();
            }

           
         }

        private void jobButtonWasClicked()
        {
            if (dgv != null)//if we've already shown table - erase it
            {
                Controls.Remove(this.dgv);
            }
        }


        private void BuildTable()
        {

            l_status.Text = "Setting Table";
            dgv = new System.Windows.Forms.DataGridView();
            dgv.Location = new System.Drawing.Point(10, 10);
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.Fixed3D;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgv.DefaultCellStyle.Font = new Font
               ("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgv.Columns.Add("Id", "Id");
            dgv.Columns.Add("Title", "Title");
            dgv.Columns.Add("isRecommanded", "isRecommanded");
            dgv.Columns.Add("URL", "URL");
            dgv.Columns.Add("CurrentStatus", "CurrentStatus");
            dgv.Columns.Add("UpdateSent", "UpdateSent");
            dgv.Columns.Add("ChangeDate", "ChangeDate");
            dgv.Columns.Add("Irrelevant", "Irrelevant");



            foreach (Job cur_job in ml.RecommendedJobs)
            {
                string statusMsg = cur_job.Status;
                string userMsg;

                if (cur_job.isRecommended == true)
                {
                    userMsg = "Yes :-)";
                }
                else
                {
                    userMsg = "Try Me";
                }
                

                l_status.Text = "Sorting Jobs To Table.";

                if ((statusMsg != "Sent") && (statusMsg != "Irrelevant"))
                {
                    var index = dgv.Rows.Add(cur_job.ID, cur_job.Title, userMsg, cur_job.URL, statusMsg, "Update", "change");
                    int curSize = dgv.RowCount - 1;
                }

            }
            l_status.Text = ml.RecommendedJobs.Count.ToString() +" Jobs Shown.";
            dgv.CellClick += new DataGridViewCellEventHandler(dgv_CellClick);
            Controls.Add(this.dgv);
            dgv.Width = 1289;
            dgv.Height = 720;
            dgv.Columns[0].Width = 100;
            dgv.Columns[1].Width = 400;
            dgv.Columns[2].Width = 100;
            dgv.Columns[3].Width = 75;
            dgv.Columns[4].Width = 100;
            dgv.Columns[5].Width = 100;
            dgv.Columns[6].Width = 100;
            
            
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = (DataGridViewCell)dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            DataGridViewRow row = dgv.Rows[e.RowIndex];
            string CurrentJobID = row.Cells[0].Value.ToString();

            if (cell.ColumnIndex == 3)
            {
                if (cell.Value != null)
                {
                    string URL = cell.Value.ToString();
                    System.Diagnostics.Process.Start(URL);
                   // DataGridViewRow row = dgv.Rows[e.RowIndex];
                   // ml.CurrentJobID = row.Cells[0].Value.ToString();
                    try
                    {
                        ml.insertOrUpdateViewed(CurrentJobID);
                    }
                    catch (Exception ex)
                    {
                        row.Cells[0].Value = "Please try again";
                    }
                }
                //  row.Cells[6].Value = "Viewed";

            }
            else if (cell.ColumnIndex == 4)
            {

               // DataGridViewRow row = dgv.Rows[e.RowIndex];

                try
                {
                    string statusMsg = ml.checkIfSent(CurrentJobID);
                    row.Cells[4].Value = statusMsg;
                }
                catch (Exception ex)
                {
                    row.Cells[4].Value = "Please try again";
                }




            }

            else if (cell.ColumnIndex == 5)
            {
                if (cell.Value != null)
                {
                   // DataGridViewRow row = dgv.Rows[e.RowIndex];
                   // ml.CurrentJobID = row.Cells[0].Value.ToString();
                    try
                    {
                        ml.insertOrUpdate(CurrentJobID);
                        row.Cells[5].Value = "Sent OK";
                        //paintItBlack(row);
                    }
                    catch (Exception ex)
                    {
                        row.Cells[5].Value = "Please try again";
                    }
                }
            }

            else if (cell.ColumnIndex == 6)
            {
                if (cell.Value != null)
                {
                   // DataGridViewRow row = dgv.Rows[e.RowIndex];
                 //   ml.CurrentJobID = row.Cells[0].Value.ToString();
                   // if ((t_day.Text != null) && (t_month.Text != null))
                    {
                        try
                        {
                       //     ml.changeDate(t_day.Text, t_month.Text);
                        //    row.Cells[6].Value = "Changed OK";
                         //   paintItBlack(row);
                        }
                        catch (Exception ex)
                        {
                            row.Cells[6].Value = "Please try again";
                        }
                    }

                }
            }
            else if (cell.ColumnIndex == 7)
            {
                // if (cell.Value != null)
                //  {
             //   DataGridViewRow row = dgv.Rows[e.RowIndex];

               
                try
                {

                    ml.changetoIrrelevant(CurrentJobID);
                    // ml.changeDate(t_day.Text, t_month.Text);
                    row.Cells[7].Value = "Irrelevant";
                   // paintItBlack(row);

                }
                catch (Exception ex)
                {
                    row.Cells[7].Value = "Please try again";
                }
                //  }
            }
        }

    }
}
