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
            
            OpacityTrackBar.Value = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "opacity", "50"));
            
            FontsCombox.Text = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextFont", "微软雅黑");
            TextSizeBox.Num = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextsize", "12"));
            ColorRTrackBar.Value = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextColorR", "0"));
            ColorGTrackBar.Value = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextColorG", "0"));
            ColorBTrackBar.Value = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextColorB", "0"));
            
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
            IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "opacity", "" + OpacityTrackBar.Value);

            if (lastPartChoose == 0)
            {
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextColorR", "" + ColorRTrackBar.Value);
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextColorG", "" + ColorGTrackBar.Value);
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextColorB", "" + ColorBTrackBar.Value);
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextFont", FontsCombox.Text);
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextsize", "" + TextSizeBox.Num);
            }
            else if (lastPartChoose == 1)
            {
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextColorR", "" + ColorRTrackBar.Value);
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextColorG", "" + ColorGTrackBar.Value);
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextColorB", "" + ColorBTrackBar.Value);
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextFont", FontsCombox.Text);
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextsize", "" + TextSizeBox.Num);
            }
            else if (lastPartChoose == 2)
            {
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextColorR", "" + ColorRTrackBar.Value);
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextColorG", "" + ColorGTrackBar.Value);
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextColorB", "" + ColorBTrackBar.Value);
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextFont", FontsCombox.Text);
                IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextsize", "" + TextSizeBox.Num);
            }

        }

        private void PartCombox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (openflag != true)
            {
                if (lastPartChoose == 0)
                {
                    IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextColorR", "" + ColorRTrackBar.Value);
                    IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextColorG", "" + ColorGTrackBar.Value);
                    IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextColorB", "" + ColorBTrackBar.Value);
                    IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextFont", FontsCombox.Text);
                    IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextsize", "" + TextSizeBox.Num);
                }
                else if (lastPartChoose == 1)
                {
                    IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextColorR", "" + ColorRTrackBar.Value);
                    IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextColorG", "" + ColorGTrackBar.Value);
                    IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextColorB", "" + ColorBTrackBar.Value);
                    IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextFont", FontsCombox.Text);
                    IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextsize", "" + TextSizeBox.Num);
                }
                else if (lastPartChoose == 2)
                {
                    IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextColorR", "" + ColorRTrackBar.Value);
                    IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextColorG", "" + ColorGTrackBar.Value);
                    IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextColorB", "" + ColorBTrackBar.Value);
                    IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextFont", FontsCombox.Text);
                    IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextsize", "" + TextSizeBox.Num);
                }

                if (PartCombox.SelectedIndex == 0)
                {
                    ColorRTrackBar.Value = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextColorR", "0"));
                    ColorGTrackBar.Value = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextColorG", "0"));
                    ColorBTrackBar.Value = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextColorB", "0"));
                    FontsCombox.Text = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextFont", "微软雅黑");
                    TextSizeBox.Num = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "srcTextsize", "12"));
                }
                else if (PartCombox.SelectedIndex == 1)
                {
                    ColorRTrackBar.Value = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextColorR", "0"));
                    ColorGTrackBar.Value = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextColorG", "0"));
                    ColorBTrackBar.Value = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextColorB", "0"));
                    FontsCombox.Text = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextFont", "微软雅黑");
                    TextSizeBox.Num = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "firstTransTextsize", "12"));
                }
                else if (PartCombox.SelectedIndex == 2)
                {
                    ColorRTrackBar.Value = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextColorR", "0"));
                    ColorGTrackBar.Value = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextColorG", "0"));
                    ColorBTrackBar.Value = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextColorB", "0"));
                    FontsCombox.Text = IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextFont", "微软雅黑");
                    TextSizeBox.Num = int.Parse(IniFileHelper.ReadItemValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "secondTransTextsize", "12"));
                }

                lastPartChoose = PartCombox.SelectedIndex;

            }
            else {
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
            IniFileHelper.WriteValue(Environment.CurrentDirectory + "\\settings.ini", "TranslateFormSettings", "BackColor", this.WinColorDialog.Color.ToArgb().ToString());
            
        }
    }
}
