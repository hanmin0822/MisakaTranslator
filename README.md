![MisakaTranslator](https://github.com/hanmin0822/MisakaTranslator/blob/master/MisakaTranslator/Resources/Background.jpg)
# MisakaTranslator御坂翻译器

开源|高效|易用

Galgame/文字游戏多语种实时机翻工具

~~MisakaTranslator的名字由来是因为本软件连接到了一万多名御坂妹妹所组成的『御坂网络』，利用其强大的计算能力来提供实时可靠的翻译（误）~~

演示视频：https://www.bilibili.com/video/av94082641

以上视频仅为测试版第一版功能，目前的实际功能和其他效果见最新版本！


## 软件功能

* 支持`Hook+OCR`两种方式提取游戏文本
* 支持完全离线工作(`Hook+Jbeijing`)
* 方便的API调用(`百度OCR+百度翻译+腾讯翻译（两种）`)
* 程序效率较使用Python开发的VNR要高
* UI亲切，易上手，有详细教程
* 更多功能，正在开发

## 帮助开发者

如果您对这个项目感兴趣，想提供任何帮助，欢迎联系作者。

作者的联系方式在下方。

## 本项目所使用到的其他开源项目

* [IgnaceMaes/MaterialSkin](https://github.com/IgnaceMaes/MaterialSkin) 
* [平原君关于MaterialSkin的fork](https://gitee.com/victorzhao/MaterialSkin)
* [Artikash/Textractor](https://github.com/Artikash/Textractor)
* [JamesNK/Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
* [kwwwvagaa/NetWinformControl](https://github.com/kwwwvagaa/NetWinformControl)
* [Config.Net](https://github.com/aloneguid/config)
* [charlesw/tesseract](https://github.com/charlesw/tesseract/)
* [tesseract-ocr/tesseract](https://github.com/tesseract-ocr/tesseract)

## 本项目开发过程中使用到的重要参考文献

* [C#调用Textractor提取文本](https://www.lgztx.com/?p=157) 
* [C#调用JBeijing离线中日翻译](https://github.com/Artikash/VNR-Core/)
* [C#图像处理(二值化,灰阶)](https://blog.csdn.net/chaoguodong/article/details/7877312)

## 联系作者

E-Mail/QQ:512240272@qq.com

## 合作开发者

* [unlsycn](https://github.com/HumphreyDotSln) 

## 贡献者名单

感谢所有对本项目开发提供帮助的人！

* [点此查看贡献者名单](https://github.com/hanmin0822/MisakaTranslator/blob/master/THANKLIST.MD)

## 其他注意

软件运行时需要调用Textractor的CLI版本读输出，但原版的控制台输出可能导致本软件无法读取或读取存在问题，针对CLI版本的修改如下，请自行编译后使用

* [hanmin0822/Textractor](https://github.com/hanmin0822/Textractor)

软件开发过程中使用到部分网络素材，如果侵犯到您的权益，请第一时间联系作者删除，谢谢！
