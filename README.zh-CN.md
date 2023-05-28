# Winform.Navigator

## 介绍
Winform.Navigator是适用于Winform框架的页面导航器，这里的“页面”可以是一个Form窗体，也可以是继承自Control类的用户控件。

## 安装
Nuget:https://www.nuget.org/packages/Winform.Navigator
GitHub:https://github.com/hlz2516/Winform.Navigator

推荐在nuget管理器中直接搜索Winform.Navigator进行安装。

## 功能
Winform.Navigator包中有两个主要的导航类，一个为Navigator，另一个为Router，它们均支持以下功能：  
- 页面间跳转，带参数跳转
- 可设置页面缓存，可自定义被缓存时以及恢复状态时的操作
- 支持设置页面权限
- 提供页面切换前与切换后事件(类似vue中的全局前置守卫和后置守卫)，默认页面首次显示事件

以下是它们不同的地方：
- Navigator是一个用户控件，因此，你需要将其从工具栏中拖拽出，放到页面容器内；而Router是一个类，因此不需要这一步操作
- Navigator需要在代码中为每个需要的页面进行注册，而Router是在app.config文件中进行路由配置，相当于前者的注册
- Router需要为每个页面类编写Route特性，Navigator不用

## 设计初衷
我在公司做第一个有点规模的工业类winform项目时，发现页面切换功能不够完善，造成了页面管理混乱的问题。之后我找了下第三方的多页面框架比如SunnyUI，
发现有些需求还是难以实现，因此我开发了Winform.Navigator，Winform.Navigator只基于Winform，因此不需要依赖任何第三方包，它只负责实现多页面框架的基础功能，
因此如果你想要找那种好看的多页面框架，可能这个包不是很适合你。

## 基本使用

**当你使用时，请选择以下其中一个使用**

### Navigator的基本适用

1. 添加Navigator控件到容器中
打开工具箱，找到Navigator，将其拖至页面容器中，如图所示：
![拖到容器中](./1.jpg)
红色方框部分就是Navigator控件

2. 设置程序使用者身份/权限。（可选，如果你需要简单的鉴权机制的话）
```c#
static void Main()
{
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Navigator.EnableAuthority = true;  //是否开启鉴权属性，默认false(关闭)
    Navigator.Navigator.SetRole(Navigator.Authority.VISITOR | Navigator.Authority.USER); //这一步是在设置程序使用者角色为游客+普通用户
    Application.Run(new Form1());
}
```
SetRole方法不一定非要写在Main方法中，也可以写在主窗体构造时，只需要注意，一定要在默认页面显示前(如果有默认页面)或切换页面前调用，因为每次切换到新页面时都会做鉴权操作

3. 给作为页面的类继承IPage接口
```c#
public partial class Page1 : Form,IPage
{
    public string Path { get; set; } //Router中才会用到
    public bool Cached { get; set; } //设置该页面是否缓存
    public Authority Authority { get; set; } = Authority.VISITOR; //设置该页面访问权限，如果你开启了鉴权的话，每次跳转到该页面会使用这里的权限属性与使用者角色权限进行比较，如果页面权限与使用者权限没有交集，那么会触发权限不符合事件
    ...
    public void Pause()  //该方法会在切换到其他页面时触发
    {

    }

    public void Restore()  //该方法会在切换回该页面时并且设置了缓存后触发
    {

    }

    public void Reset()  //该方法会在切换回该页面时并且设置了不缓存后触发
    {

    }
}
```

4. 把继承了IPage的窗口类向导航器注册。这一步可在主窗体的构造函数或Load事件中完成
```c#
private void Form1_Load(object sender, EventArgs e)
{
    //把继承了IPage的窗口类向导航器注册
    navigator1.RegisterPage<Page1>();
    navigator1.RegisterPage<Page2>(true);  //注册时可设置默认页面
}
```

5. 给按钮编写跳转逻辑  
假设button1点击后跳转到page1，button2点击后跳转到page2：
```c#
private void button1_Click(object sender, EventArgs e)
{
    navigator1.NavigateTo<Page1>();
}

private void button2_Click(object sender, EventArgs e)
{
    navigator1.NavigateTo<Page2>();
}
```

### Router的基本使用（推荐）
1. 找到项目中的app.config文件，打开后，添加routes项配置，配置完成后如下所示：
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" />
    </startup>
  <!--step 1:configure the routes here-->
  <routes>
    <route defaultPage="true">
      <path>/page-jump/page1</path>
      <name>jumpPage1</name>
    </route>
    <route>
      <path>/page-jump/page2</path>
    </route>
  </routes>
</configuration>
```

关于routes配置的更多规则，请看最后

2. 打开Program.cs，在Application.Run方法前调用Router.LoadConfig方法，如下所示：
```c#
static void Main()
{
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Router.LoadConfig();  //step 2:load the config from step1
    Application.Run(new TestForm1());
}
```

3. 打开主窗体的后台代码文件，在主窗体构造时设置页面容器，如下所示：
```c#
public TestForm1()
{
    InitializeComponent();
    Router.SetContainer(panel1);  //step 3:set page container
}
```

4. 打开作为页面的窗体类的后台代码文件，使类继承IPage接口并添加Route特性：
```c#
/**
    *  step 4: Inherit the IPage interface and  add the Route feature to the class which used as page.
    */
[Route("/page-jump/page1")]
public partial class Page11 : Form,IPage
{
    public string Path { get; set; }
    public bool Cached { get; set; }
    public Authority Authority { get; set; }

    public Page11()
    {
        InitializeComponent();
    }

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

5. 现在你就可以调用Router.RouteTo方法了。我们在主窗体中添加两个按钮，分别用来跳转page11和page12(这里没贴出page12代码，但跟page11是几乎一样的):
```c#
private void button1_Click(object sender, System.EventArgs e)
{
    Router.RouteTo("/page-jump/page1");  //step 5: call RouteTo to route
}

private void button2_Click(object sender, System.EventArgs e)
{
    var paramMap = new Dictionary<string, object>();
    paramMap.Add("TestParam", "It's test word");
    Router.RouteTo("jumpPage2", paramMap); //you can route with params
}
```

更多例子可参考仓库中的demo项目代码,demo分为demo-navigator和demo-router两个项目，其中demo-navigator中多为中文注释，demo-router中多为英文注释。

## XML配置规则

### routers项

- enableAuthority  
类别：项属性  
释义：是否开启内置的页面鉴权机制。若开启，则需要为每个页面设置Authority属性，并且在程序中设置应用使用者角色。  
值：true/false，默认false  
例子：  
```
<routes enableAuthority="true">
...
</routes>
```

### route项

- defaultPage  
类别：项属性  
释义：是否将该页面设置为容器第一次显示时的默认页面。  
值：true/false，默认false    
备注：在配置的多个route项中，只能有一个有默认页面。如果设置了多个默认页面，第一个配置为默认页面的route有效。  
例子：
```
<route defaultPage="true">
    <path>/page-jump/page1</path>
    <name>jumpPage1</name>
</route>
```

- cache
类别：项属性  
释义：是否将该页面缓存  
值：true/false，默认false

- path  
类别：子节点  
释义：该页面的路由路径  
值：自定义  
备注：该项为必填项  

- name  
类别：子节点  
释义：该页面的路由名  
值：自定义  
备注：该项为可选项