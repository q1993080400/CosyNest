﻿
/*问：什么是块？
  答：块是一种数据结构，它是对Excel中数据的抽象，
  它假设数据按照以下特征组织：

  1.同一条数据独占一块连续的区域，数据按照水平或垂直的方式依次排列

  2.数据内部的列也独占一片连续的区域，
  而且它们相对于数据最左上角单元格的位置不变

  块能够简化对Excel数据的处理，但是，即便经过这一层简化，
  仍然不推荐使用Excel处理复杂的数据，这是因为Excel本身就不是为专业用途设计的*/