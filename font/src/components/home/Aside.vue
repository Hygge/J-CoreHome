<template>
  <div>
      <el-card class="aside-box-card">
          <div>
              <el-avatar :size="100" :src="setting.avatar" />
          </div>
          <h1 style="text-align: center">{{ setting.name }}</h1>
          <div class="aside-ul">
              <ul>
                  <li><a :href="setting.blogs" target="_blank"> <img src="https://ts3.cn.mm.bing.net/th?id=ODLS.7d96c5f7-ff55-4c44-9325-ff8001ea445a&w=32&h=32&qlt=97&pcl=fffffa&o=6&pid=1.2" /></a></li>
                  <li><a  :href="setting.github" target="_blank">
                      <svg height="32" aria-hidden="true" viewBox="0 0 16 16" version="1.1" width="32" data-view-component="true" class="octicon octicon-mark-github v-align-middle color-fg-default">
                          <path d="M8 0c4.42 0 8 3.58 8 8a8.013 8.013 0 0 1-5.45 7.59c-.4.08-.55-.17-.55-.38 0-.27.01-1.13.01-2.2 0-.75-.25-1.23-.54-1.48 1.78-.2 3.65-.88 3.65-3.95 0-.88-.31-1.59-.82-2.15.08-.2.36-1.02-.08-2.12 0 0-.67-.22-2.2.82-.64-.18-1.32-.27-2-.27-.68 0-1.36.09-2 .27-1.53-1.03-2.2-.82-2.2-.82-.44 1.1-.16 1.92-.08 2.12-.51.56-.82 1.28-.82 2.15 0 3.06 1.86 3.75 3.64 3.95-.23.2-.44.55-.51 1.07-.46.21-1.61.55-2.33-.66-.15-.24-.6-.83-1.23-.82-.67.01-.27.38.01.53.34.19.73.9.82 1.13.16.45.68 1.31 2.69.94 0 .67.01 1.3.01 1.49 0 .21-.15.45-.55.38A7.995 7.995 0 0 1 0 8c0-4.42 3.58-8 8-8Z"></path>
                      </svg>
                  </a></li>
                  <li><a :href="setting.juejin" target="_blank"><img src="https://lf3-cdn-tos.bytescm.com/obj/static/xitu_juejin_web/6c61ae65d1c41ae8221a670fa32d05aa.svg" > </a></li>
                  <li><a :href="setting.QQ"> <img src="https://ts1.cn.mm.bing.net/th?id=ODLS.394b5f18-5315-46a2-9941-3be519c1624a&w=32&h=32&qlt=94&pcl=fffffa&o=6&pid=1.2" /> </a></li>
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
import {onBeforeUnmount, onMounted, reactive, ref} from "vue";
import {Get} from "@/utils/axios/restful.js";
import * as url from "url";

export default {
    name: "Aside",

        setup() {
            const bgmUrl = ref('https://bing.icodeq.com');
            const setting = reactive(JSON.parse(localStorage.getItem('setting')))
            //const avatar = ref("http://101.35.253.129:8090/upload/2021/12/%E5%A7%9C%E6%B3%B0%E8%8E%894k%E5%A3%81%E7%BA%B83840x2160_%E5%BD%BC%E5%B2%B8%E5%9B%BE%E7%BD%91-98ce877375d347cf81fd36dc1cd878b0.jpg");
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
            /*    Get('/Account/UserInfo').then(res => {
                    avatar.value = res.data.data.setting.avatar;
                })*/
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
                setting,friends, bgmUrl,styles
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