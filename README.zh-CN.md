# Winform.Navigator

## 介绍
Winform.Navigator是适用于Winform框架的页面导航器，这里的“页面”可以是一个Form窗体，也可以是继承自Control类的用户/自定义控件。

### 功能
Winform.Navigator支持以下功能：  
- 页面间跳转，带参数跳转
- 可设置页面缓存，可自定义被缓存时以及恢复状态时的操作
- 可设置页面权限
- 容器内自适应分辨率
- 提供页面切换前与切换后事件(类似全局前置守卫和后置守卫)

### 解决的问题
Winform.Navigator主要解决的是开发人员在编写代码实现页面/界面跳转时代码不合理、不规范的问题。比如：  
假设你现在有三个页面page1,page2和page3，以及三个按钮btn1,btn2和btn3，点击btn1时跳转到page1，同理对于btn2和btn3。那么，怎么实现呢？  
一般我们会这么去实现：给btn1绑定一个点击事件，事件处理程序里，先清空页面父容器里的控件，再new一个page1，把这个page1添加到父容器里show一下，OK。  
可如果page1里有我们想要暂存的信息怎么办？比如说，这个page1是一个编写文章的页面，使用者写到一半，切换到其他页面去了，当他再切回到page1时，是不是应该要还原之前写了一半的页面呢？  
所以，针对这种情况，我们需要将被切换的页面进行暂存/缓存，怎么做呢？很简单，在主窗口类中，我们声明一个Page1的类变量，用来保存这个页面实例，当点击btn1跳转时，我们不用new新的page1，而是直接把这个保存住的page1实例添加到父容器就行了，但是这样，又会出现一个新问题：  
当你的页面非常非常多时，你要在主窗体类中声明n个变量来保存他们！有没有一种办法不需要去声明变量又能方便地实现切换页面的功能呢，Winform.Navigator解决了这个问题，你只需要在主窗体创建时，向Navigator注册所有的页面类即可，Navigator会自动帮你创建页面实例并管理生命周期，而且你可以在页面类的Pause,Restore和Reset方法中去定义当页面被移出或加入容器时的动作。

## 安装


## 基本使用

1. 设置程序使用者身份/权限。
```
static void Main()
{
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    //第一步，设置程序使用者角色
    Navigator.Navigator.SetRole(Navigator.Authority.VISITOR | Navigator.Authority.USER);
    Application.Run(new Form1());
}
```
SetRole方法不一定非要写在Main方法中，也可以写在主窗体构造时，只需要注意，一定要在默认页面显示前(如果有默认页面)或切换页面前调用，因为每次切换到新页面时都会做鉴权操作

2. 给页面继承IPage接口
```
public partial class Page1 : Form,IPage
{
    public string Path { get; set; }
    public bool Cached { get; set; } //设置该页面是否缓存
    public Authority Authority { get; set; } = Authority.VISITOR; //设置该页面访问权限
    ...
    public void Pause()
    {

    }

    public void Restore()
    {

    }

    public void Reset()
    {

    }
}
```

3. 把继承了IPage的窗口类向导航器注册。这一步可在主窗体的构造函数或Load事件中完成
```
private void Form1_Load(object sender, EventArgs e)
{
    //第二步，把继承了IPage的窗口类向导航器注册
    navigator1.RegisterPage<Page1>();
    navigator1.RegisterPage<Page2>(true);  //设置为默认页面
}
```

4. 给按钮编写跳转逻辑  
假设button1点击后跳转到page1，button2点击后跳转到page2
```
private void button1_Click(object sender, EventArgs e)
{
    navigator1.NavigateTo<Page1>();
}

private void button2_Click(object sender, EventArgs e)
{
    navigator1.NavigateTo<Page2>();
}
```

更多例子可参考仓库中的demo项目代码