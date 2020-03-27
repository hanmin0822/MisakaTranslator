/*
 *Namespace         MisakaTranslator
 *Class             TranslateFrmSettingsForm
 *Description       翻译窗口字体和窗口设置的窗口
 */


using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MisakaTranslator
{
    public partial class TranslateFrmSettingsForm : MaterialForm
    {
        private GameTranslateForm gtlf;
        private GameTranslateBackForm gtlbf;
        List<string> FontList;
        List<string> SetPartList;
        private int lastPartChoose;
        private bool openflag;

        public TranslateFrmSettingsForm(GameTranslateForm formTop, GameTranslateBackForm formBack)
        {
            openflag = true;
            InitializeComponent();
            gtlf = formTop;
            gtlbf = formBack;
        }

        private void TranslateFrmSettingsForm_Load(object sender, EventArgs e)
        {
            FontList = new List<string>();

            System.Drawing.Text.InstalledFontCollection fonts = new System.Drawing.Text.InstalledFontCollection();
            foreach (System.Drawing.FontFamily family in fonts.Families)
            {
                FontList.Add(family.Name);
            }

            FontsCombox.DropDownStyle = ComboBoxStyle.DropDownList;
            FontsCombox.DataSource = FontList;

            SetPartList = new List<string>();
            SetPartList.Add("原文");
            SetPartList.Add("第一翻译源");
            SetPartList.Add("第二翻译源");

            PartCombox.DropDownStyle = ComboBoxStyle.DropDownList;
            PartCombox.DataSource = SetPartList;


            lastPartChoose = 0;
            PartCombox.SelectedIndex = 0;

            OpacityTrackBar.Value = int.Parse(Common.settings.TF_Opacity);
            
            ColorRTrackBar.Value = int.Parse(Common.settings.TF_srcTextColorR);
            ColorGTrackBar.Value = int.Parse(Common.settings.TF_srcTextColorG);
            ColorBTrackBar.Value = int.Parse(Common.settings.TF_srcTextColorB);
            FontsCombox.Text = Common.settings.TF_srcTextFont;
            TextSizeBox.Num = int.Parse(Common.settings.TF_srcTextSize);

        }

        private void OpacityTrackBar_ValueChanged(object sender, EventArgs e)
        {
            double FormOpacity = OpacityTrackBar.Value / 100;
            gtlbf.BeginInvoke(new Action(() => { gtlbf.Opacity = FormOpacity; }));
        }

        private void FontsCombox_SelectedIndexChanged(object sender, EventArgs e)
        {
            gtlf.BeginInvoke(new Action(() => { gtlf.SetTextFont(FontsCombox.Text, (int)TextSizeBox.Num, PartCombox.SelectedIndex); }));
        }

        private void TextSizeBox_NumChanged(object sender, EventArgs e)
        {

            gtlf.BeginInvoke(new Action(() => { gtlf.SetTextFont(FontsCombox.Text, (int)TextSizeBox.Num, PartCombox.SelectedIndex); }));
        }

        private void TranslateFrmSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Common.settings.TF_Opacity = OpacityTrackBar.Value.ToString();

            if (lastPartChoose == 0)
            {
                Common.settings.TF_srcTextColorR = ColorRTrackBar.Value.ToString();
                Common.settings.TF_srcTextColorG = ColorGTrackBar.Value.ToString();
                Common.settings.TF_srcTextColorB = ColorBTrackBar.Value.ToString();
                Common.settings.TF_srcTextFont = FontsCombox.Text;
                Common.settings.TF_srcTextSize = TextSizeBox.Num.ToString();
            }
            else if (lastPartChoose == 1)
            {
                Common.settings.TF_firstTransTextColorR = ColorRTrackBar.Value.ToString();
                Common.settings.TF_firstTransTextColorG = ColorGTrackBar.Value.ToString();
                Common.settings.TF_firstTransTextColorB = ColorBTrackBar.Value.ToString();
                Common.settings.TF_firstTransTextFont = FontsCombox.Text;
                Common.settings.TF_firstTransTextSize = TextSizeBox.Num.ToString();
            }
            else if (lastPartChoose == 2)
            {
                Common.settings.TF_secondTransTextColorR = ColorRTrackBar.Value.ToString();
                Common.settings.TF_secondTransTextColorG = ColorGTrackBar.Value.ToString();
                Common.settings.TF_secondTransTextColorB = ColorBTrackBar.Value.ToString();
                Common.settings.TF_secondTransTextFont = FontsCombox.Text;
                Common.settings.TF_secondTransTextSize = TextSizeBox.Num.ToString();
            }

        }

        private void PartCombox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (openflag != true)
            {
                if (lastPartChoose == 0)
                {
                    Common.settings.TF_srcTextColorR = ColorRTrackBar.Value.ToString();
                    Common.settings.TF_srcTextColorG = ColorGTrackBar.Value.ToString();
                    Common.settings.TF_srcTextColorB = ColorBTrackBar.Value.ToString();
                    Common.settings.TF_srcTextFont = FontsCombox.Text;
                    Common.settings.TF_srcTextSize = TextSizeBox.Num.ToString();
                }
                else if (lastPartChoose == 1)
                {
                    Common.settings.TF_firstTransTextColorR = ColorRTrackBar.Value.ToString();
                    Common.settings.TF_firstTransTextColorG = ColorGTrackBar.Value.ToString();
                    Common.settings.TF_firstTransTextColorB = ColorBTrackBar.Value.ToString();
                    Common.settings.TF_firstTransTextFont = FontsCombox.Text;
                    Common.settings.TF_firstTransTextSize = TextSizeBox.Num.ToString();
                }
                else if (lastPartChoose == 2)
                {
                    Common.settings.TF_secondTransTextColorR = ColorRTrackBar.Value.ToString();
                    Common.settings.TF_secondTransTextColorG = ColorGTrackBar.Value.ToString();
                    Common.settings.TF_secondTransTextColorB = ColorBTrackBar.Value.ToString();
                    Common.settings.TF_secondTransTextFont = FontsCombox.Text;
                    Common.settings.TF_secondTransTextSize = TextSizeBox.Num.ToString();
                }

                if (PartCombox.SelectedIndex == 0)
                {
                    ColorRTrackBar.Value = int.Parse(Common.settings.TF_srcTextColorR);
                    ColorGTrackBar.Value = int.Parse(Common.settings.TF_srcTextColorG);
                    ColorBTrackBar.Value = int.Parse(Common.settings.TF_srcTextColorB);
                    FontsCombox.Text = Common.settings.TF_srcTextFont;
                    TextSizeBox.Num = int.Parse(Common.settings.TF_srcTextSize);
                }
                else if (PartCombox.SelectedIndex == 1)
                {
                    ColorRTrackBar.Value = int.Parse(Common.settings.TF_firstTransTextColorR);
                    ColorGTrackBar.Value = int.Parse(Common.settings.TF_firstTransTextColorG);
                    ColorBTrackBar.Value = int.Parse(Common.settings.TF_firstTransTextColorB);
                    FontsCombox.Text = Common.settings.TF_firstTransTextFont;
                    TextSizeBox.Num = int.Parse(Common.settings.TF_firstTransTextSize);
                }
                else if (PartCombox.SelectedIndex == 2)
                {
                    ColorRTrackBar.Value = int.Parse(Common.settings.TF_secondTransTextColorR);
                    ColorGTrackBar.Value = int.Parse(Common.settings.TF_secondTransTextColorG);
                    ColorBTrackBar.Value = int.Parse(Common.settings.TF_secondTransTextColorB);
                    FontsCombox.Text = Common.settings.TF_secondTransTextFont;
                    TextSizeBox.Num = int.Parse(Common.settings.TF_secondTransTextSize);
                }

                lastPartChoose = PartCombox.SelectedIndex;

            }
            else
            {
                openflag = false;
            }


        }

        private void ColorRTrackBar_ValueChanged(object sender, EventArgs e)
        {
            gtlf.BeginInvoke(new Action(() => { gtlf.SetTextColor((int)ColorRTrackBar.Value, (int)ColorGTrackBar.Value, (int)ColorBTrackBar.Value, PartCombox.SelectedIndex); }));
        }

        private void ColorGTrackBar_ValueChanged(object sender, EventArgs e)
        {
            gtlf.BeginInvoke(new Action(() => { gtlf.SetTextColor((int)ColorRTrackBar.Value, (int)ColorGTrackBar.Value, (int)ColorBTrackBar.Value, PartCombox.SelectedIndex); }));
        }

        private void ColorBTrackBar_ValueChanged(object sender, EventArgs e)
        {
            gtlf.BeginInvoke(new Action(() => { gtlf.SetTextColor((int)ColorRTrackBar.Value, (int)ColorGTrackBar.Value, (int)ColorBTrackBar.Value, PartCombox.SelectedIndex); }));
        }

        private void ChooseColorBtn_BtnClick(object sender, EventArgs e)
        {
            WinColorDialog.ShowDialog();
            gtlbf.BeginInvoke(new Action(() => { gtlbf.BackColor = this.WinColorDialog.Color; }));
            Common.settings.TF_BackColor = this.WinColorDialog.Color.ToArgb().ToString();
        }
    }
}
