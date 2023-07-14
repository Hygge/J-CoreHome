<template>
  <div>
      <el-card class="aside-box-card">
          <div>
              <el-avatar :size="100" :src="avatar" />
          </div>
          <h1 style="text-align: center">Hygge</h1>
          <div class="aside-ul">
              <ul>
                  <li>博客园</li>
                  <li>GitHub</li>
                  <li>掘金</li>
                  <li>QQ</li>
              </ul>
          </div>
          <div class="aside-count">
              <span>文章数 &nbsp; 99 &nbsp;&nbsp;</span>
              <span>分类数 &nbsp;  99 &nbsp;&nbsp;</span>
              <span>访问量 &nbsp; 99 &nbsp;&nbsp; </span>
          </div>
      </el-card>
      <el-card class="aside-box-card">
          <div style="text-align: center;">
              <h2>友链</h2>
          </div>
          <hr/>
          <br/>
          <ul>
              <li >

              </li>

          </ul>
      </el-card>
      <!--               style="margin-top: 10px;background-image: url('https://bing.icodeq.com');background-size: cover;"-->

<!--      :style="{ backgroundImage: `url(${bgmUrl})` }"-->
      <el-card class="aside-box-card" v-for="(item, index) in friends" :key="item.fId"
               :style="styles"
      >
          <el-tooltip
              class="box-item"
              effect="dark"
              :content="item.fSign"
              placement="right"
          >
              <el-link :href="item.fUrl"  target='_blank' :underline="false" >
                  <span style="vertical-align: middle;"> <el-avatar :size="50" :src="item.fAvatar"  /> </span>
                  <span style="margin-left: 70px;color: white;font-weight: bolder">{{ item.fUserName }}</span>
              </el-link>
          </el-tooltip>
      </el-card>
  </div>
</template>

<script>
import {onBeforeUnmount, onMounted, ref} from "vue";
import {Get} from "@/utils/axios/restful.js";
import * as url from "url";

export default {
    name: "Aside",

        setup() {
            const bgmUrl = ref('https://bing.icodeq.com');
            const avatar = ref("http://101.35.253.129:8090/upload/2021/12/%E5%A7%9C%E6%B3%B0%E8%8E%894k%E5%A3%81%E7%BA%B83840x2160_%E5%BD%BC%E5%B2%B8%E5%9B%BE%E7%BD%91-98ce877375d347cf81fd36dc1cd878b0.jpg");
            const friends = ref([
                { fUserName : 'Hygge', fUrl: 'sxxx', fSign: 'sxxx', fId : 0, fAvatar: 'xx'}
            ]);
            const styles = ref({
                'margin-top': 10 + 'px',
                'background-image': 'url('+ bgmUrl.value +')',
                'background-size': 'cover',
            });
            const getFrList = () => {
                Get("/Friend/List").then((response) => {
                    let data = response.data;
                    friends.value = data.data;
                }).catch((error) => {
                    console.log(error);
                });
            }
            let timer = null;
            onMounted(()=>{
                Get('/Account/UserInfo').then(res => {
                    avatar.value = res.data.data.setting.avatar;
                })
                getFrList();

          /*      timer = setInterval(() => {
                       bgmUrl.value = 'https://www.bing.com//th?id=OHR.BlacktipSharks_ZH-…465_1920x1080.jpg&rf=LaDigue_1920x1080.jpg&pid=hp';
                }, 2000);*/
            })
            /*
            onBeforeUnmount(() => {
                clearInterval(timer)
                timer = null;
            })*/
            return {
                 avatar,friends, bgmUrl,styles
            }
        },

    }
</script>


<style scoped>
.aside-box-card{
    margin-top: 20px;
    border-radius: 25px;
    text-align: center;

    width: 80%;
    margin-left: 10%;

    .aside-ul li{
        margin-top: 10px;
    }
    .aside-count{
        margin-top: 10px;
    }
}

</style>