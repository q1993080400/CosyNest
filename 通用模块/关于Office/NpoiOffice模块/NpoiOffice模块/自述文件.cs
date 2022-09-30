﻿
/*开发目标：
  本模块是使用Npoi实现的Office操作工具，
  本实现的特点在于：

  优点：
  #可移植，在任何平台上均可工作
  #相对于COM实现的Office标准，性能较高
  #不需要创建独立进程
  #不需要安装Office
  #使用纯托管代码实现

  缺点：
  #相对于COM实现的Office标准，功能较少，
  包括但不限于缺少打印，图表等功能

  #国产开源项目的老毛病，对标准的支持不规范（疑似），
  在处理稍微复杂一点的工作簿时极易出错，包括但不限于：
  处理含有冷门函数的工作表，因为NPOI尚未实现这些函数

  目标平台：
  本模块被设计为可移植，不应调用任何特定平台的API

  重要说明：
  #Npoi在加载工作簿以后不会释放掉对Excel文件的占用，
  并且在修改时会同步写入文件流，无论是否保存（疑似），
  因此建议在代码执行完毕后，自动或手动保存所有工作簿，
  如果不这么做的话，很可能会造成工作簿损坏，无法再次打开*/