using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace TTSHelperLibrary
{
    public class TextSpeechHelper
    {
        private SpeechSynthesizer synth;
        
        public TextSpeechHelper() {
            synth = new SpeechSynthesizer();
        }

        ~TextSpeechHelper() {
            synth = null;
        }

        /// <summary>
        /// 获得当前所有可用的TTS引擎
        /// </summary>
        /// <returns>返回引擎信息的集合</returns>
        public List<string> GetAllTTSEngine() {
            List<string> res = new List<string>();

            foreach (InstalledVoice iv in synth.GetInstalledVoices())
            {
                res.Add(iv.VoiceInfo.Name);
            }

            if (res.Count > 0)
            {
                return res;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 设置TTS语音的音量
        /// </summary>
        /// <param name="vol">音量大小 0-100之间</param>
        public void SetVolume(int vol) {
            synth.Volume = vol;
        }

        /// <summary>
        /// 设置TTS语音的语速
        /// </summary>
        /// <param name="ra">语速的值 -10~10之间 值越小速度越慢</param>
        public void SetRate(int ra)
        {
            synth.Rate = ra;
        }

        /// <summary>
        /// 设置TTS的语音
        /// </summary>
        /// <param name="name">通过GetAllTTSEngine()得到的VoiceInfo对象的Name属性</param>
        public void SetTTSVoice(string name) {
            if (name != null && name != "") {
                synth.SelectVoice(name);
            }
        }

        /// <summary>
        /// 以同步方式说出字符串的内容。
        /// </summary>
        /// <param name="text">要说的字符串</param>
        public void Speak(string text) {
            synth.Speak(text);
        }

        /// <summary>
        /// 以异步方式说出字符串的内容。
        /// </summary>
        /// <param name="text">要说的字符串</param>
        public void SpeakAsync(string text) {
            synth.SpeakAsync(text);
        }

        /// <summary>
        /// 取消所有排队、 异步语音合成操作。
        /// </summary>
        public void CancelAllSpeakAsync() {
            synth.SpeakAsyncCancelAll();
        }
    }
}
