using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TP2
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new frmAccueil());
        }
    }

       class frmAccueil : Form
    {
        MenuStrip menu; ToolStrip tools; ContextMenuStrip ctx;
        ToolStripMenuItem ctxBarre, ctxDesact, ctxImage;
        ToolStripMenuItem mnuBarre, mnuImage;
        Color couleurBureau = Color.FromArgb(100, 170, 240);
        Bitmap fond;

        public frmAccueil()
        {
            Text = "Accueil"; ClientSize = new Size(800, 450);
            StartPosition = FormStartPosition.CenterScreen;
            fond = CreerFond();
            BackgroundImage = fond; BackgroundImageLayout = ImageLayout.Stretch;
            BackColor = couleurBureau;

            menu = new MenuStrip();
            var mListes = new ToolStripMenuItem("Listes");
            var mDepl = new ToolStripMenuItem("Déplacement des données", null, (s, e) => new frmListes().ShowDialog());
            mDepl.ShortcutKeys = Keys.Control | Keys.D;
            var mExit = new ToolStripMenuItem("Exit", null, (s, e) => Application.Exit());
            mExit.ShortcutKeys = Keys.Control | Keys.Q;
            mListes.DropDownItems.Add(mDepl);
            mListes.DropDownItems.Add(new ToolStripSeparator());
            mListes.DropDownItems.Add(mExit);

            var mParam = new ToolStripMenuItem("Paramètres");
            mnuBarre = new ToolStripMenuItem("Afficher Barre d'outils") { CheckOnClick = true, Checked = true };
            var mDesact = new ToolStripMenuItem("Désactiver Barre D'outils");
            mnuImage = new ToolStripMenuItem("Afficher Image pour le Bureau") { CheckOnClick = true, Checked = true };
            var mCoul = new ToolStripMenuItem("Changer Couleur du Bureau");
            mnuBarre.Click += (s, e) => AfficherBarre(mnuBarre.Checked);
            mDesact.Click += (s, e) => { tools.Enabled = !tools.Enabled; mDesact.Text = tools.Enabled ? "Désactiver Barre D'outils" : "Activer Barre D'outils"; };
            mnuImage.Click += (s, e) => AfficherImage(mnuImage.Checked);
            mCoul.Click += (s, e) => ChangerCouleur();
            mParam.DropDownItems.AddRange(new ToolStripItem[] { mnuBarre, mDesact, new ToolStripSeparator(), mnuImage, mCoul });

            var mAide = new ToolStripMenuItem("?");
            var mHelp = new ToolStripMenuItem("Aide", null, (s, e) => MessageBox.Show("Menu Listes : déplacement des données entre listes.\nMenu Paramètres : barre d'outils, image et couleur du bureau.", "Aide"));
            var mAbout = new ToolStripMenuItem("A propos de TP2", null, (s, e) => MessageBox.Show("TP 2 : Les menus et les listes", "A propos"));
            mAide.DropDownItems.AddRange(new ToolStripItem[] { mHelp, mAbout });

            menu.Items.AddRange(new ToolStripItem[] { mListes, mParam, mAide });

            tools = new ToolStrip();
            tools.Items.Add(new ToolStripButton("Exit", null, (s, e) => Application.Exit()) { ToolTipText = "Quitter l'application", DisplayStyle = ToolStripItemDisplayStyle.Text });
            tools.Items.Add(new ToolStripSeparator());
            tools.Items.Add(new ToolStripButton("Listes", null, (s, e) => new frmListes().ShowDialog()) { ToolTipText = "Déplacement des données", DisplayStyle = ToolStripItemDisplayStyle.Text });
            tools.Items.Add(new ToolStripButton("Aide", null, (s, e) => mHelp.PerformClick()) { ToolTipText = "Aide", DisplayStyle = ToolStripItemDisplayStyle.Text });

            Controls.Add(tools); Controls.Add(menu); MainMenuStrip = menu;

            ctx = new ContextMenuStrip();
            ctx.Items.Add(new ToolStripMenuItem("Déplacement des données", null, (s, e) => mDepl.PerformClick()));
            ctx.Items.Add(new ToolStripSeparator());
            ctxBarre = new ToolStripMenuItem("Afficher Barre d'outils") { CheckOnClick = true, Checked = true };
            ctxBarre.Click += (s, e) => { mnuBarre.Checked = ctxBarre.Checked; AfficherBarre(ctxBarre.Checked); };
            ctxDesact = new ToolStripMenuItem("Désactiver Barre D'outils");
            ctxDesact.Click += (s, e) => { tools.Enabled = !tools.Enabled; ctxDesact.Text = tools.Enabled ? "Désactiver Barre D'outils" : "Activer Barre D'outils"; };
            ctxImage = new ToolStripMenuItem("Afficher Image pour le Bureau") { CheckOnClick = true, Checked = true };
            ctxImage.Click += (s, e) => { mnuImage.Checked = ctxImage.Checked; AfficherImage(ctxImage.Checked); };
            ctx.Items.AddRange(new ToolStripItem[] { ctxBarre, ctxDesact, ctxImage,
                new ToolStripMenuItem("Changer Couleur du Bureau", null, (s, e) => ChangerCouleur()),
                new ToolStripSeparator(),
                new ToolStripMenuItem("Aide", null, (s, e) => mHelp.PerformClick()),
                new ToolStripMenuItem("A propos de TP2", null, (s, e) => mAbout.PerformClick()),
                new ToolStripSeparator(),
                new ToolStripMenuItem("Exit", null, (s, e) => Application.Exit()) });
            ContextMenuStrip = ctx;     // equivalent de la propriete ContextMenu du TP
        }

        void AfficherBarre(bool v) { tools.Visible = v; ctxBarre.Checked = v; mnuBarre.Checked = v; }
        void AfficherImage(bool v)
        {
            BackgroundImage = v ? fond : null; ctxImage.Checked = v; mnuImage.Checked = v;
            BackColor = couleurBureau;
        }
        void ChangerCouleur()
        {
            using (var cd = new ColorDialog { Color = couleurBureau })
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    couleurBureau = cd.Color; BackColor = couleurBureau;
                    AfficherImage(false);          // la couleur n'est visible que sans image
                }
        }

        static Bitmap CreerFond()
        {
            var b = new Bitmap(400, 300);
            using (var g = Graphics.FromImage(b))
            using (var br = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, 400, 300), Color.FromArgb(210, 235, 255), Color.FromArgb(60, 150, 235), 90f))
                g.FillRectangle(br, 0, 0, 400, 300);
            return b;
        }
    }

        class frmListes : Form
    {
        // Page 1
        ListBox LstG1, LstD1; TextBox TxtN;
        // Page 2
        ListBox LstG2, LstD2;
        // Page 3
        CheckedListBox LstG3; ListBox LstD3;
        // Page 4
        ListBox LstG4, LstD4;

        static readonly string[] Donnees = { "Ali", "Sara", "Mohamed", "Salma", "Yassine", "Nour", "Khaled", "Amel" };

        public frmListes()
        {
            Text = "Listes"; ClientSize = new Size(620, 470);
            StartPosition = FormStartPosition.CenterParent;
            var tabs = new TabControl { Dock = DockStyle.Fill };
            var p1 = new TabPage("1 er Cas"); var p2 = new TabPage("2 ème Cas");
            var p3 = new TabPage("3 ème Cas"); var p4 = new TabPage("4 ème Cas");
            tabs.TabPages.AddRange(new[] { p1, p2, p3, p4 });
            Controls.Add(tabs);
            Page1(p1); Page2(p2); Page3(p3); Page4(p4);
        }

        // --------- helpers ---------
        static Button B(Control p, string t, int x, int y, int w = 50, int h = 32)
        { var b = new Button { Text = t, Left = x, Top = y, Width = w, Height = h }; p.Controls.Add(b); return b; }
        static ListBox L(Control p, int x, int y, SelectionMode m = SelectionMode.One)
        { var l = new ListBox { Left = x, Top = y, Width = 170, Height = 200, SelectionMode = m }; p.Controls.Add(l); return l; }
        static void Remplir(ListBox l) { l.Items.AddRange(Donnees); }

        void Page1(TabPage p)
        {
            LstG1 = L(p, 25, 20); LstD1 = L(p, 400, 20);
            var bGD = B(p, ">", 230, 30); var bTGD = B(p, ">>", 230, 75);
            var bDG = B(p, "<", 230, 120); var bTDG = B(p, "<<", 230, 165);
            TxtN = new TextBox { Left = 25, Top = 240, Width = 90 }; p.Controls.Add(TxtN);
            var bAj = B(p, "Ajouter", 125, 238, 80, 26);
            var bSupp = B(p, "Supprimer", 25, 275, 90, 26);
            var bVid = B(p, "Vider", 125, 275, 80, 26);
            Remplir(LstG1);

            bAj.Click += (s, e) =>
            {
                if (TxtN.Text.Trim() != "") LstG1.Items.Add(TxtN.Text.Trim());
                else MessageBox.Show("Il faut saisir une valeur");
                TxtN.Text = ""; TxtN.Focus();
            };
            bSupp.Click += (s, e) =>
            {
                if (LstG1.SelectedIndex != -1) LstG1.Items.RemoveAt(LstG1.SelectedIndex);
                else MessageBox.Show("Aucun élément n'est sélectionné");
            };
            bVid.Click += (s, e) => { LstG1.Items.Clear(); LstD1.Items.Clear(); };
            bGD.Click += (s, e) => Deplacer1(LstG1, LstD1);
            bDG.Click += (s, e) => Deplacer1(LstD1, LstG1);
            bTGD.Click += (s, e) => { foreach (object x in LstG1.Items) LstD1.Items.Add(x); LstG1.Items.Clear(); };
            bTDG.Click += (s, e) => { foreach (object x in LstD1.Items) LstG1.Items.Add(x); LstD1.Items.Clear(); };
        }
        static void Deplacer1(ListBox src, ListBox dst)
        {
            if (src.SelectedIndex != -1)
            {
                dst.Items.Add(src.SelectedItem);
                src.Items.RemoveAt(src.SelectedIndex);
            }
            else MessageBox.Show("Aucun élément n'est sélectionné");
        }

        void Page2(TabPage p)
        {
            LstG2 = L(p, 25, 20, SelectionMode.MultiExtended);
            LstD2 = L(p, 400, 20, SelectionMode.MultiExtended);
            var bGD = B(p, ">", 230, 30); var bTGD = B(p, ">>", 230, 75);
            var bDG = B(p, "<", 230, 120); var bTDG = B(p, "<<", 230, 165);
            var bSupp = B(p, "Supprimer", 25, 240, 90, 26);
            Remplir(LstG2);

            bGD.Click += (s, e) => DeplacerSel(LstG2, LstD2);
            bDG.Click += (s, e) => DeplacerSel(LstD2, LstG2);
            bTGD.Click += (s, e) => { foreach (object x in LstG2.Items) LstD2.Items.Add(x); LstG2.Items.Clear(); };
            bTDG.Click += (s, e) => { foreach (object x in LstD2.Items) LstG2.Items.Add(x); LstD2.Items.Clear(); };
            bSupp.Click += (s, e) =>
            {
                List<object> coll = new List<object>();
                foreach (object x in LstG2.SelectedItems) coll.Add(x);
                foreach (object y in coll) LstG2.Items.Remove(y);
            };
        }
        static void DeplacerSel(ListBox src, ListBox dst)
        {
            List<object> coll = new List<object>();
            foreach (object x in src.SelectedItems) { coll.Add(x); dst.Items.Add(x); }
            foreach (object y in coll) src.Items.Remove(y);
        }

        void Page3(TabPage p)
        {
            LstG3 = new CheckedListBox { Left = 25, Top = 20, Width = 170, Height = 200, CheckOnClick = true }; p.Controls.Add(LstG3);
            LstD3 = L(p, 400, 20);
            var bGD = B(p, ">", 230, 30); var bTGD = B(p, ">>", 230, 75);
            LstG3.Items.AddRange(Donnees);

            bGD.Click += (s, e) =>
            {
                List<object> coll = new List<object>();
                foreach (object x in LstG3.CheckedItems) { coll.Add(x); LstD3.Items.Add(x); }
                foreach (object y in coll) LstG3.Items.Remove(y);
            };
            bTGD.Click += (s, e) =>
            {
                foreach (object x in LstG3.Items) LstD3.Items.Add(x);
                LstG3.Items.Clear();
            };
        }

        void Page4(TabPage p)
        {
            LstG4 = L(p, 25, 20); LstD4 = L(p, 400, 20);
            LstG4.AllowDrop = true; LstD4.AllowDrop = true;
            Remplir(LstG4);
            UI_Info(p);

            // glisser depuis la liste gauche vers la droite
            LstG4.MouseDown += (s, e) =>
            {
                int i = LstG4.IndexFromPoint(e.Location);
                if (i != ListBox.NoMatches) { LstG4.SelectedIndex = i; LstG4.DoDragDrop(new DragData(LstG4, LstG4.SelectedItem), DragDropEffects.Move); }
            };
            LstD4.MouseDown += (s, e) =>
            {
                int i = LstD4.IndexFromPoint(e.Location);
                if (i != ListBox.NoMatches) { LstD4.SelectedIndex = i; LstD4.DoDragDrop(new DragData(LstD4, LstD4.SelectedItem), DragDropEffects.Move); }
            };
            LstG4.DragOver += (s, e) => e.Effect = DragDropEffects.Move;
            LstD4.DragOver += (s, e) => e.Effect = DragDropEffects.Move;
            LstG4.DragDrop += (s, e) => Deposer(e, LstG4);
            LstD4.DragDrop += (s, e) => Deposer(e, LstD4);
        }
        static void UI_Info(Control p)
            => p.Controls.Add(new Label { Text = "Glissez un élément d'une liste vers l'autre", Left = 215, Top = 100, Width = 180, Height = 50, TextAlign = ContentAlignment.MiddleCenter });

        class DragData { public ListBox Source; public object Item; public DragData(ListBox s, object i) { Source = s; Item = i; } }

        static void Deposer(DragEventArgs e, ListBox cible)
        {
            var d = (DragData)e.Data.GetData(typeof(DragData));
            if (d == null || d.Source == cible) return;       // meme liste : rien a faire
            cible.Items.Add(d.Item);
            d.Source.Items.Remove(d.Item);
        }
    }
}