using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.Net;

namespace MisakaTranslator_WPF
{
    public interface IAppSettings
    {
        [Option(Alias = "Software.OnCloseMainWindow", DefaultValue = "Exit")]
        string OnClickCloseButton
        {
            get;
            set;
        }

        [Option(Alias = "Globalization.Language", DefaultValue = "zh-CN")]
        string AppLanguage
        {
            get;
            set;
        }

        [Option(Alias = "Textractor.AutoHook", DefaultValue = "False")]
        string AutoHook
        {
            get;
            set;
        }

        [Option(Alias = "Textractor.AutoDetach", DefaultValue = "True")]
        string AutoDetach
        {
            get;
            set;
        }

        [Option(Alias = "JBeijing.JBJCTDllPath", DefaultValue = "")]
        string JBJCTDllPath
        {
            get;
            set;
        }

        [Option(Alias = "KingsoftFastAIT.KingsoftFastAITPath", DefaultValue = "")]
        string KingsoftFastAITPath
        {
            get;
            set;
        }

        [Option(Alias = "Dreye.DreyePath", DefaultValue = "")]
        string DreyePath
        {
            get;
            set;
        }

        [Option(Alias = "TencentOldTranslator.SecretId", DefaultValue = "")]
        string TXOSecretId
        {
            get;
            set;
        }

        [Option(Alias = "TencentOldTranslator.SecretKey", DefaultValue = "")]
        string TXOSecretKey
        {
            get;
            set;
        }

        [Option(Alias = "BaiduTranslator.appID", DefaultValue = "")]
        string BDappID
        {
            get;
            set;
        }

        [Option(Alias = "BaiduTranslator.secretKey", DefaultValue = "")]
        string BDsecretKey
        {
            get;
            set;
        }

        [Option(Alias = "DeepLTranslator.secretKey", DefaultValue = "")]
        string DeepLsecretKey
        {
            get;
            set;
        }

        [Option(Alias = "TencentTranslator.appID", DefaultValue = "")]
        string TXappID
        {
            get;
            set;
        }

        [Option(Alias = "TencentTranslator.appKey", DefaultValue = "")]
        string TXappKey
        {
            get;
            set;
        }

        [Option(Alias = "CaiyunTranslator.caiyunToken", DefaultValue = "")]
        string CaiyunToken
        {
            get;
            set;
        }

        [Option(Alias = "ChatGPTTranslator.apiKey", DefaultValue = "")]
        string ChatGPTapiKey
        {
            get;
            set;
        }

        [Option(Alias = "ChatGPTTranslator.apiUrl", DefaultValue = "https://api.openai.com/v1/chat/completions")]
        string ChatGPTapiUrl
        {
            get;
            set;
        }

        [Option(Alias = "AzureOpenAITranslator.apiKey", DefaultValue = "")]
        string AzureOpenAIApiKey
        {
            get;
            set;
        }

        [Option(Alias = "AzureOpenAITranslator.apiUrl", DefaultValue = "https://XXX.openai.azure.com/openai/deployments/YYY/chat/completions")]
        string AzureOpenAIApiUrl
        {
            get;
            set;
        }

        [Option(Alias = "XiaoniuTranslator.xiaoniuApiKey", DefaultValue = "")]
        string xiaoniuApiKey
        {
            get;
            set;
        }

        [Option(Alias = "IBMTranslator.IBMApiKey", DefaultValue = "")]
        string IBMApiKey
        {
            get;
            set;
        }

        [Option(Alias = "IBMTranslator.IBMURL", DefaultValue = "")]
        string IBMURL
        {
            get;
            set;
        }

        [Option(Alias = "YandexTranslator.YandexApiKey", DefaultValue = "")]
        string YandexApiKey
        {
            get;
            set;
        }

        [Option(Alias = "YoudaoZhiyun.YDZYAppId", DefaultValue = "")]
        string YDZYAppId 
        { 
            get;
            set; 
        }

        [Option(Alias = "YoudaoZhiyun.YDZYAppSecret", DefaultValue = "")]
        string YDZYAppSecret 
        { 
            get; 
            set; 
        }

        [Option(Alias = "Translate_All.HttpProxy", DefaultValue = "")]
        string HttpProxy
        {
            get;
            set;
        }

        [Option(Alias = "Translate_All.EachRowTrans", DefaultValue = true)]
        bool EachRowTrans
        {
            get;
            set;
        }

        [Option(Alias = "Translate_All.FirstTranslator", DefaultValue = "NoTranslate")]
        string FirstTranslator
        {
            get;
            set;
        }

        [Option(Alias = "Translate_All.SecondTranslator", DefaultValue = "NoTranslate")]
        string SecondTranslator
        {
            get;
            set;
        }

        [Option(Alias = "Translate_All.TransLimitNums", DefaultValue = 100)]
        int TransLimitNums
        {
            get;
            set;
        }

        [Option(Alias = "OCR_All.OCRsource", DefaultValue = "BaiduOCR")]
        string OCRsource
        {
            get;
            set;
        }

        [Option(Alias = "OCR_All.GlobalOCRHotkey", DefaultValue = "Ctrl + Alt + Q")]
        string GlobalOCRHotkey
        {
            get;
            set;
        }

        [Option(Alias = "OCR_All.GlobalOCRLang", DefaultValue = "jpn")]
        string GlobalOCRLang
        {
            get;
            set;
        }

        [Option(Alias = "BaiduOCR.APIKEY", DefaultValue = "")]
        string BDOCR_APIKEY
        {
            get;
            set;
        }

        [Option(Alias = "BaiduOCR.SecretKey", DefaultValue = "")]
        string BDOCR_SecretKey
        {
            get;
            set;
        }

        [Option(Alias = "TesseractCli.Path", DefaultValue = "C:\\Program Files\\Tesseract-OCR\\tesseract.exe")]
        string TesseractCli_Path
        {
            get;
            set;
        }

        [Option(Alias = "TesseractCli.Mode", DefaultValue = "jpn")]
        string TesseractCli_Mode
        {
            get;
            set;
        }

        [Option(Alias = "TesseractCli.Args", DefaultValue = "")]
        string TesseractCli_Args
        {
            get;
            set;
        }

        [Option(Alias = "LE.LEPath", DefaultValue = "")]
        string LEPath
        {
            get;
            set;
        }

        [Option(Alias = "XxgJpZhDict.xxgPath", DefaultValue = "")]
        string xxgPath
        {
            get;
            set;
        }

        [Option(Alias = "TTS.Voice", DefaultValue = "")]
        string ttsVoice
        {
            get;
            set;
        }

        [Option(Alias = "TTS.volume", DefaultValue = "80")]
        int ttsVolume
        {
            get;
            set;
        }

        [Option(Alias = "TTS.rate", DefaultValue = "0")]
        int ttsRate
        {
            get;
            set;
        }

        [Option(Alias = "TranslateFormSettings.opacity", DefaultValue = "100")]
        double TF_Opacity
        {
            get;
            set;
        }

        [Option(Alias = "TranslateFormSettings.backColor", DefaultValue = "#7f000000")]
        string TF_BackColor
        {
            get;
            set;
        }

        [Option(Alias = "TranslateFormSettings.SizeW", DefaultValue = "0")]
        string TF_SizeW
        {
            get;
            set;
        }

        [Option(Alias = "TranslateFormSettings.SizeH", DefaultValue = "0")]
        string TF_SizeH
        {
            get;
            set;
        }

        [Option(Alias = "TranslateFormSettings.LocX", DefaultValue = "-1")]
        string TF_LocX
        {
            get;
            set;
        }

        [Option(Alias = "TranslateFormSettings.LocY", DefaultValue = "-1")]
        string TF_LocY
        {
            get;
            set;
        }

        [Option(Alias = "TranslateFormSettings.srcTextFont", DefaultValue = "微软雅黑")]
        string TF_srcTextFont
        {
            get;
            set;
        }

        [Option(Alias = "TranslateFormSettings.srcTextSize", DefaultValue = "15")]
        double TF_srcTextSize
        {
            get;
            set;
        }

        [Option(Alias = "TranslateFormSettings.firstTransTextFont", DefaultValue = "微软雅黑")]
        string TF_firstTransTextFont
        {
            get;
            set;
        }

        [Option(Alias = "TranslateFormSettings.firstTransTextSize", DefaultValue = "15")]
        double TF_firstTransTextSize
        {
            get;
            set;
        }

        [Option(Alias = "TranslateFormSettings.firstTransTextColor", DefaultValue = "#ff000000")]
        string TF_firstTransTextColor
        {
            get;
            set;
        }

        [Option(Alias = "TranslateFormSettings.secondTransTextFont", DefaultValue = "微软雅黑")]
        string TF_secondTransTextFont
        {
            get;
            set;
        }

        [Option(Alias = "TranslateFormSettings.secondTransTextSize", DefaultValue = "15")]
        double TF_secondTransTextSize
        {
            get;
            set;
        }

        [Option(Alias = "TranslateFormSettings.secondTransTextColor", DefaultValue = "#ff000000")]
        string TF_secondTransTextColor
        {
            get;
            set;
        }

        [Option(Alias = "TranslateFormSettings.isKanaShow", DefaultValue = true)]
        bool TF_isKanaShow
        {
            get;
            set;
        }

        [Option(Alias ="TranslateFromSetting.isDropShaow",DefaultValue = true)]
        bool TF_DropShadow 
        {   get;
            set;
        }
        [Option(Alias = "TranslateFromSetting.isUseHiragana", DefaultValue = true)]
        bool TF_Hirakana
        {
            get;
            set;
        }
        [Option(Alias = "TranslateFromSetting.isSuperBold", DefaultValue = true)]
        bool TF_SuperBold
        {
            get;
            set;
        }
        [Option(Alias = "TranslateFromSetting.isColorful", DefaultValue = true)]
        bool TF_Colorful
        {
            get;
            set;
        }

        [Option(Alias = "ArtificialTrans.patchPath", DefaultValue = "")]
        string ArtificialPatchPath
        {
            get;
            set;
        }

        [Option(Alias = "ArtificialTrans.ATon", DefaultValue = false)]
        bool ATon
        {
            get;
            set;
        }

        #region 界面设置
        #region 前景色设置
        [Option(Alias = "Appearance.Foreground", DefaultValue = "#ffcccc")]
        string ForegroundHex
        {
            get;
            set;
        }
        #endregion
        #endregion

        [Option(DefaultValue = true)]
        bool GrowlEnabled { get; set; }

        [Option(Alias = "Mecab.dicPath", DefaultValue = "")]
        string Mecab_dicPath { get; set; }

        [Option(Alias = "Textractor.Path32", DefaultValue = "")]
        string Textractor_Path32 { get; set; }

        [Option(Alias = "Textractor.Path64", DefaultValue = "")]
        string Textractor_Path64 { get; set; }
    }

    public interface IRepeatRepairSettings
    {
        [Option(Alias = "RepairFun_RemoveSingleWordRepeat.RepeatTimes", DefaultValue = 0)]
        int SingleWordRepeatTimes
        {
            get;
            set;
        }

        [Option(Alias = "RepairFun_RemoveSentenceRepeat.FindCharNum", DefaultValue = 4)]
        int SentenceRepeatFindCharNum
        {
            get;
            set;
        }

        [Option(Alias = "RepairFun_Regex.Regex", DefaultValue = "")]
        string Regex
        {
            get;
            set;
        }

        [Option(Alias = "RepairFun_Regex.Replace", DefaultValue = "")]
        string Regex_Replace
        {
            get;
            set;
        }
    }
}
