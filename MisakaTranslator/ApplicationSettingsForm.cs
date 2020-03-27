/*
 *Namespace         MisakaTranslator
 *Class             ApplicationSettingsForm
 *Description       软件本体的设置窗口，包含了各个按钮的点击事件等，由于按钮过多，不一一注释
 */


using HZH_Controls.Controls;
using MaterialSkin.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MisakaTranslator
{
    public partial class ApplicationSettingsForm : MaterialForm
    {
        public ApplicationSettingsForm()
        {
            InitializeComponent();
        }

        private void ApplicationSettingsForm_Load(object sender, EventArgs e)
        {
            MenuInit();

            
            BDOCRapikeyTextBox.InputText = Common.settings.BDOCR_APIKEY;
            BDOCRsecretkeyTextBox.InputText = Common.settings.BDOCR_SecretKey;

            BDappidTextBox.InputText = Common.settings.BDappID;
            BDKeyTextBox.InputText = Common.settings.BDsecretKey;

            TXappidTextBox.InputText = Common.settings.TXappID;
            TXKeyTextBox.InputText = Common.settings.TXappKey;

            TXOappidTextBox.InputText = Common.settings.TXOSecretId;
            TXOKeyTextBox.InputText = Common.settings.TXOSecretKey;

            JBjPathBox.InputText = Common.settings.JBJCTDllPath;

            AutoHookCheckBox.Checked = Convert.ToBoolean(Common.settings.AutoHook);

            EachRowTransCheckBox.Checked = Convert.ToBoolean(Common.settings.EachRowTrans);

            List<KeyValuePair<string, string>> transApiSrc = GetAllTranslateAPI();
            FirstTransCombox.Source = transApiSrc;
            SecondTransCombox.Source = transApiSrc;
            FirstTransCombox.BoxStyle = ComboBoxStyle.DropDownList;
            SecondTransCombox.BoxStyle = ComboBoxStyle.DropDownList;

            string first = Common.settings.FirstTranslator;
            string second = Common.settings.SecondTranslator;

            List<KeyValuePair<string, string>> OCRApiSrc = GetAllOCRAPI();
            OCRsourceCombox.Source = OCRApiSrc;
            OCRsourceCombox.BoxStyle = ComboBoxStyle.DropDownList;

            for (int i = 0; i < transApiSrc.Count; i++)
            {
                if (transApiSrc[i].Key == first)
                {
                    FirstTransCombox.SelectedIndex = i;
                }

                if (transApiSrc[i].Key == second)
                {
                    SecondTransCombox.SelectedIndex = i;
                }
            }

            for (int i = 0; i < OCRApiSrc.Count; i++)
            {
                if (OCRApiSrc[i].Key == Common.settings.OCRsource)
                {
                    OCRsourceCombox.SelectedIndex = i;
                }
            }
        }

        /// <summary>
        /// 得到所有翻译API的列表，即每更新一个API，需要在这里加上
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetAllTranslateAPI()
        {
            List<KeyValuePair<string, string>> ret = new List<KeyValuePair<string, string>>();

            ret.Add(new KeyValuePair<string, string>("NoTranslate", "无翻译"));
            ret.Add(new KeyValuePair<string, string>("BaiduTranslator", "百度翻译"));
            ret.Add(new KeyValuePair<string, string>("TencentTranslator", "腾讯翻译"));
            ret.Add(new KeyValuePair<string, string>("JBjTranslator", "JBeijing翻译"));
            ret.Add(new KeyValuePair<string, string>("TencentOldTranslator", "腾讯翻译(旧)"));

            return ret;
        }

        /// <summary>
        /// 得到所有OCR API的列表，即每更新一个API，需要在这里加上
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetAllOCRAPI()
        {
            List<KeyValuePair<string, string>> ret = new List<KeyValuePair<string, string>>();

            ret.Add(new KeyValuePair<string, string>("BaiduOCR", "百度在线OCR"));
            ret.Add(new KeyValuePair<string, string>("TesseractOCR", "Tesseract离线OCR"));

            return ret;
        }


        /// <summary>
        /// 左部导航菜单初始化
        /// </summary>
        private void MenuInit()
        {
            List<MenuItemEntity> lstMenu = new List<MenuItemEntity>();

            MenuItemEntity item1 = new MenuItemEntity()
            {
                Key = "p1",
                Text = "MisakaTranslator"
            };

            lstMenu.Add(item1);
            //=======================================================

            MenuItemEntity item2 = new MenuItemEntity()
            {
                Key = "p2",
                Text = "Textrator相关设置"
            };

            lstMenu.Add(item2);
            //=======================================================

            MenuItemEntity item3 = new MenuItemEntity()
            {
                Key = "p3",
                Text = "OCR文字识别设置"
            };
            item3.Childrens = new List<MenuItemEntity>();
            MenuItemEntity item4 = new MenuItemEntity()
            {
                Key = "c1",
                Text = "百度OCR API"
            };

            item3.Childrens.Add(item4);

            lstMenu.Add(item3);
            //=======================================================

            MenuItemEntity item5 = new MenuItemEntity()
            {
                Key = "p4",
                Text = "翻译API设置"
            };
            item5.Childrens = new List<MenuItemEntity>();
            MenuItemEntity item6 = new MenuItemEntity()
            {
                Key = "c2",
                Text = "百度翻译 API"
            };
            item5.Childrens.Add(item6);
            MenuItemEntity item7 = new MenuItemEntity()
            {
                Key = "c3",
                Text = "腾讯翻译 API"
            };
            item5.Childrens.Add(item7);
            MenuItemEntity item8 = new MenuItemEntity()
            {
                Key = "c4",
                Text = "JBeijing API"
            };
            item5.Childrens.Add(item8);
            MenuItemEntity item9 = new MenuItemEntity()
            {
                Key = "c5",
                Text = "腾讯私人 API"
            };
            item5.Childrens.Add(item9);

            lstMenu.Add(item5);
            //=======================================================


            SettingsMenu.MenuStyle = MenuStyle.Top;
            SettingsMenu.DataSource = lstMenu;
        }

        /// <summary>
        /// 导航菜单选中事件，跳转到各个页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsMenu_SelectedItem(object sender, EventArgs e)
        {
            if (sender is UCMenuChildrenItem)
            {
                UCMenuChildrenItem ucci = (UCMenuChildrenItem)sender;
                MenuItemEntity mie = ucci.DataSource;

                switch (mie.Key)
                {
                    case "c1":
                        SettingsTabControl.SelectedIndex = 2;
                        break;
                    case "c2":
                        SettingsTabControl.SelectedIndex = 3;
                        break;
                    case "c3":
                        SettingsTabControl.SelectedIndex = 4;
                        break;
                    case "c4":
                        SettingsTabControl.SelectedIndex = 5;
                        break;
                    case "c5":
                        SettingsTabControl.SelectedIndex = 7;
                        break;
                }

            }
            else if (sender is UCMenuParentItem)
            {
                UCMenuParentItem ucpi = (UCMenuParentItem)sender;
                MenuItemEntity mie = ucpi.DataSource;

                switch (mie.Key)
                {
                    case "p1":
                        SettingsTabControl.SelectedIndex = 0;
                        break;
                    case "p2":
                        SettingsTabControl.SelectedIndex = 1;
                        break;
                    case "p3":
                        SettingsTabControl.SelectedIndex = 8;
                        break;
                    case "p4":
                        SettingsTabControl.SelectedIndex = 6;
                        break;
                }
            }
            
        }

        private void BaiduTransApplyBtn_BtnClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://api.fanyi.baidu.com/product/11");
        }

        private void BaiduAPITestBtn_BtnClick(object sender, EventArgs e)
        {
            Common.settings.BDappID = BDappidTextBox.InputText;
            Common.settings.BDsecretKey = BDKeyTextBox.InputText;

            BaiduTranslator.BaiduTrans_Init();
            string ret = BaiduTranslator.Baidu_Translate("apple", "zh");

            BaiduTransOutInfo oinfo = JsonConvert.DeserializeObject<BaiduTransOutInfo>(ret);

            if (oinfo.error_code == null || oinfo.error_code == "52000")
            {
                MessageBox.Show("百度翻译API工作正常!", "提示");
            }
            else
            {
                MessageBox.Show("百度翻译API工作异常，错误代码:" + oinfo.error_code + " \n您可以核对官方描述的错误代码来尝试解决问题！", "错误");
            }
        }

        private void BDBalanceCheckBtn_BtnClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://api.fanyi.baidu.com/api/trans/product/desktop");
        }

        private void BDAPIDocBtn_BtnClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://api.fanyi.baidu.com/doc/21");
        }

        private void BDTransTextTestBtn_BtnClick(object sender, EventArgs e)
        {
            Common.settings.BDappID = BDappidTextBox.InputText;
            Common.settings.BDsecretKey = BDKeyTextBox.InputText;

            BaiduTranslator.BaiduTrans_Init();
            string ret = BaiduTranslator.Baidu_Translate(BDTestTextBox.InputText, BDDesLangBox.InputText, BDSrcLangBox.InputText);

            BaiduTransOutInfo oinfo = JsonConvert.DeserializeObject<BaiduTransOutInfo>(ret);

            if (oinfo.trans_result.Count == 1)
            {
                MessageBox.Show(oinfo.trans_result[0].dst, "翻译结果");
            }
            else
            {
                MessageBox.Show("翻译过程中出现错误，请先进行API认证测试！", "错误");
            }
        }

        private void TXAPIDocBtn_BtnClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://ai.qq.com/doc/nlptrans.shtml");
        }

        private void TXBalanceCheckBtn_BtnClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://ai.qq.com/console/home");
        }

        private void TXTransApplyBtn_BtnClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://ai.qq.com/product/nlptrans.shtml");
        }

        private void TXAPITestBtn_BtnClick(object sender, EventArgs e)
        {
            Common.settings.TXappID = TXappidTextBox.InputText;
            Common.settings.TXappKey = TXKeyTextBox.InputText;

            TencentTranslator.TencentTrans_Init();
            string ret = TencentTranslator.Fanyijun_Translate("apple", "zh", "en");

            TencentTransOutInfo oinfo = JsonConvert.DeserializeObject<TencentTransOutInfo>(ret);

            if (oinfo.ret == "0")
            {
                MessageBox.Show("腾讯翻译API工作正常!", "提示");
            }
            else
            {
                MessageBox.Show("腾讯翻译API工作异常，错误代码:" + oinfo.ret + " \n错误描述:" + oinfo.msg, "错误");
            }
        }

        private void TXTransTextTestBtn_BtnClick(object sender, EventArgs e)
        {
            Common.settings.TXappID = TXappidTextBox.InputText;
            Common.settings.TXappKey = TXKeyTextBox.InputText;

            TencentTranslator.TencentTrans_Init();
            string ret = TencentTranslator.Fanyijun_Translate(TXTestTextBox.InputText, TXDesLangBox.InputText, TXSrcLangBox.InputText);

            TencentTransOutInfo oinfo = JsonConvert.DeserializeObject<TencentTransOutInfo>(ret);

            if (oinfo.ret == "0")
            {
                MessageBox.Show(oinfo.data.target_text, "翻译结果");
            }
            else
            {
                MessageBox.Show("翻译过程中出现错误，请先进行API认证测试！", "错误");
            }
        }

        private void JBjTransTestBtn_BtnClick(object sender, EventArgs e)
        {
            Common.settings.JBJCTDllPath = JBjPathBox.InputText;

            string res = JBeijingTranslator.Translate_JapanesetoChinese(JbjTestTextBox.InputText);
            MessageBox.Show(res, "翻译结果");
        }

        private void ChooseJBjPathBtn_BtnClick(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择Txt所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show("文件夹路径不能为空", "提示");
                }
                else
                {
                    JBjPathBox.InputText = dialog.SelectedPath;
                    Common.settings.JBJCTDllPath = JBjPathBox.InputText;

                }

            }

        }

        private void FirstTransCombox_SelectedChangedEvent(object sender, EventArgs e)
        {
            Common.settings.FirstTranslator = FirstTransCombox.SelectedValue;
        }

        private void SecondTransCombox_SelectedChangedEvent(object sender, EventArgs e)
        {
            Common.settings.SecondTranslator = SecondTransCombox.SelectedValue;
        }

        private void AutoHookCheckBox_CheckedChangeEvent(object sender, EventArgs e)
        {
            Common.settings.AutoHook = Convert.ToString(AutoHookCheckBox.Checked);
        }

        private void BaiduOCRApplyBtn_BtnClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://ai.baidu.com/tech/ocr/general");
        }

        private void BDOCRBalanceCheckBtn_BtnClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://console.bce.baidu.com/ai/?fromai=1#/ai/ocr/overview/index");
        }

        private void BaiduOCRTestBtn_BtnClick(object sender, EventArgs e)
        {
            Common.settings.BDOCR_APIKEY = BDOCRapikeyTextBox.InputText;
            Common.settings.BDOCR_SecretKey = BDOCRsecretkeyTextBox.InputText;

            bool ret = BaiduGeneralOCRBasic.BaiduGeneralOCRBasic_Init();

            if (ret == true)
            {
                MessageBox.Show("百度OCR API工作正常", "提示");
            }
            else
            {
                MessageBox.Show("百度OCR API工作异常，请检查填写是否正确", "错误");
            }
        }

        private void EachRowTransCheckBox_CheckedChangeEvent(object sender, EventArgs e)
        {
            Common.settings.EachRowTrans = Convert.ToString(EachRowTransCheckBox.Checked);
        }

        private void TXOAPIDocBtn_BtnClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://cloud.tencent.com/document/api/551/15619");
        }

        private void TXOTransApplyBtn_BtnClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://cloud.tencent.com/product/tmt");
        }

        private void TXOBalanceCheckBtn_BtnClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://console.cloud.tencent.com/tmt");
        }

        private void TXOAPITestBtn_BtnClick(object sender, EventArgs e)
        {
            Common.settings.TXOSecretId = TXOappidTextBox.InputText;
            Common.settings.TXOSecretKey = TXOKeyTextBox.InputText;
            
            TencentOldTranslator.TencentOldTrans_Init();
            string ret = TencentOldTranslator.TencentOld_Translate("apple", "zh", "en");

            TencentOldTransOutInfo oinfo = JsonConvert.DeserializeObject<TencentOldTransOutInfo>(ret);

            if (oinfo.Response.Error == null)
            {
                MessageBox.Show("腾讯翻译API(旧版)工作正常!", "提示");
            }
            else
            {
                MessageBox.Show("腾讯翻译API(旧版)工作异常，错误代码:" + oinfo.Response.Error.Code + " \n错误信息:" + oinfo.Response.Error.Message, "错误");
            }
        }

        private void TXOTransTextTestBtn_BtnClick(object sender, EventArgs e)
        {
            Common.settings.TXOSecretId = TXOappidTextBox.InputText;
            Common.settings.TXOSecretKey = TXOKeyTextBox.InputText;

            TencentOldTranslator.TencentOldTrans_Init();
            string ret = TencentOldTranslator.TencentOld_Translate(TXOTestTextBox.InputText, TXODesLangBox.InputText, TXOSrcLangBox.InputText);

            TencentOldTransOutInfo oinfo = JsonConvert.DeserializeObject<TencentOldTransOutInfo>(ret);

            if (oinfo.Response.Error == null)
            {
                MessageBox.Show(oinfo.Response.TargetText, "翻译结果");
            }
            else
            {
                MessageBox.Show("翻译过程中出现错误，请先进行API认证测试！", "错误");
            }
        }

        private void HelpBtn_BtnClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://misaka.galeden.cn/");
        }

        private void GetTextractorHistoryBtn_BtnClick(object sender, EventArgs e)
        {
            Common.ExportTextractorHistory();
            MessageBox.Show("导出完成！请将目录下的TextractorOutPutHistory.txt文件发送给作者以检查。", "提示");
        }

        private void GithubBtn_BtnClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/hanmin0822/MisakaTranslator");
        }

        private void OCRsourceCombox_SelectedChangedEvent(object sender, EventArgs e)
        {
            Common.settings.OCRsource = OCRsourceCombox.SelectedValue;
        }
    }
}
