namespace MunicipalServicesApp
{
    partial class LocalEventsForm
    {
        private System.ComponentModel.IContainer components = null;

        
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code


        /// Required method for Designer support 
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {
            dgvEvents = new DataGridView();
            cmbCategoryFilter = new ComboBox();
            dtpDateFilter = new DateTimePicker();
            chkDateFilter = new CheckBox();
            txtSearch = new TextBox();
            btnSearch = new Button();
            btnClearFilters = new Button();
            lstRecommendations = new ListBox();
            lstSearchHistory = new ListBox();
            btnBack = new Button();
            lblStats = new Label();
            progressBar = new ProgressBar();
            lblTitle = new Label();
            lblCategoryFilter = new Label();
            lblSearch = new Label();
            lblRecommendations = new Label();
            lblSearchHistory = new Label();
            panelFilters = new Panel();
            panelRecommendations = new Panel();
            panelHistory = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvEvents).BeginInit();
            panelFilters.SuspendLayout();
            panelRecommendations.SuspendLayout();
            panelHistory.SuspendLayout();
            SuspendLayout();
            // 
            // dgvEvents
            // 
            dgvEvents.AllowUserToAddRows = false;
            dgvEvents.AllowUserToDeleteRows = false;
            dgvEvents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEvents.BackgroundColor = Color.White;
            dgvEvents.BorderStyle = BorderStyle.None;
            dgvEvents.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvEvents.ColumnHeadersHeight = 30;
            dgvEvents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvEvents.Location = new Point(27, 308);
            dgvEvents.Margin = new Padding(4, 5, 4, 5);
            dgvEvents.MultiSelect = false;
            dgvEvents.Name = "dgvEvents";
            dgvEvents.ReadOnly = true;
            dgvEvents.RowHeadersVisible = false;
            dgvEvents.RowHeadersWidth = 51;
            dgvEvents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEvents.Size = new Size(867, 585);
            dgvEvents.TabIndex = 0;
            dgvEvents.CellDoubleClick += dgvEvents_CellDoubleClick;
            // 
            // cmbCategoryFilter
            // 
            cmbCategoryFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoryFilter.Font = new Font("Segoe UI", 10F);
            cmbCategoryFilter.FormattingEnabled = true;
            cmbCategoryFilter.Location = new Point(13, 54);
            cmbCategoryFilter.Margin = new Padding(4, 5, 4, 5);
            cmbCategoryFilter.Name = "cmbCategoryFilter";
            cmbCategoryFilter.Size = new Size(239, 31);
            cmbCategoryFilter.TabIndex = 1;
            // 
            // dtpDateFilter
            // 
            dtpDateFilter.Enabled = false;
            dtpDateFilter.Font = new Font("Segoe UI", 10F);
            dtpDateFilter.Location = new Point(293, 54);
            dtpDateFilter.Margin = new Padding(4, 5, 4, 5);
            dtpDateFilter.Name = "dtpDateFilter";
            dtpDateFilter.Size = new Size(265, 30);
            dtpDateFilter.TabIndex = 2;
            // 
            // chkDateFilter
            // 
            chkDateFilter.AutoSize = true;
            chkDateFilter.Font = new Font("Segoe UI", 9F);
            chkDateFilter.Location = new Point(293, 18);
            chkDateFilter.Margin = new Padding(4, 5, 4, 5);
            chkDateFilter.Name = "chkDateFilter";
            chkDateFilter.Size = new Size(120, 24);
            chkDateFilter.TabIndex = 3;
            chkDateFilter.Text = "Filter by Date";
            chkDateFilter.UseVisualStyleBackColor = true;
            chkDateFilter.CheckedChanged += chkDateFilter_CheckedChanged;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(13, 131);
            txtSearch.Margin = new Padding(4, 5, 4, 5);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(399, 30);
            txtSearch.TabIndex = 4;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(33, 150, 243);
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(427, 126);
            btnSearch.Margin = new Padding(4, 5, 4, 5);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(133, 46);
            btnSearch.TabIndex = 5;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // btnClearFilters
            // 
            btnClearFilters.BackColor = Color.FromArgb(158, 158, 158);
            btnClearFilters.Cursor = Cursors.Hand;
            btnClearFilters.FlatAppearance.BorderSize = 0;
            btnClearFilters.FlatStyle = FlatStyle.Flat;
            btnClearFilters.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnClearFilters.ForeColor = Color.White;
            btnClearFilters.Location = new Point(573, 126);
            btnClearFilters.Margin = new Padding(4, 5, 4, 5);
            btnClearFilters.Name = "btnClearFilters";
            btnClearFilters.Size = new Size(133, 46);
            btnClearFilters.TabIndex = 6;
            btnClearFilters.Text = "Clear Filters";
            btnClearFilters.UseVisualStyleBackColor = false;
            btnClearFilters.Click += btnClearFilters_Click;
            // 
            // lstRecommendations
            // 
            lstRecommendations.BorderStyle = BorderStyle.None;
            lstRecommendations.Font = new Font("Segoe UI", 9F);
            lstRecommendations.FormattingEnabled = true;
            lstRecommendations.Location = new Point(13, 62);
            lstRecommendations.Margin = new Padding(4, 5, 4, 5);
            lstRecommendations.Name = "lstRecommendations";
            lstRecommendations.SelectionMode = SelectionMode.None;
            lstRecommendations.Size = new Size(357, 240);
            lstRecommendations.TabIndex = 7;
            // 
            // lstSearchHistory
            // 
            lstSearchHistory.BorderStyle = BorderStyle.None;
            lstSearchHistory.Font = new Font("Segoe UI", 9F);
            lstSearchHistory.FormattingEnabled = true;
            lstSearchHistory.Location = new Point(13, 62);
            lstSearchHistory.Margin = new Padding(4, 5, 4, 5);
            lstSearchHistory.Name = "lstSearchHistory";
            lstSearchHistory.SelectionMode = SelectionMode.None;
            lstSearchHistory.Size = new Size(357, 180);
            lstSearchHistory.TabIndex = 8;
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.FromArgb(120, 120, 120);
            btnBack.Cursor = Cursors.Hand;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(27, 28);
            btnBack.Margin = new Padding(4, 5, 4, 5);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(200, 62);
            btnBack.TabIndex = 9;
            btnBack.Text = "← Back to Menu";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // lblStats
            // 
            lblStats.AutoSize = true;
            lblStats.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStats.ForeColor = Color.FromArgb(66, 66, 66);
            lblStats.Location = new Point(27, 908);
            lblStats.Margin = new Padding(4, 0, 4, 0);
            lblStats.Name = "lblStats";
            lblStats.Size = new Size(306, 20);
            lblStats.TabIndex = 10;
            lblStats.Text = "Total Events: 0 | Categories: 0 | Searches: 0";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(27, 938);
            progressBar.Margin = new Padding(4, 5, 4, 5);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(1279, 28);
            progressBar.TabIndex = 11;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(33, 150, 243);
            lblTitle.Location = new Point(455, 28);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(432, 41);
            lblTitle.TabIndex = 12;
            lblTitle.Text = "Local Events & Announcements";
            // 
            // lblCategoryFilter
            // 
            lblCategoryFilter.AutoSize = true;
            lblCategoryFilter.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCategoryFilter.Location = new Point(13, 23);
            lblCategoryFilter.Margin = new Padding(4, 0, 4, 0);
            lblCategoryFilter.Name = "lblCategoryFilter";
            lblCategoryFilter.Size = new Size(138, 20);
            lblCategoryFilter.TabIndex = 13;
            lblCategoryFilter.Text = "Filter by Category:";
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSearch.Location = new Point(13, 100);
            lblSearch.Margin = new Padding(4, 0, 4, 0);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(185, 20);
            lblSearch.TabIndex = 14;
            lblSearch.Text = "Search Events/Keywords:";
            // 
            // lblRecommendations
            // 
            lblRecommendations.AutoSize = true;
            lblRecommendations.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblRecommendations.ForeColor = Color.FromArgb(255, 152, 0);
            lblRecommendations.Location = new Point(13, 15);
            lblRecommendations.Margin = new Padding(4, 0, 4, 0);
            lblRecommendations.Name = "lblRecommendations";
            lblRecommendations.Size = new Size(216, 25);
            lblRecommendations.TabIndex = 15;
            lblRecommendations.Text = "Recommended for You";
            // 
            // lblSearchHistory
            // 
            lblSearchHistory.AutoSize = true;
            lblSearchHistory.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSearchHistory.ForeColor = Color.FromArgb(156, 39, 176);
            lblSearchHistory.Location = new Point(13, 15);
            lblSearchHistory.Margin = new Padding(4, 0, 4, 0);
            lblSearchHistory.Name = "lblSearchHistory";
            lblSearchHistory.Size = new Size(138, 23);
            lblSearchHistory.TabIndex = 16;
            lblSearchHistory.Text = "Recent Searches";
            // 
            // panelFilters
            // 
            panelFilters.BackColor = Color.FromArgb(245, 245, 245);
            panelFilters.BorderStyle = BorderStyle.FixedSingle;
            panelFilters.Controls.Add(lblCategoryFilter);
            panelFilters.Controls.Add(cmbCategoryFilter);
            panelFilters.Controls.Add(chkDateFilter);
            panelFilters.Controls.Add(dtpDateFilter);
            panelFilters.Controls.Add(lblSearch);
            panelFilters.Controls.Add(txtSearch);
            panelFilters.Controls.Add(btnSearch);
            panelFilters.Controls.Add(btnClearFilters);
            panelFilters.Location = new Point(27, 108);
            panelFilters.Margin = new Padding(4, 5, 4, 5);
            panelFilters.Name = "panelFilters";
            panelFilters.Size = new Size(866, 184);
            panelFilters.TabIndex = 17;
            // 
            // panelRecommendations
            // 
            panelRecommendations.BackColor = Color.FromArgb(255, 248, 225);
            panelRecommendations.BorderStyle = BorderStyle.FixedSingle;
            panelRecommendations.Controls.Add(lblRecommendations);
            panelRecommendations.Controls.Add(lstRecommendations);
            panelRecommendations.Location = new Point(920, 108);
            panelRecommendations.Margin = new Padding(4, 5, 4, 5);
            panelRecommendations.Name = "panelRecommendations";
            panelRecommendations.Size = new Size(386, 337);
            panelRecommendations.TabIndex = 18;
            // 
            // panelHistory
            // 
            panelHistory.BackColor = Color.FromArgb(243, 229, 245);
            panelHistory.BorderStyle = BorderStyle.FixedSingle;
            panelHistory.Controls.Add(lblSearchHistory);
            panelHistory.Controls.Add(lstSearchHistory);
            panelHistory.Location = new Point(920, 477);
            panelHistory.Margin = new Padding(4, 5, 4, 5);
            panelHistory.Name = "panelHistory";
            panelHistory.Size = new Size(386, 276);
            panelHistory.TabIndex = 19;
            // 
            // LocalEventsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1333, 1046);
            Controls.Add(panelHistory);
            Controls.Add(panelRecommendations);
            Controls.Add(panelFilters);
            Controls.Add(lblTitle);
            Controls.Add(progressBar);
            Controls.Add(lblStats);
            Controls.Add(btnBack);
            Controls.Add(dgvEvents);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "LocalEventsForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Municipal Services - Local Events";
            ((System.ComponentModel.ISupportInitialize)dgvEvents).EndInit();
            panelFilters.ResumeLayout(false);
            panelFilters.PerformLayout();
            panelRecommendations.ResumeLayout(false);
            panelRecommendations.PerformLayout();
            panelHistory.ResumeLayout(false);
            panelHistory.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvEvents;
        private System.Windows.Forms.ComboBox cmbCategoryFilter;
        private System.Windows.Forms.DateTimePicker dtpDateFilter;
        private System.Windows.Forms.CheckBox chkDateFilter;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.ListBox lstRecommendations;
        private System.Windows.Forms.ListBox lstSearchHistory;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblCategoryFilter;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Label lblRecommendations;
        private System.Windows.Forms.Label lblSearchHistory;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.Panel panelRecommendations;
        private System.Windows.Forms.Panel panelHistory;
    }
}
