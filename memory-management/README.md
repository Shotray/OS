# memory-management

>内存管理项目
>题目：请求调页存储管理方式模拟
>指导老师：
>姓名：
>学号：


### 项目目的

通过对页面置换过程的实现加深对请求调页系统的原理和实现过程的理解。

### 项目需求

#### 基本任务
假设每个页面可存放10条指令，分配给一个作业的内存块为4。模拟一个作业的执行过程，该作业有320条指令，即它的地址空间为32页，目前所有页还没有调入内存。
#### 模拟过程
- 在模拟过程中，如果所访问指令在内存中，则显示其物理地址，并转到下一条指令；如果没有在内存中，则发生缺页，此时需要记录缺页次数，并将其调入内存。如果4个内存块中已装入作业，则需进行页面置换。

- 所有320条指令执行完成后，计算并显示作业执行过程中发生的缺页率。

- 置换算法可以选用FIFO或者LRU算法

- 作业中指令访问次序可以按照下面原则形成：50%的指令是顺序执行的，25%是均匀分布在前地址部分，25％是均匀分布在后地址部分。

### 项目实现

![image-20210612222114295](https://gitee.com/shotray/img-host/raw/master/20210612222123.png)

本项目实现了通过FIFO以及LRU算法进行内存中页面的置换，并且提供了单步执行、连续执行等操作以及算法的可视化，同时在界面上显示当前执行的进度，并实时显示缺页数和缺页率。在320条指令执行完成的时候提示指令执行完毕并且禁用两个执行按钮，实现页面的鲁棒性。

![image-20210612224915233](https://gitee.com/shotray/img-host/raw/master/20210612224916.png)

#### 指令模拟

为实现指令顺序的模拟，本项目通过随机数对指令的实行进行判断。当随机数小于0.5时顺序执行，当随机数介于0.5以及0.75之间时，跳转至前地址部分，当随机数大于0.75并且小于1时，跳转至后地址部分，由此实现50%的指令是顺序执行的，25%是均匀分布在前地址部分，25%是均匀分布在后地址部分。

```javascript
    getNextAddress:function(){
      var probability=Math.random();
      if(probability<0.5){
        return this.cur_step+1;
      }
      else if(probability>=0.5 && probability<0.75){
        return Math.floor(Math.random()*160)+1;
      }
      else if(probability>=0.75){
        return Math.floor(Math.random()*160)+161;
      }
    }
```

#### FIFO算法

FIFO算法，即First In First Out，首先调入内存的页面首先被调出。这种算法简单易实现，但可能导致频繁的调入调出降低效率。

本项目实现代码如下：

```javascript
FIFO:function(){
      var p=this.fifo.shift();
      this.fifo.push(p);
      return p;
    }
```

#### LRU算法

LRU算法，即Least Recently Used，最近最少使用。这种算法选择最近最久未使用的算法予以淘汰，具体实现过程为在每次一个页面被访问到的时候记录其距离上次被访问经历的时间`t`，当需要淘汰一个页面时，选择现有页面中`t`值最大的，即最近最少使用的页面进行淘汰。

本项目中记录的不是距离上次被访问所经过的时间`t`，而是维护一个页面数组，数组中的值为当前在内存中当前位置的页面最后一次被访问的时间`t`，用`Date().getTime()`进行生成并记录在数组中，每次遍历调出选择`t`最小的，即最久未被访问的页面。

```javascript
LRU:function(){
      var temp=0;
      for(var i=0;i<this.page_num;i++){
        if(this.lru[i]<this.lru[temp]){
          temp=i;
        }
      }
      return temp;
    }
```

### 开发环境
- Windows 10
- Vue2.x + @vue/cli 4.5.13
- node.js v14.16.1

### 项目运行

1. 在线运行：本项目部署在`github`上，可通过单击下方链接进行在线运行。

   https://shotray.github.io/OS/#/

2. 本地运行
- 安装依赖
```
npm install
```
- 编译运行
```
npm run serve
```

3. 其余可对照文档进行操作
See [Configuration Reference](https://cli.vuejs.org/config/).

