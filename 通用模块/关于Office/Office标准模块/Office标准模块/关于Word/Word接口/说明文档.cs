﻿
/*问：IWordRange和IWordParagraph有什么区别？
  答：IWordParagraph的特征：
  
  1.可以包括文本或其他对象
  2.段落共享且可以设置对齐方式
  3.同一个段落可以包括多个范围
  4.一定用换行符结尾

  IWordRange的特征：
  1.只能包含文本
  2.可以通过范围设置文本格式，但是同一范围的各个部分可能具有不同的格式
  3.范围可以跨越多个段落
  4.不一定以换行符结尾*/