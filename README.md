问：这是什么项目？

答：这是弗朗西斯的个人自用类库，封装了一些工作和生活中经常使用的API，以避免重复的劳动

问：那么，这个项目是工具类库吗？

答：不是，虽然本框架中的很多模块都叫“某某工具模块”，但那只是习惯叫法，工具类库只进行了简单的封装，而本项目是结构清晰的框架，它是完全模块化的，对如何抽象问题提出了自己的理解，但是与商业项目不同的是，由于个人精力有限，本项目遵循用进废退原则，本人经常使用的功能会反复修改和优化，很少使用的功能会慢慢停止更新（即便其他人需要这个功能）

问：这个项目使用什么语言写成？可以运行在哪些平台上？

答：在目前全部使用C#开发，但如果以后有些模块比较适合使用其它语言，作者会学习并用这些语言编写它们

至于可移植性，作者非常重视模块与平台的解耦，如果某一模块不需要调用特定平台的API，那么它不会依赖于任何特定于某一平台的模块或BCL，这保证了本项目中的大部分模块可以运行于任何平台

问：这个项目可以解决什么问题？

答：本项目不是为了解决特定问题专门设计的，而是遵循这样的原则：越基本的模块越通用，越复杂的模块越专用，你可以这样理解本项目中不同模块的分工：上层模块是开发工具，底层模块是开发开发工具的工具

问：既然这个项目的目标是大而全，那么个人能够胜任这个工程吗？

答：首先，本项目的开发目标不是大而全，作者只开发自己需要的功能，不会开发自己不需要，但是别人需要的功能，因此它不存在这个问题，作者从未想过，也没有能力取代net框架

但同时作者也认为，完全特化，只针对一项需求的框架是极大的浪费，因为要实现这个功能，它必须要依赖很多基础的API，而这些API中的大部分肯定在其他的需求中也有用处，将它们全部挤在一个框架中，不仅无法复用，而且无法和其他框架通讯（因为它们需要遵循共同的接口），因此对于精力和资源有限的个人而言，层次分明，分工明确，高度泛化的框架反而是最节约成本的选择

问：设计这个项目时侧重什么？

答：作者最重视的是低耦合，可扩展，因为这是当代软件工程中最需要的东西，但同时作者也很重视性能，会尽量优化掉执行过程中不必要的步骤，当这两者发生冲突时，前者优先级更高

本框架的设计原则是：在高度封装的同时，不损失可扩展性，是的，我全都要

问：这个项目注释齐全吗？

答：由于本项目是自用项目，而且经常被重构，因此作者非常重视可维护性，所有公开API和大部分私有API都有完整的文档注释，如果某个代码片段看上去比较奇怪，作者还会在旁边注释说明为什么要这样写，因为作者希望这个项目不仅仅是一个可以运行的程序，还是对作者宝贵工作经验的记录

问：你刚才说这个项目绝大多数API都有完整的文档注释，但是为什么我看到的不是这样的？

答：因为派生类的API可以自动继承基类和接口的注释，请克隆本项目，并用VS2019  16.4以上版本打开，注释就会出现

问：我看到这个项目似乎没有什么高端的技术，那么它有什么意义？

答：软件工程发展到今天，几乎任何需求都有完善的封装，很少有什么功能是不能实现的，因此作者认为，与其想办法做别人做不到的事，不如想办法简化大多数人每天的工作内容，这是很有益处的

问：这个项目承诺向下兼容吗？

答：本项目性质特殊，它只基于个人兴趣和方便工作的需求，所以它不遵循通常使用的发布模型，事实上，作者在闲暇时间只要看到需要修改的部分，就会顺手去改一下，你可以这样理解本项目的发布策略：那就是它永远处于开发阶段，永远不会发布一个稳定的版本，因此它根本没有版本这个概念，也不存在向下兼容这种说法，本项目中的所有API都随时可能被修改，重构，删除，如果你需要使用它，请悉知这一点，同时因为这个原因，本框架无法发布稳定的文档，有关如何使用本框架的说明只能直接放在代码注释中

完全抛弃向下兼容并非愚蠢或缺乏计划，它对代码质量有极大的好处，所以本项目从17年8月开始构建，至今不存在任何遗留问题（俗称屎山），除此之外的任何手段因为投鼠忌器，不可能产生如此良好的效果

问：本项目的名称CosyNest是什么意思？

答：意为“安乐窝”，作者看待它就像宅男看待自己的房子一样，是一个很小，但是自成体系的世界