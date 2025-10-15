using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace MunicipalServicesApp
{
    public partial class LocalEventsForm : Form
    {
        public class EventInfo : IComparable<EventInfo>
        {
            public string Title { get; set; }
            public string Category { get; set; }
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public int Priority { get; set; }

            public int CompareTo(EventInfo other)
            {
                int priorityComparison = this.Priority.CompareTo(other.Priority);
                return priorityComparison != 0 ? priorityComparison : this.Date.CompareTo(other.Date);
            }
        }

        // Data structures
        private SortedDictionary<DateTime, List<EventInfo>> eventsByDate;
        private SortedDictionary<string, int> categoryAccessCount;
        private Dictionary<string, List<EventInfo>> categoryLookup;
        private Dictionary<string, string> categoryDescriptions;
        private HashSet<string> uniqueCategories;
        private HashSet<DateTime> eventDates;
        private Stack<EventInfo> recentlyViewedStack;
        private Queue<string> searchHistoryQueue;
        private SortedSet<EventInfo> priorityEvents;
        private Dictionary<string, int> userSearchPatterns;
        private List<string> userPreferredCategories;

        private Panel headerPanel;
        private Panel recommendationsContentPanel;
        private Panel historyContentPanel;

        public LocalEventsForm()
        {
            InitializeComponent();
            SetupModernUI();
            InitializeDataStructures();
            LoadSampleEvents();
            SetupDataGridView();
            PopulateFilters();
            DisplayAllEvents();
            UpdateStatistics();
        }

        private void SetupModernUI()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(1400, 900);
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    NativeMethods.ReleaseCapture();
                    NativeMethods.SendMessage(this.Handle, 0xA1, 0x2, 0);
                }
            };

            CreateModernHeader();
            HideDesignerControls();
            RepositionControls();
            StylePanels();
            StyleDataGridView();
            StyleButtons();
            StyleRecommendationsPanel();
            StyleHistoryPanel();
        }

        private void HideDesignerControls()
        {
            lblTitle.Visible = false;
            btnBack.Visible = false;
        }

        private void RepositionControls()
        {
            int headerHeight = 100;
            int topMargin = headerHeight + 25;
            int spacing = 20;

            // Filters panel
            panelFilters.Location = new Point(30, topMargin);
            panelFilters.Size = new Size(860, 190);

            // Events grid
            dgvEvents.Location = new Point(30, panelFilters.Bottom + spacing);
            dgvEvents.Size = new Size(860, 480);

            // Right panels
            int rightX = 920;
            panelRecommendations.Location = new Point(rightX, topMargin);
            panelRecommendations.Size = new Size(450, 420);

            panelHistory.Location = new Point(rightX, panelRecommendations.Bottom + spacing);
            panelHistory.Size = new Size(450, 270);

            // Stats and progress bar
            lblStats.Location = new Point(30, dgvEvents.Bottom + 15);
            progressBar.Location = new Point(30, lblStats.Bottom + 8);
            progressBar.Size = new Size(1340, 25);
        }

        private void CreateModernHeader()
        {
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.White
            };

            headerPanel.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(new Pen(Color.FromArgb(220, 220, 220), 1),
                    0, headerPanel.Height - 1, headerPanel.Width, headerPanel.Height - 1);
            };

            IconButton backIconBtn = new IconButton
            {
                IconChar = IconChar.ArrowLeft,
                IconColor = Color.White,
                IconSize = 24,
                Text = " Back to Menu",
                TextAlign = ContentAlignment.MiddleRight,
                ImageAlign = ContentAlignment.MiddleLeft,
                Size = new Size(180, 50),
                Location = new Point(30, 25),
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            backIconBtn.FlatAppearance.BorderSize = 0;
            backIconBtn.Click += btnBack_Click;

            IconPictureBox titleIcon = new IconPictureBox
            {
                IconChar = IconChar.CalendarCheck,
                IconColor = Color.FromArgb(52, 152, 219),
                IconSize = 50,
                Size = new Size(60, 60),
                Location = new Point(this.Width / 2 - 280, 20)
            };

            Label titleLbl = new Label
            {
                Text = "Local Events & Announcements",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(this.Width / 2 - 210, 32)
            };

            IconButton closeBtn = new IconButton
            {
                IconChar = IconChar.Times,
                IconColor = Color.FromArgb(149, 165, 166),
                IconSize = 20,
                Size = new Size(45, 45),
                Location = new Point(this.Width - 55, 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(231, 76, 60);
            closeBtn.Click += (s, e) => Application.Exit();
            closeBtn.MouseEnter += (s, e) => closeBtn.IconColor = Color.White;
            closeBtn.MouseLeave += (s, e) => closeBtn.IconColor = Color.FromArgb(149, 165, 166);

            IconButton minBtn = new IconButton
            {
                IconChar = IconChar.WindowMinimize,
                IconColor = Color.FromArgb(149, 165, 166),
                IconSize = 20,
                Size = new Size(45, 45),
                Location = new Point(this.Width - 100, 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            minBtn.FlatAppearance.BorderSize = 0;
            minBtn.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

            headerPanel.Controls.AddRange(new Control[] { backIconBtn, titleIcon, titleLbl, closeBtn, minBtn });
            this.Controls.Add(headerPanel);
            headerPanel.BringToFront();
        }

        private void StylePanels()
        {
            panelFilters.BackColor = Color.White;
            panelFilters.Paint += (s, e) => DrawPanelShadow(e.Graphics, panelFilters, Color.FromArgb(52, 152, 219));

            panelRecommendations.BackColor = Color.White;
            panelRecommendations.Paint += (s, e) => DrawPanelShadow(e.Graphics, panelRecommendations, Color.FromArgb(241, 196, 15));

            panelHistory.BackColor = Color.White;
            panelHistory.Paint += (s, e) => DrawPanelShadow(e.Graphics, panelHistory, Color.FromArgb(155, 89, 182));
        }

        private void DrawPanelShadow(Graphics g, Panel panel, Color accentColor)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle shadowRect = new Rectangle(5, 5, panel.Width - 10, panel.Height - 10);
            using (GraphicsPath shadowPath = GetRoundedRectangle(shadowRect, 15))
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(20, 0, 0, 0)))
            {
                g.FillPath(shadowBrush, shadowPath);
            }

            Rectangle panelRect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            using (GraphicsPath panelPath = GetRoundedRectangle(panelRect, 15))
            using (SolidBrush panelBrush = new SolidBrush(Color.White))
            {
                g.FillPath(panelBrush, panelPath);
            }

            Rectangle accentRect = new Rectangle(0, 0, 4, panel.Height);
            using (SolidBrush accentBrush = new SolidBrush(accentColor))
            {
                g.FillRectangle(accentBrush, accentRect);
            }

            using (GraphicsPath borderPath = GetRoundedRectangle(panelRect, 15))
            using (Pen borderPen = new Pen(Color.FromArgb(230, 230, 230), 1))
            {
                g.DrawPath(borderPen, borderPath);
            }
        }

        private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void StyleDataGridView()
        {
            dgvEvents.BorderStyle = BorderStyle.None;
            dgvEvents.BackgroundColor = Color.White;
            dgvEvents.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvEvents.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgvEvents.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvEvents.DefaultCellStyle.BackColor = Color.White;
            dgvEvents.DefaultCellStyle.ForeColor = Color.FromArgb(44, 62, 80);
            dgvEvents.DefaultCellStyle.Padding = new Padding(5);
            dgvEvents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dgvEvents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEvents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvEvents.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            dgvEvents.EnableHeadersVisualStyles = false;
            dgvEvents.RowTemplate.Height = 40;
            dgvEvents.ColumnHeadersHeight = 45;
        }

        private void StyleButtons()
        {
            StyleIconButton(btnSearch, Color.FromArgb(52, 152, 219));
            StyleIconButton(btnClearFilters, Color.FromArgb(149, 165, 166));
        }

        private void StyleIconButton(Button btn, Color color)
        {
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;

            Color originalColor = btn.BackColor;
            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(originalColor, 0.2f);
            btn.MouseLeave += (s, e) => btn.BackColor = originalColor;
        }

        private void StyleRecommendationsPanel()
        {
            panelRecommendations.Controls.Clear();
            panelRecommendations.BackColor = Color.White;
            panelRecommendations.Padding = new Padding(0);
            panelRecommendations.AutoScroll = false;

            Panel titlePanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 15, 20, 10)
            };

            IconPictureBox recIcon = new IconPictureBox
            {
                IconChar = IconChar.Star,
                IconColor = Color.FromArgb(241, 196, 15),
                IconSize = 28,
                Size = new Size(32, 32),
                Location = new Point(20, 14)
            };

            Label recTitle = new Label
            {
                Text = "Recommended for You",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(60, 17)
            };

            titlePanel.Controls.AddRange(new Control[] { recIcon, recTitle });
            panelRecommendations.Controls.Add(titlePanel);

            // Create scrollable content panel
            recommendationsContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoScroll = true,
                Padding = new Padding(15, 5, 15, 15)
            };
            panelRecommendations.Controls.Add(recommendationsContentPanel);

            lstRecommendations.Visible = false;
            lblRecommendations.Visible = false;
        }

        private void StyleHistoryPanel()
        {
            panelHistory.Controls.Clear();
            panelHistory.BackColor = Color.White;
            panelHistory.Padding = new Padding(0);
            panelHistory.AutoScroll = false;

            Panel titlePanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 15, 20, 10)
            };

            IconPictureBox histIcon = new IconPictureBox
            {
                IconChar = IconChar.History,
                IconColor = Color.FromArgb(155, 89, 182),
                IconSize = 28,
                Size = new Size(32, 32),
                Location = new Point(20, 14)
            };

            Label histTitle = new Label
            {
                Text = "Recent Searches",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(60, 17)
            };

            titlePanel.Controls.AddRange(new Control[] { histIcon, histTitle });
            panelHistory.Controls.Add(titlePanel);

            // Create scrollable content panel
            historyContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoScroll = true,
                Padding = new Padding(15, 5, 15, 15)
            };
            panelHistory.Controls.Add(historyContentPanel);

            lstSearchHistory.Visible = false;
            lblSearchHistory.Visible = false;
        }

        private void UpdateRecommendations()
        {
            recommendationsContentPanel.Controls.Clear();
            recommendationsContentPanel.SuspendLayout();

            var recommendations = new List<EventInfo>();

            var highPriorityEvents = priorityEvents.Where(e => e.Priority == 1 && e.Date >= DateTime.Today).Take(2);
            recommendations.AddRange(highPriorityEvents);

            if (userPreferredCategories.Any())
            {
                var mostAccessedCategory = categoryAccessCount
                    .Where(kv => userPreferredCategories.Contains(kv.Key))
                    .OrderByDescending(kv => kv.Value)
                    .FirstOrDefault().Key;

                if (!string.IsNullOrEmpty(mostAccessedCategory) && categoryLookup.ContainsKey(mostAccessedCategory))
                {
                    var categoryEvents = categoryLookup[mostAccessedCategory]
                        .Where(e => e.Date >= DateTime.Today && !recommendations.Contains(e))
                        .OrderBy(e => e.Date)
                        .Take(2);
                    recommendations.AddRange(categoryEvents);
                }
            }

            if (recentlyViewedStack.Any())
            {
                var recentCategory = recentlyViewedStack.Peek().Category;
                if (categoryLookup.ContainsKey(recentCategory))
                {
                    var similarEvents = categoryLookup[recentCategory]
                        .Where(e => e.Date >= DateTime.Today && !recommendations.Contains(e))
                        .OrderBy(e => e.Date)
                        .Take(2);
                    recommendations.AddRange(similarEvents);
                }
            }

            if (recommendations.Count < 5)
            {
                var upcomingEvents = eventsByDate
                    .Where(kv => kv.Key >= DateTime.Today)
                    .SelectMany(kv => kv.Value)
                    .Where(e => !recommendations.Contains(e))
                    .OrderBy(e => e.Date)
                    .Take(5 - recommendations.Count);
                recommendations.AddRange(upcomingEvents);
            }

            int yPos = 5;

            if (!recommendations.Any())
            {
                Label emptyLabel = new Label
                {
                    Text = "Start searching to get\npersonalized suggestions!",
                    Font = new Font("Segoe UI", 11, FontStyle.Italic),
                    ForeColor = Color.FromArgb(127, 140, 141),
                    AutoSize = false,
                    Size = new Size(recommendationsContentPanel.Width - 30, 80),
                    Location = new Point(10, yPos + 30),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                recommendationsContentPanel.Controls.Add(emptyLabel);
            }
            else
            {
                foreach (var rec in recommendations.Take(5))
                {
                    Panel recItem = CreateRecommendationCard(rec);
                    recItem.Location = new Point(0, yPos);
                    recommendationsContentPanel.Controls.Add(recItem);
                    yPos += recItem.Height + 15;
                }
            }

            recommendationsContentPanel.ResumeLayout();
        }

        private Panel CreateRecommendationCard(EventInfo ev)
        {
            Panel card = new Panel
            {
                Size = new Size(recommendationsContentPanel.Width - 30, 115),
                BackColor = Color.FromArgb(250, 250, 250),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 0, 15)
            };

            card.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);
                using (GraphicsPath path = GetRoundedRectangle(rect, 10))
                {
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(250, 250, 250)))
                    {
                        g.FillPath(brush, path);
                    }
                    using (Pen pen = new Pen(Color.FromArgb(230, 230, 230), 1))
                    {
                        g.DrawPath(pen, path);
                    }
                }

                Color priorityColor = ev.Priority == 1 ? Color.FromArgb(231, 76, 60) :
                                     ev.Priority == 2 ? Color.FromArgb(241, 196, 15) :
                                     Color.FromArgb(46, 204, 113);
                using (SolidBrush priBrush = new SolidBrush(priorityColor))
                {
                    g.FillRectangle(priBrush, 0, 0, 5, card.Height);
                }
            };

            string priorityText = ev.Priority == 1 ? "HIGH" : ev.Priority == 2 ? "MED" : "LOW";
            Color badgeColor = ev.Priority == 1 ? Color.FromArgb(231, 76, 60) :
                               ev.Priority == 2 ? Color.FromArgb(241, 196, 15) :
                               Color.FromArgb(46, 204, 113);

            Label priorityBadge = new Label
            {
                Text = priorityText,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = badgeColor,
                AutoSize = false,
                Size = new Size(50, 22),
                Location = new Point(20, 15),
                TextAlign = ContentAlignment.MiddleCenter
            };
            priorityBadge.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = GetRoundedRectangle(new Rectangle(0, 0, priorityBadge.Width - 1, priorityBadge.Height - 1), 5))
                {
                    using (SolidBrush brush = new SolidBrush(badgeColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
            };

            Label titleLabel = new Label
            {
                Text = ev.Title,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = false,
                Size = new Size(card.Width - 45, 45),
                Location = new Point(20, 45),
                TextAlign = ContentAlignment.TopLeft
            };

            IconPictureBox calIcon = new IconPictureBox
            {
                IconChar = IconChar.Calendar,
                IconColor = Color.FromArgb(127, 140, 141),
                IconSize = 16,
                Size = new Size(22, 22),
                Location = new Point(20, 88)
            };

            Label dateLabel = new Label
            {
                Text = $"{ev.Date:MMM dd, yyyy} • {ev.Category}",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = true,
                Location = new Point(46, 90)
            };

            card.Controls.AddRange(new Control[] { priorityBadge, titleLabel, calIcon, dateLabel });

            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(245, 245, 245);
            card.MouseLeave += (s, e) => card.BackColor = Color.FromArgb(250, 250, 250);

            card.Click += (s, e) =>
            {
                recentlyViewedStack.Push(ev);
                categoryAccessCount[ev.Category]++;

                string priorityTxt = ev.Priority == 1 ? "HIGH PRIORITY" :
                                    ev.Priority == 2 ? "MEDIUM PRIORITY" : "LOW PRIORITY";

                MessageBox.Show(
                    $"{ev.Title}\n\n" +
                    $"Date: {ev.Date:dddd, MMMM dd, yyyy}\n" +
                    $"Category: {ev.Category}\n" +
                    $"Priority: {priorityTxt}\n\n" +
                    $"Description:\n{ev.Description}\n\n" +
                    $"{categoryDescriptions.GetValueOrDefault(ev.Category, "")}",
                    "Event Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                UpdateRecommendations();
                UpdateStatistics();
            };

            return card;
        }

        private void UpdateSearchHistoryDisplay()
        {
            historyContentPanel.Controls.Clear();
            historyContentPanel.SuspendLayout();

            int yPos = 5;

            if (!searchHistoryQueue.Any())
            {
                Label emptyLabel = new Label
                {
                    Text = "No recent searches yet",
                    Font = new Font("Segoe UI", 11, FontStyle.Italic),
                    ForeColor = Color.FromArgb(127, 140, 141),
                    AutoSize = true,
                    Location = new Point(10, yPos + 20)
                };
                historyContentPanel.Controls.Add(emptyLabel);
            }
            else
            {
                foreach (var search in searchHistoryQueue.Reverse())
                {
                    Panel histItem = CreateHistoryItem(search);
                    histItem.Location = new Point(0, yPos);
                    historyContentPanel.Controls.Add(histItem);
                    yPos += histItem.Height + 10;
                }
            }

            historyContentPanel.ResumeLayout();
        }

        private Panel CreateHistoryItem(string searchText)
        {
            Panel item = new Panel
            {
                Size = new Size(historyContentPanel.Width - 30, 42),
                BackColor = Color.FromArgb(248, 249, 250),
                Margin = new Padding(0, 0, 0, 10)
            };

            item.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(0, 0, item.Width - 1, item.Height - 1);
                using (GraphicsPath path = GetRoundedRectangle(rect, 8))
                {
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(248, 249, 250)))
                    {
                        g.FillPath(brush, path);
                    }
                }
            };

            IconPictureBox searchIcon = new IconPictureBox
            {
                IconChar = IconChar.Search,
                IconColor = Color.FromArgb(155, 89, 182),
                IconSize = 16,
                Size = new Size(20, 20),
                Location = new Point(12, 11)
            };

            Label searchLabel = new Label
            {
                Text = searchText,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = false,
                Size = new Size(item.Width - 45, 25),
                Location = new Point(38, 11),
                TextAlign = ContentAlignment.MiddleLeft
            };

            item.Controls.AddRange(new Control[] { searchIcon, searchLabel });
            return item;
        }

        private void InitializeDataStructures()
        {
            eventsByDate = new SortedDictionary<DateTime, List<EventInfo>>();
            categoryAccessCount = new SortedDictionary<string, int>();
            categoryLookup = new Dictionary<string, List<EventInfo>>(StringComparer.OrdinalIgnoreCase);
            categoryDescriptions = new Dictionary<string, string>();
            uniqueCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            eventDates = new HashSet<DateTime>();
            recentlyViewedStack = new Stack<EventInfo>();
            searchHistoryQueue = new Queue<string>();
            priorityEvents = new SortedSet<EventInfo>();
            userSearchPatterns = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            userPreferredCategories = new List<string>();

            categoryDescriptions.Add("Community", "Community engagement and social events");
            categoryDescriptions.Add("Roads", "Road maintenance and infrastructure updates");
            categoryDescriptions.Add("Utilities", "Water, electricity, and utility services");
            categoryDescriptions.Add("Public Safety", "Safety, security and emergency services");
            categoryDescriptions.Add("Health", "Health services and medical announcements");
            categoryDescriptions.Add("Sports", "Sports events and recreational activities");
            categoryDescriptions.Add("Environment", "Environmental initiatives and conservation");
            categoryDescriptions.Add("Economy", "Economic development and business events");
        }

        private void SetupDataGridView()
        {
            dgvEvents.AutoGenerateColumns = false;
            dgvEvents.Columns.Clear();

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Priority",
                HeaderText = "Priority",
                DataPropertyName = "Priority",
                Width = 100
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Title",
                HeaderText = "Event Title",
                DataPropertyName = "Title",
                Width = 250
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Category",
                HeaderText = "Category",
                DataPropertyName = "Category",
                Width = 120
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Date",
                HeaderText = "Date",
                DataPropertyName = "DateFormatted",
                Width = 150
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvEvents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEvents.ReadOnly = true;
            dgvEvents.AllowUserToAddRows = false;
            dgvEvents.RowHeadersVisible = false;
        }

        private void LoadSampleEvents()
        {
            var samples = new List<EventInfo>
            {
                new EventInfo { Title="Emergency Water Shutdown", Category="Utilities", Date=DateTime.Today.AddDays(1),
                    Description="Emergency maintenance on main water line. Area affected: Downtown. Duration: 6-8 hours.", Priority=1 },
                new EventInfo { Title="Road Closure - Main Street", Category="Roads", Date=DateTime.Today.AddDays(2),
                    Description="Complete closure for bridge repairs. Detour routes available via Oak Ave.", Priority=1 },
                new EventInfo { Title="Community Safety Alert", Category="Public Safety", Date=DateTime.Today,
                    Description="Increased security presence due to recent incidents. Residents advised to be vigilant.", Priority=1 },
                new EventInfo { Title="Beach Cleanup Day", Category="Community", Date=DateTime.Today.AddDays(5),
                    Description="Join us for our monthly beach cleanup. Equipment provided. Meet at Pier 12 at 8AM.", Priority=2 },
                new EventInfo { Title="Local Farmers Market", Category="Economy", Date=DateTime.Today.AddDays(3),
                    Description="Fresh produce and local crafts. Every Saturday 7AM-2PM at City Square.", Priority=2 },
                new EventInfo { Title="Youth Soccer Tournament", Category="Sports", Date=DateTime.Today.AddDays(7),
                    Description="Annual under-12 soccer tournament. Free entry for spectators. Snacks available.", Priority=2 },
                new EventInfo { Title="Tree Planting Initiative", Category="Environment", Date=DateTime.Today.AddDays(10),
                    Description="Help green our city! We aim to plant 500 trees. Volunteers needed.", Priority=2 },
                new EventInfo { Title="Free Health Screening", Category="Health", Date=DateTime.Today.AddDays(6),
                    Description="Free blood pressure and diabetes screening at Community Center. No appointment needed.", Priority=2 },
                new EventInfo { Title="City Council Meeting", Category="Community", Date=DateTime.Today.AddDays(14),
                    Description="Monthly public meeting. Public comments welcome. Live streamed online.", Priority=3 },
                new EventInfo { Title="Park Renovation Update", Category="Community", Date=DateTime.Today.AddDays(20),
                    Description="Progress update on Central Park renovation project. Completion expected Q3.", Priority=3 },
                new EventInfo { Title="Business Development Workshop", Category="Economy", Date=DateTime.Today.AddDays(12),
                    Description="Workshop for small business owners. Topics: Digital marketing and social media.", Priority=3 },
                new EventInfo { Title="Library Book Sale", Category="Community", Date=DateTime.Today.AddDays(15),
                    Description="Annual book sale. Thousands of books at great prices. Proceeds support library programs.", Priority=3 },
                new EventInfo { Title="Recycling Program Expansion", Category="Environment", Date=DateTime.Today.AddDays(25),
                    Description="New recycling bins to be distributed. Learn about expanded recycling options.", Priority=3 },
                new EventInfo { Title="Street Light Upgrades", Category="Utilities", Date=DateTime.Today.AddDays(30),
                    Description="LED street light installation begins in Phase 1 areas. Energy efficient and brighter.", Priority=3 },
            };

            foreach (var ev in samples)
            {
                if (!eventsByDate.ContainsKey(ev.Date.Date))
                    eventsByDate[ev.Date.Date] = new List<EventInfo>();
                eventsByDate[ev.Date.Date].Add(ev);

                if (!categoryLookup.ContainsKey(ev.Category))
                    categoryLookup[ev.Category] = new List<EventInfo>();
                categoryLookup[ev.Category].Add(ev);

                uniqueCategories.Add(ev.Category);
                eventDates.Add(ev.Date.Date);
                priorityEvents.Add(ev);

                if (!categoryAccessCount.ContainsKey(ev.Category))
                    categoryAccessCount[ev.Category] = 0;
            }
        }

        private void PopulateFilters()
        {
            cmbCategoryFilter.Items.Clear();
            cmbCategoryFilter.Items.Add("All Categories");

            foreach (var cat in categoryAccessCount.Keys.OrderBy(k => k))
            {
                cmbCategoryFilter.Items.Add(cat);
            }

            cmbCategoryFilter.SelectedIndex = 0;
            chkDateFilter.Checked = false;
            dtpDateFilter.Value = DateTime.Today;
            dtpDateFilter.Enabled = false;
        }

        private void DisplayAllEvents()
        {
            var allEvents = eventsByDate.SelectMany(kv => kv.Value)
                                       .OrderBy(e => e.Priority)
                                       .ThenBy(e => e.Date)
                                       .ToList();

            DisplayEventsInGrid(allEvents);
            UpdateRecommendations();
        }

        private void DisplayEventsInGrid(List<EventInfo> events)
        {
            try
            {
                var displayList = events.Select(e => new
                {
                    Priority = e.Priority == 1 ? "HIGH" : e.Priority == 2 ? "MEDIUM" : "LOW",
                    Title = e.Title,
                    Category = e.Category,
                    DateFormatted = e.Date.ToString("ddd, MMM dd yyyy"),
                    Description = e.Description,
                    EventObject = e
                }).ToList();

                dgvEvents.DataSource = null;
                dgvEvents.DataSource = displayList;

                if (dgvEvents.Columns.Contains("EventObject"))
                {
                    dgvEvents.Columns["EventObject"].Visible = false;
                }

                foreach (DataGridViewRow row in dgvEvents.Rows)
                {
                    string priority = row.Cells["Priority"].Value?.ToString();
                    if (priority == "HIGH")
                    {
                        row.Cells["Priority"].Style.ForeColor = Color.FromArgb(231, 76, 60);
                        row.Cells["Priority"].Style.Font = new Font(dgvEvents.Font, FontStyle.Bold);
                    }
                    else if (priority == "MEDIUM")
                    {
                        row.Cells["Priority"].Style.ForeColor = Color.FromArgb(241, 196, 15);
                        row.Cells["Priority"].Style.Font = new Font(dgvEvents.Font, FontStyle.Bold);
                    }
                    else
                    {
                        row.Cells["Priority"].Style.ForeColor = Color.FromArgb(46, 204, 113);
                    }
                }

                dgvEvents.ClearSelection();
                UpdateProgressBar(events.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying events: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateProgressBar(int eventCount)
        {
            int totalEvents = eventsByDate.SelectMany(kv => kv.Value).Count();
            progressBar.Maximum = totalEvents > 0 ? totalEvents : 1;
            progressBar.Value = Math.Min(eventCount, totalEvents);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            string selectedCategory = cmbCategoryFilter.SelectedItem?.ToString() ?? "All Categories";
            DateTime? dateFilter = chkDateFilter.Checked ? dtpDateFilter.Value.Date : (DateTime?)null;

            if (!string.IsNullOrEmpty(keyword))
            {
                string searchTerm = keyword.ToLower();
                searchHistoryQueue.Enqueue($"{DateTime.Now:HH:mm} - \"{keyword}\"");
                if (searchHistoryQueue.Count > 10)
                    searchHistoryQueue.Dequeue();

                if (userSearchPatterns.ContainsKey(searchTerm))
                    userSearchPatterns[searchTerm]++;
                else
                    userSearchPatterns[searchTerm] = 1;
            }

            if (selectedCategory != "All Categories")
            {
                categoryAccessCount[selectedCategory]++;

                if (!userPreferredCategories.Contains(selectedCategory))
                    userPreferredCategories.Add(selectedCategory);

                searchHistoryQueue.Enqueue($"{DateTime.Now:HH:mm} - Category: {selectedCategory}");
                if (searchHistoryQueue.Count > 10)
                    searchHistoryQueue.Dequeue();
            }

            UpdateSearchHistoryDisplay();

            IEnumerable<EventInfo> results;

            if (selectedCategory != "All Categories" && categoryLookup.ContainsKey(selectedCategory))
            {
                results = categoryLookup[selectedCategory];
            }
            else
            {
                results = eventsByDate.SelectMany(kv => kv.Value);
            }

            if (dateFilter.HasValue && eventDates.Contains(dateFilter.Value))
            {
                results = results.Where(ev => ev.Date.Date == dateFilter.Value);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                string lowered = keyword.ToLower();
                results = results.Where(ev =>
                    ev.Title.ToLower().Contains(lowered) ||
                    ev.Description.ToLower().Contains(lowered) ||
                    ev.Category.ToLower().Contains(lowered));
            }

            var resultList = results.OrderBy(e => e.Priority).ThenBy(e => e.Date).ToList();
            DisplayEventsInGrid(resultList);
            UpdateRecommendations();
            UpdateStatistics();

            MessageBox.Show($"Found {resultList.Count} event(s) matching your criteria.",
                "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateStatistics()
        {
            int totalEvents = eventsByDate.SelectMany(kv => kv.Value).Count();
            int uniqueCategoryCount = uniqueCategories.Count;
            int upcomingEvents = eventsByDate.Where(kv => kv.Key >= DateTime.Today)
                                            .SelectMany(kv => kv.Value).Count();
            int searchCount = searchHistoryQueue.Count;

            lblStats.Text = $"Total Events: {totalEvents} | Categories: {uniqueCategoryCount} | " +
                          $"Upcoming: {upcomingEvents} | Searches: {searchCount}";
            lblStats.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblStats.ForeColor = Color.FromArgb(66, 66, 66);
        }

        private void dgvEvents_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                string title = dgvEvents.Rows[e.RowIndex].Cells["Title"].Value.ToString();
                var ev = eventsByDate.SelectMany(kv => kv.Value).FirstOrDefault(x => x.Title == title);

                if (ev == null) return;

                recentlyViewedStack.Push(ev);
                categoryAccessCount[ev.Category]++;

                string priorityText = ev.Priority == 1 ? "HIGH PRIORITY" :
                                     ev.Priority == 2 ? "MEDIUM PRIORITY" : "LOW PRIORITY";

                MessageBox.Show(
                    $"{ev.Title}\n\n" +
                    $"Date: {ev.Date:dddd, MMMM dd, yyyy}\n" +
                    $"Category: {ev.Category}\n" +
                    $"Priority: {priorityText}\n\n" +
                    $"Description:\n{ev.Description}\n\n" +
                    $"{categoryDescriptions.GetValueOrDefault(ev.Category, "")}",
                    "Event Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                UpdateRecommendations();
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error viewing event: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkDateFilter_CheckedChanged(object sender, EventArgs e)
        {
            dtpDateFilter.Enabled = chkDateFilter.Checked;
        }

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbCategoryFilter.SelectedIndex = 0;
            chkDateFilter.Checked = false;
            dtpDateFilter.Value = DateTime.Today;
            DisplayAllEvents();
            UpdateStatistics();
            MessageBox.Show("All filters have been cleared.", "Filters Reset",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 main = new Form1();
            main.Show();
        }
    }
}








