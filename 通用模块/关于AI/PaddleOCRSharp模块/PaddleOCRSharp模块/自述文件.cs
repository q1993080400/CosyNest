﻿
/*开发目标：
  本模块是底层使用PaddleOCRSharp实现的OCR库，
  能够用来识别图片中的文字

  目标平台：
  本模块只能在X64的CPU上编译和使用，只能在具有AVX指令集上的CPU上使用，
  但是至于本模块是否具备跨操作系统能力，还有待研究

  警告：
  暂时不要升级本模块所依赖的PaddleOCRSharp，
  新版本好像存在问题，在连续识别图片的时候，会产生非法内存访问异常*/