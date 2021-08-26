# memory-management

>文件系统管理项目
>指导老师：
>姓名：
>学号：

### 项目目的
- 理解文件存储空间的管理
- 掌握文件的物理结构、目录结构和文件操作
- 实现简单文件系统管理
- 加深文件系统实现过程的理解

### 项目需求
在内存中开辟一个空间作为文件存储器，在其上实现一个简单的文件系统，退出这个文件系统时，需要该文件系统的内容保存到磁盘上，以便下次可以将其回复到内存中来。

### 项目实现

本项目实现了文件的创建、打开、关闭、读写以及删除，同时可以根据不同的路径进入不同的目录，实现对目录的访问以及对文件的读写。

1. 创建文件夹/文本文档（通过单击右键来创建文件夹/文本文档）
		本项目通过多级目录结构实现对每个目录项的管理，默认文件夹所占空间为0B，文本文档的大小取决于其中内容的大小。
   ![image-20210626152005602](https://gitee.com/shotray/img-host/raw/master/20210626152014.png)![image-20210626152145783](https://gitee.com/shotray/img-host/raw/master/20210626152147.png)

   ![image-20210626152202887](https://gitee.com/shotray/img-host/raw/master/20210626152204.png)

   ![image-20210626152216267](https://gitee.com/shotray/img-host/raw/master/20210626152217.png)

2. 打开/关闭文件夹/文本文档（通过双击打开文件夹/文本文档）
		本项目实现了文件的读写管理,当下一次打开时会读入磁盘块中的内容,显示已经写入的内容。
   ![image-20210626152230459](https://gitee.com/shotray/img-host/raw/master/20210626152231.png)

   ![image-20210626152459295](https://gitee.com/shotray/img-host/raw/master/20210626152500.png)

   ![image-20210626152810559](https://gitee.com/shotray/img-host/raw/master/20210626152811.png)

   ![image-20210626152823715](https://gitee.com/shotray/img-host/raw/master/20210626152824.png)

3. 格式化（通过右键选择格式化或者单击右上角的格式化进行磁盘格式化）

   ![image-20210626152408183](https://gitee.com/shotray/img-host/raw/master/20210626152409.png)

除了上述功能以外,本项目在左下角实现了计算当前磁盘已使用大小的功能,方便用户进行使用情况的查看;在当前目录的右侧也提供了返回键,可以返回至上层目录;在开始页面最下侧可以进行拖动,查看当前文件的文件名、创建时间、修改时间、类型以及大小;左侧目录树也可以直接点击进入相应的文件夹。

### 项目环境

- 开发环境

  windows 10

  c#

  Visual Studio 2019

- 运行

  双击`executable_file`中的`file-management.exe`

- 源文件

  源文件在`src`文件夹中

  

