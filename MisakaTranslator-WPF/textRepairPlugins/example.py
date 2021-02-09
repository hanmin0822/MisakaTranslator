# coding=utf-8
# 不加上面那行就不能写中文注释
# Iron Python自带了re，如果要使用其他标准库，可以把Python2.7 lib目录下的文件复制到翻译器的lib目录下。
import re

# 函数名必须是process，接受一个string，返回一个string，其他随意
def process(source):
    #去除HTML标签
    source = re.sub("<[^>]+>", "", source)
    source = re.sub("&[^;]+;", "", source)
    #句子去重（根据空格分割句子，取最后一段）
    parts = [p for p in source.split(" ") if len(p) > 0]
    return parts[-1] if len(parts) != 0 else ""
