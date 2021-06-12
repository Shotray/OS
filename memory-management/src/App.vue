<template>
  <div id="app">
    <div>
      <el-row>
        <el-col :span="3">
          <h3>页面置换算法</h3>
          <el-menu
            default-active="1"
            class="menu"
            @select="selectAlgorithm"
          >
            <el-menu-item index="FIFO" to="/FIFO">
              <template slot="title">
                <i class="el-icon-s-help"></i>
                <span>FIFO</span>
              </template>
            </el-menu-item>
            <el-menu-item index="LRU" to="/LRU">
              <i class="el-icon-help"></i>
              <span slot="title">LRU</span>
            </el-menu-item>
          </el-menu>
        </el-col>
        <el-col :span="12">
          <h3>{{algo}}算法</h3>
          <el-row>
            <el-col :span="5" :offset="1"
              ><div class="grid-content bg-purple">
                <div class="common-layout">
                  <el-container class="el-card is-always-shadow box-card" :style="page_style[0]">
                    <el-header class="el-card__header">
                      <p>{{ pages[0].page }}</p>
                    </el-header>
                    <div
                      class="el-card__body"
                      style="height: 340px; padding: 10px"
                    >
                      <div
                        class="transition-box"
                        style="background: rgb(109, 178, 250)"
                        :style="li_style[item+pages[0].page*10]"
                        v-for="item in pages[0].list"
                        :key="item" 
                      >
                        {{ item }}
                      </div>
                    </div>
                  </el-container>
                </div>
              </div></el-col
            >
            <el-col :span="5" :offset="1"
              ><div class="grid-content bg-purple">
                <div class="common-layout">
                  <el-container class="el-card is-always-shadow box-card" :style="page_style[1]">
                    <el-header class="el-card__header">
                      <p>{{ pages[1].page }}</p>
                    </el-header>
                    <div
                      class="el-card__body"
                      style="height: 340px; padding: 10px"
                    >
                      <div
                        class="transition-box"
                        style="background: rgb(109, 178, 250)"
                        :style="li_style[item+pages[1].page*10]"
                        v-for="item in pages[1].list"
                        :key="item"
                      >
                        {{ item }}
                      </div>
                    </div>
                  </el-container>
                </div>
              </div></el-col
            >
            <el-col :span="5" :offset="1"
              ><div class="grid-content bg-purple">
                <div class="common-layout">
                  <el-container class="el-card is-always-shadow box-card" :style="page_style[2]">
                    <el-header class="el-card__header">
                      <p>{{ pages[2].page}}</p>
                    </el-header>
                    <div
                      class="el-card__body"
                      style="height: 340px; padding: 8px"
                    >
                      <div
                        class="transition-box"
                        style="background: rgb(109, 178, 250)"
                        :style="li_style[item+pages[2].page*10]"
                        v-for="item in pages[2].list"
                        :key="item"
                      >
                        {{ item }}
                      </div>
                    </div>
                  </el-container>
                </div>
              </div></el-col
            >
            <el-col :span="5" :offset="1"
              ><div class="grid-content bg-purple">
                <div class="common-layout">
                  <el-container class="el-card is-always-shadow box-card" :style="page_style[3]">
                    <el-header class="el-card__header">
                      <p>{{ pages[3].page }}</p>
                    </el-header>
                    <div
                      class="el-card__body"
                      style="height: 340px; padding: 10px"
                    >
                      <div
                        class="transition-box"
                        style="background: rgb(109, 178, 250)"
                        :style="li_style[item+pages[3].page*10]"
                        v-for="item in pages[3].list"
                        :key="item"
                      >
                        {{ item }}
                      </div>
                    </div>
                  </el-container>
                </div>
              </div></el-col
            >
          </el-row>
          <el-row>
            <el-col span="5" offset="3">
              <el-form ref="form" :model="form" label-width="80px">
                <el-form-item label="缺页数">
                  <el-input disabled :value="miss_page"></el-input>
                </el-form-item>
              </el-form>
            </el-col>
            <el-col span="5" offset="4">
              <el-form ref="form" :model="form" label-width="80px">
                <el-form-item label="缺页率">
                  <el-input disabled :value="miss_page_rate"></el-input>
                </el-form-item>
              </el-form>
            </el-col>
          </el-row>
          <el-row :gutter="30">
            <el-col :span="20" :offset="2">
              <el-progress :percentage="percent" :color="customColorMethod"></el-progress>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="2">
              <el-button round type="primary" icon="el-icon-arrow-right" :disabled="is_disabled"
                @click="singleStep">单步执行</el-button
              >
            </el-col>
            <el-col :span="2" :offset="8">
              <el-button round type="primary" icon="el-icon-d-arrow-right" :disabled="is_disabled"
                @click="multiStep">{{button_name}}</el-button
              >
            </el-col>
            <el-col :span="2" :offset="8">
              <el-button round type="primary" icon="el-icon-refresh-right"
                @click="reset">重新开始</el-button
              >
            </el-col>
          </el-row>
        </el-col>
        <el-col :span="7" :offset="1">
          <h3>已执行指令</h3>
          <el-table
            class="change-table"
            :data="tableData"
            height="450"
            :border="false"
            style="width: 100%"
          >
            <el-table-column type="index" width="50"> </el-table-column>
            <el-table-column prop="address" label="地址" width="90">
            </el-table-column>
            <el-table-column prop="loss_page" label="缺页" width="90">
            </el-table-column>
            <el-table-column prop="out_page" label="换出页" width="90">
            </el-table-column>
            <el-table-column prop="in_page" label="换入页"> </el-table-column>
          </el-table>
        </el-col>
      </el-row>
    </div>
    <router-view />
  </div>
</template>

<script>
export default {
  data() {
    return {
      tableData: [],
      page_num:4,
      page_size:10,
      pages:[{page:"",list:[]},{page:"",list:[]},{page:"",list:[]},{page:"",list:[]}],
      algo:"FIFO",
      button_name:"连续执行",
      miss_page:"0",
      miss_page_rate:"0%",
      cur_step:0,
      all_step:320,
      fifo:[0,1,2,3],
      lru:["","","",""],
      percent:0,
      page_style:["","","",""],
      li_style:[],
      is_disabled:false,
      is_muti:false,
      interval:"",
    };
  },
  methods:{
    customColorMethod(percentage) {
        if (percentage < 30) {
          return '#909399';
        } else if (percentage < 70) {
          return '#e6a23c';
        } else {
          return '#67c23a';
        }
    },
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
    },
    algoChanged:function(index){
      if(this.algo!=index) return true;
      return false;
    },
    selectAlgorithm:function(index,index_path){
      if(index==="FIFO"){
        if(this.algoChanged(index)){
          this.reset();
        }
        this.algo="FIFO";
        console.log("FIFO-processing");
      }
      else if(index==="LRU"){
        if(this.algoChanged(index)){
          this.reset();
        }
        this.algo="LRU";
        console.log("LRU-processing");
      }
    },
    reset:function(){
      for(var i=0;i<this.page_num;i++){
        this.pages[i].page="";
        this.pages[i].list=[];
      }
      this.button_name="连续执行";
      this.miss_page="0";
      this.miss_page_rate="0%";
      this.cur_step=0;
      this.page_style=["","","",""];
      this.tableData=[];
      this.button_name="连续执行";
      this.fifo=[0,1,2,3];
      this.lru=["","","",""];
      this.percent=0;
      this.is_disabled=false;
      this.is_muti=false;
      this.li_style=[];
    },
    FIFO:function(){
      var p=this.fifo.shift();
      this.fifo.push(p);
      return p;
    },
    LRU:function(){
      var temp=0;
      for(var i=0;i<this.page_num;i++){
        if(this.lru[i]<this.lru[temp]){
          temp=i;
        }
      }
      return temp;
    },
    resetPage(){
      this.li_style=[];
      for(var i=0;i<this.page_num;i++){
        this.$set(this.page_style,i,{background : ''});
      }
      for(var i=0;i<this.all_step;i++){
          this.li_style.push("");
      }
    },
    singleStep:function(){
      console.log(this.li_style[0]);
      this.resetPage();

      if(this.tableData.length===this.all_step){
        this.$message('320条指令执行完毕');
        this.button_name="连续执行";
        this.is_disabled=true;
        clearInterval(this.interval);
        return;
      }

      var next_step=this.getNextAddress();
      this.cur_step=next_step;
      var next_page=this.isInPages(next_step);
      
      var data={address:"",loss_page:"",out_page:"",in_page:""};
      data.address=next_step;
      
      if(next_page!=-1){
        data.loss_page="No";
        data.out_page="-";
        data.in_page="-";
        if(this.algo=="LRU"){
          this.lru[next_page]=new Date().getTime();
        }
      }
      else if(next_page==-1){

        var m_p=Number(this.miss_page);
        this.miss_page=m_p+1;

        var p=Math.floor(next_step/10);

        data.loss_page="Yes";
        data.in_page=p;

        if(this.algo=="FIFO"){
          next_page=this.FIFO();
        }
        else if(this.algo=="LRU"){
          next_page=this.LRU();
          this.lru[next_page]=new Date().getTime();
        }

        data.out_page=this.pages[next_page].page;

        this.pages[next_page].page=p;
        this.pages[next_page].list=[0,1,2,3,4,5,6,7,8,9];
        this.$set(this.page_style,next_page,{background : 'cornflowerblue'});
      }

      this.$set(this.li_style,next_step,{background : 'Tomato'});

      this.tableData.push(data);
      this.miss_page_rate=Math.floor(this.miss_page*100/(this.tableData.length));
      this.miss_page_rate.toString();
      this.miss_page_rate+="%";

      this.percent=this.tableData.length*100/this.all_step;
    },
    multiStep:function(){
      if(this.tableData.length==this.all_step){
        clearInterval(this.interval);
      }

      if(this.is_muti==false){
        this.is_muti=true;
        this.button_name="停止执行";
      }else{
        this.is_muti=false;
        this.button_name="连续执行";
      }

      if(this.is_muti==true){
        this.interval=setInterval(this.singleStep,100);
      }
      else{
        clearInterval(this.interval);
      }
    },
    isInPages:function(next_step){
      for(var i=0;i<this.page_num;i++){
        if(this.pages[i].page==="") continue;
        if(Number(this.pages[i].page)*10<=next_step&&(Number(this.pages[i].page)+1)*10>next_step){
          return i;
        }
      }
      return -1;
    }
  }
};
</script>

<style>
.change-table {
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.12), 0 0 6px rgba(0, 0, 0, 0.04);
}

.transition-box {
  color: #fff;
  border: 1px solid;
  margin: 8px auto;
  border-radius: 4px;
}

.el-row {
  margin-bottom: 20px;
}

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
</style>
