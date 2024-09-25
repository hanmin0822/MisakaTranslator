<h1 align="center">
  MisakaTranslator 御坂翻译器
  <br>
</h1>

<p align="center">
  <b>Galgame/文字游戏/漫画多语种实时机翻工具</b>
  <br>
  <b>开源 | 高效 | 易用</b>
  <br>
  <img src="https://github.com/hanmin0822/MisakaTranslator/workflows/CI/badge.svg" alt="CI">
  <br>
  <br>
</p>

<p align="center">
  <a href="/README.md">中文</a> •
  <a href="/README_EN.md">English</a>
</p>

~~MisakaTranslator的名字由来是因为本软件连接到了一万多名御坂妹妹所组成的『御坂网络』，利用其强大的计算能力来提供实时可靠的翻译（误）~~

[MisakaTranslator相关文件下载地址](https://blog.csdn.net/hanmin822/article/details/119796334)

[测试版演示视频](https://www.bilibili.com/video/av94082641)

[2.0版本教程](https://www.bilibili.com/video/BV1Qt4y11713)

## 软件功能及特点

* 兼容性强：支持`Hook+OCR`两种方式提取游戏文本，能适配绝大多数游戏
* 可离线：支持完全离线工作(`Hook模块离线、可选的Tesseract-OCR模块离线、三种离线翻译API`)
* 高度可拓展的文本修复：针对Hook提取到的重复文本提供多种去重方式
* 提供更好体验的在线API：提供多种在线API(`百度OCR+多种在线翻译API+多种公共接口翻译`)
* 更高的OCR精度：支持在提交OCR前对图片进行预处理（多种处理方法）
* 分词及字典：支持Mecab分词和字典功能，可针对单词进行查询
* 翻译优化系统：支持人名地名预翻译，提高翻译质量，这个系统正在被不断完善
* 人工翻译系统：支持用户自己定义语句翻译并生成人工翻译文件以供分享
* TTS语音发音：支持朗读句子和单词
* 漫画翻译：更友好、准确的漫画翻译
* 高效：使用C#开发，程序效率较使用Python开发的VNR要高
* 易用：UI亲切，易上手，有详细教程
* ~~更多功能，正在开发~~ 本项目目前处于维护状态，不会添加新功能

## 从源码构建

* 本项目的目标框架为4.7.2
* KeyboardMouseMonitor为VC项目，如不想构建可去掉，之后复制lib
* **其余二进制依赖需在构建完成后手动从已Release的包中复制lib文件夹**
* Actions的artifact为nightly build，也不含lib
* 如果想用Tesseract4，复制tessdata文件夹

## 帮助开发者

如果您对这个项目感兴趣，想提供任何帮助，欢迎联系作者。

E-Mail/QQ:512240272@qq.com

## 本项目所使用到的其他开源项目

* [lgztx96/texthost](https://github.com/lgztx96/texthost)
* [Artikash/Textractor](https://github.com/Artikash/Textractor)
* 其他参见[Dependencies](https://github.com/hanmin0822/MisakaTranslator/network/dependencies)

## 本项目开发过程中使用到的重要参考文献

* [C#调用多种翻译API和使用Textractor提取文本](https://www.lgztx.com/)
* [C#图像处理(二值化,灰阶)](https://blog.csdn.net/chaoguodong/article/details/7877312)
* [C#中使用全局键盘鼠标钩子](https://www.cnblogs.com/CJSTONE/p/4961865.html)

## 项目团队成员

* 画师-[洛奚](https://www.pixiv.net/users/13495987)
* 官网制作-[Disviel](https://github.com/Disviel)
* 人设&画师-[百川行](https://www.pixiv.net/users/17591894)
* 合作开发者-[unlsycn](https://github.com/HumphreyDotSln)
* 合作开发者-[tpxxn](https://github.com/tpxxn)
* 合作开发者-[imba-tjd](https://github.com/imba-tjd)

## MisakaProject

MisakaProject是基于MisakaTranslator或辅助其功能所开发的一切项目的集合。

如果您有意将自己的相关类型项目加入到MisakaProject中，请联系作者。

* [jsc723/MisakaPatcher](https://github.com/jsc723/MisakaPatcher)

MisakaPatcher添加了对外挂汉化补丁的支持，因此本工具更适合喜欢人工翻译的玩家，也为解包封包遇到困难的汉化人员提供了另一种发布汉化的途径。

* [hanmin0822/MisakaHookFinder](https://github.com/hanmin0822/MisakaHookFinder)

MisakaHookFinder适用于部分游戏无法使用此翻译器直接得到文本Hook方法的场合，用户可以通过它自行搜索得到Hook特殊码或直接使用其获取源文本，同时也支持剪贴板输出原文。

## 贡献者名单

感谢所有对本项目开发提供帮助的人！

* 提供软件的英语版本翻译-[Words](https://github.com/CPCer)
* 多次提供作者开发上的帮助-[lgztx96](https://github.com/lgztx96)
* [点此查看其他的贡献者名单](https://github.com/hanmin0822/MisakaTranslator/blob/master/THANKLIST.MD)

## 作出贡献

如果您想为本项目作出贡献，请参阅[贡献指南](https://github.com/hanmin0822/MisakaTranslator/blob/master/CONTRIBUTING.md)

## 其他注意

软件开发过程中使用到部分网络素材，如果侵犯到您的权益，请第一时间联系作者删除，谢谢！

## 友情链接

[HumiHumi](https://humihumi.co.jp/)

