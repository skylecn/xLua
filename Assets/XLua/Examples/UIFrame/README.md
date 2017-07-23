

## 关于lua的UI函数式编程模板

lua只定义当前UI窗口或控件的函数，而实现当前UI窗口功能的数据（lua table）在由MonoBehaviour继承来的FuncLuaBehavior的Awake中定义，在lua代码中动态增加变量到该table中，在OnDestroy时，该table也随之销毁

为什么要这样设计？

* 函数式编程比面向对象思维模式简单清晰，容易上手，而UI框架也比较简单，用函数式编程足够实现
* lua的面向对象基于metatable实现，写一个类声明就需要好多行代码，相比高级语言的class声明还复杂
* UI实例是有Unity创建和销毁的，lua实现UI功能定义的数据需要和UI实例对应，通过Awake中创建对应的table，OnDestroy中销毁



